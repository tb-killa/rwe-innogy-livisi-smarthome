using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;

public class DeviceInclusionTimeoutEventArgs : EventArgs
{
	public Guid DeviceId { get; set; }

	public DeviceInclusionTimeoutEventArgs(Guid deviceId)
	{
		DeviceId = deviceId;
	}
}
