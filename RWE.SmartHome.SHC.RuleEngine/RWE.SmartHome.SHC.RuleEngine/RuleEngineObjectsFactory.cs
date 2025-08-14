using System.Collections.Generic;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.RuleEngine.Conditions;
using RWE.SmartHome.SHC.RuleEngine.DataBinders;

namespace RWE.SmartHome.SHC.RuleEngine;

public class RuleEngineObjectsFactory : IRuleEngineObjectsFactory
{
	public RuleEngineWorkerObjects CreateRuleEngineWorkerObjects(ILogicalDeviceStateRepository statesRepository, IProtocolMultiplexer protocolMultiplexer, IRepository configRepository)
	{
		RuleEngineWorkerObjects ruleEngineWorkerObjects = new RuleEngineWorkerObjects();
		ruleEngineWorkerObjects.DataBinders = CreateDataBinders(statesRepository, configRepository, protocolMultiplexer);
		ruleEngineWorkerObjects.ConditionEvaluator = CreateConditionEvaluator(statesRepository, ruleEngineWorkerObjects.DataBinders);
		return ruleEngineWorkerObjects;
	}

	public ConditionEvaluator CreateConditionEvaluator(ILogicalDeviceStateRepository repository, List<IDataBinder> binders)
	{
		return new ConditionEvaluator(binders);
	}

	public List<IDataBinder> CreateDataBinders(ILogicalDeviceStateRepository statesRepository, IRepository configRepository, IProtocolMultiplexer protocolMultiplexer)
	{
		List<IDataBinder> list = new List<IDataBinder>();
		list.Add(new ConstantBooleanBinder());
		list.Add(new ConstantDateTimeBinder());
		list.Add(new ConstantStringBinder());
		list.Add(new ConstantNumericBinder());
		list.Add(new FunctionBinder(list, statesRepository, configRepository, protocolMultiplexer));
		list.Add(new LinkBinder());
		return list;
	}
}
