using System.Net;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;

public class SenderService<T> : LemonbeatService<T>
{
	protected SenderService(ILemonbeatCommunication aggregator, ServiceType serviceId, string defaultNamespace)
		: base(aggregator, serviceId, defaultNamespace, ServiceCommunicationType.SenderOnly)
	{
	}

	protected sealed override void Handle(int gatewayId, IPAddress address, T message)
	{
	}
}
