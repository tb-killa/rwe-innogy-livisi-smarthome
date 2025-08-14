using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;

public class DeviceReadyForUpdateEventArgs
{
	public Guid DeviceId { get; set; }

	public string PhysicalDeviceType { get; set; }

	public string AppId { get; set; }

	public bool Occurred { get; set; }
}
