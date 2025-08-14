using System;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.FirmwareUpdate;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Events;

public class LemonbeatDeviceUpdateStateChangedEventArgs : EventArgs
{
	public Guid DeviceId { get; private set; }

	public LemonbeatDeviceUpdateState DeviceUpdateState { get; private set; }

	public LemonbeatDeviceUpdateStateChangedEventArgs(Guid deviceId, LemonbeatDeviceUpdateState deviceUpdateState)
	{
		DeviceId = deviceId;
		DeviceUpdateState = deviceUpdateState;
	}
}
