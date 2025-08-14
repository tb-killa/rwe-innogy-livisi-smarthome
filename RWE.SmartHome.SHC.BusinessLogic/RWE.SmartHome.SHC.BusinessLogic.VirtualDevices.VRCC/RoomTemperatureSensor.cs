using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;

internal class RoomTemperatureSensor : CompositeSensorDevice
{
	protected override string StatePropertyName => "Temperature";

	public override string RelatedDevicePropertyName => "VRCCTemperature";

	public RoomTemperatureSensor(LogicalDevice logicalDevice, IEnumerable<LogicalDevice> relatedDevices)
		: base(logicalDevice, relatedDevices)
	{
	}

	protected override UnderlyingDevice CreateUnderlyingDevice(LogicalDevice logicalDevice)
	{
		return new TemperatureSensorDevice(logicalDevice, this);
	}
}
