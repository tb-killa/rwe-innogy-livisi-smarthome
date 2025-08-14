using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.CoreEvents;

public class TriggerRulesEventArgs : EventArgs
{
	public Guid[] RuleIds { get; private set; }

	public TriggerRulesEventArgs(Guid[] ruleIds)
	{
		RuleIds = ruleIds;
	}
}
