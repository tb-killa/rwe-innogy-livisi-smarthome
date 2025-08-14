using System;

namespace SmartHome.SHC.API.Control;

public class CapabilityStateChangedEventArgs : EventArgs
{
	public CapabilityState State { get; private set; }

	public Guid DeviceId { get; private set; }

	public CapabilityStateChangedEventArgs(Guid deviceId, CapabilityState state)
	{
		DeviceId = deviceId;
		State = state;
	}
}
