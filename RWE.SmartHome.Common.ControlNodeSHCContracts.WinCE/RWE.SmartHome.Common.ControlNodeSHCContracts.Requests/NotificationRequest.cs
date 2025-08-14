using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;

public class NotificationRequest : BaseRequest
{
	public NotificationAction Action { get; set; }

	public NotificationType NotificationType { get; set; }

	public NotificationRequest()
	{
	}

	public NotificationRequest(NotificationAction action, NotificationType notificationType)
	{
		Action = action;
		NotificationType = notificationType;
	}
}
