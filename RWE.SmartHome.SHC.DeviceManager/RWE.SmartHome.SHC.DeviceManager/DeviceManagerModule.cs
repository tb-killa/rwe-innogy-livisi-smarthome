using System;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.BusinessLogic.LocalCommunication;
using RWE.SmartHome.SHC.BusinessLogic.LocalDeviceKeys;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration.DevicesDefinitions;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalCommunication;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalDeviceKeys;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.DataAccessInterfaces.ProtocolSpecificData;
using RWE.SmartHome.SHC.DeviceManager.Calibration;
using RWE.SmartHome.SHC.DeviceManager.DeviceHandler;
using RWE.SmartHome.SHC.DeviceManager.Persistence;
using RWE.SmartHome.SHC.DeviceManager.SwitchDelegate;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Calibrators;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Persistence;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.SwitchDelegate;
using SerialApiInterfaces;

namespace RWE.SmartHome.SHC.DeviceManager;

public class DeviceManagerModule : IModule
{
	public void Configure(Container container)
	{
		DeviceList deviceList = new DeviceList();
		SipCosPersistence persistence = new SipCosPersistence(container.Resolve<IProtocolSpecificDataPersistence>());
		container.Register((Func<Container, ISipCosPersistence>)((Container c) => persistence));
		container.Register((Func<Container, IDeviceMasterKeyRepository>)((Container c) => new DeviceMasterKeyRepository(container.Resolve<ICertificateManager>())));
		container.Register((Func<Container, IDeviceKeyRepository>)((Container c) => new DeviceKeyRepository(container.Resolve<IDeviceMasterKeyRepository>(), container.Resolve<IConfigurationManager>())));
		container.Register((Func<Container, IStopBackendServicesHandler>)((Container c) => new StopBackendServicesHandler(container.Resolve<IEventManager>(), container.Resolve<IScheduler>(), container.Resolve<IRegistrationService>())));
		CommunicationWrapper communicationWrapper = new CommunicationWrapper(container.Resolve<ISerialPort>(), deviceList, container.Resolve<IEventManager>(), container.Resolve<ISipCosPersistence>(), container.Resolve<IConfigurationManager>(), container.Resolve<IScheduler>(), container.Resolve<IDeviceKeyRepository>());
		container.Register((Func<Container, ICosIPFirmwareUpdateController>)((Container c) => new CosIPFirmwareUpdateController(communicationWrapper, container.Resolve<IEventManager>())));
		container.Register((Func<Container, IMulticastController>)((Container c) => new MulticastController(communicationWrapper)));
		container.Register((Func<Container, ICoprocessorAccess>)((Container c) => new CoprocessorAccess(communicationWrapper))).ReusedWithin(ReuseScope.Container);
		container.Register((Func<Container, IDeviceManager>)((Container c) => new DeviceManager(container.Resolve<IEventManager>(), container.Resolve<ISipCosPersistence>(), container.Resolve<IScheduler>(), communicationWrapper, deviceList, container.Resolve<IConfigurationManager>(), DeviceDefinitionProviderSingleton.Instance, container.Resolve<IDeviceMasterKeyRepository>(), container.Resolve<IDeviceKeyRepository>()))).InitializedBy(delegate(Container c, IDeviceManager v)
		{
			v.Initialize();
		}).ReusedWithin(ReuseScope.Container);
		container.Register((Func<Container, ISwitchDelegate>)((Container c) => new RWE.SmartHome.SHC.DeviceManager.SwitchDelegate.SwitchDelegate(communicationWrapper, c.Resolve<IDeviceManager>())));
		container.Register((Func<Container, IRollerShutterCalibrator>)((Container c) => new RollerShutterCalibrator(communicationWrapper, c.Resolve<IEventManager>())));
		container.Register((Func<Container, IBidCosConfigurator>)((Container c) => new BidCosConfigurator(communicationWrapper.SendScheduler)));
	}
}
