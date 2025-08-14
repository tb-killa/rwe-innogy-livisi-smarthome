using System;
using System.Net;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.PacketSender;

public interface IPacketSender
{
	IPEndPoint LocalEndpoint { get; set; }

	Predicate<byte[]> IsMessageComplete { get; set; }

	byte[] Send(IPEndPoint remoteEndPoint, byte[] message, bool responseExpected);

	void Close();
}
