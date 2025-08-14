namespace RWE.SmartHome.SHC.WebSocketsService.Client;

public enum WSErrorCode
{
	Success,
	InvalidMessageType,
	Faulted,
	NativeError,
	NotAWebSocket,
	UnsupportedVersion,
	UnsupportedProtocol,
	HeaderError,
	ConnectionClosedPrematurely,
	InvalidState
}
