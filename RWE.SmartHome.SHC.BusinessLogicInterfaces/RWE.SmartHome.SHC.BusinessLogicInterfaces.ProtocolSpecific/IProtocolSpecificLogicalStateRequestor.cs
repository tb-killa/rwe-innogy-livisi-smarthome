using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;

public interface IProtocolSpecificLogicalStateRequestor
{
	ProtocolIdentifier ProtocolId { get; }

	void RequestState(LogicalDevice logicalDevice);

	void RequestState(BaseDevice baseDevice);
}
