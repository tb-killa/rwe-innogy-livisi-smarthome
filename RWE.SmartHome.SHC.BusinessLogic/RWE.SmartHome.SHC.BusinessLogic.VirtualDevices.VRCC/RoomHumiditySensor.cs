using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;

internal class RoomHumiditySensor : CompositeSensorDevice
{
	protected override string StatePropertyName => "Humidity";

	public override string RelatedDevicePropertyName => "VRCCHumidity";

	public RoomHumiditySensor(LogicalDevice logicalDevice, IEnumerable<LogicalDevice> relatedDevices)
		: base(logicalDevice, relatedDevices)
	{
	}

	protected override UnderlyingDevice CreateUnderlyingDevice(LogicalDevice logicalDevice)
	{
		return new HumiditySensorDevice(logicalDevice, this);
	}
}
