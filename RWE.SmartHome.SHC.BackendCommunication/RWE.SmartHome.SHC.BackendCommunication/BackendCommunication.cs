using System;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.BackendCommunication.Clients;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Authentication;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.DisplayManagerInterfaces;

namespace RWE.SmartHome.SHC.BackendCommunication;

public class BackendCommunication : IBackendCommunication
{
	public BackendCommunication(Container container)
	{
		INetworkingMonitor networkingMonitor = container.Resolve<INetworkingMonitor>();
		RegistrationService registrationService = new RegistrationService(networkingMonitor, container.Resolve<IUserManager>(), container.Resolve<IDisplayManager>(), container.Resolve<IEventManager>());
		Configuration configuration = new Configuration(container.Resolve<IConfigurationManager>());
		container.Register((Func<Container, IRegistrationService>)((Container c) => registrationService)).ReusedWithin(ReuseScope.Container);
		container.Register((Func<Container, IConfigurationClient>)((Container c) => new ConfigurationClient(networkingMonitor, configuration, registrationService))).ReusedWithin(ReuseScope.Container);
		container.Resolve<IConfigurationClient>();
		container.Register((Func<Container, IDeviceManagementClient>)((Container c) => new DeviceManagementClient(networkingMonitor, configuration, container.Resolve<ICertificateManager>(), registrationService))).ReusedWithin(ReuseScope.Container);
		container.Resolve<IDeviceManagementClient>();
		container.Register((Func<Container, IKeyExchangeClient>)((Container c) => new KeyExchangeClient(networkingMonitor, configuration, container.Resolve<ICertificateManager>(), registrationService))).ReusedWithin(ReuseScope.Container);
		container.Resolve<IKeyExchangeClient>();
		container.Register((Func<Container, IShcInitializationClient>)((Container c) => new ShcInitializationClient(networkingMonitor, configuration, registrationService))).ReusedWithin(ReuseScope.Container);
		container.Resolve<IShcInitializationClient>();
		container.Register((Func<Container, ISoftwareUpdateClient>)((Container c) => new SoftwareUpdateClient(networkingMonitor, configuration, registrationService))).ReusedWithin(ReuseScope.Container);
		container.Resolve<ISoftwareUpdateClient>();
		container.Register((Func<Container, IShcMessagingClient>)((Container c) => new ShcMessagingClient(networkingMonitor, configuration, container.Resolve<ICertificateManager>(), registrationService))).ReusedWithin(ReuseScope.Container);
		container.Resolve<IShcMessagingClient>();
		container.Register((Func<Container, ISmsClient>)((Container c) => new SmsClient(networkingMonitor, configuration, container.Resolve<ICertificateManager>(), registrationService))).ReusedWithin(ReuseScope.Container);
		container.Resolve<ISmsClient>();
		container.Register((Func<Container, IApplicationTokenClient>)((Container c) => new ApplicationTokenClient(networkingMonitor, configuration, registrationService))).ReusedWithin(ReuseScope.Container);
		container.Resolve<IApplicationTokenClient>();
		container.Register((Func<Container, INotificationServiceClient>)((Container c) => new NotificationClient(networkingMonitor, configuration, container.Resolve<ICertificateManager>(), registrationService))).ReusedWithin(ReuseScope.Container);
		container.Resolve<INotificationServiceClient>();
		container.Register((Func<Container, IDeviceUpdateClient>)((Container c) => new DeviceUpdateClient(networkingMonitor, container.Resolve<ICertificateManager>(), configuration, registrationService))).ReusedWithin(ReuseScope.Container);
		container.Resolve<IDeviceUpdateClient>();
		container.Register((Func<Container, IDataTrackingClient>)((Container c) => new DataTrackingClient(networkingMonitor, container.Resolve<ICertificateManager>(), configuration, registrationService))).ReusedWithin(ReuseScope.Container);
		container.Resolve<IDataTrackingClient>();
	}
}
