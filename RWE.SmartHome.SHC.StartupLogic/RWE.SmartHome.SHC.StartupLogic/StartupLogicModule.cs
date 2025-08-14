using System;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.StartupLogic;

public class StartupLogicModule : IModule
{
	public void Configure(Container container)
	{
		try
		{
			container.Register("StartupLogic", (Func<Container, IService>)delegate(Container c)
			{
				StartupLogic startupLogic = new StartupLogic(container);
				c.Resolve<ITaskManager>().Register(startupLogic);
				return startupLogic;
			}).InitializedBy(delegate(Container c, IService v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			container.ResolveNamed<IService>("StartupLogic");
			Log.Information(Module.StartupLogic, "Initialization of StartupLogic successful");
		}
		catch (Exception ex)
		{
			Log.Information(Module.StartupLogic, $"Initialization of StartupLogic failed: {ex.Message}");
		}
	}
}
