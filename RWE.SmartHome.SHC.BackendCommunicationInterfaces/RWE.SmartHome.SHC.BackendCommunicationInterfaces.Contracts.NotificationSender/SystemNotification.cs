using System.Collections.Generic;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender;

public class SystemNotification
{
	public string ProductId { get; set; }

	public List<KeyValuePair<string, string>> Parameters { get; set; }

	public string Type { get; set; }

	public string Class { get; set; }
}
