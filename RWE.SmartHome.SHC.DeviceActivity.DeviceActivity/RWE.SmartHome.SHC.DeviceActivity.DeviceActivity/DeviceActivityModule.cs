using System;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.DataAccessInterfaces.DeviceActivityLogging;
using RWE.SmartHome.SHC.DeviceActivity.DeviceActivityInterfaces;

namespace RWE.SmartHome.SHC.DeviceActivity.DeviceActivity;

public class DeviceActivityModule : IModule
{
	private const string DalSectionName = "RWE.SmartHome.SHC.DeviceActivity";

	private const string BatchSizeSettingName = "BatchSize";

	public void Configure(Container container)
	{
		IApplicationsHost appHost = container.Resolve<IApplicationsHost>();
		DalTypeResolver dalTypeResolver = new DalTypeResolver(appHost);
		int batchSize = container.Resolve<IConfigurationManager>()["RWE.SmartHome.SHC.DeviceActivity"].GetInt("BatchSize") ?? 100;
		DeviceActivityLogger logger = new DeviceActivityLogger(container.Resolve<IEventManager>(), container.Resolve<IDeviceActivityPersistence>(), container.Resolve<ITrackDataPersistence>(), container.Resolve<IRepository>(), container.Resolve<INetworkingMonitor>(), container.Resolve<IBusinessLogic>(), container.Resolve<IDataTrackingClient>(), container.Resolve<IScheduler>(), batchSize, dalTypeResolver, container.Resolve<IRegistrationService>());
		container.Register((Func<Container, IDeviceActivityLogger>)((Container c) => logger)).InitializedBy(delegate(Container c, IDeviceActivityLogger l)
		{
			l.Initialize();
		}).ReusedWithin(ReuseScope.Container);
		container.Resolve<IDeviceActivityLogger>();
		IShcBaseDeviceWatchers shcBaseDeviceWatchers = container.Resolve<IShcBaseDeviceWatchers>();
		shcBaseDeviceWatchers.RegisterWatcher(logger);
	}
}
