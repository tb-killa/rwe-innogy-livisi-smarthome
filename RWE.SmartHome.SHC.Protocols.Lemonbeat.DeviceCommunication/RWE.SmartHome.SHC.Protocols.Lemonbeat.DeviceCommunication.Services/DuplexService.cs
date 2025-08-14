using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;

public abstract class DuplexService<T> : LemonbeatService<T>
{
	protected DuplexService(ILemonbeatCommunication aggregator, ServiceType serviceId, string defaultNamespace)
		: base(aggregator, serviceId, defaultNamespace, ServiceCommunicationType.Bidirectional)
	{
	}
}
