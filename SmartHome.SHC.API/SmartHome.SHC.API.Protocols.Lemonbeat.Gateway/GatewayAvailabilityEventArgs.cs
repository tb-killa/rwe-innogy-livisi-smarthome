using System;

namespace SmartHome.SHC.API.Protocols.Lemonbeat.Gateway;

public class GatewayAvailabilityEventArgs : EventArgs
{
	public bool Available { get; private set; }

	public GatewayAvailabilityEventArgs(bool available)
	{
		Available = available;
	}
}
