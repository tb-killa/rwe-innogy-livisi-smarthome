using System;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;

public class DeviceConfiguredEventArgs : EventArgs
{
	public Guid DeviceId { get; set; }

	public DeviceConfigurationState State { get; set; }
}
