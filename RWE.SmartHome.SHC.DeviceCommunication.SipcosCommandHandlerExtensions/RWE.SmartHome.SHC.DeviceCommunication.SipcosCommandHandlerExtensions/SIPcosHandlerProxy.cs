using SerialAPI;

namespace RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;

public class SIPcosHandlerProxy : ISIPcosHandler
{
	private readonly SIPcosHandler handler;

	public SIPcosHandlerProxy(SIPcosHandler handler)
	{
		this.handler = handler;
	}

	public SendStatus SendMessage(SIPCOSMessage message)
	{
		return handler.SendMessage(message);
	}

	public SendStatus SendMessageDefaultSync(SIPcosHeader header, byte[] message, SendMode mode)
	{
		return handler.SendMessageDefaultSync(header, message, mode);
	}

	public byte? GetSequenceNumber(byte[] adress)
	{
		return handler.GetBidCosCounter(adress);
	}
}
