namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;

public enum CosIPFirmwareUpdateStatusCode
{
	Ack = 0,
	Nak = 1,
	NakFirmwareDataDoesNotMatch = 2,
	NakWrongFrameSequenceNumber = 3,
	NakBadSignature = 4,
	Alive = 255
}
