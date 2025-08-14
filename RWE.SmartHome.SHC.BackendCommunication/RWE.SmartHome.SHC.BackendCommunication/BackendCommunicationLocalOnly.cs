using System;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.BackendCommunication.LocalCommunication;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Authentication;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.DisplayManagerInterfaces;

namespace RWE.SmartHome.SHC.BackendCommunication;

public class BackendCommunicationLocalOnly : IBackendCommunication
{
	public BackendCommunicationLocalOnly(Container container)
	{
		INetworkingMonitor networkMonitor = container.Resolve<INetworkingMonitor>();
		RegistrationService registrationService = new RegistrationService(networkMonitor, container.Resolve<IUserManager>(), container.Resolve<IDisplayManager>(), container.Resolve<IEventManager>());
		new Configuration(container.Resolve<IConfigurationManager>());
		container.Register((Func<Container, IRegistrationService>)((Container c) => registrationService)).ReusedWithin(ReuseScope.Container);
		container.Register((Func<Container, IConfigurationClient>)((Container c) => new ConfigurationClientLocalOnly())).ReusedWithin(ReuseScope.Container);
		container.Resolve<IConfigurationClient>();
		container.Register((Func<Container, IDeviceManagementClient>)((Container c) => new DeviceManagementClientLocalOnly())).ReusedWithin(ReuseScope.Container);
		container.Resolve<IDeviceManagementClient>();
		container.Register((Func<Container, IKeyExchangeClient>)((Container c) => new KeyExchangeClientLocalOnly())).ReusedWithin(ReuseScope.Container);
		container.Resolve<IKeyExchangeClient>();
		container.Register((Func<Container, IShcInitializationClient>)((Container c) => new ShcInitializationClientLocalOnly())).ReusedWithin(ReuseScope.Container);
		container.Resolve<IShcInitializationClient>();
		container.Register((Func<Container, ISoftwareUpdateClient>)((Container c) => new SoftwareUpdateClientLocalOnly())).ReusedWithin(ReuseScope.Container);
		container.Resolve<ISoftwareUpdateClient>();
		container.Register((Func<Container, IShcMessagingClient>)((Container c) => new ShcMessagingClientLocalOnly())).ReusedWithin(ReuseScope.Container);
		container.Resolve<IShcMessagingClient>();
		container.Register((Func<Container, ISmsClient>)((Container c) => new SmsClientLocalOnly())).ReusedWithin(ReuseScope.Container);
		container.Resolve<ISmsClient>();
		container.Register((Func<Container, IApplicationTokenClient>)((Container c) => new ApplicationTokenClientLocalOnly())).ReusedWithin(ReuseScope.Container);
		container.Resolve<IApplicationTokenClient>();
		container.Register((Func<Container, INotificationServiceClient>)((Container c) => new NotificationClientLocalOnly())).ReusedWithin(ReuseScope.Container);
		container.Resolve<INotificationServiceClient>();
		container.Register((Func<Container, IDeviceUpdateClient>)((Container c) => new DeviceUpdateClientLocalOnly())).ReusedWithin(ReuseScope.Container);
		container.Resolve<IDeviceUpdateClient>();
		container.Register((Func<Container, IDataTrackingClient>)((Container c) => new DataTrackingClientLocalOnly())).ReusedWithin(ReuseScope.Container);
		container.Resolve<IDataTrackingClient>();
		Log.Information(Module.BackendCommunication, "Using the mock implementation for the backend requests");
	}
}
