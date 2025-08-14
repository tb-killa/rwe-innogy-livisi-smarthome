using System;
using System.Net;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.PacketReceivers;

public interface IPacketReceiver
{
	Func<IPEndPoint, byte[], byte[]> ProcessIncomingMessage { get; set; }

	event Action ErrorOccured;

	void Start();

	void Stop();
}
