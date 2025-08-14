using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceMonitoring;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalDeviceKeys;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.DataAccessInterfaces.ProtocolSpecificData;

namespace RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter;

public class WMBusProtocolAdapterModule : IModule
{
	public void Configure(Container container)
	{
		WMBusProtocolAdapter protocolAdapter = new WMBusProtocolAdapter(container.Resolve<IEventManager>(), container.Resolve<IProtocolSpecificDataPersistence>(), container.Resolve<IApplicationsHost>(), container.Resolve<IDeviceMonitor>(), container.Resolve<IRepository>(), container.Resolve<IDeviceKeyRepository>());
		container.Resolve<IProtocolRegistration>().RegisterProtocolAdapter(protocolAdapter);
	}
}
