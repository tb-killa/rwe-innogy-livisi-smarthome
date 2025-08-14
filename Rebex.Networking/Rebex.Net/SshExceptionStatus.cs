namespace Rebex.Net;

public enum SshExceptionStatus
{
	UnclassifiableError,
	ConnectFailure,
	ConnectionClosed,
	ProtocolError,
	UnexpectedMessage,
	Timeout,
	OperationFailure,
	PasswordChangeRequired,
	SocketError
}
