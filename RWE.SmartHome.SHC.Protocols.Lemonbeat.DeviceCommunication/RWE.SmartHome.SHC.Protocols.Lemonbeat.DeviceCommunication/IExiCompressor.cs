using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication;

public interface IExiCompressor
{
	byte[] GetCompressedMessage(string uncompressedMessage, LemonbeatServicePort port);

	string DecompressMessage(byte[] exiMessage, LemonbeatServicePort port);
}
