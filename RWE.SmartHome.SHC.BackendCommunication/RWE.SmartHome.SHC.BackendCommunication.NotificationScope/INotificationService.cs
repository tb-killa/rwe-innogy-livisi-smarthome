using System.CodeDom.Compiler;

namespace RWE.SmartHome.SHC.BackendCommunication.NotificationScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public interface INotificationService
{
	NotificationResponse SendNotifications(CustomNotification notification);

	NotificationResponse SendSystemNotifications(SystemNotification notification);
}
