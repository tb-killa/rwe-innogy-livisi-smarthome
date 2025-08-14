using System;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Authentication;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.DataAccessInterfaces.TechnicalConfiguration;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Calibrators;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Persistence;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.SwitchDelegate;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.DevicePolling;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.EventForwarding;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.RuleEngineCommunication;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter;

public class SipCosProtocolAdapterModule : IModule
{
	public void Configure(Container container)
	{
		IDeviceManager deviceManager = container.Resolve<IDeviceManager>();
		ILogicalDeviceStateRepository logicalDeviceStateRepository = container.Resolve<ILogicalDeviceStateRepository>();
		LogicalDeviceHandlerCollection logicalDeviceHandlerCollection = new LogicalDeviceHandlerCollection(container.Resolve<IRepository>(), container.Resolve<IDeviceManager>(), logicalDeviceStateRepository, container.Resolve<IEventManager>());
		container.Register((Func<Container, IDevicePolling>)((Container c) => new RWE.SmartHome.SHC.SipCosProtocolAdapter.DevicePolling.DevicePolling(container.Resolve<IDeviceManager>(), container.Resolve<IRepository>(), container.Resolve<IEventManager>(), container.Resolve<IScheduler>(), logicalDeviceHandlerCollection)));
		IEventManager eventManager = container.Resolve<IEventManager>();
		ISipCosPersistence sipCosPersistence = container.Resolve<ISipCosPersistence>();
		IConfigurationManager configurationManager = container.Resolve<IConfigurationManager>();
		SipCosProtocolSpecificProtocolAdapter protocol = new SipCosProtocolSpecificProtocolAdapter(container.Resolve<IRepository>(), logicalDeviceStateRepository, eventManager, deviceManager, container.Resolve<IUserManager>(), logicalDeviceHandlerCollection, new TriggerCapableDeviceHandlerCollection(), sipCosPersistence, configurationManager, container.Resolve<ICoprocessorAccess>(), container.Resolve<ITechnicalConfigurationPersistence>(), container.Resolve<ISwitchDelegate>(), container.Resolve<IRollerShutterCalibrator>(), container.Resolve<IScheduler>(), container.Resolve<ICosIPFirmwareUpdateController>(), container.Resolve<IDeviceFirmwareManager>(), container.Resolve<IBidCosConfigurator>());
		container.Register((Func<Container, ISipCosProtocolAdapter>)((Container c) => protocol));
		container.Resolve<IProtocolRegistration>().RegisterProtocolAdapter(protocol);
		EventForwarder eventForwarder = new EventForwarder(container.Resolve<IEventManager>(), container.Resolve<IRepository>(), container.Resolve<IDevicePolling>(), logicalDeviceHandlerCollection, deviceManager);
		container.Register((Container c) => eventForwarder);
	}
}
