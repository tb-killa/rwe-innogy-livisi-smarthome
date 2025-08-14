using System;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;

public class DeviceDiscoveryStatusChangedEventArgs : EventArgs
{
	public DiscoveryPhase Phase { get; set; }

	public List<string> AppIds { get; set; }
}
