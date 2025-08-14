using System;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher;

public class ExternalCommandDispatcherModule : IModule
{
	public void Configure(Container container)
	{
		container.Register((Func<Container, IExternalCommandDispatcher>)((Container c) => new ExternalCommandDispatcher(container))).InitializedBy(delegate(Container c, IExternalCommandDispatcher v)
		{
			v.Initialize();
		}).ReusedWithin(ReuseScope.Container);
		container.Resolve<IExternalCommandDispatcher>();
	}
}
