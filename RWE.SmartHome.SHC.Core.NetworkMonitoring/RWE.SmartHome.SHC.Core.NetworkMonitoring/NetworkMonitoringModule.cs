using System;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.Core.Scheduler;

namespace RWE.SmartHome.SHC.Core.NetworkMonitoring;

public class NetworkMonitoringModule : IModule
{
	public void Configure(Container container)
	{
		if (SettingsFileHelper.ShouldRegisterBackendRequests())
		{
			NetworkingMonitor networkMonitor = new NetworkingMonitor(container.Resolve<IEventManager>(), container.Resolve<IScheduler>(), container.Resolve<IConfigurationManager>());
			container.Register((Func<Container, INetworkingMonitor>)((Container c) => networkMonitor)).InitializedBy(delegate(Container c, INetworkingMonitor v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
		}
		else
		{
			NetworkingMonitorLocalOnly networkMonitor2 = new NetworkingMonitorLocalOnly();
			container.Register((Func<Container, INetworkingMonitor>)((Container c) => networkMonitor2)).ReusedWithin(ReuseScope.Container);
			Log.Information(Module.BackendCommunication, "Using the mock implementation for the backend requests");
		}
	}
}
