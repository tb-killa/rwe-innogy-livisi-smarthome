using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.RuleEngine.Triggers;

namespace RWE.SmartHome.SHC.RuleEngine.DataBinders;

public interface IDataBinder
{
	bool CanEvaluate(DataBinding binding);

	IComparable GetValue(DataBinding binding, EventContext context);

	DataBinderType GetBinderType(DataBinding binding);

	bool IsAffectedByTrigger(DataBinding binding, DeviceStateTriggerData stateTriggerData);
}
