using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class VirtualDeviceAvailableArgs : EventArgs
{
	public Guid DeviceId { get; private set; }

	public VirtualDeviceAvailableArgs(Guid deviceId)
	{
		DeviceId = deviceId;
	}
}
