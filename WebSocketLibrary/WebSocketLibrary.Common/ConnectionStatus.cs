namespace WebSocketLibrary.Common;

public enum ConnectionStatus : byte
{
	Connecting = 1,
	Connected = 2,
	Closing = 3,
	Closed = 4,
	Undefined = 0
}
