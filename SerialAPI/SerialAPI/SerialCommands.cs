namespace SerialAPI;

internal enum SerialCommands : byte
{
	DefaultIP = 0,
	FactoryReset = 1,
	CSMA_CA_Attempts = 2,
	Send_Attempts = 3,
	NetworkKey = 4,
	DutyCycle = 5,
	SyncWord = 6,
	TransmitPower = 7,
	Version = 8,
	Partners = 9,
	SequenceCount = 16,
	FlashCRC = 17,
	RegisteredIP = 18
}
