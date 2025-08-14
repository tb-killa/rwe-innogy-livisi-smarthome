namespace Rebex.Net;

public enum NetworkSessionExceptionStatus
{
	UnclassifiableError,
	OperationFailure,
	ConnectFailure,
	ConnectionClosed,
	SocketError,
	NameResolutionFailure,
	ProtocolError,
	OperationAborted,
	ServerProtocolViolation,
	Timeout,
	AsyncError
}
