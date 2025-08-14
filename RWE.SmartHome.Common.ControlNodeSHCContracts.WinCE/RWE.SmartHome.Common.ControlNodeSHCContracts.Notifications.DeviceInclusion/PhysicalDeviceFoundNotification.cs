using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.DeviceInclusion;

public class PhysicalDeviceFoundNotification : BaseNotification
{
	public BaseDevice FoundDevice { get; set; }

	public DeviceFoundState State { get; set; }
}
