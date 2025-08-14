using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.DeviceState;

public class PhysicalDeviceStateChangedNotification : BaseNotification
{
	public PhysicalDeviceState DeviceState { get; set; }

	public PhysicalDeviceStateChangedNotification()
	{
	}

	public PhysicalDeviceStateChangedNotification(PhysicalDeviceState deviceState)
	{
		DeviceState = deviceState;
	}
}
