using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;

namespace RWE.SmartHome.SHC.ChannelInterfaces;

public interface IBaseChannel
{
	void SubscribeRequestProcessor(IRequestProcessor processor);

	void QueueNotification(BaseNotification notification);
}
