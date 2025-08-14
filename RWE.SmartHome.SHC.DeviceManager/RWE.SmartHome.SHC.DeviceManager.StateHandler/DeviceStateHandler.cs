using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using SerialAPI;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceManager.StateHandler;

internal class DeviceStateHandler
{
	private const string LoggingSource = "DeviceStateHandler";

	private const int SmokeDetectorChannelNumber = 1;

	private readonly ICommunicationWrapper communicationWrapper;

	private readonly IEventManager eventManager;

	private readonly byte[] defaultShcAddress;

	private readonly IList<byte[]> shcAddresses;

	private readonly SmokeDetectorNotifier smokeDetectorNotifier;

	private readonly int[] SmokeDetectedValues = new int[3] { 196, 198, 200 };

	private readonly int[] NonSmokeDetectedValues = new int[2] { 0, 1 };

	public DeviceStateHandler(IEventManager eventManager, ICommunicationWrapper communicationWrapper, byte[] defaultShcAddress, IList<byte[]> shcAddresses)
	{
		this.defaultShcAddress = defaultShcAddress;
		this.shcAddresses = shcAddresses;
		this.eventManager = eventManager;
		this.communicationWrapper = communicationWrapper;
		smokeDetectorNotifier = new SmokeDetectorNotifier(eventManager);
		communicationWrapper.StatusInfoHandler.ReceiveStatus += ReceivedDeviceStatusInfo;
		communicationWrapper.AnswerCommandHandler.ReceiveAnswer += ReceivedAnswer;
	}

	private void ReceivedAnswer(SIPcosAnswerFrame answer)
	{
		if (answer.Status == SIPcosAnswerFrameStatus.STATUSACK)
		{
			UpdateDeviceStatus(answer.Header, answer.Header.SequenceNumber, new DeviceStatusInfo(answer), new DeviceChannelStatusInfo(answer), answer.Header.BiDi);
		}
	}

	private void ReceivedDeviceStatusInfo(SIPcosStatusFrame deviceStatus)
	{
		DeviceChannelStatusInfo deviceChannelStatusInfo = new DeviceChannelStatusInfo(deviceStatus);
		communicationWrapper.SendScheduler.AcknowledgePacket(deviceStatus.Header.Source, deviceStatus.Header.SequenceNumber, SIPcosAnswerFrameStatus.ACK, isStatusInfo: true);
		UpdateDeviceStatus(deviceStatus.Header, deviceStatus.Header.SequenceNumber, new DeviceStatusInfo(deviceStatus), deviceChannelStatusInfo, deviceStatus.Header.BiDi);
	}

	private void UpdateDeviceStatus(SIPcosHeader header, byte sequenceNumber, DeviceStatusInfo deviceStatusInfo, DeviceChannelStatusInfo deviceChannelStatusInfo, bool biDirectional)
	{
		lock (communicationWrapper.DeviceList.SyncRoot)
		{
			IDeviceInformation deviceInformation = communicationWrapper.DeviceList[header.Source];
			if (deviceInformation == null)
			{
				Log.Debug(Module.DeviceManager, "DeviceStateHandler", "Received StatusInfo from non-included device");
				return;
			}
			string message = "Received StatusInfo from " + communicationWrapper.DeviceList.LogInfoByDeviceInfo(deviceInformation);
			Log.Debug(Module.DeviceManager, "DeviceStateHandler", message);
			if (biDirectional)
			{
				communicationWrapper.SendAppAck(header.Source, defaultShcAddress, sequenceNumber, forceStayAwake: false);
			}
			if (deviceInformation.DeviceInclusionState == DeviceInclusionState.Included)
			{
				ProcessLowBat(deviceInformation, deviceStatusInfo);
				ProcessFreeze(deviceInformation, deviceStatusInfo);
				ProcessMold(deviceInformation, deviceStatusInfo);
				if (deviceInformation.ManufacturerDeviceType == 18)
				{
					Log.Debug(Module.DeviceManager, "DeviceStateHandler", $"Received smoke signal from {deviceInformation.Address.ToReadable()}");
					AdjustSmokeValue(deviceChannelStatusInfo);
					if (!shcAddresses.Any((byte[] address) => address.Compare(header.Destination)))
					{
						Log.Debug(Module.DeviceManager, "DeviceStateHandler", $"Processing alarm code from {deviceInformation.Address.ToReadable()}");
						Log.Debug(Module.DeviceManager, "DeviceStateHandler", $"Message source: {deviceInformation.Address.ToReadable()}, Message destination: {header.Destination.ToReadable()}");
						ProcessAlarmCodes(deviceInformation, deviceStatusInfo, deviceChannelStatusInfo);
					}
					else
					{
						Log.Debug(Module.DeviceManager, "DeviceStateHandler", $"Skipping alarm code processing for {deviceInformation.Address.ToReadable()}");
					}
				}
				eventManager.GetEvent<SipCosValueChangedEvent>().Publish(new SipCosValueChangedEventArgs(deviceInformation.DeviceId, deviceChannelStatusInfo.ChannelStates));
			}
			if (deviceInformation.StatusInfo == null)
			{
				deviceInformation.StatusInfo = deviceStatusInfo;
				return;
			}
			deviceInformation.StatusInfo.Update(deviceStatusInfo);
			ResetSmokeAlarm(deviceInformation, deviceChannelStatusInfo);
		}
	}

	private void AdjustSmokeValue(DeviceChannelStatusInfo deviceChannelStatusInfo)
	{
		if (deviceChannelStatusInfo.ChannelStates.TryGetValue(1, out var value) && SmokeDetectedValues.Contains(value.Value))
		{
			deviceChannelStatusInfo.ChannelStates[1].Value = 200;
		}
	}

	private void ProcessAlarmCodes(IDeviceInformation deviceInformation, DeviceStatusInfo deviceStatusInfo, DeviceChannelStatusInfo deviceChannelStatusInfo)
	{
		Log.Information(Module.DeviceManager, $"Received smoke detector updates: Id={deviceInformation.DeviceId}");
		foreach (KeyValuePair<byte, ChannelState> channelState in deviceChannelStatusInfo.ChannelStates)
		{
			Log.Information(Module.DeviceManager, $"   Smoke Detector Channel: {channelState.Key} Value: {channelState.Value.Value}");
		}
		if (deviceChannelStatusInfo.ChannelStates.TryGetValue(1, out var value))
		{
			SmokeDetectedState smokeState = ((value.Value != 200) ? SmokeDetectedState.SmokeResolved : SmokeDetectedState.SmokeDetected);
			smokeDetectorNotifier.SendStatusUpdate(deviceInformation.DeviceId, smokeState);
			if (value.Value == 50)
			{
				eventManager.GetEvent<DeviceLowBatteryChangedEvent>().Publish(new DeviceLowBatteryChangedEventArgs(deviceInformation.DeviceId, lowBattery: true));
			}
			if (value.Value == 0)
			{
				eventManager.GetEvent<DeviceLowBatteryChangedEvent>().Publish(new DeviceLowBatteryChangedEventArgs(deviceInformation.DeviceId, lowBattery: false));
			}
		}
	}

	private void ResetSmokeAlarm(IDeviceInformation deviceInformation, DeviceChannelStatusInfo deviceChannelStatusInfo)
	{
		if (deviceInformation.ManufacturerDeviceType == 18 && deviceChannelStatusInfo.ChannelStates.Keys.Count == 1 && deviceChannelStatusInfo.ChannelStates.Keys[0] == 1 && NonSmokeDetectedValues.Contains(deviceChannelStatusInfo.ChannelStates.Values[0].Value))
		{
			Log.Information(Module.DeviceManager, "Non-smoke status received, reseting smoke.");
			smokeDetectorNotifier.SendStatusUpdate(deviceInformation.DeviceId, SmokeDetectedState.SmokeResolved);
		}
	}

	private void ProcessMold(IDeviceInformation deviceInformation, DeviceStatusInfo deviceStatusInfo)
	{
		if (deviceStatusInfo.Mold.HasValue && ((deviceInformation.StatusInfo == null && deviceStatusInfo.Mold.Value) || (deviceInformation.StatusInfo != null && deviceInformation.StatusInfo.Mold != deviceStatusInfo.Mold.Value)))
		{
			if (deviceInformation.ManufacturerDeviceType == 10)
			{
				Log.Debug(Module.DeviceManager, $"Received Mold state for {deviceInformation.ToString()}");
			}
			else
			{
				eventManager.GetEvent<ClimateControlStatusUpdateEvent>().Publish(new ClimateControlStatusUpdateEventArgs(deviceInformation.DeviceId, StatusEventType.Mold, deviceStatusInfo.Mold.Value));
			}
		}
	}

	private void ProcessFreeze(IDeviceInformation deviceInformation, DeviceStatusInfo deviceStatusInfo)
	{
		if (deviceStatusInfo.Freeze.HasValue && ((deviceInformation.StatusInfo == null && deviceStatusInfo.Freeze.Value) || (deviceInformation.StatusInfo != null && deviceInformation.StatusInfo.Freeze != deviceStatusInfo.Freeze.Value)))
		{
			if (deviceInformation.ManufacturerDeviceType == 10)
			{
				Log.Debug(Module.DeviceManager, $"Received Freeze state for {deviceInformation.ToString()}");
			}
			else
			{
				eventManager.GetEvent<ClimateControlStatusUpdateEvent>().Publish(new ClimateControlStatusUpdateEventArgs(deviceInformation.DeviceId, StatusEventType.Freeze, deviceStatusInfo.Freeze.Value));
			}
		}
	}

	private void ProcessLowBat(IDeviceInformation deviceInformation, DeviceStatusInfo deviceStatusInfo)
	{
		bool flag = false;
		if (deviceInformation.StatusInfo != null && (deviceInformation.StatusInfo == null || deviceInformation.StatusInfo.LowBat == deviceStatusInfo.LowBat))
		{
			return;
		}
		if (deviceInformation.ManufacturerDeviceType == 10)
		{
			Log.Debug(Module.DeviceManager, $"Received Low Battery state for {deviceInformation.ToString()}");
			return;
		}
		if (deviceInformation.ManufacturerCode == 1)
		{
			flag = deviceInformation.ManufacturerDeviceType != 3;
		}
		if (flag)
		{
			eventManager.GetEvent<DeviceLowBatteryChangedEvent>().Publish(new DeviceLowBatteryChangedEventArgs(deviceInformation.DeviceId, deviceStatusInfo.LowBat));
		}
	}
}
