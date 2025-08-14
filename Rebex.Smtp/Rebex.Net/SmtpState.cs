namespace Rebex.Net;

public enum SmtpState
{
	Disconnected = 0,
	Connecting = 1,
	Ready = 2,
	Sending = 3,
	Reading = 4,
	Processing = 5,
	Pipelining = 7,
	Disposed = 6
}
