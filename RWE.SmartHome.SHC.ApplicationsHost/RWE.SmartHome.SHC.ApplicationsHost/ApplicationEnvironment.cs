using RWE.SmartHome.SHC.ApplicationsHost.Configuration;
using RWE.SmartHome.SHC.ApplicationsHost.CustomTriggers;
using RWE.SmartHome.SHC.ApplicationsHost.Events;
using RWE.SmartHome.SHC.ApplicationsHost.Logging;
using RWE.SmartHome.SHC.ApplicationsHost.Messaging;
using RWE.SmartHome.SHC.ApplicationsHost.Settings;
using RWE.SmartHome.SHC.ApplicationsHost.Storage;
using RWE.SmartHome.SHC.ApplicationsHost.SystemInformation;
using RWE.SmartHome.SHC.ApplicationsHost.SystemServices;
using RWE.SmartHome.SHC.ApplicationsHost.TaskScheduler;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.DataAccessInterfaces.Applications;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;
using SmartHome.SHC.API;
using SmartHome.SHC.API.Configuration;
using SmartHome.SHC.API.Configuration.Services;
using SmartHome.SHC.API.DeviceActivityLogging;
using SmartHome.SHC.API.Events;
using SmartHome.SHC.API.Logging;
using SmartHome.SHC.API.Messaging;
using SmartHome.SHC.API.Settings;
using SmartHome.SHC.API.Storage;
using SmartHome.SHC.API.SystemServices;
using SmartHome.SHC.API.TaskScheduler;

namespace RWE.SmartHome.SHC.ApplicationsHost;

internal class ApplicationEnvironment : ICoreServices
{
	private readonly ApplicationsMessageHandler applicationMessagesHandler;

	private readonly IEventService applicationEventService;

	private readonly ApplicationStorage applicationStorage;

	private readonly FilesystemStorage fileSystemStorage;

	private readonly ILogger applicationLogger;

	private readonly ConfigurationProvider configurationProvider;

	private readonly ISettingsProvider settingsProvider;

	private readonly ISystemInformation shcSystemInformation;

	private readonly RWE.SmartHome.SHC.ApplicationsHost.TaskScheduler.TaskScheduler taskScheduler;

	private readonly ISystemServices systemServices;

	private readonly IActivityLoggingService activityLoggingService;

	private readonly IEventManager eventManager;

	private readonly ICustomTriggerServices customTriggerServices;

	IIsolatedStorage ICoreServices.ApplicationStorage => applicationStorage;

	IFilesystemStorage ICoreServices.FilesystemStorage => fileSystemStorage;

	IMessageService ICoreServices.ApplicationMessages => applicationMessagesHandler;

	IEventService ICoreServices.EventsService => applicationEventService;

	ILogger ICoreServices.ApplicationLogger => applicationLogger;

	IConfigurationProvider ICoreServices.ConfigurationProvider => configurationProvider;

	ISettingsProvider ICoreServices.SettingsProvider => settingsProvider;

	ISystemInformation ICoreServices.ShcSystemInformation => shcSystemInformation;

	ITaskScheduler ICoreServices.TaskScheduler => taskScheduler;

	ISystemServices ICoreServices.SystemServices => systemServices;

	IActivityLoggingService ICoreServices.ActivityLoggingService => activityLoggingService;

	ICustomTriggerServices ICoreServices.CustomTriggerServices => customTriggerServices;

	internal ApplicationEnvironment(string applicationId, string applicationVersion, IApplicationsSettings settingsPersistence, IExternalCommandDispatcher externalCommandDispatcher, INetworkingMonitor networkMonitor, ICertificateManager certificateManager, IRepository configurationRepository, IRepositorySync repositorySync, IEventManager eventManager, string appFileName, AddinsConfigurationValidator addinsConfigurationValidator, IMessagesAndAlertsManager messagesAndAlerts, IConfigurationValidation addinConfigurationValidator, AddinsConfigurationRepository addinsConfigurationRepository, IScheduler scheduler)
	{
		applicationMessagesHandler = new ApplicationsMessageHandler(applicationId, applicationVersion, messagesAndAlerts);
		applicationEventService = new ApplicationEventsHandler(externalCommandDispatcher, applicationId, applicationVersion);
		applicationStorage = new ApplicationStorage(settingsPersistence, applicationId);
		fileSystemStorage = new FilesystemStorage(applicationId);
		applicationLogger = new ApplicationLogger(applicationId);
		configurationProvider = new ConfigurationProvider(applicationId, configurationRepository, repositorySync, addinsConfigurationValidator, addinConfigurationValidator, addinsConfigurationRepository);
		settingsProvider = new SettingsProvider(applicationId, appFileName);
		shcSystemInformation = new ShcSystemInformation(certificateManager, networkMonitor, eventManager, configurationRepository);
		taskScheduler = new RWE.SmartHome.SHC.ApplicationsHost.TaskScheduler.TaskScheduler(applicationId);
		systemServices = new RWE.SmartHome.SHC.ApplicationsHost.SystemServices.SystemServices(certificateManager, scheduler);
		activityLoggingService = new ActivityLoggingService(applicationId, configurationRepository);
		customTriggerServices = new CustomTriggerServices(eventManager);
		this.eventManager = eventManager;
	}

	public void Uninitialize()
	{
		configurationProvider.Uninitialize();
		taskScheduler.Uninitialize();
	}

	public void Uninstall()
	{
		Uninitialize();
		applicationStorage.Uninstall();
		fileSystemStorage.Uninstall();
		applicationMessagesHandler.Uninstall();
	}
}
