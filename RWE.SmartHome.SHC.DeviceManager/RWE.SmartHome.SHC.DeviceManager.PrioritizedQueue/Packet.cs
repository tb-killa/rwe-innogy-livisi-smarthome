using SerialAPI;

namespace RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;

public class Packet
{
	public CORESTACKHeader Header { get; private set; }

	public byte[] Message { get; private set; }

	public PacketSendState State { get; set; }

	public Packet(SIPcosHeader header, byte[] message)
	{
		Header = header;
		Message = message;
	}

	public Packet(SIPCOSMessage message)
	{
		Header = message.Header;
		Message = message.Data.ToArray();
	}

	public Packet(CORESTACKMessage message)
	{
		Header = message.Header;
		Message = message.Data.ToArray();
	}
}
