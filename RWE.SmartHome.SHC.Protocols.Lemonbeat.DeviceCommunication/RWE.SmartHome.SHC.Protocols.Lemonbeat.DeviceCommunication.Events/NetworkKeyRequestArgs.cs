using System;
using System.Net;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Events;

public class NetworkKeyRequestArgs : EventArgs
{
	public IPAddress IPAddress { get; private set; }

	public NetworkKeyRequestArgs(IPAddress ipAddress)
	{
		IPAddress = ipAddress;
	}
}
