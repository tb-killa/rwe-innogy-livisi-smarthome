using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces;

public interface INotificationServiceClient
{
	NotificationResponse SendNotifications(string certificateThumbprint, CustomNotification notifications);

	NotificationResponse SendSystemNotifications(SystemNotification notification);
}
