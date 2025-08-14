using SerialAPI;

namespace RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;

public interface ISIPcosHandler
{
	SendStatus SendMessage(SIPCOSMessage message);

	SendStatus SendMessageDefaultSync(SIPcosHeader header, byte[] message, SendMode mode);

	byte? GetSequenceNumber(byte[] address);
}
