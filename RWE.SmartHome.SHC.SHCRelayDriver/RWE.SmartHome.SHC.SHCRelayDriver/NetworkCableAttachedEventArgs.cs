using System;

namespace RWE.SmartHome.SHC.SHCRelayDriver;

public class NetworkCableAttachedEventArgs : EventArgs
{
	public bool IsAttached { get; set; }
}
