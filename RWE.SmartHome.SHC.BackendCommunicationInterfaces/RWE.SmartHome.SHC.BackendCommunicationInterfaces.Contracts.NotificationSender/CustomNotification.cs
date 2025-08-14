namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender;

public class CustomNotification
{
	public string Title { get; set; }

	public string Body { get; set; }

	public NotificationChannelType Channel { get; set; }

	public bool ChannelSpecified { get; set; }

	public string[] UserNames { get; set; }

	public string[] CustomRecipients { get; set; }
}
