using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;

internal class TemperatureSensorDevice : UnderlyingSensorDevice
{
	public TemperatureSensorDevice(LogicalDevice logicalDevice, CompositeDevice compositeDevice)
		: base(logicalDevice, compositeDevice)
	{
	}
}
