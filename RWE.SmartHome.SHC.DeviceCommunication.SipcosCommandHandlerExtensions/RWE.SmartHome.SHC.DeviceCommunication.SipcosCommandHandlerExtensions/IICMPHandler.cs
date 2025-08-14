using SerialAPI;

namespace RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;

public interface IICMPHandler
{
	event ICMPHandlerExt.ReceiveRawData ReceiveData;

	SendStatus SendICMPMessage(CORESTACKMessage message);
}
