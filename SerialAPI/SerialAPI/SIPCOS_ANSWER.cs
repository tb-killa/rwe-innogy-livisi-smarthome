namespace SerialAPI;

public enum SIPCOS_ANSWER : byte
{
	ACK = 0,
	ACK_STATUS = 1,
	NAK = 128,
	NAK_INHIBIT = 129,
	NAK_PEER = 130
}
