using System;
using System.IO;
using System.Net;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalCommunication;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalDeviceKeys;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Authentication;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.DataAccessInterfaces.Applications;
using RWE.SmartHome.SHC.DataAccessInterfaces.DeviceActivityLogging;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.ModelTransformationService;
using SmartHome.Common.Generic.LogManager;
using WebServerHost;
using WebServerHost.Services;
using WebServerHost.Web;
using WebServerHost.Web.Routing;

namespace RWE.SmartHome.SHC.WebServerHost;

internal class WebServerHost : Task, IService, ICommunicationChannel, IBaseChannel
{
	private readonly WebServerHostApp webApp;

	private IRequestProcessor requestProcessor;

	private ILocalUserManager userManager;

	private IUserManager remoteUserManager;

	private ITrackDataPersistence trackDataStorage;

	private IApplicationsSettings appSettings;

	private IApplicationsTokenPersistence appTokenPersistence;

	private IAddinRepoManager addinRepoManager;

	private IUtilityDataPersistence utilityPersistence;

	private IRepository repository;

	private ILogicalDeviceStateRepository statesRepository;

	private IEventManager eventManager;

	private INetworkingMonitor netMonitor;

	private IConfigurationManager configManager;

	private IRegistrationService registrationService;

	private IApplicationsHost applicationsHost;

	private IEmailSender emailSender;

	private IDeviceMasterKeyRepository masterKeyRepository;

	private IDeviceKeyRepository deviceKeyRepository;

	public string ChannelId => "Core.Local";

	public ChannelType ChannelType => ChannelType.Local;

	public bool Connected => webApp.IsRunning;

	public WebServerHost(Container container)
	{
		base.Name = "WebServiceHost";
		webApp = new WebServerHostApp();
		LogManager.OnLogMessage += LogManager_OnLogMessage;
		userManager = container.Resolve<ILocalUserManager>();
		remoteUserManager = container.Resolve<IUserManager>();
		trackDataStorage = container.Resolve<ITrackDataPersistence>();
		appSettings = container.Resolve<IApplicationsSettings>();
		appTokenPersistence = container.Resolve<IApplicationsTokenPersistence>();
		addinRepoManager = container.Resolve<IAddinRepoManager>();
		utilityPersistence = container.Resolve<IUtilityDataPersistence>();
		repository = container.Resolve<IRepository>();
		statesRepository = container.Resolve<ILogicalDeviceStateRepository>();
		eventManager = container.Resolve<IEventManager>();
		netMonitor = container.Resolve<INetworkingMonitor>();
		configManager = container.Resolve<IConfigurationManager>();
		registrationService = container.Resolve<IRegistrationService>();
		applicationsHost = container.Resolve<IApplicationsHost>();
		emailSender = container.Resolve<IEmailSender>();
		masterKeyRepository = container.Resolve<IDeviceMasterKeyRepository>();
		deviceKeyRepository = container.Resolve<IDeviceKeyRepository>();
	}

	public void Initialize()
	{
		eventManager.GetEvent<ShcRebootScheduledEvent>().Subscribe(OnReboot, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<PerformFactoryResetEvent>().Subscribe(OnFactoryReset, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<StartLocalCommunicationServerEvent>().Subscribe(OnStartLocalCommunication, null, ThreadOption.BackgroundThread, null);
		eventManager.GetEvent<SoftwareUpdateProgressEvent>().Subscribe(OnSwUpdateProgress, null, ThreadOption.BackgroundThread, null);
		eventManager.GetEvent<DeleteAllLocalDataEvent>().Subscribe(OnDeleteAllLocalData, null, ThreadOption.BackgroundThread, null);
		Log.Information(Module.WebServerHost, "Web Server Initializing ...");
		ConfigureServer(webApp.ServerConfiguration);
		ConfigureServices(webApp.ServicesProvider);
		webApp.UseStaticResources();
		webApp.UseAuth("/auth");
		webApp.UseWebSockets("/events");
		webApp.UseRouting("/data", delegate(EndpointRoutes endpoints)
		{
			endpoints.MapControllers("WebServerHost.Controllers.Data");
		});
		webApp.UseRouting("/", delegate(EndpointRoutes endpoints)
		{
			endpoints.MapControllers("WebServerHost.Controllers.ClientAPI");
			endpoints.MapControllers("WebServerHost.Controllers.Account");
		});
		CheckManualReset();
		Log.Information(Module.WebServerHost, "Web Server Initialized");
	}

	private void ConfigureServer(WebServerConfiguration webConf)
	{
		webConf.IPAddress = IPAddress.Any;
		webConf.Port = 80;
		webConf.ApplicationPort = 8080;
		webConf.ServerRoot = "\\Nandflash\\SHC\\WWWRoot";
		webConf.AddDefaultFile("index.html");
		webConf.AddSpecialFileType(".cgi");
		try
		{
			webConf.ServerName = Dns.GetHostName();
		}
		catch
		{
		}
	}

	private void ConfigureServices(ServiceProvider services)
	{
		services.AddSingleton(requestProcessor);
		services.AddSingleton(userManager);
		services.AddSingleton(remoteUserManager);
		services.AddSingleton(trackDataStorage);
		services.AddSingleton(appSettings);
		services.AddSingleton(appTokenPersistence);
		services.AddSingleton(addinRepoManager);
		services.AddSingleton(utilityPersistence);
		services.AddSingleton(repository);
		services.AddSingleton(statesRepository);
		services.AddSingleton(netMonitor);
		services.AddSingleton(configManager);
		services.AddSingleton(eventManager);
		services.AddSingleton(registrationService);
		services.AddSingleton(applicationsHost);
		services.AddSingleton(emailSender);
		services.AddSingleton(masterKeyRepository);
		services.AddSingleton(deviceKeyRepository);
		services.AddSingleton((IShcClient)new ShcClient(requestProcessor));
		services.AddSingleton((IAuthorization)new AuthorizationService(userManager, configManager, services.Get<IRegistrationService>()));
		services.AddSingleton((IUserStorageService)new UserStorageService());
		services.Add<IProductsService, ProductsService>();
		services.Add<IUtilityDataService, UtilityDataService>();
		services.Add<IImageService, ImageService>();
		services.Add<IEmailService, EmailService>();
		services.Add<IRebootService, RebootService>();
		services.Add<IDeviceKeysService, DeviceKeysService>();
		services.Add<NotificationService>();
		services.Add<IConversionContext, ConversionContext>();
		services.Add<ILocationConverterService, LocationConverterService>();
		services.Add<ICapabilityConverterService, CapabilityConverterService>();
		services.Add<IDeviceConverterService, DeviceConverterService>();
		services.Add<IEventConverterService, EventConverterService>();
		services.Add<IHomeConverterService, HomeConverterService>();
		services.Add<IHomeSetupConverterService, HomeSetupConverterService>();
		services.Add<IStatusConverterService, StatusConverterService>();
		services.Add<IInteractionConverterService, InteractionConverterService>();
		services.Add<IActionConverterService, ActionConverterService>();
		services.Add<IMessageConverterService, MessageConverterService>();
	}

	private void OnReboot(ShcRebootScheduledEventArgs args)
	{
		Stop();
	}

	private void OnFactoryReset(PerformFactoryResetEventArgs args)
	{
		webApp.ServicesProvider.Get<IAuthorization>()?.InvalidateTokens();
		ServiceProvider.Services.Get<IImageService>()?.DeleteAllImages();
		webApp.ServicesProvider.Get<NotificationService>()?.NotifyFactoryReset();
		DeleteEmailSettings();
	}

	private void DeleteEmailSettings()
	{
		FilePersistence.EmailSettings = null;
	}

	private void OnStartLocalCommunication(StartLocalCommunicationServerEventArgs args)
	{
		Log.Information(Module.WebServerHost, "OnStartup(): Starting webserver...");
		webApp.Run();
	}

	private void OnSwUpdateProgress(SoftwareUpdateProgressEventArgs args)
	{
		Log.Debug(Module.WebServerHost, $"Sw update progress event: {args}");
		if (args.State == SoftwareUpdateState.Started)
		{
			Log.Information(Module.WebServerHost, "Stoping webserver...");
			webApp.Stop();
		}
		else
		{
			Log.Information(Module.WebServerHost, "Starting webserver...");
			webApp.Run();
		}
	}

	private void CheckManualReset()
	{
		if (!File.Exists("\\NandFlash\\ManualReseted"))
		{
			return;
		}
		try
		{
			FilePersistence.InteractionsVerified = false;
			IRegistrationService registrationService = webApp.ServicesProvider.Get<IRegistrationService>();
			if (registrationService != null)
			{
				registrationService.ResetTaC();
				registrationService.ResetIsShcLocalOnlyFlag();
			}
			userManager.ResetToDefault();
		}
		catch (Exception arg)
		{
			Log.Warning(Module.WebServerHost, $"Check manual reset error occurred {arg}");
		}
		finally
		{
			File.Delete("\\NandFlash\\ManualReseted");
		}
	}

	public void Uninitialize()
	{
		Log.Information(Module.WebServerHost, "Web Server Uninitializing ...");
		Stop();
		eventManager.GetEvent<ShcRebootScheduledEvent>().Unsubscribe(OnReboot);
		eventManager.GetEvent<PerformFactoryResetEvent>().Unsubscribe(OnFactoryReset);
	}

	protected override void Run()
	{
	}

	public override void Stop()
	{
		webApp.Stop();
	}

	public void SubscribeRequestProcessor(IRequestProcessor processor)
	{
		requestProcessor = processor;
	}

	public void QueueNotification(BaseNotification notification)
	{
		Log.Debug(Module.WebServerHost, $"New notification: {notification.GetType()}");
		if (notification is GenericNotification genericNotification && genericNotification.EventType.EqualsIgnoreCase("UtilityRead"))
		{
			Log.Debug(Module.WebServerHost, $"EventType: {genericNotification.EventType}");
			webApp.ServicesProvider.Get<IUtilityDataService>()?.HandleUtilityReadNotification(genericNotification);
		}
		else if (webApp.IsRunning)
		{
			if (notification is LogoutNotification { Reason: LogoutNotificationReason.FactoryReset })
			{
				IRegistrationService registrationService = webApp.ServicesProvider.Get<IRegistrationService>();
				registrationService.ResetTaC();
			}
			webApp.ServicesProvider.Get<NotificationService>()?.HandleNotification(notification);
		}
		else
		{
			Log.Debug(Module.WebServerHost, "Webserver is stopped, dropping websocket message");
		}
	}

	private void LogManager_OnLogMessage(Type logType, LogLevel logLevel, string logMessage)
	{
		switch (logLevel)
		{
		case LogLevel.Debug:
			Log.Debug(Module.WebServerHost, logMessage);
			break;
		case LogLevel.Infomration:
			Log.Information(Module.WebServerHost, logMessage);
			break;
		case LogLevel.Warning:
			Log.Warning(Module.WebServerHost, logMessage);
			break;
		case LogLevel.Error:
			Log.Error(Module.WebServerHost, logMessage);
			break;
		}
	}

	private void OnDeleteAllLocalData(DeleteAllLocalDataEventArgs eventArgs)
	{
		IImageService imageService = ServiceProvider.Services.Get<IImageService>();
		imageService.DeleteAllImages();
		using (File.Create("\\NandFlash\\DeleteDalDatabaseFileFlag"))
		{
		}
		eventManager.GetEvent<PerformResetEvent>().Publish(new PerformResetEventArgs());
	}
}
