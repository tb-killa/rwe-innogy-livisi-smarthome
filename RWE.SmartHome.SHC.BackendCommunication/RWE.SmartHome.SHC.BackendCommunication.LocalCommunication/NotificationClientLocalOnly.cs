using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender;

namespace RWE.SmartHome.SHC.BackendCommunication.LocalCommunication;

internal class NotificationClientLocalOnly : INotificationServiceClient
{
	public NotificationResponse SendNotifications(string certificateThumbprint, CustomNotification notification)
	{
		return new NotificationResponse(NotificationSendResult.Success, 0, null);
	}

	public NotificationResponse SendSystemNotifications(SystemNotification notification)
	{
		return new NotificationResponse(NotificationSendResult.Success, 0, null);
	}
}
