using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.Core;

namespace RWE.SmartHome.SHC.Virtual.ProtocolAdapter;

public class VirtualProtocolAdapterModule : IModule
{
	public void Configure(Container container)
	{
		VirtualDevicesProtocolAdapter protocolAdapter = new VirtualDevicesProtocolAdapter(container);
		container.Resolve<IProtocolRegistration>().RegisterProtocolAdapter(protocolAdapter);
	}
}
