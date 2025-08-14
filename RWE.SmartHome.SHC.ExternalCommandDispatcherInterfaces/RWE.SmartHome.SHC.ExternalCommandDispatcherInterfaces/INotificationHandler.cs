using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

public interface INotificationHandler
{
	void SendNotification(BaseNotification notification);
}
