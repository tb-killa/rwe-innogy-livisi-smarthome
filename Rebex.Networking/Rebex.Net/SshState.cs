namespace Rebex.Net;

public enum SshState
{
	None = 0,
	Connecting = 1,
	KeyExchange = 2,
	Ready = 4,
	Closed = 5
}
