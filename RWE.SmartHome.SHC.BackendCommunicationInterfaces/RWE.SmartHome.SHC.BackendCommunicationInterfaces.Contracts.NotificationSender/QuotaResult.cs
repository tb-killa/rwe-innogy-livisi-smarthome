namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender;

public class QuotaResult
{
	public int RemainingQuota { get; set; }

	public bool RemainingQuotaSpecified { get; set; }

	public NotificationSendResult Result { get; set; }

	public bool ResultSpecified { get; set; }
}
