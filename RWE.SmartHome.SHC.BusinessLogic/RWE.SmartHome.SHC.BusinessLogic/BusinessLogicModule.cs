using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BusinessLogic.ConfigurationTransformation;
using RWE.SmartHome.SHC.BusinessLogic.CoprocessorUpdate;
using RWE.SmartHome.SHC.BusinessLogic.CoreDevices;
using RWE.SmartHome.SHC.BusinessLogic.DeleteEntitiesRequestValidation;
using RWE.SmartHome.SHC.BusinessLogic.DeviceFirmwareUpdate;
using RWE.SmartHome.SHC.BusinessLogic.DeviceInclusion;
using RWE.SmartHome.SHC.BusinessLogic.DeviceMonitoring;
using RWE.SmartHome.SHC.BusinessLogic.IntegrityManagement;
using RWE.SmartHome.SHC.BusinessLogic.LocalCommunication;
using RWE.SmartHome.SHC.BusinessLogic.LocalDeviceKeys;
using RWE.SmartHome.SHC.BusinessLogic.LogicalDeviceStateRepository;
using RWE.SmartHome.SHC.BusinessLogic.MessagesAndAlerts;
using RWE.SmartHome.SHC.BusinessLogic.Persistence;
using RWE.SmartHome.SHC.BusinessLogic.Persistence.Backups;
using RWE.SmartHome.SHC.BusinessLogic.ProtocolMultiplexer;
using RWE.SmartHome.SHC.BusinessLogic.RepositoryOperations;
using RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation;
using RWE.SmartHome.SHC.BusinessLogic.ShcSecurityNotifications;
using RWE.SmartHome.SHC.BusinessLogic.SoftwareUpdate;
using RWE.SmartHome.SHC.BusinessLogic.USBLogExporter;
using RWE.SmartHome.SHC.BusinessLogic.VirtualDevices;
using RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.HomeSetupEntity;
using RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.NotificationSender;
using RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.SHCBaseDevice;
using RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration.DevicesDefinitions;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeleteEntitiesRequestValidation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceInclusion;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceMonitoring;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.IntegrityManagement;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalCommunication;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalDeviceKeys;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ShcSecurityNotifications;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.Database;
using RWE.SmartHome.SHC.Core.FileDownload;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.TypeManager;
using RWE.SmartHome.SHC.DataAccessInterfaces.Applications;
using RWE.SmartHome.SHC.DeviceFirmware.Reinclusion;
using RWE.SmartHome.SHC.DisplayManagerInterfaces;
using RWE.SmartHome.SHC.DomainModel.Constants;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.BusinessLogic;

public class BusinessLogicModule : IModule
{
	private DeviceConfigurationSupport deviceConfigurationSupport;

	private VrccStateHandler vrccStateHandler;

	public void Configure(Container container)
	{
		object businessLogicMutex = new object();
		bool areBackendRequestsAvailable = SettingsFileHelper.ShouldRegisterBackendRequests();
		try
		{
			IEventManager eventManager = container.Resolve<IEventManager>();
			container.Register((Func<Container, IRepositorySync>)((Container c) => new RepositorySync(c.Resolve<IRepository>(), c.Resolve<IConfigurationProcessor>(), c.Resolve<IEventManager>())));
			eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(delegate
			{
				OnShcStartupCompleted(container);
			}, (ShcStartupCompletedEventArgs x) => x.Progress == StartupProgress.CompletedRound1, ThreadOption.PublisherThread, null);
			container.Register((Func<Container, IBackendPersistence>)((Container c) => new BackendPersistence(c, businessLogicMutex))).ReusedWithin(ReuseScope.Container);
			container.Resolve<IBackendPersistence>();
			ShcBaseDeviceWatchers watchersList = new ShcBaseDeviceWatchers();
			RFCommFailureNotificationHandler bdUpdateHandler = new RFCommFailureNotificationHandler(container.Resolve<IRepository>());
			container.Register((Func<Container, IShcBaseDeviceWatchers>)((Container c) => watchersList)).ReusedWithin(ReuseScope.Container);
			container.Register((Func<Container, IRFCommFailureNotificationHandler>)((Container c) => bdUpdateHandler)).ReusedWithin(ReuseScope.Container);
			container.Resolve<IRFCommFailureNotificationHandler>();
			IConfigurationManager configurationManager = container.Resolve<IConfigurationManager>();
			Configuration configuration = new Configuration(configurationManager);
			container.Register((Func<Container, ISoftwareUpdateProcessor>)delegate(Container c)
			{
				SoftwareUpdateProcessor softwareUpdateProcessor = new SoftwareUpdateProcessor(c.Resolve<IScheduler>(), c.Resolve<IEventManager>(), c.Resolve<ICertificateManager>(), c.Resolve<ISoftwareUpdateClient>(), c.Resolve<IBackendPersistence>(), configuration, c.Resolve<IDisplayManager>(), businessLogicMutex, c.Resolve<IRepository>(), c.Resolve<IRegistrationService>());
				container.Resolve<ITaskManager>().Register(softwareUpdateProcessor);
				return softwareUpdateProcessor;
			}).InitializedBy(delegate(Container c, ISoftwareUpdateProcessor v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			container.Resolve<ISoftwareUpdateProcessor>();
			container.Register((Func<Container, IMemoryWatchdog>)((Container c) => new MemoryWatchdog(c.Resolve<IScheduler>(), c.Resolve<IEventManager>(), configuration)));
			container.Resolve<IMemoryWatchdog>();
			container.Register((Func<Container, IExportDevicesKeysEmailSenderScheduler>)((Container c) => new ExportDevicesKeysEmailSenderScheduler(c.Resolve<IEventManager>(), configuration, c.Resolve<IScheduler>(), c.Resolve<INotificationServiceClient>())));
			container.Register((Func<Container, IDownloadDevicesKeysScheduler>)((Container c) => new DownloadDevicesKeysScheduler(c.Resolve<IEventManager>(), configuration, c.Resolve<IScheduler>())));
			IRepository repository = container.Resolve<IRepository>();
			container.Resolve<IProxyRepository>();
			IApplicationsHost applicationsHost = container.Resolve<IApplicationsHost>();
			IScheduler scheduler = container.Resolve<IScheduler>();
			RWE.SmartHome.SHC.BusinessLogic.LogicalDeviceStateRepository.LogicalDeviceStateRepository logicalDeviceStateRepository = new RWE.SmartHome.SHC.BusinessLogic.LogicalDeviceStateRepository.LogicalDeviceStateRepository(eventManager, repository, applicationsHost, container.Resolve<IApplicationsSettings>(), container.Resolve<IDateTimeProvider>());
			container.Register((Func<Container, ILogicalDeviceStateRepository>)((Container c) => logicalDeviceStateRepository));
			container.Register((Func<Container, IEmailSender>)((Container c) => new EmailSender(repository)));
			container.Resolve<IEmailSender>();
			RWE.SmartHome.SHC.BusinessLogic.ProtocolMultiplexer.ProtocolMultiplexer multiplexer = new RWE.SmartHome.SHC.BusinessLogic.ProtocolMultiplexer.ProtocolMultiplexer(eventManager, repository, applicationsHost, scheduler, configurationManager, container.Resolve<IShcTypeManager>(), repository, logicalDeviceStateRepository);
			container.Register((Func<Container, IProtocolMultiplexer>)((Container c) => multiplexer));
			container.Register((Func<Container, IProtocolRegistration>)((Container c) => multiplexer));
			IProtocolMultiplexer protocolMultiplexer = container.Resolve<IProtocolMultiplexer>();
			container.Register((ICoprocessorUpdater)new CoprocessorUpdater(container, protocolMultiplexer.DataBackup));
			ICommunicationChannel remoteChannel = container.ResolveNamed<IService>("RelayDriver") as ICommunicationChannel;
			RequestValidationRulesProvider requestValidationRulesProvider = new RequestValidationRulesProvider(repository);
			requestValidationRulesProvider.AddRulesSet(RequestValidationRulesProvider.LoadCoreConfig());
			SetEntitiesRequestValidator instance = new SetEntitiesRequestValidator(repository, requestValidationRulesProvider);
			container.Register((ISetEntitiesRequestValidator)instance);
			DeleteEntitiesRequestValidator instance2 = new DeleteEntitiesRequestValidator(repository);
			container.Register((IDeleteEntitiesRequestValidator)instance2);
			SetEntitiesConfigurationValidator validator = new SetEntitiesConfigurationValidator(requestValidationRulesProvider);
			container.Register((Func<Container, IConfigurationProcessor>)((Container c) => new ConfigurationProcessor(eventManager, scheduler, repository, container.Resolve<IBackendPersistence>(), protocolMultiplexer, remoteChannel, container.Resolve<DatabaseConnectionsPool>(), container.Resolve<IRegistrationService>()))).InitializedBy(delegate(Container c, IConfigurationProcessor v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			IConfigurationProcessor configurationProcessor = container.Resolve<IConfigurationProcessor>();
			configurationProcessor.RegisterConfigurationValidator(validator);
			configurationProcessor.RegisterConfigurationValidator(new CoreDevicesValidator());
			container.Register((Func<Container, IMessagesAndAlertsManager>)((Container c) => new MessagesAndAlertsManager(container, configuration))).InitializedBy(delegate(Container c, IMessagesAndAlertsManager v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			container.Register((Func<Container, IBusinessLogic>)delegate(Container c)
			{
				BusinessLogic businessLogic = new BusinessLogic(c, businessLogicMutex, areBackendRequestsAvailable);
				c.Resolve<ITaskManager>().Register(businessLogic);
				return businessLogic;
			}).InitializedBy(delegate(Container c, IBusinessLogic v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			container.Resolve<IBusinessLogic>();
			container.Register((Func<Container, IDeviceMonitor>)((Container c) => new DeviceMonitor(container))).InitializedBy(delegate(Container c, IDeviceMonitor monitor)
			{
				monitor.Start();
			});
			USBDeviceMonitor usbDeviceMonitor = new USBDeviceMonitor(container);
			container.Register((Func<Container, IUSBDeviceMonitor>)((Container c) => usbDeviceMonitor));
			container.Resolve<IUSBDeviceMonitor>();
			container.Register((Func<Container, IDeviceFirmwareManager>)delegate
			{
				IFileDownloader fileDownloaderInstance = new FileDownloader();
				DeviceFirmwareImagesService firmwareImagesService = new DeviceFirmwareImagesService(new DeviceFirmwareRepository(fileDownloaderInstance), container.Resolve<IDeviceUpdateClient>(), container.Resolve<IBusinessLogic>());
				return new DeviceFirmwareManager(eventManager, applicationsHost, scheduler, configuration, firmwareImagesService, new ReincludeHandler(container, DeviceDefinitionProviderSingleton.Instance));
			}).InitializedBy(delegate(Container c, IDeviceFirmwareManager v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			container.Register((Func<Container, ILogicalDeviceInclusionFactory>)((Container c) => new CapabilityInclusionFactory(applicationsHost, DeviceDefinitionProviderSingleton.Instance)));
			IIntegrityHandlerAggregator integrityHandlerAggregator = container.Resolve<IIntegrityHandlerAggregator>();
			ILogicalDeviceInclusionFactory inclusionFactory = container.Resolve<ILogicalDeviceInclusionFactory>();
			integrityHandlerAggregator.SubscribeHandler(new BaseDeviceIntegrityHandler(repository));
			integrityHandlerAggregator.SubscribeHandler(new LogicalDeviceIntegrityHandler(repository, inclusionFactory));
			integrityHandlerAggregator.SubscribeHandler(new ProfileIntegrityHandler(repository));
			integrityHandlerAggregator.SubscribeHandler(new ClimateControlIntegrityHandler(repository));
			integrityHandlerAggregator.SubscribeHandler(new LocationIntegrityHandler(repository));
			container.Register("UsbStickLogExport", (Func<Container, IService>)delegate(Container c)
			{
				UsbStickLogExport usbStickLogExport = new UsbStickLogExport(c, c.Resolve<IFileLogger>() as FileLogger, c.Resolve<IEventManager>(), c.Resolve<IDisplayManager>(), c.Resolve<ICertificateManager>());
				c.Resolve<ITaskManager>().Register(usbStickLogExport);
				return usbStickLogExport;
			}).InitializedBy(delegate(Container c, IService v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			container.ResolveNamed<IService>("UsbStickLogExport");
			container.Register((Func<Container, IDiscoveryController>)((Container c) => new DiscoveryController(repository, container.Resolve<IEventManager>(), container.Resolve<INotificationHandler>(), container.Resolve<IScheduler>(), container.Resolve<IProtocolMultiplexer>(), container.Resolve<IApplicationsHost>(), DiscoveryController.DefaultDiscoveredDeviceMaxAge))).ReusedWithin(ReuseScope.Container);
			deviceConfigurationSupport = new DeviceConfigurationSupport(applicationsHost, repository, container.Resolve<IRepositorySync>());
			container.Register((Func<Container, IDeviceKeyExporter>)((Container c) => new DeviceKeyExporter(container.Resolve<IEventManager>(), container.Resolve<IDeviceKeyRepository>(), container.Resolve<IExportDevicesKeysEmailSenderScheduler>())));
			container.Resolve<IDeviceKeyExporter>();
			Log.Information(Module.BusinessLogic, "Initialization of BusinessLogic successful");
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, $"Initialization of BusinessLogic failed: {ex.Message}");
		}
	}

	private void OnShcStartupCompleted(Container container)
	{
		List<IVirtualCurrentPhysicalStateHandler> list = new List<IVirtualCurrentPhysicalStateHandler>();
		List<IVirtualCoreActionHandler> list2 = new List<IVirtualCoreActionHandler>();
		IRepository repository = container.Resolve<IRepository>();
		IRepositorySync repositorySync = container.Resolve<IRepositorySync>();
		IEventManager eventManager = container.Resolve<IEventManager>();
		new TimeTriggerValidator(container.Resolve<IConfigurationProcessor>());
		new TimeEventScheduler(container.Resolve<IRulesRepository>(), container.Resolve<IEventManager>(), container.Resolve<IScheduler>());
		SHCBaseDeviceHandler sHCBaseDeviceHandler = new SHCBaseDeviceHandler(repository, container.Resolve<IRepositorySync>(), container.Resolve<IEventManager>(), container.Resolve<IScheduler>(), container.Resolve<INotificationHandler>(), container.Resolve<ITokenCache>(), container.Resolve<IDeviceMonitor>(), container.Resolve<ISoftwareUpdateProcessor>());
		HomeSetupHandler homeSetupHandler = new HomeSetupHandler(repository);
		CreateInitialDevices(repository, repositorySync, sHCBaseDeviceHandler, homeSetupHandler);
		list.Add(sHCBaseDeviceHandler);
		new SHCBaseDeviceValidator(container.Resolve<IConfigurationProcessor>(), sHCBaseDeviceHandler);
		list2.Add(new SHCBaseDeviceActionHandler(container.Resolve<IProtocolMultiplexer>(), eventManager, repository, container.Resolve<IRepositorySync>(), container.Resolve<IDeviceFirmwareManager>(), container.Resolve<ISoftwareUpdateProcessor>(), container.Resolve<IScheduler>(), sHCBaseDeviceHandler, container.Resolve<IDiscoveryController>()));
		list2.Add(new VRCCDeviceHandler(repository, container.Resolve<ILogicalDeviceStateRepository>(), eventManager));
		vrccStateHandler = new VrccStateHandler(repository, container.Resolve<ILogicalDeviceStateRepository>(), container.Resolve<IEventManager>());
		IProtocolMultiplexer multiplexer = container.Resolve<IProtocolMultiplexer>();
		VrccStateHandler.AddDefaultVrccStateUpdateEventHandler(vrccStateHandler, multiplexer);
		list.Add(new NotificationSenderDeviceHandler(repository));
		list2.Add(new NotificationSenderActionHandler(repository, eventManager, container.Resolve<ICertificateManager>(), container.Resolve<INotificationServiceClient>(), container.Resolve<IMessagesAndAlertsManager>(), container.Resolve<IScheduler>(), container.Resolve<ITokenCache>(), container.Resolve<ISmsClient>(), container.Resolve<IEmailSender>(), container.Resolve<IRegistrationService>()));
		list.ForEach(delegate(IVirtualCurrentPhysicalStateHandler h)
		{
			eventManager.GetEvent<VirtualDeviceHandlerAvailableEvent>().Publish(new VirtualDeviceHandlerAvailableEventArgs(h));
		});
		list2.ForEach(delegate(IVirtualCoreActionHandler h)
		{
			eventManager.GetEvent<VirtualStateHandlerAvailableEvent>().Publish(new VirtualStateHandlerAvailableEventArgs(h));
		});
		container.Resolve<IConfigurationProcessor>().RegisterConfigurationValidator(new VRCCDeviceValidator());
		container.Resolve<IConfigurationProcessor>().RegisterConfigurationValidator(new NotificationSenderValidator());
		new CustomApplicationSettingsBackup(container.Resolve<IBackendPersistence>(), new RandomNumberGenerator(), container.Resolve<IScheduler>());
	}

	private void CreateInitialDevices(IRepository repository, IRepositorySync repositorySync, SHCBaseDeviceHandler shcBaseDeviceHandler, HomeSetupHandler homeSetupHandler)
	{
		Log.Information(Module.BusinessLogic, "BusinessLogicModule", "CreateInitialDevices");
		using RepositoryLockContext repositoryLockContext = repositorySync.WaitForLock("BusinessLogicModule::CreateInitialDevices", new RepositoryUpdateContextData(CoreConstants.CoreAppId));
		repositoryLockContext.Commit = false;
		bool flag = false;
		bool flag2 = false;
		BaseDevice baseDevice = (from d in repository.GetBaseDevices()
			where d.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.NotificationSender
			select d).FirstOrDefault();
		if (baseDevice == null)
		{
			Log.Information(Module.BusinessLogic, "BusinessLogicModule", "NotificationSender device not found, adding.");
			baseDevice = NotificationSenderCreator.CreateNotificationSender();
			repository.SetBaseDevice(baseDevice);
			flag = true;
		}
		else if (!baseDevice.TimeOfAcceptance.HasValue)
		{
			BaseDevice shcBaseDevice = repository.GetShcBaseDevice();
			if (shcBaseDevice != null)
			{
				DateTime? dateTimeValue = shcBaseDevice.Properties.GetDateTimeValue(ShcBaseDeviceConstants.ConfigurationProperties.RegistrationTime);
				if (dateTimeValue.HasValue)
				{
					baseDevice.TimeOfAcceptance = dateTimeValue;
					repository.SetBaseDevice(baseDevice);
					flag2 = true;
				}
			}
		}
		bool flag3 = shcBaseDeviceHandler.EnsureDevice();
		bool flag4 = homeSetupHandler.AddHomeSetupIfNotExists();
		repositoryLockContext.Commit = flag || flag2 || flag3 || flag4;
		if (repositoryLockContext.Commit)
		{
			Log.Information(Module.BusinessLogic, "BusinessLogicModule", "Creating initial devices.");
		}
	}
}
