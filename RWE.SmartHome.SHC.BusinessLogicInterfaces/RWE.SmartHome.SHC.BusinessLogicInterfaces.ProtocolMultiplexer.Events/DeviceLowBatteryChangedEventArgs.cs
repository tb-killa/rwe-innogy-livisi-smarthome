using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;

public class DeviceLowBatteryChangedEventArgs
{
	public bool LowBattery { get; set; }

	public Guid DeviceId { get; set; }

	public DeviceLowBatteryChangedEventArgs(Guid deviceId, bool lowBattery)
	{
		LowBattery = lowBattery;
		DeviceId = deviceId;
	}
}
