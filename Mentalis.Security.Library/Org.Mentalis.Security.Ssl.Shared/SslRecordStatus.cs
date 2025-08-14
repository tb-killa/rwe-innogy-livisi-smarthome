namespace Org.Mentalis.Security.Ssl.Shared;

internal struct SslRecordStatus
{
	public byte[] Buffer;

	public byte[] Decrypted;

	public SslStatus Status;

	public SslRecordStatus(SslStatus status, byte[] buffer, byte[] decrypted)
	{
		Status = status;
		Buffer = buffer;
		Decrypted = decrypted;
	}
}
