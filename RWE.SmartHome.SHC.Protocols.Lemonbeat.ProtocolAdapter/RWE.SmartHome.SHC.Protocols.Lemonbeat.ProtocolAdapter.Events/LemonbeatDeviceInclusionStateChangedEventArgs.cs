using System;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Events;

public class LemonbeatDeviceInclusionStateChangedEventArgs : EventArgs
{
	public Guid DeviceId { get; private set; }

	public LemonbeatDeviceInclusionState DeviceInclusionState { get; private set; }

	public LemonbeatDeviceInclusionStateChangedEventArgs(Guid deviceId, LemonbeatDeviceInclusionState deviceInclusionState)
	{
		DeviceId = deviceId;
		DeviceInclusionState = deviceInclusionState;
	}
}
