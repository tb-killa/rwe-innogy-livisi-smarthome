namespace Org.Mentalis.Security.Ssl.Shared;

internal struct CompatibilityResult
{
	public RecordLayer RecordLayer;

	public SslRecordStatus Status;

	public CompatibilityResult(RecordLayer rl, SslRecordStatus status)
	{
		RecordLayer = rl;
		Status = status;
	}
}
