using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.ChannelMultiplexerInterfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.DeviceActivity.BusinessLogicInterfaces;

namespace RWE.SmartHome.SHC.BusinessLogic.UserNotifications;

public class MessageFactory
{
	private IRepository repository;

	public MessageFactory(IRepository repository)
	{
		this.repository = repository;
	}

	public Message CreateLowRfMessage(Guid deviceId, bool occured)
	{
		return CreateCoreAlert(deviceId, occured, MessageType.DeviceLowRfQuality.ToString(), new List<StringProperty>());
	}

	public Message CreateLowBatMessage(Guid deviceId, bool occured)
	{
		return CreateCoreAlert(deviceId, occured, MessageType.DeviceLowBattery.ToString(), GetAlertProperties(deviceId));
	}

	public Message CreateUnreachableMessage(Guid deviceId, bool occured)
	{
		return CreateCoreAlert(deviceId, occured, MessageType.DeviceUnreachable.ToString(), GetAlertProperties(deviceId));
	}

	public Message CreateFreezeMessage(Guid deviceId, bool occured)
	{
		return CreateCoreAlert(deviceId, occured, MessageType.DeviceFreeze.ToString(), new List<StringProperty>());
	}

	public Message CreateBidCosInclusionTimeoutMessage(Guid deviceId, bool occoured)
	{
		return CreateCoreAlert(deviceId, occoured, MessageType.BidCosInclusionTimeout.ToString(), new List<StringProperty>());
	}

	public Message CreateMoldMessage(Guid deviceId, bool occured)
	{
		return CreateCoreAlert(deviceId, occured, MessageType.DeviceMold.ToString(), new List<StringProperty>());
	}

	public Message CreateLogLevelChangedMessage(Guid deviceId, LoggingLevelAdjustedEventArgs args)
	{
		Message message = new Message(MessageClass.Message, MessageType.LogLevelChanged.ToString(), new List<StringProperty>
		{
			new StringProperty(MessageParameterKey.ShcLogLevelExpirationInfo.ToString(), args.ElevationExpiration.TotalMinutes.ToString()),
			new StringProperty(MessageParameterKey.RequesterInfo.ToString(), string.IsNullOrEmpty(args.Requester) ? "Administrator" : args.Requester)
		});
		message.AppId = CoreConstants.CoreAppId;
		message.AddinVersion = "1.0";
		message.BaseDeviceIds = new List<Guid> { deviceId };
		Message message2 = message;
		if (!string.IsNullOrEmpty(args.Reason))
		{
			message2.Properties.Add(new StringProperty(MessageParameterKey.ShcLogLevelChangeReason.ToString(), args.Reason.ToString()));
		}
		return message2;
	}

	public Message CreateDALEnabledMessage(Guid deviceId)
	{
		Message message = new Message(MessageClass.Message, MessageType.DeviceActivityLoggingEnabled.ToString(), null);
		message.AppId = CoreConstants.CoreAppId;
		message.AddinVersion = "1.0";
		message.BaseDeviceIds = new List<Guid> { deviceId };
		return message;
	}

	public Message CreateShcRebootScheduledMessage(Guid deviceId, ShcRebootScheduledEventArgs args)
	{
		Message message = new Message(MessageClass.Message, MessageType.ShcRemoteRebooted.ToString(), new List<StringProperty>());
		message.AppId = CoreConstants.CoreAppId;
		message.AddinVersion = "1.0";
		message.BaseDeviceIds = new List<Guid> { deviceId };
		Message message2 = message;
		message2.Properties.Add(new StringProperty(MessageParameterKey.ShcRemoteRebootReason.ToString(), args.Reason ?? string.Empty));
		message2.Properties.Add(new StringProperty(MessageParameterKey.RequesterInfo.ToString(), args.Requester ?? string.Empty));
		return message2;
	}

	public Message CreateShcUpdateStateMessage(SoftwareUpdateState updateState)
	{
		Message message = new Message();
		message.AppId = CoreConstants.CoreAppId;
		message.AddinVersion = "1.0";
		Message message2 = message;
		switch (updateState)
		{
		case SoftwareUpdateState.Failed:
			message2 = new Message(MessageClass.Alert, MessageType.ShcUpdateCanceled.ToString(), new List<StringProperty>());
			break;
		case SoftwareUpdateState.Success:
			message2 = new Message(MessageClass.Message, MessageType.ShcUpdateCompleted.ToString(), new List<StringProperty>());
			break;
		}
		message2.AppId = CoreConstants.CoreAppId;
		message2.AddinVersion = "1.0";
		return message2;
	}

	public Message CreateShcUpdateAvailableMessage(Guid deviceId, SoftwareUpdateType updateType)
	{
		Message message = new Message();
		message.AppId = CoreConstants.CoreAppId;
		message.AddinVersion = "1.0";
		Message message2 = message;
		switch (updateType)
		{
		case SoftwareUpdateType.Mandatory:
			message2.Class = MessageClass.Alert;
			message2.Type = MessageType.ShcMandatoryUpdate.ToString();
			break;
		case SoftwareUpdateType.Optional:
			message2.Class = MessageClass.Alert;
			message2.Type = MessageType.ShcOptionalUpdate.ToString();
			break;
		}
		message2.AppId = CoreConstants.CoreAppId;
		message2.AddinVersion = "1.0";
		message2.BaseDeviceIds = new List<Guid> { deviceId };
		return message2;
	}

	public Message CreateProfileExecutionFailedMessage(Guid deviceId, Guid profileId)
	{
		Message message = new Message(MessageClass.Message, MessageType.RuleExecutionFailed.ToString(), new List<StringProperty>
		{
			new StringProperty(MessageParameterKey.RuleId.ToString(), profileId.ToString("N"))
		});
		message.AppId = CoreConstants.CoreAppId;
		message.AddinVersion = "1.0";
		message.BaseDeviceIds = new List<Guid> { deviceId };
		return message;
	}

	public Message CreateLemonbeatDongleFailedMessage(Guid deviceId, LemonbeatDongleFailedEventArgs args)
	{
		Message message = new Message(args.DongleFailure ? MessageClass.Alert : MessageClass.CounterMessage, MessageType.LemonBeatDongleInitializationFailed.ToString(), new List<StringProperty>());
		message.AppId = CoreConstants.CoreAppId;
		message.AddinVersion = "1.0";
		message.BaseDeviceIds = new List<Guid> { deviceId };
		return message;
	}

	public Message CreateMemoryShortageAlert(Guid deviceId, MemoryShortageEventArgs args)
	{
		Message message = new Message(args.IsShortage ? MessageClass.Alert : MessageClass.CounterMessage, MessageType.MemoryShortage.ToString(), new List<StringProperty>
		{
			new StringProperty(MessageParameterKey.MemoryLoad.ToString(), args.MemoryLoad.ToString())
		});
		message.AppId = CoreConstants.CoreAppId;
		message.AddinVersion = "1.0";
		message.BaseDeviceIds = new List<Guid> { deviceId };
		return message;
	}

	public Message CreateConfigurationPersistedMessage(Guid deviceId, BackendPersistenceResult result)
	{
		Message message = new Message();
		message.AppId = CoreConstants.CoreAppId;
		message.AddinVersion = "1.0";
		message.Type = MessageType.BackendConfigOutOfSync.ToString();
		message.BaseDeviceIds = new List<Guid> { deviceId };
		Message message2 = message;
		switch (result)
		{
		case BackendPersistenceResult.Success:
			message2.Class = MessageClass.CounterMessage;
			break;
		case BackendPersistenceResult.ServiceAccessError:
		case BackendPersistenceResult.ServiceFailure:
			message2.Class = MessageClass.Alert;
			break;
		default:
			message2.Class = MessageClass.Alert;
			Log.Warning(Module.BusinessLogic, "Unknown backend persistence result received.");
			break;
		}
		return message2;
	}

	public Message CreateUsbDeviceConnectionChangedMessage(Guid deviceId, UsbDeviceConnectionChangedEventArgs args)
	{
		List<StringProperty> list = new List<StringProperty>();
		list.Add(new StringProperty(MessageParameterKey.ProtocolId.ToString(), args.ProtocolIdentifier.ToString()));
		List<StringProperty> properties = list;
		Message message = new Message((!args.Connected) ? MessageClass.Alert : MessageClass.CounterMessage, MessageType.USBDeviceUnplugged.ToString(), properties);
		message.AppId = CoreConstants.CoreAppId;
		message.AddinVersion = "1.0";
		return message;
	}

	public Message CreateSmokeDetectedAlert(SendSmokeDetectionNotificationEventArgs args)
	{
		Message message = new Message(args.Occurred ? MessageClass.Alert : MessageClass.CounterMessage, MessageType.SmokeDetected.ToString(), new List<StringProperty>());
		message.AppId = CoreConstants.CoreAppId;
		message.AddinVersion = "1.0";
		message.BaseDeviceIds = new List<Guid> { args.DeviceId };
		return message;
	}

	public Message CreateDeviceUpdateFailedMessage(DeviceUpdateFailedEventArgs args)
	{
		List<StringProperty> list = new List<StringProperty>();
		list.Add(new StringProperty(MessageParameterKey.DeviceGroup.ToString(), args.PhysicalDeviceType));
		List<StringProperty> properties = list;
		Message message = new Message(MessageClass.Message, MessageType.DeviceUpdateFailed.ToString(), properties);
		message.AppId = args.AppId;
		message.AddinVersion = "1.0";
		message.BaseDeviceIds = new List<Guid> { args.DeviceId };
		return message;
	}

	private Message CreateFirmwareUpdateMessage(MessageClass messageClass, string appId, string addinVersion, string physicalDeviceType, List<Guid> deviceIds)
	{
		Message message = new Message(messageClass, MessageType.DeviceUpdateAvailable.ToString(), new List<StringProperty>
		{
			new StringProperty(MessageParameterKey.DeviceGroup.ToString(), physicalDeviceType)
		});
		message.AppId = appId;
		message.AddinVersion = addinVersion;
		message.BaseDeviceIds = deviceIds;
		return message;
	}

	public List<Message> CreateDeviceReadyForUpdateMessage(IMessagesAndAlertsManager messagesAndAlerts, DeviceReadyForUpdateEventArgs args)
	{
		List<Message> list = new List<Message>();
		Message message = messagesAndAlerts.GetAllMessages((Message msg) => msg.Type == MessageType.DeviceUpdateAvailable.ToString() && msg.Properties.Any((StringProperty prop) => prop.Name == MessageParameterKey.DeviceGroup.ToString() && prop.GetValueAsString() == args.PhysicalDeviceType)).FirstOrDefault();
		if (args.Occurred)
		{
			if (message == null || message.BaseDeviceIds.All((Guid x) => x != args.DeviceId))
			{
				List<Guid> list2 = new List<Guid>();
				if (message != null)
				{
					list.Add(CreateFirmwareUpdateMessage(MessageClass.CounterMessage, args.AppId, "1.0", args.PhysicalDeviceType, message.BaseDeviceIds));
					list2.AddRange(message.BaseDeviceIds);
				}
				list2.Add(args.DeviceId);
				list.Add(CreateFirmwareUpdateMessage(MessageClass.Alert, args.AppId, "1.0", args.PhysicalDeviceType, list2));
			}
		}
		else if (message != null && message.BaseDeviceIds.Any((Guid x) => x == args.DeviceId))
		{
			list.Add(CreateFirmwareUpdateMessage(MessageClass.CounterMessage, args.AppId, "1.0", args.PhysicalDeviceType, message.BaseDeviceIds));
			List<Guid> list3 = message.BaseDeviceIds.Where((Guid x) => x != args.DeviceId).ToList();
			if (list3.Count > 0)
			{
				list.Add(CreateFirmwareUpdateMessage(MessageClass.Alert, args.AppId, "1.0", args.PhysicalDeviceType, list3));
			}
		}
		return list;
	}

	public Message CreateChannelConnectivityChangedMessage(Guid deviceId, ChannelConnectivityChangedEventArgs args)
	{
		Message message = new Message((!args.Connected) ? MessageClass.Alert : MessageClass.CounterMessage, MessageType.ShcNoConnectionToBackend.ToString(), new List<StringProperty>());
		message.AppId = CoreConstants.CoreAppId;
		message.AddinVersion = "1.0";
		message.BaseDeviceIds = new List<Guid> { deviceId };
		return message;
	}

	public Message CreateInternetAccessChangedMessage(Guid deviceId, InternetAccessAllowedChangedEventArgs args)
	{
		Message message = new Message((!args.InternetAccessAllowed) ? MessageClass.Alert : MessageClass.CounterMessage, MessageType.ShcOnlineSwitchIsOff.ToString(), new List<StringProperty>());
		message.AppId = CoreConstants.CoreAppId;
		message.AddinVersion = "1.0";
		message.BaseDeviceIds = new List<Guid> { deviceId };
		return message;
	}

	public Message CreateFactoryResetMessage(BaseDevice baseDevice)
	{
		List<StringProperty> list = new List<StringProperty>();
		list.Add(new StringProperty(MessageParameterKey.DeviceName.ToString(), baseDevice.Name));
		list.Add(new StringProperty(MessageParameterKey.DeviceSerial.ToString(), baseDevice.SerialNumber));
		list.Add(new StringProperty(MessageParameterKey.DeviceLocation.ToString(), (baseDevice.Location != null) ? baseDevice.Location.Name : string.Empty));
		list.Add(new StringProperty(MessageParameterKey.DeviceType.ToString(), baseDevice.DeviceType));
		List<StringProperty> properties = list;
		Message message = new Message(MessageClass.Alert, MessageType.DeviceFactoryReset.ToString(), properties);
		message.AppId = CoreConstants.CoreAppId;
		message.AddinVersion = "1.0";
		message.BaseDeviceIds = new List<Guid> { baseDevice.Id };
		return message;
	}

	public Message CreateLocalAccessActivatedMessage(string ip)
	{
		return CreateLocalAccessMessage(ip, "LocalAccessActivated");
	}

	public Message CreateLocalAccessDeactivatedMessage(string ip)
	{
		return CreateLocalAccessMessage(ip, "LocalAccessDeactivated");
	}

	public Message CreateNotEnoughDiskSpaceAvailableForUpdateAlert(string diskSpaceNecessaryToEmpty)
	{
		List<StringProperty> list = new List<StringProperty>();
		list.Add(new StringProperty("DiskSpaceNecessaryToEmpty", diskSpaceNecessaryToEmpty));
		List<StringProperty> properties = list;
		Message message = new Message(MessageClass.Alert, "NotEnoughDiskSpaceAvailableForUpdate", properties);
		message.AppId = CoreConstants.CoreAppId;
		message.AddinVersion = "1.0";
		return message;
	}

	private Message CreateLocalAccessMessage(string ip, string localAccessMessageType)
	{
		List<StringProperty> list = new List<StringProperty>();
		list.Add(new StringProperty("IP", ip));
		List<StringProperty> properties = list;
		Message message = new Message(MessageClass.Message, localAccessMessageType, properties);
		message.AppId = CoreConstants.CoreAppId;
		message.AddinVersion = "1.0";
		return message;
	}

	private Message CreateCoreAlert(Guid deviceId, bool occured, string type, List<StringProperty> properties)
	{
		Message message = new Message(occured ? MessageClass.Alert : MessageClass.CounterMessage, type, properties);
		message.AppId = CoreConstants.CoreAppId;
		message.AddinVersion = "1.0";
		message.BaseDeviceIds = new List<Guid> { deviceId };
		return message;
	}

	private List<StringProperty> GetAlertProperties(Guid deviceId)
	{
		List<StringProperty> list = new List<StringProperty>();
		BaseDevice baseDevice = repository.GetBaseDevice(deviceId);
		if (baseDevice != null)
		{
			list.Add(new StringProperty("DeviceName", baseDevice.Name));
			list.Add(new StringProperty("SerialNumber", baseDevice.SerialNumber));
			list.Add(new StringProperty("LocationName", (baseDevice.Location != null) ? baseDevice.Location.Name : string.Empty));
		}
		return list;
	}
}
