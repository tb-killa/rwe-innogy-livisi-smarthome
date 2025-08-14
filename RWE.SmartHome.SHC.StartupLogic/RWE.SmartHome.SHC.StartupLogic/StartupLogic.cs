using System;
using System.Text;
using System.Threading;
using Microsoft.Practices.Mobile.ContainerModel;
using Microsoft.Win32;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalDeviceKeys;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Authentication;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DisplayManagerInterfaces;
using RWE.SmartHome.SHC.DisplayManagerInterfaces.Events;
using RWE.SmartHome.SHC.ErrorHandling;
using RWE.SmartHome.SHC.StartupLogic.ErrorHandling;
using RWE.SmartHome.SHC.StartupLogicInterfaces;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.StartupLogic;

public sealed class StartupLogic : Task, IStartupLogic, IService
{
	private const string ModuleIdentifier = "StartupLogic";

	private const int SoftWareUpdateRetryWaitTime = 5;

	private const int RestoreDatabaseWaitTimeIncrement = 20;

	private const int RestoreDatabaseRetries = 3;

	private readonly ICoprocessorAccess coprocessorAccess;

	private readonly IEventManager eventManager;

	private readonly IUserManager userManagement;

	private readonly ICertificateManager certificateManager;

	private readonly Configuration configuration;

	private readonly IDisplayManager displayManager;

	private readonly IRepository uiConfigRepository;

	private readonly IShcInitializationClient initializationClient;

	private readonly IKeyExchangeClient keyExchangeClient;

	private readonly IBackendPersistence backendPersistence;

	private readonly ISoftwareUpdateProcessor softwareUpdateProcessor;

	private readonly IBusinessLogic businessLogic;

	private readonly AutoResetEvent buttonPressed = new AutoResetEvent(initialState: false);

	private readonly INetworkingMonitor networkingMonitor;

	private readonly ICoprocessorUpdater coprocessorUpdater;

	private bool ipAddressAssigned;

	private readonly IProtocolSpecificDataBackup protocolSpecificDataBackup;

	private readonly IFileLogger fileLogger;

	private readonly IRegistrationService registrationService;

	private readonly IDeviceMasterKeyRepository deviceMasterKeyRepository;

	private bool syncUsersAndRoles;

	public StartupLogic(Container container)
	{
		syncUsersAndRoles = false;
		base.Name = "StartupLogic";
		eventManager = container.Resolve<IEventManager>();
		userManagement = container.Resolve<IUserManager>();
		certificateManager = container.Resolve<ICertificateManager>();
		configuration = new Configuration(container.Resolve<IConfigurationManager>());
		displayManager = container.Resolve<IDisplayManager>();
		uiConfigRepository = container.Resolve<IRepository>();
		initializationClient = container.Resolve<IShcInitializationClient>();
		keyExchangeClient = container.Resolve<IKeyExchangeClient>();
		backendPersistence = container.Resolve<IBackendPersistence>();
		softwareUpdateProcessor = container.Resolve<ISoftwareUpdateProcessor>();
		coprocessorUpdater = container.Resolve<ICoprocessorUpdater>();
		businessLogic = container.Resolve<IBusinessLogic>();
		networkingMonitor = container.Resolve<INetworkingMonitor>();
		coprocessorAccess = container.Resolve<ICoprocessorAccess>();
		registrationService = container.Resolve<IRegistrationService>();
		protocolSpecificDataBackup = container.Resolve<IProtocolMultiplexer>().DataBackup;
		deviceMasterKeyRepository = container.Resolve<IDeviceMasterKeyRepository>();
		eventManager.GetEvent<DisplayButtonPressedEvent>().Subscribe(OnButtonPress, (DisplayButtonPressedEventArgs e) => e.Button == DisplayButton.Button1, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<OwnershipReassignmentCompletedEvent>().Subscribe(OnOwnershipReassignmentCompleted, null, ThreadOption.PublisherThread, null);
		fileLogger = container.Resolve<IFileLogger>();
		TimeSpan timeSpan = TimeSpan.FromMinutes(new Random().Next(0, 1439));
		container.Resolve<IScheduler>().AddSchedulerTask(new FixedTimeSchedulerTask(Guid.NewGuid(), ForceTimeSync, timeSpan));
		Log.Debug(Module.StartupLogic, $"Time sync daily at {timeSpan}");
		ThreadPool.SetMaxThreads(40, 500);
		ThreadPool.GetMaxThreads(out var workerThreads, out var completionPortThreads);
		Log.Information(Module.StartupLogic, $"ThreadPool limits: {workerThreads}/{completionPortThreads}");
	}

	public override void Stop()
	{
	}

	protected override void Run()
	{
		StartupMode startMode = StartupMode.General;
		FactoryResetRequestedStatus factoryResetRequestedStatus = FactoryResetHandling.WasFactoryResetRequested(checkFactoryResetButton: false);
		bool flag = true;
		bool backupDatabase = true;
		try
		{
			eventManager.GetEvent<LogSystemInformationEvent>().Publish(new LogSystemInformationEventArgs());
			WaitUntilSystemTimeIsValid();
			DateTime dateTime;
			try
			{
				dateTime = DateTime.Parse(configuration.StopBackendRequestsDate);
			}
			catch (Exception ex)
			{
				Log.Error(Module.StartupLogic, $"There was an error getting the from persistence file {ex.Message} {ex.StackTrace}");
				dateTime = DateTime.Parse("3/1/2024");
			}
			if (ShcDateTime.Now >= dateTime)
			{
				if (!registrationService.IsShcLocalOnly)
				{
					registrationService.SetIsShcLocalOnlyFlagTrue();
				}
				FilePersistence.LocalAccessEnabled = true;
			}
			if (certificateManager.PersonalCertificateThumbprint == null || (configuration.ForceRegistration ?? false))
			{
				WaitUntilConnected();
				if (factoryResetRequestedStatus != FactoryResetRequestedStatus.NotRequested)
				{
					Log.Information(Module.StartupLogic, $"Factory reset request detected but ignored since the SHC is not yet initialized");
					FactoryResetHandling.UndoFactoryResetRequest();
				}
				while (!softwareUpdateProcessor.DoUpdate(SoftwareUpdateModifier.OnlyMandatory, usePersonalCertificate: false, saveDatabase: false, suppressErrorOutput: false, downloadOnly: false))
				{
					Log.Error(Module.StartupLogic, $"Initial Software Update check failed. Retrying...");
					Thread.Sleep(5000);
				}
				PerformInitialRegistration();
				PendingUpdateTasks.DatabaseUpdateNecessary = true;
				PendingUpdateTasks.RestoreSequenceCounterNecessary = true;
				flag = false;
				backupDatabase = false;
				certificateManager.ExtractPersonalCertificateThumbprint();
			}
			if (factoryResetRequestedStatus != FactoryResetRequestedStatus.NotRequested)
			{
				ResetCoprocessor();
				WaitUntilConnected();
				startMode = StartupMode.FactoryReset;
				while (!softwareUpdateProcessor.DoUpdate(SoftwareUpdateModifier.OnlyMandatory, usePersonalCertificate: false, saveDatabase: false, suppressErrorOutput: false, downloadOnly: false))
				{
					Log.Error(Module.StartupLogic, $"Software Update check after factory reset failed. Retrying...");
					Thread.Sleep(5000);
				}
				flag = true;
				Thread thread = new Thread(PerformOwnershipReassignment);
				thread.Start();
				backupDatabase = false;
				eventManager.GetEvent<DeleteAddinsFromPersistenceFileEvent>().Publish(new DeleteAddinsFromPersistenceFileEventArgs());
			}
			if (PendingUpdateTasks.DatabaseUpdateNecessary)
			{
				startMode = StartupMode.ReloadDatabase;
			}
			UpdatePerformedStatus updatePerformedStatus = UpdatePerformedHandling.WasUpdatePerformed();
			bool success = false;
			if (updatePerformedStatus != 0)
			{
				success = FlagsEnumExtension.HasFlag(updatePerformedStatus, UpdatePerformedStatus.Successful);
				startMode = StartupMode.SoftwareUpdate;
				flag = false;
				backupDatabase = false;
				if (!PendingUpdateTasks.DatabaseUpdateNecessary)
				{
					LogSoftwareUpdateResult(updatePerformedStatus, success);
					PendingUpdateTasks.DatabaseUpdateNecessary = true;
				}
			}
			HandleStartupTasks(startMode, success);
			if (!deviceMasterKeyRepository.IsMasterExportKeyAlreadyCreated())
			{
				keyExchangeClient.GetMasterKey(certificateManager.PersonalCertificateThumbprint, out var masterKey);
				deviceMasterKeyRepository.StoreMasterKey(masterKey);
			}
			FilePersistence.LocalAccessEnabled = true;
			bool flag2 = false;
			flag2 = coprocessorUpdater.DoUpdate();
			if (PendingUpdateTasks.RestoreSequenceCounterNecessary)
			{
				if (!flag2)
				{
					protocolSpecificDataBackup.Restore(restoreDefaults: false);
				}
				PendingUpdateTasks.RestoreSequenceCounterNecessary = false;
			}
			PendingUpdateTasks.DatabaseUpdateNecessary = false;
			if (flag)
			{
				if (!softwareUpdateProcessor.DoUpdate(SoftwareUpdateModifier.OnlyMandatory, usePersonalCertificate: true, backupDatabase, suppressErrorOutput: true, downloadOnly: false))
				{
					Log.Warning(Module.StartupLogic, "Unable to perform mandatory update check - postponing for 3 minutes");
					new Thread((ThreadStart)delegate
					{
						Log.Debug(Module.StartupLogic, "Spawning retry thread...");
						Thread.Sleep(180000);
						Log.Debug(Module.StartupLogic, "Retrying software update postponed from startup...");
						if (!softwareUpdateProcessor.DoUpdate(SoftwareUpdateModifier.OnlyMandatory, usePersonalCertificate: true, backupDatabase, suppressErrorOutput: true, downloadOnly: false))
						{
							Log.Error(Module.StartupLogic, "Update check during startup failed, giving up...");
						}
					}).Start();
				}
				UpdatePerformedHandling.ReleaseUpdatePerformedState();
			}
			if (registrationService.RemotelyRegistered || registrationService.IsShcLocalOnly)
			{
				registrationService.PublishShcStartupEvents();
			}
			eventManager.GetEvent<StartLocalCommunicationServerEvent>().Publish(new StartLocalCommunicationServerEventArgs());
			businessLogic.BegingStopBackendServicesScheduler(configuration.StopBackendRequestsDate);
		}
		catch (Exception ex2)
		{
			if (ex2 is ShcException { ErrorCode: 5 })
			{
				syncUsersAndRoles = true;
			}
			Log.Error(Module.StartupLogic, $"SHC startup aborted due to the following error: {ex2.Message} with details: {ex2}");
			displayManager.WorkflowFailed(Workflow.Startup, WorkflowError.SystemStartupError);
			while (true)
			{
				Thread.Sleep(1000);
			}
		}
	}

	private void ResetCoprocessor()
	{
		Log.Information(Module.StartupLogic, "Performing factory reset of the coprocessor...");
		coprocessorAccess.ResetCoprocessor();
		Log.Information(Module.StartupLogic, "Factory reset of the coprocessor finished");
	}

	private void WaitUntilSystemTimeIsValid()
	{
		bool flag = true;
		int num = 0;
		ForceTimeSync();
		while (!Ntp.AssertSystemTimeValidity())
		{
			if (flag)
			{
				if (!networkingMonitor.InternetAccessAllowed)
				{
					Log.Error(Module.StartupLogic, "Internet connectivity switch is currently set to off. NTP synchronization will not be forced.");
				}
				string message = $"System time not valid. Will check every {500} ms{(networkingMonitor.InternetAccessAllowed ? $" and force an NTP update every {10000} ms" : string.Empty)}.";
				Log.Error(Module.StartupLogic, message);
				ForceTimeSync();
				flag = false;
			}
			num++;
			if (num > 20)
			{
				WorkflowError? workflowError = (networkingMonitor.InternetAccessAllowed ? ToWorkflowError() : new WorkflowError?(WorkflowError.NetworkAdapterNotOperational));
				if (!workflowError.HasValue)
				{
					workflowError = WorkflowError.NtpUnavailable;
				}
				displayManager.WorkflowFailed(Workflow.Startup, workflowError.Value);
				ForceTimeSync();
				num = 0;
			}
			Thread.Sleep(500);
		}
		if (!flag)
		{
			Log.Information(Module.StartupLogic, "System time is now valid.");
			displayManager.WorkflowFinished(Workflow.Startup);
		}
	}

	private void WaitUntilConnected()
	{
		if (ipAddressAssigned)
		{
			return;
		}
		bool flag = true;
		while (!networkingMonitor.InternetAccessAllowed || NetworkTools.GetDeviceIp() == "127.0.0.1")
		{
			StringBuilder stringBuilder = new StringBuilder("Network access is not available. ");
			WorkflowError? workflowError;
			if (networkingMonitor.InternetAccessAllowed)
			{
				workflowError = ToWorkflowError("rwe-smarthome.de");
				if (workflowError.HasValue)
				{
					stringBuilder.AppendFormat("Error: {0}.", new object[1] { workflowError.Value });
				}
				else
				{
					workflowError = WorkflowError.NetworkAdapterNotOperational;
					stringBuilder.Append("Unknown error.");
				}
			}
			else
			{
				stringBuilder.Append("The internet access switch is disabled.");
				workflowError = WorkflowError.NetworkAdapterNotOperational;
			}
			if (flag)
			{
				stringBuilder.AppendFormat(" Retrying every {0} seconds.", new object[1] { 10 });
				displayManager.WorkflowFailed(Workflow.Startup, workflowError.Value);
				Log.Warning(Module.StartupLogic, stringBuilder.ToString());
				flag = false;
			}
			Thread.Sleep(10000);
		}
		ipAddressAssigned = true;
		if (!flag)
		{
			displayManager.WorkflowFinished(Workflow.Startup);
			Log.Information(Module.StartupLogic, "Network access restored. Forcing time synchronization.");
			Thread.Sleep(1000);
			ForceTimeSync();
			Thread.Sleep(1000);
			Log.Information(Module.StartupLogic, "After forced time synchronization.");
		}
	}

	private void ForceTimeSync()
	{
		if (networkingMonitor.InternetAccessAllowed)
		{
			if (Ntp.ForceTimeSync())
			{
				Log.Information(Module.StartupLogic, "Time synchronization started successfully...");
			}
			else
			{
				Log.Error(Module.StartupLogic, "Time synchronization failed to start!");
			}
		}
		else
		{
			Log.Information(Module.StartupLogic, "Network switch is off, can't synchronize time.");
		}
	}

	private static WorkflowError? ToWorkflowError()
	{
		string[] array;
		using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Services"))
		{
			using RegistryKey registryKey2 = registryKey.OpenSubKey("TIMESVC");
			array = registryKey2.GetValue("server") as string[];
		}
		string host = ((array == null) ? string.Empty : array[0]);
		return ToWorkflowError(host);
	}

	private static WorkflowError? ToWorkflowError(string host)
	{
		return NetworkTools.DiagnoseNetworkProblem(host, null) switch
		{
			NetworkProblem.NetworkAdapterNotOperational => WorkflowError.NetworkAdapterNotOperational, 
			NetworkProblem.NoDhcpIpAddress => WorkflowError.NoDhcpIpAddress, 
			NetworkProblem.NoDhcpDefaultGateway => WorkflowError.NoDhcpDefaultGateway, 
			NetworkProblem.NameResolutionFailed => WorkflowError.NameResolutionFailed, 
			NetworkProblem.NameResolutionFailedNetworkDown => WorkflowError.NameResolutionFailedNetworkDown, 
			_ => null, 
		};
	}

	private void LogSoftwareUpdateResult(UpdatePerformedStatus state, bool success)
	{
		string arg = (FlagsEnumExtension.HasFlag(state, UpdatePerformedStatus.ApplicationOnly) ? "application only" : "full image");
		string arg2 = (FlagsEnumExtension.HasFlag(state, UpdatePerformedStatus.Controlled) ? "Controlled" : "Uncontrolled");
		string arg3 = (success ? "was successful" : "has failed");
		string text = $"{arg2} {arg} update {arg3}.";
		if (!success && !FlagsEnumExtension.HasFlag(state, UpdatePerformedStatus.ApplicationOnly) && FlagsEnumExtension.HasFlag(state, UpdatePerformedStatus.Controlled))
		{
			text += " Operating system version after update has not changed.";
		}
		if (success)
		{
			Log.Information(Module.StartupLogic, text);
		}
		else
		{
			Log.Error(Module.StartupLogic, text);
		}
	}

	private void HandleStartupTasks(StartupMode startMode, bool success)
	{
		switch (startMode)
		{
		case StartupMode.General:
			uiConfigRepository.LoadAllEntities();
			Log.Information(Module.StartupLogic, "Optional synchronization of users and roles: Triggered");
			eventManager.GetEvent<SyncUsersEvent>().Publish(new SyncUsersEventArgs());
			break;
		case StartupMode.SoftwareUpdate:
			WaitUntilConnected();
			syncUsersAndRoles = true;
			if (!registrationService.IsShcLocalOnly)
			{
				RestoreDatabase(syncUsersAndRoles);
			}
			if (softwareUpdateProcessor.AnnounceUpdate())
			{
				Log.Debug(Module.StartupLogic, "Announced software update to the backend.");
			}
			else
			{
				Log.Warning(Module.StartupLogic, "Failed to announce software update. Will not restore configuration.");
			}
			HandleSoftwareUpdate(success);
			break;
		case StartupMode.ReloadDatabase:
			WaitUntilConnected();
			RestoreDatabase(syncUsersAndRoles);
			break;
		case StartupMode.FactoryReset:
			Log.Information(Module.StartupLogic, "Started in Factory Reset mode");
			break;
		default:
			throw new ArgumentOutOfRangeException("startMode");
		}
	}

	private void HandleSoftwareUpdate(bool success)
	{
		eventManager.GetEvent<SoftwareUpdateProgressEvent>().Publish(new SoftwareUpdateProgressEventArgs(success ? SoftwareUpdateState.Success : SoftwareUpdateState.Failed));
		if (success)
		{
			while (!softwareUpdateProcessor.DoUpdate(SoftwareUpdateModifier.OnlyMandatory, usePersonalCertificate: true, saveDatabase: false, suppressErrorOutput: false, downloadOnly: false))
			{
				Log.Error(Module.StartupLogic, "Cascaded Software Update check failed. Retrying...");
				Thread.Sleep(5000);
			}
			displayManager.WorkflowFinished(Workflow.ShcSoftwareUpdate);
		}
		UpdatePerformedHandling.ReleaseUpdatePerformedState();
	}

	private void RestoreDatabase(bool syncUsersAndRoles)
	{
		if (syncUsersAndRoles)
		{
			RestoreDatabaseStepWithRetries(businessLogic.SyncUsersAndRoles, ErrorStrings.FailedToSyncUsersAndRoles, ErrorCode.FailedToSyncUsersAndRoles, "[UsersAndRoles] successfully restored from backend");
		}
		RestoreDatabaseStepWithRetries(backendPersistence.RestoreDeviceList, ErrorStrings.FailedToRestoreDeviceList, ErrorCode.FailedToRestoreDeviceList, "[DeviceList] successfully restored from backend");
		RestoreDatabaseStepWithRetries(backendPersistence.RestoreTechnicalConfiguration, ErrorStrings.FailedToRestoreTechnicalConfiguration, ErrorCode.FailedToRestoreTechnicalConfiguration, "[TechnicalConfiguration] successfully restored from backend");
		RestoreDatabaseStepWithRetries(backendPersistence.RestoreUIConfiguration, ErrorStrings.FailedToRestoreUiConfiguration, ErrorCode.FailedToRestoreUiConfiguration, "[UIConfiguration] successfully restored from backend");
		RestoreDatabaseStepWithRetries(backendPersistence.RestoreMessagesAndAlerts, ErrorStrings.FailedToRestoreMessageAndAlerts, ErrorCode.FailedToRestoreMessageAndAlerts, "[MessagesAndAlerts] successfully restored from backend");
		RestoreDatabaseStepWithRetries(backendPersistence.RestoreCustomApplicationsSettings, ErrorStrings.FailedToRestoreCustomApplicationsData, ErrorCode.FailedToRestoreCustomApplicationsData, "[CustomAppsData] successfully restored from backend");
		RestoreLogFromFilesystem();
	}

	private void RestoreLogFromFilesystem()
	{
		fileLogger.RestoreLegacyLog("\\NandFlash\\logStore");
		fileLogger.RestoreLogFrom("\\NandFlash\\");
		Log.Information(Module.StartupLogic, "[Log] successfully restored from local file");
	}

	private void RestoreDatabaseStepWithRetries(Func<bool> stepAction, string errorMessage, ErrorCode errorCode, string successMessage)
	{
		int num = 0;
		bool flag;
		do
		{
			if (num != 0)
			{
				if (num >= 3)
				{
					throw new ShcException(errorMessage, "StartupLogic", (int)errorCode);
				}
				int num2 = num * 20;
				Log.Warning(Module.StartupLogic, $"{errorMessage} Will retry in {num2} seconds.");
				Thread.Sleep(num2 * 1000);
			}
			flag = stepAction();
			num++;
		}
		while (!flag);
		Log.Information(Module.StartupLogic, successMessage);
	}

	private void PerformInitialRegistration()
	{
		InitialRegistration initialRegistration = new InitialRegistration(userManagement, displayManager, initializationClient, configuration, certificateManager);
		PerformWithRetries(initialRegistration.PerformIntialRegistration, 20000, TimeSpan.FromMinutes(10.0));
	}

	private void PerformOwnershipReassignment()
	{
		OwnershipReassignment ownershipReassignment = new OwnershipReassignment(userManagement, displayManager, initializationClient, configuration, certificateManager, registrationService, eventManager);
		PerformWithRetries(ownershipReassignment.PerformOwnershipReassignment, 20000, TimeSpan.FromMinutes(10.0));
	}

	private void OnOwnershipReassignmentCompleted(OwnershipReassignmentCompletedEventArgs eventArgs)
	{
		RestoreDatabase(eventArgs.SyncUsersAndRoles);
		protocolSpecificDataBackup.Restore(restoreDefaults: false);
	}

	public void Initialize()
	{
		AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
	}

	public void Uninitialize()
	{
		AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
	}

	private void PerformWithRetries(Func<bool> method, int waitTime, TimeSpan maxDuration)
	{
		bool flag = false;
		DateTime utcNow = DateTime.UtcNow;
		while (!flag)
		{
			bool flag2 = false;
			try
			{
				flag = method();
				flag2 = !flag;
			}
			catch (Exception ex)
			{
				Log.Error(Module.StartupLogic, ex.Message);
				flag = false;
				Thread.Sleep(waitTime);
				flag2 = DateTime.UtcNow.Subtract(utcNow) >= maxDuration;
			}
			if (flag2)
			{
				Log.Information(Module.StartupLogic, "Waiting for button press");
				buttonPressed.Reset();
				buttonPressed.WaitOne();
				utcNow = DateTime.UtcNow;
			}
		}
	}

	private void OnButtonPress(DisplayButtonPressedEventArgs args)
	{
		buttonPressed.Set();
	}

	private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
	{
		Log.Error(Module.StartupLogic, "Unhandled error: " + args.ExceptionObject, isPersisted: true);
		Thread.Sleep(1000);
		eventManager.GetEvent<PerformResetEvent>().Publish(new PerformResetEventArgs());
	}
}
