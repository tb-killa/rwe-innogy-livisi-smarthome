using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;

namespace RWE.SmartHome.SHC.RuleEngine;

public interface IRuleEngineObjectsFactory
{
	RuleEngineWorkerObjects CreateRuleEngineWorkerObjects(ILogicalDeviceStateRepository statesRepository, IProtocolMultiplexer protocolMultiplexer, IRepository configRepository);
}
