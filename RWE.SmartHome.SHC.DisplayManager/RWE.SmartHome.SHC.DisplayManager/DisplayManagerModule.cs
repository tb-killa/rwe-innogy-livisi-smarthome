using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.DisplayManagerInterfaces;

namespace RWE.SmartHome.SHC.DisplayManager;

internal class DisplayManagerModule : IModule
{
	public void Configure(Container container)
	{
		IRegistration<IDisplayManager> registration = container.Register((Container c) => CreateDisplayManager(c));
		registration.InitializedBy(delegate(Container c, IDisplayManager v)
		{
			v.Initialize();
		});
		registration.ReusedWithin(ReuseScope.Container);
	}

	private static IDisplayManager CreateDisplayManager(Container container)
	{
		DisplayManager displayManager = new DisplayManager(container.Resolve<IEventManager>(), container.Resolve<INetworkingMonitor>());
		container.Resolve<ITaskManager>().Register(displayManager);
		return displayManager;
	}
}
