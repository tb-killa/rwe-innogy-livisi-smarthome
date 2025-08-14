using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;

public class DeviceUnreachableChangedEventArgs : EventArgs
{
	public bool Unreachable { get; set; }

	public Guid DeviceId { get; set; }

	public DeviceUnreachableChangedEventArgs(Guid deviceId, bool unreachable)
	{
		Unreachable = unreachable;
		DeviceId = deviceId;
	}
}
