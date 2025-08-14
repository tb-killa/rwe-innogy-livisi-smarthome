using System;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Configuration;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Events;

public class CurrentConfigurationReceivedEventArgs : EventArgs
{
	public Guid DeviceId { get; set; }

	public PhysicalConfiguration Configuration { get; set; }
}
