namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class ShcSecurityNotificationUpdateEventArgs
{
	public SecurityNotificationType NotificationType { get; set; }

	public SecurityNotificationState NotificationState { get; set; }
}
