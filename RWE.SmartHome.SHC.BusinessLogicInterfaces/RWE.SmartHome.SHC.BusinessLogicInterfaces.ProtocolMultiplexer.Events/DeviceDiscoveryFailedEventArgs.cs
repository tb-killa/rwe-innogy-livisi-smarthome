using System;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;

public class DeviceDiscoveryFailedEventArgs : EventArgs
{
	public List<string> AppIds { get; set; }
}
