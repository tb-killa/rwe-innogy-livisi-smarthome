using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.Core;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

public interface IExternalCommandDispatcher : IRequestProcessor, IService
{
	void SendNotification(BaseNotification notification);
}
