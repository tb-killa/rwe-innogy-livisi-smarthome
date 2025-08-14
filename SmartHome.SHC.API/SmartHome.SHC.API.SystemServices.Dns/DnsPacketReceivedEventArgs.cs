using System;

namespace SmartHome.SHC.API.SystemServices.Dns;

public class DnsPacketReceivedEventArgs : EventArgs
{
	public DnsPacket Packet { get; private set; }

	public DnsPacketReceivedEventArgs(DnsPacket dnsPacket)
	{
		Packet = dnsPacket;
	}
}
