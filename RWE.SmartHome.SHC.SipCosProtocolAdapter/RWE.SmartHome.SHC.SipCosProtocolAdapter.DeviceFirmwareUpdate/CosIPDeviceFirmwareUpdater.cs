using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceFirmwareUpdate;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Persistence;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.DeviceFirmwareUpdate;

public class CosIPDeviceFirmwareUpdater : IProtocolSpecificDeviceUpdater
{
	private const string LoggingSource = "CosIPFirmwareUpdate";

	private const int TransferQueueTimeout = 180;

	private const int DutyCycleEnablingWaitTime = 615;

	private const byte ImagePacketSize = 32;

	private readonly IDeviceList deviceList;

	private readonly IEventManager eventManager;

	private readonly ISipCosPersistence sipCosPersistence;

	private readonly ICosIPFirmwareUpdateController updateController;

	private SubscriptionToken deviceIncludedToken;

	private SubscriptionToken deviceExcludedToken;

	private SubscriptionToken cosIPDeviceUpdateStatusEventToken;

	private SubscriptionToken configurationProcessedToken;

	private SubscriptionToken cosipTrafficControlToken;

	private readonly IScheduler scheduler;

	private readonly List<IDeviceInformation> firmwareTransferQueue = new List<IDeviceInformation>();

	private readonly object processingQueueSync = new object();

	private readonly object updateTriggeredDevicesSync = new object();

	private volatile IDeviceInformation currentDevice;

	private readonly List<Guid> updateTriggeredDevices = new List<Guid>();

	private DateTime lastSendingTime;

	private volatile bool trafficSuspended;

	private SipCosDeviceUpdateStatusEventArgs savedDeviceUpdateStatusArgs;

	private int otauPackageDelay;

	private int outaPackageDelayEventListeners;

	private Dictionary<Guid, DeviceFirmwareDescriptor> firmwareUpdates = new Dictionary<Guid, DeviceFirmwareDescriptor>();

	public event EventHandler<UpdateFailedEventArgs> UpdateFailed;

	public CosIPDeviceFirmwareUpdater(IScheduler scheduler, IEventManager eventManager, ISipCosPersistence sicCosPersistence, ICosIPFirmwareUpdateController updateController, IDeviceList deviceList, int otauPackageSendDelay, int otauPackageSendDelayEL)
	{
		this.deviceList = deviceList;
		this.scheduler = scheduler;
		this.eventManager = eventManager;
		sipCosPersistence = sicCosPersistence;
		this.updateController = updateController;
		otauPackageDelay = otauPackageSendDelay;
		outaPackageDelayEventListeners = otauPackageSendDelayEL;
		savedDeviceUpdateStatusArgs = null;
		trafficSuspended = false;
		SubscribeEvents();
	}

	private void UnsubscribeEvents()
	{
		if (deviceIncludedToken != null)
		{
			DeviceInclusionStateChangedEvent deviceInclusionStateChangedEvent = eventManager.GetEvent<DeviceInclusionStateChangedEvent>();
			deviceInclusionStateChangedEvent.Unsubscribe(deviceIncludedToken);
		}
		if (deviceExcludedToken != null)
		{
			DeviceInclusionStateChangedEvent deviceInclusionStateChangedEvent2 = eventManager.GetEvent<DeviceInclusionStateChangedEvent>();
			deviceInclusionStateChangedEvent2.Unsubscribe(deviceExcludedToken);
		}
		if (cosIPDeviceUpdateStatusEventToken != null)
		{
			SipCosDeviceUpdateStatusEvent sipCosDeviceUpdateStatusEvent = eventManager.GetEvent<SipCosDeviceUpdateStatusEvent>();
			sipCosDeviceUpdateStatusEvent.Unsubscribe(cosIPDeviceUpdateStatusEventToken);
		}
		if (configurationProcessedToken != null)
		{
			eventManager.GetEvent<ConfigurationProcessedEvent>().Unsubscribe(configurationProcessedToken);
			configurationProcessedToken = null;
		}
	}

	private void SubscribeEvents()
	{
		if (deviceIncludedToken == null)
		{
			DeviceInclusionStateChangedEvent deviceInclusionStateChangedEvent = eventManager.GetEvent<DeviceInclusionStateChangedEvent>();
			deviceIncludedToken = deviceInclusionStateChangedEvent.Subscribe(HandleDeviceIncluded, (DeviceInclusionStateChangedEventArgs args) => args.DeviceInclusionState == DeviceInclusionState.Included, ThreadOption.BackgroundThread, null);
		}
		if (deviceExcludedToken == null)
		{
			DeviceInclusionStateChangedEvent deviceInclusionStateChangedEvent2 = eventManager.GetEvent<DeviceInclusionStateChangedEvent>();
			deviceExcludedToken = deviceInclusionStateChangedEvent2.Subscribe(HandleDeviceExcluded, (DeviceInclusionStateChangedEventArgs args) => args.DeviceInclusionState == DeviceInclusionState.Excluded || args.DeviceInclusionState == DeviceInclusionState.FactoryReset, ThreadOption.BackgroundThread, null);
		}
		if (cosIPDeviceUpdateStatusEventToken == null)
		{
			SipCosDeviceUpdateStatusEvent sipCosDeviceUpdateStatusEvent = eventManager.GetEvent<SipCosDeviceUpdateStatusEvent>();
			cosIPDeviceUpdateStatusEventToken = sipCosDeviceUpdateStatusEvent.Subscribe(OnUpdateStatusInfoReceived, null, ThreadOption.BackgroundThread, null);
		}
		if (cosipTrafficControlToken == null)
		{
			cosipTrafficControlToken = eventManager.GetEvent<CosIPTrafficControlEvent>().Subscribe(OnCosipTrafficControlEvent, null, ThreadOption.PublisherThread, null);
		}
		scheduler.AddSchedulerTask(new FixedTimeSpanSchedulerTask(Guid.NewGuid(), UnblockTransferQueue, new TimeSpan(0, 3, 0), runOnce: false));
	}

	private void UpdateDevice(Guid deviceId)
	{
		lock (updateTriggeredDevicesSync)
		{
			if (!updateTriggeredDevices.Contains(deviceId))
			{
				updateController.SendDoUpdate(deviceId);
				updateTriggeredDevices.Add(deviceId);
			}
		}
	}

	private void HandleDeviceIncluded(DeviceInclusionStateChangedEventArgs args)
	{
		IDeviceInformation deviceInformation = deviceList[args.DeviceId];
		if (deviceInformation == null)
		{
			return;
		}
		try
		{
			SetUpdateState(deviceInformation, CosIPDeviceUpdateState.UpToDate);
		}
		catch (Exception arg)
		{
			Log.Error(Module.SipCosProtocolAdapter, "CosIPFirmwareUpdate", $"Error handling device inclusion: {arg}");
		}
	}

	private void HandleDeviceExcluded(DeviceInclusionStateChangedEventArgs args)
	{
		IDeviceInformation deviceInformation = deviceList[args.DeviceId];
		if (deviceInformation == null)
		{
			return;
		}
		try
		{
			Log.Debug(Module.SipCosProtocolAdapter, "Handling device excluded for " + args.DeviceId);
			SetUpdateState(deviceInformation, CosIPDeviceUpdateState.UpToDate);
			Log.Debug(Module.SipCosProtocolAdapter, "Successfully removed excluded device queues.");
		}
		catch (Exception arg)
		{
			Log.Error(Module.SipCosProtocolAdapter, "CosIPFirmwareUpdate", $"Error handling device exclusion: {arg}");
		}
	}

	private void AddToUpdateQueue(IDeviceInformation device)
	{
		if (device == null)
		{
			return;
		}
		if (device.UpdateState == CosIPDeviceUpdateState.ReactivateDutyCycle)
		{
			ReactivateDutyCycleForDevice(device);
		}
		if (device.BestOperationMode == DeviceInfoOperationModes.EventListener)
		{
			SendNextPacket(device, 0u);
			return;
		}
		lock (processingQueueSync)
		{
			if (!firmwareTransferQueue.Any((IDeviceInformation d) => d.DeviceId == device.DeviceId))
			{
				firmwareTransferQueue.Add(device);
				Log.Debug(Module.SipCosProtocolAdapter, "CosIPFirmwareUpdate", $"Device {deviceList.LogInfoByDeviceInfo(device)}, added to transfer queue");
			}
			if (currentDevice != null && currentDevice.DeviceId == device.DeviceId)
			{
				currentDevice = null;
			}
		}
		ProcessTransferQueue(null);
	}

	private void RemoveFromQueueByDeviceId(Guid deviceId)
	{
		if (!deviceList.Contains(deviceId))
		{
			return;
		}
		bool flag = false;
		lock (processingQueueSync)
		{
			IDeviceInformation deviceInformation = firmwareTransferQueue.FirstOrDefault((IDeviceInformation device) => device.DeviceId == deviceId);
			if (deviceInformation != null)
			{
				firmwareTransferQueue.Remove(deviceInformation);
				firmwareUpdates.Remove(deviceId);
				Log.Debug(Module.SipCosProtocolAdapter, "CosIPFirmwareUpdate", $"Device {deviceList.LogInfoByGuid(deviceId)} removed from transfer queue");
				if (currentDevice != null && currentDevice.DeviceId == deviceId)
				{
					currentDevice = null;
					flag = true;
				}
			}
		}
		if (flag)
		{
			ProcessTransferQueue(null);
		}
		updateController.RemoveUpdatePackagesForDevice(deviceId);
	}

	private void OnCosipTrafficControlEvent(CosIPTrafficControlEventArgs args)
	{
		switch (args.TrafficState)
		{
		case CosIPTrafficState.Suspend:
			trafficSuspended = true;
			break;
		case CosIPTrafficState.Resume:
			trafficSuspended = false;
			if (savedDeviceUpdateStatusArgs != null)
			{
				ProcessTransferQueue(savedDeviceUpdateStatusArgs);
				savedDeviceUpdateStatusArgs = null;
			}
			break;
		}
	}

	private void OnUpdateStatusInfoReceived(SipCosDeviceUpdateStatusEventArgs args)
	{
		if (!deviceList.Contains(args.DeviceId))
		{
			return;
		}
		try
		{
			bool flag;
			lock (updateTriggeredDevicesSync)
			{
				flag = updateTriggeredDevices.Contains(args.DeviceId);
			}
			if (flag)
			{
				OnResponseFromUpdateTriggeredDevice(args);
			}
			else
			{
				ProcessTransferQueue(args);
			}
		}
		catch (Exception arg)
		{
			Log.Error(Module.SipCosProtocolAdapter, "CosIPFirmwareUpdate", $"Error handling received status info: {arg}");
		}
	}

	private void ProcessTransferQueue(SipCosDeviceUpdateStatusEventArgs args)
	{
		if (trafficSuspended)
		{
			savedDeviceUpdateStatusArgs = args;
			return;
		}
		uint nextSequence = 0u;
		IDeviceInformation deviceInformation = null;
		bool flag = true;
		try
		{
			lock (processingQueueSync)
			{
				if (args == null)
				{
					if (firmwareTransferQueue.Count <= 0 || firmwareTransferQueue[0] == currentDevice)
					{
						Log.Debug(Module.SipCosProtocolAdapter, "CosIPFirmwareUpdate", $"ProcessTransferQueue was invoked but queue is empty or processing the head of the queue is already in progress. Call will be ignored. Queue count: {firmwareTransferQueue.Count}");
						return;
					}
					currentDevice = firmwareTransferQueue[0];
					deviceInformation = currentDevice;
				}
				else
				{
					deviceInformation = deviceList[args.DeviceId];
					if (deviceInformation == null)
					{
						return;
					}
					bool flag2 = deviceInformation.BestOperationMode == DeviceInfoOperationModes.EventListener;
					if ((currentDevice == null || !(args.DeviceId == currentDevice.DeviceId)) && !flag2)
					{
						Log.Debug(Module.SipCosProtocolAdapter, "CosIPFirmwareUpdate", $"Confirmation received from a device which is no longer the head of the queue. Call will be ignored. Device ID: {args.DeviceId}, StatusCode = {args.StatusCode}");
						return;
					}
					switch (args.StatusCode)
					{
					case CosIPFirmwareUpdateStatusCode.Ack:
						flag = ProcessAcknowledgementReceived(args, ref nextSequence, deviceInformation);
						break;
					case CosIPFirmwareUpdateStatusCode.NakWrongFrameSequenceNumber:
						Log.Debug(Module.SipCosProtocolAdapter, "CosIPFirmwareUpdate", $"NakWrongFrameSequenceNumber received from device {deviceList.LogInfoByDeviceInfo(deviceInformation)}. Next frame = {args.NextFrame}");
						if (args.NextFrame == 65535)
						{
							SetUpdateState(deviceInformation, CosIPDeviceUpdateState.TransferConfirmationPending);
						}
						else
						{
							nextSequence = (uint)args.NextFrame;
						}
						break;
					case CosIPFirmwareUpdateStatusCode.Alive:
						flag = false;
						return;
					default:
						if (!flag2)
						{
							firmwareTransferQueue.Remove(currentDevice);
							firmwareTransferQueue.Add(currentDevice);
							currentDevice = firmwareTransferQueue[0];
						}
						SetUpdateState(deviceInformation, CosIPDeviceUpdateState.UpdateAvailable);
						Log.Debug(Module.SipCosProtocolAdapter, "CosIPFirmwareUpdate", $"Nak received from device {deviceList.LogInfoByDeviceInfo(deviceInformation)}. StatusCode = {args.StatusCode.ToString()}, Next frame = {args.NextFrame}");
						break;
					}
				}
				if (deviceInformation != null && flag)
				{
					SendNextPacket(deviceInformation, nextSequence);
				}
			}
		}
		catch (Exception arg)
		{
			Log.Error(Module.SipCosProtocolAdapter, "CosIPFirmwareUpdate", $"Error while processing queue. Current device will be removed. {arg}");
			if (deviceInformation != null)
			{
				firmwareTransferQueue.Remove(deviceInformation);
			}
		}
	}

	private bool ProcessAcknowledgementReceived(SipCosDeviceUpdateStatusEventArgs args, ref uint nextSequence, IDeviceInformation deviceInformation)
	{
		bool result = true;
		switch (deviceInformation.UpdateState)
		{
		case CosIPDeviceUpdateState.UpToDate:
			result = false;
			break;
		case CosIPDeviceUpdateState.UpdateAvailable:
			SetUpdateState(deviceInformation, CosIPDeviceUpdateState.TransferInProgress);
			nextSequence = (uint)args.NextFrame;
			Log.Information(Module.SipCosProtocolAdapter, "CosIPFirmwareUpdate", $"Image transfer started for device {deviceList.LogInfoByDeviceInfo(deviceInformation)}");
			break;
		case CosIPDeviceUpdateState.TransferInProgress:
			if (args.NextFrame == 65535)
			{
				SetUpdateState(deviceInformation, CosIPDeviceUpdateState.TransferConfirmationPending);
			}
			else
			{
				nextSequence = (uint)args.NextFrame;
			}
			break;
		case CosIPDeviceUpdateState.TransferConfirmationPending:
		case CosIPDeviceUpdateState.ReactivateDutyCycle:
			if (updateController.DeviceSupportsDutyCycle(deviceInformation) && deviceInformation.UpdateState == CosIPDeviceUpdateState.TransferConfirmationPending)
			{
				SetUpdateState(deviceInformation, CosIPDeviceUpdateState.ReactivateDutyCycle);
				break;
			}
			SetUpdateState(deviceInformation, CosIPDeviceUpdateState.UpdateTransferred);
			Log.Information(Module.SipCosProtocolAdapter, "CosIPFirmwareUpdate", $"Image transferred on device {deviceList.LogInfoByDeviceInfo(deviceInformation)}");
			if (deviceInformation.BestOperationMode != DeviceInfoOperationModes.EventListener)
			{
				lock (processingQueueSync)
				{
					firmwareTransferQueue.Remove(currentDevice);
					if (firmwareTransferQueue.Count > 0)
					{
						currentDevice = firmwareTransferQueue[0];
					}
					else
					{
						currentDevice = null;
						deviceInformation = null;
						Log.Debug(Module.SipCosProtocolAdapter, "CosIPFirmwareUpdate", "Process transfer queue stopped. No more devices in queue.");
					}
				}
			}
			result = false;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case CosIPDeviceUpdateState.UpdateTransferred:
		case CosIPDeviceUpdateState.UpdatePending:
		case CosIPDeviceUpdateState.Updating:
			break;
		}
		return result;
	}

	private void ReactivateDutyCycle(Guid deviceId)
	{
		Log.Debug(Module.SipCosProtocolAdapter, "CosIPFirmwareUpdate", $"Send reenable duty cycle command for device {deviceId}");
		updateController.EnableDutyCycle(deviceId);
	}

	private Stream GetDeviceFirmwareStream(Guid deviceId)
	{
		if (firmwareUpdates.TryGetValue(deviceId, out var value))
		{
			return File.Open(value.ImageFile, FileMode.Open, FileAccess.Read, FileShare.Read);
		}
		return null;
	}

	private byte ExtractFirmwareVersionNumber(string version)
	{
		return byte.Parse(version.Replace(".", ""), NumberStyles.AllowHexSpecifier);
	}

	private void SendNextPacket(IDeviceInformation deviceInformation, uint nextSequence)
	{
		try
		{
			DeviceFirmwareDescriptor deviceFirmwareDescriptor = firmwareUpdates[deviceInformation.DeviceId];
			switch (deviceInformation.UpdateState)
			{
			case CosIPDeviceUpdateState.UpdateAvailable:
			{
				using (Stream stream = GetDeviceFirmwareStream(deviceInformation.DeviceId))
				{
					if (deviceInformation.UpdateState == CosIPDeviceUpdateState.UpdateAvailable)
					{
						updateController.SendStartUpdate(deviceInformation.DeviceId, ExtractFirmwareVersionNumber(deviceFirmwareDescriptor.TargetVersion), (uint)stream.Length);
						Log.Debug(Module.SipCosProtocolAdapter, "CosIPFirmwareUpdate", $"Send Start Update. Device: {deviceList.LogInfoByDeviceInfo(deviceInformation)}");
					}
				}
				break;
			}
			case CosIPDeviceUpdateState.TransferInProgress:
			{
				using (Stream image = GetDeviceFirmwareStream(deviceInformation.DeviceId))
				{
					byte[] firmwareData = BuildPacket(image, nextSequence);
					Log.Debug(Module.SipCosProtocolAdapter, "CosIPFirmwareUpdate", $"Send Update Data. Device: {deviceList.LogInfoByDeviceInfo(deviceInformation)}, Sequence: {nextSequence}");
					if (nextSequence > 1)
					{
						Thread.Sleep((deviceInformation.BestOperationMode != DeviceInfoOperationModes.EventListener) ? otauPackageDelay : outaPackageDelayEventListeners);
					}
					updateController.SendFirmwareUpdateData(deviceInformation.DeviceId, (ushort)nextSequence, firmwareData);
				}
				break;
			}
			case CosIPDeviceUpdateState.TransferConfirmationPending:
				updateController.SendEndUpdate(deviceInformation.DeviceId);
				Log.Debug(Module.SipCosProtocolAdapter, "CosIPFirmwareUpdate", $"Send End Update. Device: {deviceList.LogInfoByDeviceInfo(deviceInformation)}");
				break;
			case CosIPDeviceUpdateState.ReactivateDutyCycle:
				ReactivateDutyCycleForDevice(deviceInformation);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case CosIPDeviceUpdateState.UpToDate:
			case CosIPDeviceUpdateState.UpdateTransferred:
			case CosIPDeviceUpdateState.UpdatePending:
			case CosIPDeviceUpdateState.Updating:
				break;
			}
			lastSendingTime = DateTime.Now;
		}
		catch (Exception ex)
		{
			Log.Error(Module.SipCosProtocolAdapter, "Error occured while sending firmware update package: " + ex.Message);
			SignalDeviceUpdateFailure(deviceInformation.DeviceId, FailedUpdateStep.Transfer);
		}
	}

	private void ReactivateDutyCycleForDevice(IDeviceInformation deviceInformation)
	{
		Log.Debug(Module.SipCosProtocolAdapter, "CosIPFirmwareUpdate", $"Reenable duty cycle command for device {deviceList.LogInfoByDeviceInfo(deviceInformation)} scheduled");
		Guid deviceId = deviceInformation.DeviceId;
		scheduler.AddSchedulerTask(new FixedTimeSpanSchedulerTask(Guid.NewGuid(), delegate
		{
			ReactivateDutyCycle(deviceId);
		}, new TimeSpan(0, 0, 615), runOnce: true));
	}

	private static byte[] BuildPacket(Stream image, uint sequence)
	{
		image.Position = sequence * 32;
		long num = ((image.Length - image.Position < 32) ? (image.Length - image.Position) : 32);
		byte[] array = new byte[num];
		int num2 = 0;
		int num3 = array.Length;
		while (num3 > 0)
		{
			int num4 = image.Read(array, num2, num3);
			if (num4 <= 0)
			{
				throw new EndOfStreamException($"End of stream reached with {num3} bytes left to read");
			}
			num3 -= num4;
			num2 += num4;
		}
		return array;
	}

	private void OnResponseFromUpdateTriggeredDevice(SipCosDeviceUpdateStatusEventArgs args)
	{
		Guid deviceId = args.DeviceId;
		IDeviceInformation deviceInformation = deviceList[deviceId];
		lock (updateTriggeredDevicesSync)
		{
			updateTriggeredDevices.Remove(deviceId);
		}
		if (args.StatusCode == CosIPFirmwareUpdateStatusCode.Ack)
		{
			SetUpdateState(deviceInformation, CosIPDeviceUpdateState.Updating);
			return;
		}
		if (args.StatusCode == CosIPFirmwareUpdateStatusCode.Nak)
		{
			Log.Warning(Module.SipCosProtocolAdapter, $"DoUpdate command for device {deviceId.ToString()} failed.");
			SetUpdateState(deviceInformation, CosIPDeviceUpdateState.UpdateAvailable);
		}
		else if (args.StatusCode != CosIPFirmwareUpdateStatusCode.Alive)
		{
			SetUpdateState(deviceInformation, CosIPDeviceUpdateState.UpToDate);
		}
		SignalDeviceUpdateFailure(deviceId, FailedUpdateStep.Update);
	}

	private void SetUpdateState(IDeviceInformation deviceInformation, CosIPDeviceUpdateState state)
	{
		if (deviceInformation != null)
		{
			deviceInformation.UpdateState = state;
			if (state == CosIPDeviceUpdateState.UpToDate)
			{
				deviceInformation.PendingVersionNumber = 0;
			}
			sipCosPersistence.Save(deviceInformation, suppressEvent: false);
			Log.Information(Module.SipCosProtocolAdapter, "CosIPFirmwareUpdate", $"Changing update state of {deviceList.LogInfoByDeviceInfo(deviceInformation)} to {state}");
		}
	}

	private void UnblockTransferQueue()
	{
		if (trafficSuspended)
		{
			return;
		}
		bool flag = false;
		lock (processingQueueSync)
		{
			if (currentDevice != null)
			{
				int num = ((currentDevice.UpdateState == CosIPDeviceUpdateState.ReactivateDutyCycle) ? 615 : 0) + 180;
				if (DateTime.Now.Subtract(lastSendingTime).TotalSeconds > (double)num)
				{
					firmwareTransferQueue.Remove(currentDevice);
					if (deviceList.Contains(currentDevice.DeviceId))
					{
						firmwareTransferQueue.Add(currentDevice);
					}
					currentDevice = null;
					flag = true;
				}
			}
		}
		if (flag)
		{
			Log.Warning(Module.SipCosProtocolAdapter, "CosIPFirmwareUpdate", "UnblockTransferQueue invoked because it is needed.");
			ProcessTransferQueue(null);
		}
	}

	public List<DeviceUpdateInfo> GetDeviceInfo()
	{
		List<DeviceUpdateInfo> deviceUpdateInfos = new List<DeviceUpdateInfo>();
		deviceList.Where((IDeviceInformation di) => di.DeviceInclusionState == DeviceInclusionState.Included).ToList().ForEach(delegate(IDeviceInformation device)
		{
			deviceUpdateInfos.Add(device.GetDeviceUpdateInfo());
		});
		return deviceUpdateInfos;
	}

	public DeviceUpdateInfo GetDeviceInfo(Guid deviceId)
	{
		IDeviceInformation deviceInformation = deviceList[deviceId];
		if (deviceInformation == null || deviceInformation.DeviceInclusionState != DeviceInclusionState.Included)
		{
			return null;
		}
		return deviceList[deviceId].GetDeviceUpdateInfo();
	}

	public void EnqueueFirmwareTransfer(Guid deviceId, DeviceFirmwareDescriptor firmware)
	{
		IDeviceInformation deviceInformation = deviceList[deviceId];
		if (deviceInformation != null)
		{
			if (!firmwareUpdates.ContainsKey(deviceId))
			{
				firmwareUpdates.Add(deviceId, firmware);
			}
			SetUpdateState(deviceInformation, CosIPDeviceUpdateState.TransferInProgress);
			AddToUpdateQueue(deviceInformation);
		}
	}

	public void AbortUpdate(Guid deviceId)
	{
		IDeviceInformation deviceInformation = deviceList[deviceId];
		try
		{
			RemoveFromQueueByDeviceId(deviceId);
			SignalDeviceUpdateFailure(deviceId, FailedUpdateStep.Transfer);
			if (deviceInformation != null && deviceInformation.DeviceInclusionState == DeviceInclusionState.Included && deviceInformation.DeviceUnreachable)
			{
				SetUpdateState(deviceInformation, CosIPDeviceUpdateState.UpToDate);
			}
		}
		catch (Exception arg)
		{
			Log.Error(Module.SipCosProtocolAdapter, "CosIPFirmwareUpdate", $"Error handling device unreachable: {arg}");
		}
	}

	public void PerformUpdate(List<Guid> deviceIdList)
	{
		foreach (Guid deviceId in deviceIdList)
		{
			IDeviceInformation deviceInformation = deviceList[deviceId];
			if (deviceInformation != null && deviceInformation.UpdateState == CosIPDeviceUpdateState.UpdateTransferred)
			{
				SetUpdateState(deviceInformation, CosIPDeviceUpdateState.UpdatePending);
				UpdateDevice(deviceId);
			}
		}
	}

	private void SignalDeviceUpdateFailure(Guid deviceId, FailedUpdateStep updateStep)
	{
		this.UpdateFailed?.Invoke(this, new UpdateFailedEventArgs
		{
			DeviceId = deviceId,
			UpdateStep = updateStep
		});
	}
}
