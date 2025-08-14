using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceMonitoring;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.DataAccessInterfaces.ProtocolSpecificData;

namespace RWE.SmartHome.SHC.Lemonbeat.ProtocolAdapter;

public class LemonbeatProtocolAdapterModule : IModule
{
	public void Configure(Container container)
	{
		LemonbeatProtocolAdapter protocolAdapter = new LemonbeatProtocolAdapter(container.Resolve<IEventManager>(), container.Resolve<IProtocolSpecificDataPersistence>(), container.Resolve<IApplicationsHost>(), container.Resolve<IDeviceMonitor>(), container.Resolve<IScheduler>(), container.Resolve<IRepository>(), container.Resolve<IDeviceFirmwareManager>());
		container.Resolve<IProtocolRegistration>().RegisterProtocolAdapter(protocolAdapter);
	}
}
