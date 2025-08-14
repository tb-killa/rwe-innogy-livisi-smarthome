using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;

internal class HumiditySensorDevice : UnderlyingSensorDevice
{
	public HumiditySensorDevice(LogicalDevice logicalDevice, CompositeDevice compositeDevice)
		: base(logicalDevice, compositeDevice)
	{
	}
}
