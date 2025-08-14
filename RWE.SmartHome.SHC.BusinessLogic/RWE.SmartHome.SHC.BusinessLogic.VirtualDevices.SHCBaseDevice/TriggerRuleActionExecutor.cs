using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.CoreEvents;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DomainModel.Actions;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.SHCBaseDevice;

internal class TriggerRuleActionExecutor
{
	private IEventManager eventManager;

	public TriggerRuleActionExecutor(IEventManager eventManager)
	{
		this.eventManager = eventManager;
	}

	internal ExecutionResult HandleRequest(List<Parameter> actionParameters)
	{
		try
		{
			Guid[] ruleIds = (from x in actionParameters
				where x.Name == "RuleId"
				select x.Value as ConstantStringBinding into y
				select new Guid(y.Value)).ToArray();
			eventManager.GetEvent<TriggerRulesEvent>().Publish(new TriggerRulesEventArgs(ruleIds));
		}
		catch (Exception ex)
		{
			Log.Exception(Module.BusinessLogic, "TriggerRuleActionExecutor", ex, "Could not trigger rule.");
			return ExecutionResult.Error(ex.Message);
		}
		return ExecutionResult.Success();
	}
}
