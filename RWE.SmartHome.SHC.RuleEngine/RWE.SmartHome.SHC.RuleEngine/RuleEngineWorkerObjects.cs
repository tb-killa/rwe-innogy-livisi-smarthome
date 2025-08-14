using System.Collections.Generic;
using RWE.SmartHome.SHC.RuleEngine.Conditions;
using RWE.SmartHome.SHC.RuleEngine.DataBinders;

namespace RWE.SmartHome.SHC.RuleEngine;

public class RuleEngineWorkerObjects
{
	public ConditionEvaluator ConditionEvaluator { get; set; }

	public List<IDataBinder> DataBinders { get; set; }
}
