namespace Rebex.Net;

public enum ProxySocketExceptionStatus
{
	ConnectFailure,
	ConnectionClosed,
	SocketError,
	NameResolutionFailure,
	ProtocolError,
	ProxyNameResolutionFailure,
	ReceiveFailure,
	UnclassifiableError,
	ServerProtocolViolation,
	AsyncError,
	NotConnected,
	SendRetryTimeout,
	Timeout
}
