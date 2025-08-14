namespace SipcosCommandHandler;

public enum FirmwareReplyStatus : byte
{
	ACK,
	NAK,
	NAK_FIRMWARE_DATA_DO_NOT_MATCH,
	NAK_WRONG_SEQUENCE_NUMBER,
	NAK_BAD_SIGNARTURE
}
