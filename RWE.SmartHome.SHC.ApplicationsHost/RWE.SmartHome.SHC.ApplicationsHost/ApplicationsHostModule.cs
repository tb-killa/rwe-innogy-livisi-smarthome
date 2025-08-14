using System;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.Core.TypeManager;
using RWE.SmartHome.SHC.DataAccessInterfaces.Applications;

namespace RWE.SmartHome.SHC.ApplicationsHost;

public class ApplicationsHostModule : IModule
{
	public void Configure(Container container)
	{
		IApplicationsTokenPersistence applicationsTokenPersistence = container.Resolve<IApplicationsTokenPersistence>();
		ITokenCache tokenCache = new TokenCache(applicationsTokenPersistence, container.Resolve<IApplicationTokenClient>(), container.Resolve<ICertificateManager>(), container.Resolve<IShcTypeManager>(), container.Resolve<IEventManager>(), container.Resolve<INetworkingMonitor>(), container.Resolve<IRegistrationService>());
		container.Register((Container c) => tokenCache).InitializedBy(delegate(Container c, ITokenCache v)
		{
			v.Initialize();
		}).ReusedWithin(ReuseScope.Container);
		container.Resolve<ITokenCache>();
		container.Register((Func<Container, IAddinRepoManager>)((Container c) => new AddinsRepoManager(applicationsTokenPersistence, tokenCache, container.Resolve<IRegistrationService>(), container.Resolve<IEventManager>()))).ReusedWithin(ReuseScope.Container);
		container.Resolve<IAddinRepoManager>();
		ConfigurationProperties configuration = new ConfigurationProperties(container.Resolve<IConfigurationManager>());
		container.Register((Func<Container, IApplicationsHost>)((Container c) => new ApplicationsHost(tokenCache, container, new AppHostSysData
		{
			MemoryLimit = configuration.AddinLoadMemoryLimit,
			MemoryLimitStartup = configuration.AddinLoadMemoryLimitStartup
		}))).ReusedWithin(ReuseScope.Container);
		container.Resolve<IApplicationsHost>();
	}
}
