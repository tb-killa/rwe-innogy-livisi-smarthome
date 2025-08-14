using System;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.Core;

namespace RWE.SmartHome.SHC.BackendCommunication;

public class BackendCommunicationModule : IModule
{
	public void Configure(Container container)
	{
		if (SettingsFileHelper.ShouldRegisterBackendRequests())
		{
			container.Register((Func<Container, IBackendCommunication>)((Container c) => new BackendCommunication(container))).ReusedWithin(ReuseScope.Container);
		}
		else
		{
			container.Register((Func<Container, IBackendCommunication>)((Container c) => new BackendCommunicationLocalOnly(container))).ReusedWithin(ReuseScope.Container);
		}
		container.Resolve<IBackendCommunication>();
	}
}
