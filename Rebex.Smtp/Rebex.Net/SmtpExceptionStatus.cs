namespace Rebex.Net;

public enum SmtpExceptionStatus
{
	ConnectFailure,
	ConnectionClosed,
	SocketError,
	NameResolutionFailure,
	Pending,
	ProtocolError,
	ProxyNameResolutionFailure,
	ReceiveFailure,
	OperationAborted,
	UnclassifiableError,
	SendFailure,
	ServerProtocolViolation,
	Timeout,
	AsyncError,
	OperationFailure
}
