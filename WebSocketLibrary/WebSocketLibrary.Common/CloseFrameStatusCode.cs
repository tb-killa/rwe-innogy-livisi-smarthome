namespace WebSocketLibrary.Common;

public enum CloseFrameStatusCode : ushort
{
	Undefined = 0,
	NormalClosure = 1000,
	GoingAway = 1001,
	ProtocolError = 1002,
	DataNotAccepted = 1003,
	Reserved = 1004,
	DataNotConsistent = 1007,
	PolicyVioldated = 1008,
	MessageIsToBig = 1009,
	ExtensionsNotnegociated = 1010,
	ServerUnexpectedClosed = 1011
}
