using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.GlobalContracts;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using RWE.SmartHome.SHC.BusinessLogic.SystemInformation;
using RWE.SmartHome.SHC.BusinessLogic.UserNotifications;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalCommunication;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalDeviceKeys;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ShcSecurityNotifications;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Authentication;
using RWE.SmartHome.SHC.Core.Authentication.Entities;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.Database;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.CoreApiConverters;
using RWE.SmartHome.SHC.DataAccessInterfaces.Events;
using RWE.SmartHome.SHC.DataAccessInterfaces.Messages;
using RWE.SmartHome.SHC.DataAccessInterfaces.ProtocolSpecificData;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using RWE.SmartHome.SHC.DomainModel.Actions;
using RWE.SmartHome.SHC.SHCRelayDriver;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;
using SHCWrapper.Misc;

namespace RWE.SmartHome.SHC.BusinessLogic;

public class BusinessLogic : Dispatcher, IBusinessLogic, IService
{
	private class DeviceCalibrationInfo
	{
		public Guid PhysicalDeviceId { get; private set; }

		private bool measuredFullUp { get; set; }

		private bool measuredFullDown { get; set; }

		public bool IsCalibrationFinished
		{
			get
			{
				if (measuredFullDown)
				{
					return measuredFullUp;
				}
				return false;
			}
		}

		public DeviceCalibrationInfo(Guid deviceId)
		{
			PhysicalDeviceId = deviceId;
			measuredFullUp = false;
			measuredFullDown = false;
		}

		public void UpdateCalibrationState(CalibrationState newState)
		{
			switch (newState)
			{
			case CalibrationState.MeasuredFullUp:
				measuredFullUp = true;
				break;
			case CalibrationState.MeasuredFullDown:
				measuredFullDown = true;
				break;
			case CalibrationState.WaitingForUnicastUp:
				break;
			}
		}
	}

	private const int MaxBackendRetriesForDeviceListPersistance = 5;

	private const int BackendSyncWaitTimeIncrement = 20;

	private const int BackendSyncRetries = 3;

	private readonly IEventManager eventManager;

	private readonly IFileLogger fileLogger;

	private readonly IUserManager userManagement;

	private readonly IRepository configurationRepository;

	private readonly IMessagePersistence messagePersistence;

	private readonly IBackendPersistence backendPersistence;

	private readonly IScheduler scheduler;

	private readonly IProtocolSpecificDataPersistence protocolSpecificDataPersistence;

	private readonly DatabaseConnectionsPool dbManager;

	private readonly ITokenCache tokenCache;

	private readonly IRFCommFailureNotificationHandler securitySettingsPersistence;

	private readonly ICertificateManager certificateManager;

	private readonly IDeviceManagementClient deviceManagementClient;

	private readonly IKeyExchangeClient keyExchangeClient;

	private readonly IConfigurationClient configurationClient;

	private readonly IShcMessagingClient messagingClient;

	private readonly ICoprocessorAccess coprocessorAccess;

	private readonly IMessagesAndAlertsManager messagesAndAlerts;

	private readonly ILocalUserManager localUserManager;

	private readonly IRegistrationService registrationService;

	private readonly IDeviceMasterKeyRepository deviceMasterKeyRepository;

	private readonly IDeviceKeyRepository deviceKeyRepository;

	private readonly IDownloadDevicesKeysScheduler downloadDevicesKeysScheduler;

	private readonly IStopBackendServicesHandler stopBackendServicesHandler;

	private readonly IEmailSender emailSender;

	private SubscriptionToken uploadLogEventToken;

	private SubscriptionToken uploadSysInfoEventToken;

	private SubscriptionToken syncUsersEventToken;

	private SubscriptionToken getEncryptedKeyEventToken;

	private SubscriptionToken getCosipDeviceKeysEventToken;

	private SubscriptionToken performFactoryResetEventToken;

	private SubscriptionToken performResetEventToken;

	private SubscriptionToken deviceListModifiedEventToken;

	private SubscriptionToken sendSmokeDetectionNotificationEventToken;

	private SubscriptionToken logSystemInformationEventToken;

	private SubscriptionToken persistLogEventToken;

	private SubscriptionToken checkDeviceInclusionToken;

	private SubscriptionToken startupCompletedToken;

	private SubscriptionToken deviceCalibrationStateChangedToken;

	private SubscriptionToken deviceConfigurationFinishedToken;

	private SubscriptionToken configurationPersistenceSubscriptionToken;

	private SubscriptionToken usbDeviceConnectionChangedSubscriptionToken;

	private SubscriptionToken inclusionStateChangedSubscriptionToken;

	private readonly RWE.SmartHome.SHC.SHCRelayDriver.Configuration relayDriverConfiguration;

	private SubscriptionToken rfCommStateUpdated;

	private readonly string shcSerial;

	private readonly XmlSerializer sysInfoSerializer;

	private readonly object businessLogicMutex;

	private volatile bool deviceListNeedsSave;

	private volatile bool shcStartupCompleted;

	private bool isDstActive;

	private Guid backendPersistanceTaskId;

	private readonly bool areBackendRequestsAvailable;

	private readonly IProtocolMultiplexer protocolMultiplexer;

	private List<DeviceCalibrationInfo> calibratingDevices = new List<DeviceCalibrationInfo>();

	public BusinessLogic(Container container, object businessLogicMutex, bool areBackendRequestsAvailable)
	{
		base.Name = "BusinessLogic";
		eventManager = container.Resolve<IEventManager>();
		fileLogger = container.Resolve<IFileLogger>();
		userManagement = container.Resolve<IUserManager>();
		configurationRepository = container.Resolve<IRepository>();
		messagePersistence = container.Resolve<IMessagePersistence>();
		scheduler = container.Resolve<IScheduler>();
		protocolSpecificDataPersistence = container.Resolve<IProtocolSpecificDataPersistence>();
		this.areBackendRequestsAvailable = areBackendRequestsAvailable;
		certificateManager = container.Resolve<ICertificateManager>();
		deviceManagementClient = container.Resolve<IDeviceManagementClient>();
		keyExchangeClient = container.Resolve<IKeyExchangeClient>();
		configurationClient = container.Resolve<IConfigurationClient>();
		messagingClient = container.Resolve<IShcMessagingClient>();
		securitySettingsPersistence = container.Resolve<IRFCommFailureNotificationHandler>();
		coprocessorAccess = container.Resolve<ICoprocessorAccess>();
		protocolMultiplexer = container.Resolve<IProtocolMultiplexer>();
		relayDriverConfiguration = new RWE.SmartHome.SHC.SHCRelayDriver.Configuration(container.Resolve<IConfigurationManager>());
		shcSerial = SHCSerialNumber.SerialNumber();
		sysInfoSerializer = new XmlSerializer(typeof(RWE.SmartHome.SHC.BusinessLogic.SystemInformation.SystemInformation));
		backendPersistence = container.Resolve<IBackendPersistence>();
		this.businessLogicMutex = businessLogicMutex;
		dbManager = container.Resolve<DatabaseConnectionsPool>();
		tokenCache = container.Resolve<ITokenCache>();
		messagesAndAlerts = container.Resolve<IMessagesAndAlertsManager>();
		localUserManager = container.Resolve<ILocalUserManager>();
		registrationService = container.Resolve<IRegistrationService>();
		deviceMasterKeyRepository = container.Resolve<IDeviceMasterKeyRepository>();
		deviceKeyRepository = container.Resolve<IDeviceKeyRepository>();
		downloadDevicesKeysScheduler = container.Resolve<IDownloadDevicesKeysScheduler>();
		stopBackendServicesHandler = container.Resolve<IStopBackendServicesHandler>();
		emailSender = container.Resolve<IEmailSender>();
		new UserNotificationManager(messagesAndAlerts, eventManager, protocolMultiplexer, configurationRepository);
	}

	public void Initialize()
	{
		if (uploadLogEventToken == null)
		{
			UploadLogEvent uploadLogEvent = eventManager.GetEvent<UploadLogEvent>();
			uploadLogEventToken = uploadLogEvent.Subscribe(UploadLog, null, ThreadOption.SubscriberThread, this);
		}
		if (syncUsersEventToken == null)
		{
			SyncUsersEvent syncUsersEvent = eventManager.GetEvent<SyncUsersEvent>();
			syncUsersEventToken = syncUsersEvent.Subscribe(SyncUsersAndRoles, null, ThreadOption.SubscriberThread, this);
		}
		if (getEncryptedKeyEventToken == null)
		{
			GetEncryptedKeyEvent getEncryptedKeyEvent = eventManager.GetEvent<GetEncryptedKeyEvent>();
			getEncryptedKeyEventToken = getEncryptedKeyEvent.Subscribe(GetEncryptedKey, null, ThreadOption.SubscriberThread, this);
		}
		if (getCosipDeviceKeysEventToken == null)
		{
			GetDevicesKeysEvent getDevicesKeysEvent = eventManager.GetEvent<GetDevicesKeysEvent>();
			getCosipDeviceKeysEventToken = getDevicesKeysEvent.Subscribe(GetCosipDevicesKeys, null, ThreadOption.SubscriberThread, this);
		}
		if (uploadSysInfoEventToken == null)
		{
			UploadSysInfoEvent uploadSysInfoEvent = eventManager.GetEvent<UploadSysInfoEvent>();
			uploadSysInfoEventToken = uploadSysInfoEvent.Subscribe(UploadSysInfo, null, ThreadOption.SubscriberThread, this);
		}
		if (performFactoryResetEventToken == null)
		{
			PerformFactoryResetEvent performFactoryResetEvent = eventManager.GetEvent<PerformFactoryResetEvent>();
			performFactoryResetEventToken = performFactoryResetEvent.Subscribe(PerformFactoryReset, null, ThreadOption.PublisherThread, null);
		}
		if (performResetEventToken == null)
		{
			PerformResetEvent performResetEvent = eventManager.GetEvent<PerformResetEvent>();
			performResetEventToken = performResetEvent.Subscribe(PerformReset, null, ThreadOption.PublisherThread, null);
		}
		if (startupCompletedToken == null)
		{
			startupCompletedToken = eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnStartupCompleted, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.CompletedRound1, ThreadOption.PublisherThread, null);
		}
		if (deviceListModifiedEventToken == null)
		{
			ProtocolSpecificDataModifiedEvent protocolSpecificDataModifiedEvent = eventManager.GetEvent<ProtocolSpecificDataModifiedEvent>();
			deviceListModifiedEventToken = protocolSpecificDataModifiedEvent.Subscribe(DeviceListModified, null, ThreadOption.SubscriberThread, this);
		}
		if (sendSmokeDetectionNotificationEventToken == null)
		{
			SendSmokeDetectionNotificationEvent sendSmokeDetectionNotificationEvent = eventManager.GetEvent<SendSmokeDetectionNotificationEvent>();
			sendSmokeDetectionNotificationEventToken = sendSmokeDetectionNotificationEvent.Subscribe(SendSmokeDetectionNotification, null, ThreadOption.SubscriberThread, this);
		}
		if (logSystemInformationEventToken == null)
		{
			LogSystemInformationEvent logSystemInformationEvent = eventManager.GetEvent<LogSystemInformationEvent>();
			logSystemInformationEventToken = logSystemInformationEvent.Subscribe(LogSystemInformation, null, ThreadOption.PublisherThread, null);
		}
		if (persistLogEventToken == null)
		{
			PersistDataBeforeSoftwareUpdateEvent persistDataBeforeSoftwareUpdateEvent = eventManager.GetEvent<PersistDataBeforeSoftwareUpdateEvent>();
			persistLogEventToken = persistDataBeforeSoftwareUpdateEvent.Subscribe(PersistLog, null, ThreadOption.PublisherThread, null);
		}
		if (checkDeviceInclusionToken == null)
		{
			eventManager.GetEvent<GetDeviceKeyEvent>().Subscribe(OnCheckDeviceInclusion, null, ThreadOption.PublisherThread, null);
		}
		if (rfCommStateUpdated == null)
		{
			eventManager.GetEvent<RfCommunicationStateChangedEvent>().Subscribe(OnRfCommStateUpdated, null, ThreadOption.SubscriberThread, this);
		}
		if (deviceConfigurationFinishedToken == null)
		{
			deviceConfigurationFinishedToken = eventManager.GetEvent<DeviceConfigurationFinishedEvent>().Subscribe(OnDeviceConfigFinished, (DeviceConfigurationFinishedEventArgs p) => p.Successful, ThreadOption.PublisherThread, null);
		}
		if (deviceCalibrationStateChangedToken == null)
		{
			deviceCalibrationStateChangedToken = eventManager.GetEvent<DeviceCalibrationStateChangedEvent>().Subscribe(OnCalibrationStateChanged, null, ThreadOption.PublisherThread, null);
		}
		ScheduleNextDSTChangeEvent();
	}

	private void ScheduleNextDSTChangeEvent()
	{
		isDstActive = TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now);
		scheduler.AddSchedulerTask(new FixedTimeSpanSchedulerTask(Guid.NewGuid(), CheckDstChange, new TimeSpan(0, 5, 0)));
	}

	private void CheckDstChange()
	{
		if (isDstActive != TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now))
		{
			NotifyDSTChanged();
			isDstActive = !isDstActive;
		}
	}

	private void NotifyDSTChanged()
	{
		Log.Information(Module.BusinessLogic, "DST changed.");
		eventManager.GetEvent<DSTChangedEvent>().Publish(new DSTChangedEventArgs());
	}

	public void Uninitialize()
	{
		if (deviceCalibrationStateChangedToken != null)
		{
			eventManager.GetEvent<DeviceCalibrationStateChangedEvent>().Unsubscribe(deviceCalibrationStateChangedToken);
			deviceCalibrationStateChangedToken = null;
		}
		if (deviceConfigurationFinishedToken != null)
		{
			eventManager.GetEvent<DeviceConfigurationFinishedEvent>().Unsubscribe(deviceConfigurationFinishedToken);
		}
		if (deviceListModifiedEventToken != null)
		{
			ProtocolSpecificDataModifiedEvent protocolSpecificDataModifiedEvent = eventManager.GetEvent<ProtocolSpecificDataModifiedEvent>();
			protocolSpecificDataModifiedEvent.Unsubscribe(deviceListModifiedEventToken);
			deviceListModifiedEventToken = null;
		}
		if (startupCompletedToken != null)
		{
			eventManager.GetEvent<ShcStartupCompletedEvent>().Unsubscribe(startupCompletedToken);
			startupCompletedToken = null;
		}
		if (performResetEventToken != null)
		{
			PerformResetEvent performResetEvent = eventManager.GetEvent<PerformResetEvent>();
			performResetEvent.Unsubscribe(performResetEventToken);
			performResetEventToken = null;
		}
		if (performFactoryResetEventToken != null)
		{
			PerformFactoryResetEvent performFactoryResetEvent = eventManager.GetEvent<PerformFactoryResetEvent>();
			performFactoryResetEvent.Unsubscribe(performFactoryResetEventToken);
			performFactoryResetEventToken = null;
		}
		if (uploadLogEventToken != null)
		{
			UploadLogEvent uploadLogEvent = eventManager.GetEvent<UploadLogEvent>();
			uploadLogEvent.Unsubscribe(uploadLogEventToken);
			uploadLogEventToken = null;
		}
		if (syncUsersEventToken != null)
		{
			SyncUsersEvent syncUsersEvent = eventManager.GetEvent<SyncUsersEvent>();
			syncUsersEvent.Unsubscribe(syncUsersEventToken);
			syncUsersEventToken = null;
		}
		if (getEncryptedKeyEventToken != null)
		{
			GetEncryptedKeyEvent getEncryptedKeyEvent = eventManager.GetEvent<GetEncryptedKeyEvent>();
			getEncryptedKeyEvent.Unsubscribe(getEncryptedKeyEventToken);
			getEncryptedKeyEventToken = null;
		}
		if (getCosipDeviceKeysEventToken != null)
		{
			GetDevicesKeysEvent getDevicesKeysEvent = eventManager.GetEvent<GetDevicesKeysEvent>();
			getDevicesKeysEvent.Unsubscribe(getCosipDeviceKeysEventToken);
			getCosipDeviceKeysEventToken = null;
		}
		if (uploadSysInfoEventToken != null)
		{
			UploadSysInfoEvent uploadSysInfoEvent = eventManager.GetEvent<UploadSysInfoEvent>();
			uploadSysInfoEvent.Unsubscribe(uploadSysInfoEventToken);
			uploadSysInfoEventToken = null;
		}
		if (sendSmokeDetectionNotificationEventToken != null)
		{
			SendSmokeDetectionNotificationEvent sendSmokeDetectionNotificationEvent = eventManager.GetEvent<SendSmokeDetectionNotificationEvent>();
			sendSmokeDetectionNotificationEvent.Unsubscribe(sendSmokeDetectionNotificationEventToken);
			sendSmokeDetectionNotificationEventToken = null;
		}
		if (logSystemInformationEventToken != null)
		{
			LogSystemInformationEvent logSystemInformationEvent = eventManager.GetEvent<LogSystemInformationEvent>();
			logSystemInformationEvent.Unsubscribe(logSystemInformationEventToken);
			logSystemInformationEventToken = null;
		}
		if (persistLogEventToken != null)
		{
			PersistDataBeforeSoftwareUpdateEvent persistDataBeforeSoftwareUpdateEvent = eventManager.GetEvent<PersistDataBeforeSoftwareUpdateEvent>();
			persistDataBeforeSoftwareUpdateEvent.Unsubscribe(persistLogEventToken);
			persistLogEventToken = null;
		}
		if (checkDeviceInclusionToken != null)
		{
			eventManager.GetEvent<GetDeviceKeyEvent>().Unsubscribe(checkDeviceInclusionToken);
			checkDeviceInclusionToken = null;
		}
		if (configurationPersistenceSubscriptionToken != null)
		{
			eventManager.GetEvent<ConfigurationPersistenceEvent>().Unsubscribe(configurationPersistenceSubscriptionToken);
			configurationPersistenceSubscriptionToken = null;
		}
		if (usbDeviceConnectionChangedSubscriptionToken != null)
		{
			eventManager.GetEvent<UsbDeviceConnectionChangedEvent>().Unsubscribe(usbDeviceConnectionChangedSubscriptionToken);
			usbDeviceConnectionChangedSubscriptionToken = null;
		}
		if (inclusionStateChangedSubscriptionToken != null)
		{
			eventManager.GetEvent<DeviceInclusionStateChangedEvent>().Unsubscribe(inclusionStateChangedSubscriptionToken);
			inclusionStateChangedSubscriptionToken = null;
		}
		if (rfCommStateUpdated != null)
		{
			eventManager.GetEvent<RfCommunicationStateChangedEvent>().Unsubscribe(rfCommStateUpdated);
			rfCommStateUpdated = null;
		}
	}

	public override void Start()
	{
		base.Start();
		Log.Information(Module.BusinessLogic, "Business logic service started.");
	}

	public override void Stop()
	{
		base.Stop();
		Log.Information(Module.BusinessLogic, "Business logic service stopped.");
	}

	public bool SyncUsersAndRoles()
	{
		return SyncUsersAndRoles(lastAttempt: true);
	}

	public void PerformBackendCommunicationWithRetries(string actionText, Func<bool, bool> backendCommunicationMethod)
	{
		PerformBackendCommunicationWithRetries(actionText, 0, backendCommunicationMethod);
	}

	public void BegingStopBackendServicesScheduler(string stopBackendRequestsDate)
	{
		stopBackendServicesHandler.ScheduleStoppingBackendServices(stopBackendRequestsDate);
	}

	private bool IsRfNotificationActive()
	{
		return true;
	}

	private void OnDeviceConfigFinished(DeviceConfigurationFinishedEventArgs args)
	{
		RollerShutterActuator rollerShutterActuator = (from dev in configurationRepository.GetLogicalDevices().OfType<RollerShutterActuator>()
			where dev.BaseDeviceId == args.PhysicalDeviceId
			select dev).FirstOrDefault();
		if (rollerShutterActuator == null)
		{
			return;
		}
		if (rollerShutterActuator.IsCalibrating)
		{
			calibratingDevices.Add(new DeviceCalibrationInfo(args.PhysicalDeviceId));
			return;
		}
		DeviceCalibrationInfo deviceCalibrationInfo = calibratingDevices.FirstOrDefault((DeviceCalibrationInfo d) => d.PhysicalDeviceId == args.PhysicalDeviceId);
		if (deviceCalibrationInfo != null)
		{
			if (deviceCalibrationInfo.IsCalibrationFinished)
			{
				Log.Debug(Module.BusinessLogic, "Rollershutter calibration successfully finished.");
				SetCalibratedDeviceState(rollerShutterActuator);
			}
			calibratingDevices.Remove(deviceCalibrationInfo);
		}
	}

	private void SetCalibratedDeviceState(RollerShutterActuator calibratedISR)
	{
		Log.Debug(Module.BusinessLogic, "Setting initial rollershutter level after successful calibration.");
		ActionContext context = new ActionContext(ContextType.ConfigurationCommit, Guid.Empty);
		protocolMultiplexer.DeviceController.ExecuteAction(context, TemporaryConverters.FromActuatorState(new GenericDeviceState
		{
			LogicalDeviceId = calibratedISR.Id,
			Properties = new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property>
			{
				new NumericProperty
				{
					Name = "ShutterLevel",
					Value = 100m
				}
			}
		}));
	}

	private void OnCalibrationStateChanged(DeviceCalibrationStateChangedEventArgs args)
	{
		switch (args.CalibrationState)
		{
		case CalibrationState.MeasuredFullDown:
		case CalibrationState.MeasuredFullUp:
			calibratingDevices.FirstOrDefault((DeviceCalibrationInfo cd) => cd.PhysicalDeviceId == args.DeviceId)?.UpdateCalibrationState(args.CalibrationState);
			break;
		case CalibrationState.WaitingForUnicastUp:
			break;
		}
	}

	private void OnRfCommStateUpdated(RfCommunicationStateChangedEventArgs args)
	{
		if (securitySettingsPersistence.RFCommFailureNotificationEnabled())
		{
			DateTime date = DateTime.Now;
			PerformBackendCommunicationWithRetries("Send RF communication failure notification", (bool lastAttempt) => SendRfCommFailureNotificationEmail(args.CommunicationState, date));
			eventManager.GetEvent<ShcSecurityNotificationUpdateEvent>().Publish(new ShcSecurityNotificationUpdateEventArgs
			{
				NotificationType = SecurityNotificationType.RfCommunication,
				NotificationState = ((args.CommunicationState != RfCommunicationStates.Failed) ? SecurityNotificationState.SignalRecovery : SecurityNotificationState.SignalFailure)
			});
		}
	}

	private bool SendRfCommFailureNotificationEmail(RfCommunicationStates state, DateTime date)
	{
		bool flag = false;
		lock (businessLogicMutex)
		{
			try
			{
				DateTime now = ShcDateTime.Now;
				int shcTimeOffset = (int)Math.Round((now - now.ToUniversalTime()).TotalHours);
				SendNotificationEmailResult sendNotificationEmailResult = messagingClient.SendNotificationEmail(certificateManager.PersonalCertificateThumbprint, shcSerial, (state == RfCommunicationStates.Failed) ? EmailTemplate.EmailNotifRfCommFailure : EmailTemplate.EmailNotifRfCommFailureResolved, new Dictionary<string, string>(), date.ToUniversalTime(), shcTimeOffset);
				flag = sendNotificationEmailResult == SendNotificationEmailResult.Success;
				if (flag)
				{
					Log.Information(Module.BusinessLogic, $"Send RF failure notification finished with the following confirmation result: {sendNotificationEmailResult}");
				}
				else
				{
					Log.Error(Module.BusinessLogic, $"Send RF failure notification finished with the following confirmation result: {sendNotificationEmailResult}");
				}
			}
			catch (Exception ex)
			{
				Log.Error(Module.BusinessLogic, $"Failed to send Send RF failure notification. See details: [{ex.Message}]");
			}
			return flag;
		}
	}

	private void SendSmokeDetectionNotification(SendSmokeDetectionNotificationEventArgs args)
	{
		if (args == null)
		{
			return;
		}
		string roomName = string.Empty;
		BaseDevice baseDevice = configurationRepository.GetBaseDevice(args.DeviceId);
		if (baseDevice != null && baseDevice.Location != null)
		{
			roomName = baseDevice.Location.Name;
			if (args.Occurred)
			{
				try
				{
					emailSender.SendSmokeDetectedEmail(roomName, args.Date.ToString("dd.MM.yyyy"), args.Date.ToString("HH:mm"));
				}
				catch (Exception ex)
				{
					Log.Error(Module.BusinessLogic, $"There was a problem sending the email using the SMTP server {ex.Message} {ex.StackTrace}");
				}
				PerformBackendCommunicationWithRetries("Send smoke detection notification", (bool lastAttempt) => SendSmokeDetectionNotification(roomName, args.Date, lastAttempt));
			}
		}
		else
		{
			Log.Warning(Module.BusinessLogic, "Could not find the smoke sensor that triggered the alarm, won't send notification.");
		}
	}

	private bool SendSmokeDetectionNotification(string roomName, DateTime date, bool lastAttempt)
	{
		bool flag = false;
		lock (businessLogicMutex)
		{
			try
			{
				DateTime now = ShcDateTime.Now;
				int shcTimeOffset = (int)Math.Round((now - now.ToUniversalTime()).TotalHours);
				SendSmokeDetectedNotificationResult sendSmokeDetectedNotificationResult = messagingClient.SendSmokeDetectionNotification(certificateManager.PersonalCertificateThumbprint, shcSerial, roomName, date.ToUniversalTime(), shcTimeOffset);
				flag = sendSmokeDetectedNotificationResult == SendSmokeDetectedNotificationResult.Success;
				if (flag)
				{
					Log.Information(Module.BusinessLogic, $"Send smoke detection notification finished with the following confirmation result: {sendSmokeDetectedNotificationResult}");
				}
				else
				{
					Log.Error(Module.BusinessLogic, $"Send smoke detection notification finished with the following confirmation result: {sendSmokeDetectedNotificationResult}");
				}
			}
			catch (Exception ex)
			{
				Log.Error(Module.BusinessLogic, $"Failed to send smoke detection notification. See details: [{ex.Message}]");
			}
			return flag;
		}
	}

	private void LogSystemInformation(LogSystemInformationEventArgs args)
	{
		SyncLog.Information(Module.BusinessLogic, $"SHC version: {GetVersionInformation()}\nSHC network parameters: {GetNetworkParameters()}\nSHC performance info: {PerformanceMonitoring.GetPerformanceReport()}. SHC Time zone: {GetCurrentTimeZone()}");
	}

	private void UploadLog(UploadLogEventArgs args)
	{
		List<string> list = ICMPTools.TraceRoute(relayDriverConfiguration.RelayServiceAddress);
		foreach (string item in list)
		{
			SyncLog.Information(Module.BusinessLogic, $"TRACERT: {item}");
		}
		PerformBackendCommunicationWithRetries("Uploading logfile", UploadLog);
	}

	private bool UploadLog(bool lastAttempt)
	{
		bool result = false;
		string correlationId = Guid.NewGuid().ToString();
		try
		{
			eventManager.GetEvent<LogSystemInformationEvent>().Publish(new LogSystemInformationEventArgs());
			ChunkWriter chunkWriter = new ChunkWriter(32768, delegate(byte[] chunk, int currentPackage, int nextPackage)
			{
				UploadFileResponse uploadFileResponse = deviceManagementClient.UploadLogFile(certificateManager.PersonalCertificateThumbprint, shcSerial, chunk, currentPackage, nextPackage, correlationId);
				if (uploadFileResponse != UploadFileResponse.Success)
				{
					throw new ApplicationException("Upload failed, response: " + uploadFileResponse);
				}
				return BackendPersistenceResult.Success;
			});
			BackendPersistenceResult backendPersistenceResult = BackendPersistenceResult.ServiceAccessError;
			if (fileLogger.ProcessAllLines((string line) => BackendPersistenceResult.Success == chunkWriter.AddUtf8String(line)))
			{
				backendPersistenceResult = chunkWriter.Flush();
			}
			if (backendPersistenceResult == BackendPersistenceResult.Success)
			{
				Log.Information(Module.BusinessLogic, "Upload log finished.");
				result = true;
			}
			else
			{
				Log.Error(Module.BusinessLogic, $"Upload of logfile failed with error {backendPersistenceResult}");
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, $"Failed to upload log file. See details: [{ex.Message}]");
		}
		return result;
	}

	private void PersistLog(PersistDataBeforeSoftwareUpdateEventArgs obj)
	{
		fileLogger.StopLoggingAndMoveTo("\\NandFlash\\");
	}

	private void SyncUsersAndRoles(SyncUsersEventArgs args)
	{
		if (!registrationService.IsShcLocalOnly)
		{
			PerformBackendCommunicationWithRetries("Synchronizing users and roles", SyncUsersAndRoles);
		}
	}

	private bool SyncUsersAndRoles(bool lastAttempt)
	{
		if (registrationService.IsShcLocalOnly)
		{
			return false;
		}
		bool flag = false;
		lock (businessLogicMutex)
		{
			try
			{
				Log.Information(Module.BusinessLogic, $"Sync users and roles: Started");
				ConfigurationResultCode shcSyncRecord = configurationClient.GetShcSyncRecord(certificateManager.PersonalCertificateThumbprint, shcSerial, out var syncRecord);
				Log.Information(Module.BusinessLogic, $"Sync users and roles: Received answer from backend with the following result: {shcSyncRecord}");
				if (shcSyncRecord == ConfigurationResultCode.Success && syncRecord != null)
				{
					Dictionary<Guid, Role> roles = syncRecord.Roles.ToDictionary((ShcRole shcRole) => shcRole.Id, (ShcRole shcRole) => new Role
					{
						Id = shcRole.Id,
						Name = shcRole.Name
					});
					Dictionary<Guid, User> dictionary = syncRecord.Users.ToDictionary((ShcUser shcUser) => shcUser.Id, (ShcUser shcUser) => new User
					{
						Id = shcUser.Id,
						Name = shcUser.Name,
						Password = shcUser.PasswordHash,
						CreateDate = shcUser.CreateDate,
						Roles = shcUser.Roles.Select((ShcRef shcRef) => roles[shcRef.RefId]).ToList()
					});
					Log.Information(Module.BusinessLogic, $"Sync users and roles: Processing {dictionary.Count} users and {roles.Count} roles");
					userManagement.SyncRolesAndUsers(roles, dictionary);
					shcSyncRecord = configurationClient.ConfirmShcSyncRecord(certificateManager.PersonalCertificateThumbprint, shcSerial);
					flag = shcSyncRecord == ConfigurationResultCode.Success;
					if (flag)
					{
						Log.Information(Module.BusinessLogic, $"Sync user and roles: Finished with the following confirmation result: {shcSyncRecord}");
					}
					else
					{
						Log.Warning(Module.BusinessLogic, $"Sync user and roles: Finished with the following confirmation result: {shcSyncRecord}");
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error(Module.BusinessLogic, $"Failed to synchronize users and roles. See details: [{ex.Message}]");
			}
			return flag;
		}
	}

	private void GetEncryptedKey(GetEncryptedKeyEventArgs args)
	{
		PerformBackendCommunicationWithRetries("Retrieving encrypted key", (bool lastAttempt) => GetEncryptedKey(args.DeviceId, args.OneTimeKey, args.Sgtin, args.SecNumber, args.EncOnceNetworkKey, args.FirmwareVersion, lastAttempt));
	}

	private void GetCosipDevicesKeys(GetDevicesKeysEventArgs args)
	{
		PerformBackendCommunicationWithRetries("Download Cosip device keys", (bool lastAttempt) => GetDevicesKeys(args.Sgtins, lastAttempt));
	}

	private static EncryptedKeyResponseStatus ToEncryptedKeyResponseStatus(KeyExchangeResult result)
	{
		return result switch
		{
			KeyExchangeResult.Success => EncryptedKeyResponseStatus.Success, 
			KeyExchangeResult.DeviceNotFound => EncryptedKeyResponseStatus.DeviceNotFound, 
			KeyExchangeResult.UnexpectedException => EncryptedKeyResponseStatus.UnexpectedException, 
			KeyExchangeResult.InvalidTenant => EncryptedKeyResponseStatus.InvalidTenant, 
			KeyExchangeResult.DeviceBlacklisted => EncryptedKeyResponseStatus.Blacklisted, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
	}

	private bool GetEncryptedKey(Guid deviceId, byte[] oneTimeKey, byte[] sgtin, byte[] secNumber, byte[] encOnceNetworkKey, string deviceFirmwareVersion, bool lastAttempt)
	{
		if (!areBackendRequestsAvailable)
		{
			return true;
		}
		bool flag = false;
		lock (businessLogicMutex)
		{
			byte[] encTwiceNetworkKey;
			byte[] keyAuthentication;
			EncryptedKeyResponseStatus encryptedKeyResponseStatus;
			try
			{
				KeyExchangeResult keyExchangeResult = keyExchangeClient.EncryptNetworkKey(certificateManager.PersonalCertificateThumbprint, sgtin, secNumber, encOnceNetworkKey, deviceFirmwareVersion, out encTwiceNetworkKey, out keyAuthentication);
				encryptedKeyResponseStatus = ToEncryptedKeyResponseStatus(keyExchangeResult);
				flag = encryptedKeyResponseStatus == EncryptedKeyResponseStatus.Success;
				if (!flag)
				{
					Log.Warning(Module.BusinessLogic, $"EncryptNetworkKey returned failure: {keyExchangeResult}");
				}
				flag = flag || encryptedKeyResponseStatus == EncryptedKeyResponseStatus.InvalidTenant;
			}
			catch (Exception ex)
			{
				Log.Warning(Module.BusinessLogic, $"EncryptNetworkKey threw exception '{ex.Message}' with details: {ex}");
				encryptedKeyResponseStatus = EncryptedKeyResponseStatus.BackendServiceNotReachable;
				encTwiceNetworkKey = null;
				keyAuthentication = null;
			}
			if (flag || lastAttempt)
			{
				eventManager.GetEvent<EncryptedKeyResponseEvent>().Publish(new EncryptedKeyResponseEventArgs
				{
					DeviceId = deviceId,
					Result = encryptedKeyResponseStatus,
					OneTimeKey = oneTimeKey,
					EncTwiceNetworkKey = encTwiceNetworkKey,
					KeyAuthentication = keyAuthentication
				});
			}
			return flag;
		}
	}

	private bool GetDevicesKeys(List<byte[]> sgtins, bool lastAttempt)
	{
		if (!areBackendRequestsAvailable)
		{
			return true;
		}
		if (sgtins == null || sgtins.Count == 0)
		{
			return false;
		}
		List<List<byte[]>> list = deviceKeyRepository.SplitDevices(sgtins);
		if (!list.Any())
		{
			return false;
		}
		bool flag = false;
		lock (businessLogicMutex)
		{
			foreach (List<byte[]> item in list)
			{
				try
				{
					ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary[] deviceKeys;
					KeyExchangeResult devicesKeys = keyExchangeClient.GetDevicesKeys(certificateManager.PersonalCertificateThumbprint, item.ToArray(), out deviceKeys);
					if (devicesKeys == KeyExchangeResult.Success)
					{
						flag = true;
						if (deviceKeys != null)
						{
							deviceKeyRepository.StoreKeys(deviceKeys);
							downloadDevicesKeysScheduler.RemoveDevicesToDownloadLaterFromScheduler(item);
						}
					}
					if (!flag)
					{
						Log.Warning(Module.BusinessLogic, $"GetDevicesKeys returned failure: {devicesKeys}");
						if (!registrationService.IsShcLocalOnly)
						{
							downloadDevicesKeysScheduler.AddDevicesToDownloadLaterInScheduler(item);
						}
					}
				}
				catch (Exception ex)
				{
					Log.Warning(Module.BusinessLogic, $"GetDevicesKeys threw exception '{ex.Message}' with details: {ex}");
					if (!registrationService.IsShcLocalOnly)
					{
						downloadDevicesKeysScheduler.AddDevicesToDownloadLaterInScheduler(item);
					}
					flag = false;
				}
			}
			return flag;
		}
	}

	private void OnCheckDeviceInclusion(CheckDeviceInclusionEventArgs args)
	{
		PerformBackendCommunicationWithRetries("Checking if the device can be included", (bool lastAttempt) => CheckDeviceInclusion(args.Sgtin, lastAttempt));
	}

	private bool CheckDeviceInclusion(byte[] sgtin, bool lastAttempt)
	{
		bool flag = false;
		byte[] deviceKey = null;
		lock (businessLogicMutex)
		{
			EncryptedKeyResponseStatus encryptedKeyResponseStatus;
			try
			{
				KeyExchangeResult deviceKey2 = keyExchangeClient.GetDeviceKey(certificateManager.PersonalCertificateThumbprint, sgtin, out deviceKey);
				encryptedKeyResponseStatus = ToEncryptedKeyResponseStatus(deviceKey2);
				flag = encryptedKeyResponseStatus == EncryptedKeyResponseStatus.Success;
				if (!flag)
				{
					Log.Warning(Module.BusinessLogic, $"CheckForDeviceKey returned failure: {deviceKey2}");
				}
				if (!deviceKeyRepository.DeviceExistsInStorage(sgtin) && deviceKey != null)
				{
					deviceKeyRepository.StoreDeviceKey(new ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary
					{
						SGTIN = sgtin,
						Key = deviceKey
					});
					FilePersistence.DevicesKeysExported = false;
				}
			}
			catch (Exception ex)
			{
				Log.Warning(Module.BusinessLogic, $"CheckForDeviceKey threw exception '{ex.Message}' with details: {ex}");
				encryptedKeyResponseStatus = EncryptedKeyResponseStatus.BackendServiceNotReachable;
			}
			if (flag || lastAttempt)
			{
				eventManager.GetEvent<CheckDeviceInclusionResultEvent>().Publish(new GetDeviceKeyEventArgs
				{
					Sgtin = SGTIN96.Create(sgtin),
					Result = encryptedKeyResponseStatus,
					DeviceKey = deviceKey
				});
			}
			return flag;
		}
	}

	private void UploadSysInfo(UploadSysInfoEventArgs args)
	{
		PerformBackendCommunicationWithRetries("Uploading system info", UploadSysInfo);
	}

	private bool UploadSysInfo(bool lastAttempt)
	{
		bool result = false;
		lock (businessLogicMutex)
		{
			try
			{
				SHCInformation shcInformation = new SHCInformation
				{
					SerialNumber = SHCSerialNumber.SerialNumber(),
					SoftwareVersion = SHCVersion.ApplicationVersion,
					HardwareVersion = SHCVersion.HardwareVersion,
					OSVersion = SHCVersion.OsVersion,
					IPAddress = NetworkTools.GetDeviceIp(),
					Hostname = NetworkTools.GetHostName()
				};
				UploadSysInfo(shcInformation);
				result = true;
			}
			catch (Exception ex)
			{
				Log.Error(Module.BusinessLogic, $"Failed to upload system information. See details: [{ex.Message}]");
			}
			return result;
		}
	}

	internal void UploadSysInfo(SHCInformation shcInformation)
	{
		string correlationId = Guid.NewGuid().ToString();
		ChunkWriter chunkWriter = new ChunkWriter(32768, delegate(byte[] chunk, int currentPackage, int nextPackage)
		{
			string content = Encoding.UTF8.GetString(chunk, 0, chunk.Length);
			UploadFileResponse uploadFileResponse = deviceManagementClient.UploadSystemInfo(certificateManager.PersonalCertificateThumbprint, shcSerial, content, SystemInfoType.GeneralInformation, "System information record", currentPackage, nextPackage, correlationId);
			if (uploadFileResponse != UploadFileResponse.Success)
			{
				throw new ApplicationException("Upload failed, response: " + uploadFileResponse);
			}
			return BackendPersistenceResult.Success;
		});
		RWE.SmartHome.SHC.BusinessLogic.SystemInformation.SystemInformation o = RWE.SmartHome.SHC.BusinessLogic.SystemInformation.SystemInformation.Create(configurationRepository, messagePersistence, userManagement, shcInformation, protocolSpecificDataPersistence, protocolMultiplexer);
		string text;
		using (StringWriter stringWriter = new StringWriter())
		{
			sysInfoSerializer.Serialize(stringWriter, o);
			text = stringWriter.ToString();
			stringWriter.Close();
		}
		text = text.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&");
		chunkWriter.AddUtf8String(text);
		BackendPersistenceResult backendPersistenceResult = chunkWriter.Flush();
		if (backendPersistenceResult == BackendPersistenceResult.Success)
		{
			Log.Information(Module.BusinessLogic, "Upload of system information finished.");
		}
		else
		{
			Log.Error(Module.BusinessLogic, $"Upload of system information failed with error {backendPersistenceResult}");
		}
	}

	private void PerformFactoryReset(PerformFactoryResetEventArgs args)
	{
		try
		{
			PerformFactoryReset();
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, $"Failed to perform factory reset. See details: [{ex.Message}]");
		}
	}

	private void PerformFactoryReset()
	{
		Log.Information(Module.BusinessLogic, "Performing factory reset");
		FactoryResetHandling.RequestFactoryReset();
		localUserManager.ResetToDefault();
		registrationService.ResetIsShcLocalOnlyFlag();
		PerformReset();
	}

	private void PerformReset(PerformResetEventArgs args)
	{
		try
		{
			PerformReset();
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, $"Failed to reset device. See details: [{ex.Message}]");
		}
	}

	private void PerformReset()
	{
		Log.Information(Module.BusinessLogic, "Rebooting device");
		ShutDownAndResetDevice();
	}

	private void DeviceListModified(ProtocolSpecificDataModifiedEventArgs args)
	{
		if (registrationService.IsShcLocalOnly)
		{
			Log.Information(Module.BusinessLogic, "The SHC is not registered, do not persist the device list in backend ");
		}
		else
		{
			PersistDeviceListToBackendWithRetries(5, backendPersistanceTaskId);
		}
	}

	private void PersistDeviceListToBackendWithRetries(int retriesLeft, Guid taskId)
	{
		lock (businessLogicMutex)
		{
			if (retriesLeft == 0 || taskId != backendPersistanceTaskId)
			{
				return;
			}
			if (retriesLeft == 5 && backendPersistanceTaskId != Guid.Empty)
			{
				scheduler.RemoveSchedulerTask(backendPersistanceTaskId);
			}
			if (PersistDeviceListToBackend() != BackendPersistenceResult.Success)
			{
				retriesLeft--;
				Log.Debug(Module.BusinessLogic, "DeviceList persistance to backend failed and will retry in 1 minute. Retryes left : " + retriesLeft);
				backendPersistanceTaskId = Guid.NewGuid();
				scheduler.AddSchedulerTask(new FixedTimeSpanSchedulerTask(backendPersistanceTaskId, delegate
				{
					PersistDeviceListToBackendWithRetries(retriesLeft, backendPersistanceTaskId);
				}, TimeSpan.FromMinutes(1.0), runOnce: true));
			}
		}
	}

	private BackendPersistenceResult PersistDeviceListToBackend()
	{
		if (!shcStartupCompleted)
		{
			deviceListNeedsSave = true;
			return BackendPersistenceResult.Success;
		}
		BackendPersistenceResult result;
		try
		{
			result = backendPersistence.BackupDeviceList(null);
			deviceListNeedsSave = false;
		}
		catch (Exception ex)
		{
			result = BackendPersistenceResult.ServiceFailure;
			Log.Error(Module.BusinessLogic, $"Error persisting device list: {ex.Message}");
		}
		return result;
	}

	private void OnStartupCompleted(ShcStartupCompletedEventArgs args)
	{
		lock (businessLogicMutex)
		{
			shcStartupCompleted = true;
			if (deviceListNeedsSave)
			{
				DeviceListModified(null);
			}
		}
	}

	private void ShutDownAndResetDevice()
	{
		eventManager.GetEvent<ShutdownEvent>().Publish(new ShutdownEventArgs
		{
			TimeoutMilliseconds = 60000
		});
		dbManager.Uninitialize();
		Console.WriteLine("SHC is shutting down and will be restarted.");
		ResetManager.Reset();
		Thread.Sleep(int.MaxValue);
	}

	private void PerformBackendCommunicationWithRetries(string actionText, int retries, Func<bool, bool> backendCommunicationMethod)
	{
		int tries = retries + 1;
		bool flag = tries >= 3;
		if (backendCommunicationMethod(flag))
		{
			return;
		}
		string arg = string.Format("{0} failed {1} time{2}.", actionText, tries, (tries != 1) ? "s" : string.Empty);
		if (flag)
		{
			Log.Error(Module.BusinessLogic, $"{arg} Giving up.");
			return;
		}
		int num = tries * 20;
		Log.Warning(Module.BusinessLogic, $"{arg} Retrying in {num} seconds.");
		DateTime fixedDate = ShcDateTime.Now.AddSeconds(num);
		FixedTimeAndDateSchedulerTask schedulerTask = new FixedTimeAndDateSchedulerTask(Guid.NewGuid(), delegate
		{
			PerformBackendCommunicationWithRetries(actionText, tries, backendCommunicationMethod);
		}, fixedDate);
		scheduler.AddSchedulerTask(schedulerTask);
	}

	private static string GetNetworkParameters()
	{
		string hostName = Dns.GetHostName();
		string deviceMacAddress = NetworkTools.GetDeviceMacAddress();
		string deviceIp = NetworkTools.GetDeviceIp();
		NetworkSpecifics networkSpecifics = NetworkTools.GetNetworkSpecifics();
		return $"Host name: {hostName}, IP address: {deviceIp}, MAC address: {deviceMacAddress}, subnet mask: {networkSpecifics.SubnetMask}, default gateway: {networkSpecifics.DefaultGateway}, DHCP server: {networkSpecifics.DhcpServer}, DNS server: {networkSpecifics.DnsServer}";
	}

	private string GetVersionInformation()
	{
		return $"SHC application version '{SHCVersion.ApplicationVersion}', OS version '{SHCVersion.OsVersion}', hardware version '{SHCVersion.HardwareVersion}', CoProcessor version '{coprocessorAccess.Version.ToReadable()}'";
	}

	private static string GetCurrentTimeZone()
	{
		return TimeZoneManager.GetShcTimeZoneName();
	}
}
