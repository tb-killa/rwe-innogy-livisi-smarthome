using System;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Events;

public class DeviceReachabilityChangedEventArgs : EventArgs
{
	public bool IsReachable { get; set; }

	public DeviceInformation Device { get; set; }

	public DeviceReachabilityChangedEventArgs(DeviceInformation device, bool isReachable)
	{
		IsReachable = isReachable;
		Device = device;
	}
}
