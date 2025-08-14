namespace Org.Mentalis.Security.Ssl.Shared;

internal struct SslHandshakeStatus
{
	public byte[] Message;

	public SslStatus Status;

	public SslHandshakeStatus(SslStatus status, byte[] message)
	{
		Status = status;
		Message = message;
	}
}
