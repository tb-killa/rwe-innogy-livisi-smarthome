using System;

namespace SmartHome.SHC.API.Control;

public class DeviceStateChangedEventArgs : EventArgs
{
	public DeviceState DeviceState { get; private set; }

	public Guid DeviceId { get; private set; }

	public DeviceStateChangedEventArgs(Guid deviceId, DeviceState deviceState)
	{
		DeviceId = deviceId;
		DeviceState = deviceState;
	}
}
