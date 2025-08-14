using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.PropertiesSets;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;
using RWE.SmartHome.SHC.ChannelMultiplexerInterfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.DeviceActivity.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.DeviceActivity.DeviceActivityInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.BusinessLogic.UserNotifications;

public class UserNotificationManager
{
	private IMessagesAndAlertsManager messagesAndAlerts;

	private IEventManager eventManager;

	private IProtocolMultiplexer protocolMultiplexer;

	private IRepository repository;

	private MessageFactory messageFactory;

	private string LoggingSource = "UserNotificationManager";

	public UserNotificationManager(IMessagesAndAlertsManager messagesAndAlerts, IEventManager eventManager, IProtocolMultiplexer protocolMultiplexer, IRepository repository)
	{
		this.messagesAndAlerts = messagesAndAlerts;
		this.eventManager = eventManager;
		this.protocolMultiplexer = protocolMultiplexer;
		this.repository = repository;
		messageFactory = new MessageFactory(repository);
		SubscribeToEvents();
	}

	private void SubscribeToEvents()
	{
		eventManager.GetEvent<RuleExecutionFailedEvent>().Subscribe(OnProfileExecutionFailed, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<LemonbeatDongleFailedEvent>().Subscribe(OnLemonbeatDongleFailed, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<MemoryShortageEvent>().Subscribe(OnMemoryShortage, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<ConfigurationPersistenceEvent>().Subscribe(OnConfigurationPersisted, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<DeviceInclusionStateChangedEvent>().Subscribe(OnDeviceInclusionStateChanged, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<UsbDeviceConnectionChangedEvent>().Subscribe(OnUsbDeviceConnectionChanged, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<SoftwareUpdateAvailableEvent>().Subscribe(OnSoftwareUpdateAvailable, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<SoftwareUpdateNotAvailableEvent>().Subscribe(OnSoftwareUpdateNotAvailable, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<SoftwareUpdateProgressEvent>().Subscribe(OnSoftwareUpdateProgress, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<ShcRebootScheduledEvent>().Subscribe(OnShcRebootScheduled, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<DeviceActivityLoggingChangedEvent>().Subscribe(OnDalStateChanged, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<DeviceUnreachableChangedEvent>().Subscribe(OnDeviceReachabilityChanged, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<DeviceInclusionTimeoutEvent>().Subscribe(OnDeviceInclusionTimeoutEvent, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<SendSmokeDetectionNotificationEvent>().Subscribe(OnSmokeDetectedNotification, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<DeviceUpdateFailedEvent>().Subscribe(OnDeviceUpdateFailed, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<DeviceReadyForUpdateEvent>().Subscribe(OnDeviceGroupReadyForUpdate, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<ChannelConnectivityChangedEvent>().Subscribe(OnChannelConnectivityChanged, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<InternetAccessAllowedChangedEvent>().Subscribe(OnInternetAccessAllowedChanged, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnShcStartupCompleted, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<DeviceLowBatteryChangedEvent>().Subscribe(OnDeviceLowBatteryEvent, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<DeviceErrorEvent>().Subscribe(OnDeviceOrApplicationError, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<ApplicationErrorEvent>().Subscribe(OnDeviceOrApplicationError, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<ClimateControlStatusUpdateEvent>().Subscribe(OnClimateControlStatusUpdate, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<LoggingLevelAdjustedEvent>().Subscribe(OnLoggingLevelChanged, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<DeviceWasFactoryResetEvent>().Subscribe(OnDeviceWasFactoryReset, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<ShcSecurityNotificationUpdateEvent>().Subscribe(OnSecurityNotificationUpdate, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<LocalAccessStateChangedEvent>().Subscribe(OnLocalAccessStateChangedEvent, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<NotifyUserDiskSpaceForUpdateEvent>().Subscribe(NotifyUserAboutDiskSpaceForUpdate, null, ThreadOption.PublisherThread, null);
		RfCommStateUpdated(SecurityNotificationState.SignalRecovery);
	}

	private void OnClimateControlStatusUpdate(ClimateControlStatusUpdateEventArgs eventArgs)
	{
		switch (eventArgs.StatusType)
		{
		case StatusEventType.Freeze:
		{
			TemperatureSensor temperatureSensor = repository.GetLogicalDevices().OfType<TemperatureSensor>().FirstOrDefault((TemperatureSensor d) => d.BaseDeviceId == eventArgs.DeviceId);
			if (temperatureSensor != null && (temperatureSensor.IsFreezeProtectionActivated || !eventArgs.Occurred))
			{
				Message message2 = messageFactory.CreateFreezeMessage(eventArgs.DeviceId, eventArgs.Occurred);
				SendMessage(message2);
			}
			break;
		}
		case StatusEventType.Mold:
		{
			HumiditySensor humiditySensor = repository.GetLogicalDevices().OfType<HumiditySensor>().FirstOrDefault((HumiditySensor d) => d.BaseDeviceId == eventArgs.DeviceId);
			if (humiditySensor != null && (humiditySensor.IsMoldProtectionActivated || !eventArgs.Occurred))
			{
				Message message = messageFactory.CreateMoldMessage(eventArgs.DeviceId, eventArgs.Occurred);
				SendMessage(message);
			}
			break;
		}
		default:
			throw new ArgumentOutOfRangeException("eventArgs", "Not implemented StatusEventType: " + eventArgs.StatusType);
		}
	}

	private void OnDeviceOrApplicationError(ErrorEventArgs args)
	{
		List<StringProperty> list = new List<StringProperty>();
		list.Add(new StringProperty(MessageParameterKey.DeviceId.ToString(), args.DeviceId.ToString()));
		List<StringProperty> properties = list;
		SendMessage(new Message(args.ErrorConditionActive ? MessageClass.Alert : MessageClass.CounterMessage, args.MessageType.ToString(), properties));
	}

	private void OnDeviceLowBatteryEvent(DeviceLowBatteryChangedEventArgs eventArgs)
	{
		Message message = messageFactory.CreateLowBatMessage(eventArgs.DeviceId, eventArgs.LowBattery);
		SendMessage(message);
	}

	private void OnShcStartupCompleted(ShcStartupCompletedEventArgs args)
	{
		if (args.Progress == StartupProgress.DatabaseAvailable)
		{
			DeleteRouterMessages();
			DeleteConfigFixEntityMessages();
		}
	}

	private void OnChannelConnectivityChanged(ChannelConnectivityChangedEventArgs args)
	{
		Message message = messageFactory.CreateChannelConnectivityChangedMessage(GetShcBaseDeviceId(), args);
		SendMessage(message);
	}

	private void OnInternetAccessAllowedChanged(InternetAccessAllowedChangedEventArgs args)
	{
		Message message = messageFactory.CreateInternetAccessChangedMessage(GetShcBaseDeviceId(), args);
		messagesAndAlerts.AddMessage(message, MessageAddMode.NewMessage);
	}

	private void OnLoggingLevelChanged(LoggingLevelAdjustedEventArgs args)
	{
		Message message = messageFactory.CreateLogLevelChangedMessage(GetShcBaseDeviceId(), args);
		SendMessage(message);
	}

	private void OnDalStateChanged(DeviceActivityLoggingChangedEventArgs args)
	{
		if (args.DALState == DeviceActivityLoggingState.DALEnabled)
		{
			Message message = messageFactory.CreateDALEnabledMessage(GetShcBaseDeviceId());
			SendMessage(message);
		}
	}

	private void OnShcRebootScheduled(ShcRebootScheduledEventArgs args)
	{
		Message message = messageFactory.CreateShcRebootScheduledMessage(GetShcBaseDeviceId(), args);
		SendMessage(message);
	}

	private void OnSoftwareUpdateProgress(SoftwareUpdateProgressEventArgs eventArgs)
	{
		if (eventArgs.State != SoftwareUpdateState.Failed && eventArgs.State != SoftwareUpdateState.Success)
		{
			return;
		}
		Message message = messageFactory.CreateShcUpdateStateMessage(eventArgs.State);
		switch (eventArgs.State)
		{
		case SoftwareUpdateState.Failed:
		{
			IEnumerable<Message> allMessages = messagesAndAlerts.GetAllMessages((Message msg) => msg.Type == MessageType.ShcUpdateCanceled.ToString());
			if (allMessages.Any())
			{
				messagesAndAlerts.DeleteAllMessages((Message msg) => msg.Type == MessageType.ShcUpdateCanceled.ToString());
			}
			SendMessage(message);
			break;
		}
		case SoftwareUpdateState.Success:
			messagesAndAlerts.ReplaceMessage(message, IsMessageUpdateType);
			CleanupDeviceUpdateAlerts();
			CleanUpNotEnoughDiskSpaceAlerts();
			break;
		default:
			throw new ArgumentOutOfRangeException("eventArgs", "Software update LogicalState not implemented: " + eventArgs.State);
		case SoftwareUpdateState.NotAvailable:
		case SoftwareUpdateState.Started:
			break;
		}
	}

	private void CleanupDeviceUpdateAlerts()
	{
		try
		{
			IEnumerable<Message> allMessages = messagesAndAlerts.GetAllMessages((Message msg) => msg.Type == MessageType.DeviceUpdateAvailable.ToString() && string.IsNullOrEmpty(msg.AppId));
			Message alert;
			foreach (Message item in allMessages)
			{
				alert = item;
				messagesAndAlerts.DeleteAllMessages((Message msg) => msg.Id == alert.Id);
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.ExternalCommandDispatcher, "Error while cleaning up device update alerts after software update: " + ex.Message);
		}
	}

	private void CleanUpNotEnoughDiskSpaceAlerts()
	{
		try
		{
			IEnumerable<Message> allMessages = messagesAndAlerts.GetAllMessages((Message msg) => msg.Type == "NotEnoughDiskSpaceAvailableForUpdate");
			Message alert;
			foreach (Message item in allMessages)
			{
				alert = item;
				messagesAndAlerts.DeleteAllMessages((Message msg) => msg.Id == alert.Id);
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.ExternalCommandDispatcher, "Error while cleaning up device not enough disk space alerts after software update: " + ex.Message);
		}
	}

	internal static bool IsMessageUpdateType(Message msg)
	{
		bool result = false;
		if (!string.IsNullOrEmpty(msg.Type))
		{
			if (msg.Type == MessageType.ShcDeferrableUpdate.ToString() || msg.Type == MessageType.ShcMandatoryUpdate.ToString() || msg.Type == MessageType.ShcOptionalUpdate.ToString())
			{
				return true;
			}
		}
		else
		{
			Log.Debug(Module.BusinessLogic, "Message type unknown, unable to tell if it can be replaced or not.");
		}
		return result;
	}

	private void OnSoftwareUpdateAvailable(SoftwareUpdateAvailableEventArgs eventArgs)
	{
		Message message = messageFactory.CreateShcUpdateAvailableMessage(GetShcBaseDeviceId(), eventArgs.UpdateType);
		switch (eventArgs.UpdateType)
		{
		case SoftwareUpdateType.Mandatory:
			messagesAndAlerts.ReplaceMessage(message, (Message m) => m.Type == MessageType.ShcMandatoryUpdate.ToString());
			break;
		case SoftwareUpdateType.Optional:
			if (!messagesAndAlerts.ContainsMessage((Message m) => m.Type == MessageType.ShcOptionalUpdate.ToString()))
			{
				SendMessage(message);
			}
			break;
		default:
			Log.Debug(Module.BusinessLogic, $"No alert will be generated for update type {eventArgs.UpdateType}");
			break;
		case SoftwareUpdateType.Forced:
			break;
		}
	}

	private void OnSoftwareUpdateNotAvailable(SoftwareUpdateNotAvailableEventArgs eventArgs)
	{
		if (messagesAndAlerts.ContainsMessage(IsMessageUpdateType))
		{
			messagesAndAlerts.DeleteAllMessages(IsMessageUpdateType);
		}
	}

	private void OnProfileExecutionFailed(RuleExecutionFailedEventArgs args)
	{
		Message message = messageFactory.CreateProfileExecutionFailedMessage(GetShcBaseDeviceId(), args.RuleId);
		Func<Message, bool> func = (Message msg) => msg.Type == MessageType.RuleExecutionFailed.ToString() && msg.Properties.Any((StringProperty param) => param.Value == args.RuleId.ToString());
		if (messagesAndAlerts.ContainsMessage(func))
		{
			messagesAndAlerts.ReplaceMessage(message, func);
		}
		else
		{
			SendMessage(message);
		}
	}

	private void OnLemonbeatDongleFailed(LemonbeatDongleFailedEventArgs args)
	{
		if (args != null)
		{
			Log.Debug(Module.ExternalCommandDispatcher, (args.DongleFailure ? "Set" : "Remove") + " Lemonbeat dongle failed alert.");
			Func<Message, bool> func = (Message msg) => msg.Type == MessageType.LemonBeatDongleInitializationFailed.ToString();
			Message message = messageFactory.CreateLemonbeatDongleFailedMessage(GetShcBaseDeviceId(), args);
			if (message.Class == MessageClass.CounterMessage)
			{
				messagesAndAlerts.ReplaceMessage(message, func);
			}
			else if (!messagesAndAlerts.ContainsMessage(func))
			{
				messagesAndAlerts.AddMessage(message, MessageAddMode.NewMessage);
			}
		}
	}

	private void OnMemoryShortage(MemoryShortageEventArgs args)
	{
		if (args != null && (!args.IsShortage || !messagesAndAlerts.ContainsMessage((Message m) => m.Type == MessageType.MemoryShortage.ToString())))
		{
			Log.Debug(Module.ExternalCommandDispatcher, (args.IsShortage ? "Set" : "Reset") + " memory shortage alert.");
			Message newMessage = messageFactory.CreateMemoryShortageAlert(GetShcBaseDeviceId(), args);
			messagesAndAlerts.ReplaceMessage(newMessage, (Message msg) => msg.Type == MessageType.MemoryShortage.ToString());
		}
	}

	private void OnConfigurationPersisted(ConfigurationPersistenceEventArgs args)
	{
		if (args.Result != BackendPersistenceResult.OperationPostponed && args.Result != BackendPersistenceResult.OperationCancelled)
		{
			Message message = messageFactory.CreateConfigurationPersistedMessage(GetShcBaseDeviceId(), args.Result);
			SendMessage(message);
		}
	}

	private void OnDeviceInclusionStateChanged(DeviceInclusionStateChangedEventArgs eventArgs)
	{
		if (eventArgs.DeviceInclusionState == RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums.DeviceInclusionState.ExclusionPending || eventArgs.DeviceInclusionState == RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums.DeviceInclusionState.Excluded || eventArgs.DeviceInclusionState == RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums.DeviceInclusionState.Found)
		{
			messagesAndAlerts.DeleteAllMessages((Message msg) => msg.Properties.Where((StringProperty p) => p.Name == MessageParameterKey.DeviceId.ToString() && p.Value == eventArgs.DeviceId.ToString()).Any());
			messagesAndAlerts.DeleteAllMessages((Message msg) => msg.BaseDeviceIds != null && msg.BaseDeviceIds.Contains(eventArgs.DeviceId));
		}
		PhysicalDeviceState physicalDeviceState = protocolMultiplexer.PhysicalState.Get(eventArgs.DeviceId);
		if (physicalDeviceState != null)
		{
			if (eventArgs.DeviceInclusionState == RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums.DeviceInclusionState.FactoryReset && physicalDeviceState.DeviceProperties.GetValue<UpdateState>(PhysicalDeviceBasicProperties.UpdateState) != UpdateState.Updating)
			{
				SendFactoryResetAlert(eventArgs.DeviceId);
			}
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.Enums.DeviceInclusionState? value = physicalDeviceState.DeviceProperties.GetValue<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.Enums.DeviceInclusionState>(PhysicalDeviceBasicProperties.DeviceInclusionState);
			if (value == RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.Enums.DeviceInclusionState.Included && value.HasValue && eventArgs.ProtocolType == ProtocolType.BidCos.ToString())
			{
				Message message = messageFactory.CreateBidCosInclusionTimeoutMessage(eventArgs.DeviceId, occoured: false);
				SendMessage(message);
			}
		}
	}

	private void OnUsbDeviceConnectionChanged(UsbDeviceConnectionChangedEventArgs args)
	{
		SendMessage(messageFactory.CreateUsbDeviceConnectionChangedMessage(GetShcBaseDeviceId(), args));
	}

	private void OnDeviceReachabilityChanged(DeviceUnreachableChangedEventArgs args)
	{
		if (repository.GetBaseDevice(args.DeviceId) != null)
		{
			Message message = messageFactory.CreateUnreachableMessage(args.DeviceId, args.Unreachable);
			SendMessage(message);
		}
	}

	private void OnDeviceWasFactoryReset(DeviceWasFactoryResetEventArgs args)
	{
		BaseDevice baseDevice = repository.GetBaseDevice(args.DeviceId);
		if (baseDevice != null)
		{
			Message message = messageFactory.CreateFactoryResetMessage(baseDevice);
			SendMessage(message);
		}
	}

	private void OnDeviceInclusionTimeoutEvent(DeviceInclusionTimeoutEventArgs args)
	{
		Message message = messageFactory.CreateBidCosInclusionTimeoutMessage(args.DeviceId, occoured: true);
		SendMessage(message);
	}

	private void OnSmokeDetectedNotification(SendSmokeDetectionNotificationEventArgs args)
	{
		BaseDevice baseDevice = repository.GetBaseDevice(args.DeviceId);
		if (baseDevice != null)
		{
			Message message = messageFactory.CreateSmokeDetectedAlert(args);
			SendMessage(message);
		}
		else
		{
			Log.Warning(Module.BusinessLogic, "Could not find the smoke sensor that triggered the alarm, won't create message.");
		}
	}

	private void OnDeviceUpdateFailed(DeviceUpdateFailedEventArgs args)
	{
		Message message = messageFactory.CreateDeviceUpdateFailedMessage(args);
		SendMessage(message);
	}

	private void OnDeviceGroupReadyForUpdate(DeviceReadyForUpdateEventArgs args)
	{
		List<Message> list = messageFactory.CreateDeviceReadyForUpdateMessage(messagesAndAlerts, args);
		list.ForEach(SendMessage);
	}

	private void DeleteRouterMessages()
	{
		try
		{
			Router router = repository.GetLogicalDevices().OfType<Router>().FirstOrDefault();
			if (router == null)
			{
				return;
			}
			messagesAndAlerts.DeleteAllMessages((Message msg) => msg.Properties.Any((StringProperty p) => p.Name == MessageParameterKey.DeviceId.ToString() && p.Value == router.BaseDeviceId.ToString()) && (msg.Type == MessageType.DeviceLowBattery.ToString() || msg.Type == MessageType.DeviceMold.ToString() || msg.Type == MessageType.DeviceFreeze.ToString()));
		}
		catch (Exception ex)
		{
			Log.Error(Module.ExternalCommandDispatcher, $"Error occured when deleting wrong router alerts {ex.ToString()}");
		}
	}

	private void DeleteConfigFixEntityMessages()
	{
		try
		{
			messagesAndAlerts.DeleteAllMessages((Message msg) => msg.Type == MessageType.ConfigFixEntityDeleted.ToString());
		}
		catch (Exception ex)
		{
			Log.Error(Module.ExternalCommandDispatcher, $"Error occured when deleting ConfigFixEntityDeleted messages: {ex.ToString()}");
		}
	}

	private void SendFactoryResetAlert(Guid deviceId)
	{
		BaseDevice baseDevice = repository.GetBaseDevice(deviceId);
		if (baseDevice == null)
		{
			Log.Information(Module.ExternalCommandDispatcher, $"Reset alert suppressed for device {deviceId}, not found in logical config.");
			return;
		}
		Message message = messageFactory.CreateFactoryResetMessage(baseDevice);
		SendMessage(message);
	}

	private void SendMessage(Message message)
	{
		try
		{
			messagesAndAlerts.AddMessage(message, MessageAddMode.NewMessage);
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, LoggingSource, "Error occurred while adding message: " + ex.Message);
		}
	}

	private Guid GetShcBaseDeviceId()
	{
		return repository.GetShcBaseDeviceId();
	}

	private string CreateShcBaseDeviceLink()
	{
		Guid shcBaseDeviceId = repository.GetShcBaseDeviceId();
		return CreateDeviceLink(shcBaseDeviceId);
	}

	private string CreateDeviceLink(Guid deviceId)
	{
		BaseDevice baseDevice = repository.GetBaseDevice(deviceId);
		if (baseDevice != null)
		{
			return $"/desc/device/{baseDevice.DeviceType}.{baseDevice.Manufacturer}/1.0/{deviceId.ToString()}";
		}
		return string.Empty;
	}

	private string CreateCapabilityLink(Guid deviceId)
	{
		LogicalDevice logicalDevice = repository.GetLogicalDevice(deviceId);
		if (logicalDevice != null)
		{
			return $"/desc/device/{logicalDevice.DeviceType}.{logicalDevice.BaseDevice.Manufacturer}/1.0/capability/{deviceId.ToString()}";
		}
		return string.Empty;
	}

	private void OnSecurityNotificationUpdate(ShcSecurityNotificationUpdateEventArgs args)
	{
		if (args.NotificationType == SecurityNotificationType.RfCommunication)
		{
			RfCommStateUpdated(args.NotificationState);
		}
	}

	private void RfCommStateUpdated(SecurityNotificationState newState)
	{
		messagesAndAlerts.AddMessage(new Message((newState == SecurityNotificationState.SignalFailure) ? MessageClass.Alert : MessageClass.CounterMessage, MessageType.RfCommFailureDetected.ToString(), new List<StringProperty>()), MessageAddMode.OverwriteExisting);
	}

	private void OnLocalAccessStateChangedEvent(LocalAccessStateChangedEventArgs args)
	{
		Message message = ((!args.IsLocalAccessEnabled) ? messageFactory.CreateLocalAccessDeactivatedMessage(args.IP) : messageFactory.CreateLocalAccessActivatedMessage(args.IP));
		SendMessage(message);
	}

	private void NotifyUserAboutDiskSpaceForUpdate(NotifyUserDiskSpaceForUpdateEventArgs args)
	{
		if (args.IsEnoughSpace)
		{
			messagesAndAlerts.DeleteAllMessages((Message msg) => msg.Type == "NotEnoughDiskSpaceAvailableForUpdate");
			return;
		}
		IEnumerable<Message> allMessages = messagesAndAlerts.GetAllMessages((Message msg) => msg.Type == "NotEnoughDiskSpaceAvailableForUpdate");
		Message message = messageFactory.CreateNotEnoughDiskSpaceAvailableForUpdateAlert(args.DiskSpaceNecessaryToEmpty);
		if (allMessages.Any())
		{
			messagesAndAlerts.DeleteAllMessages((Message msg) => msg.Type == "NotEnoughDiskSpaceAvailableForUpdate");
		}
		SendMessage(message);
	}
}
