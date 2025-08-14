using System.Collections.Generic;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender;

public class NotificationResponse
{
	public NotificationSendResult NotificationSendResult { get; private set; }

	public int RemainingQuota { get; private set; }

	public List<KeyValuePair<NotificationChannelType, int>> Quotas { get; private set; }

	public NotificationResponse(NotificationSendResult result, int remainingQuota, List<KeyValuePair<NotificationChannelType, int>> quotas)
	{
		NotificationSendResult = result;
		RemainingQuota = remainingQuota;
		Quotas = quotas;
	}
}
