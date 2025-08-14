using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.RuleEngine.Triggers;

namespace RWE.SmartHome.SHC.RuleEngine.DataBinders;

internal abstract class BaseDataBinder<TDataBinding> : IDataBinder where TDataBinding : DataBinding
{
	public bool CanEvaluate(DataBinding binding)
	{
		return binding as TDataBinding != null;
	}

	public IComparable GetValue(DataBinding binding, EventContext context)
	{
		if (!CanEvaluate(binding))
		{
			throw new InvalidOperationException();
		}
		return GetValue(binding as TDataBinding, context);
	}

	public virtual bool IsAffectedByTrigger(DataBinding binding, DeviceStateTriggerData stateTriggerData)
	{
		return IsAffectedByTrigger(binding as TDataBinding, stateTriggerData);
	}

	public abstract DataBinderType GetBinderType(DataBinding binding);

	protected abstract IComparable GetValue(TDataBinding binding, EventContext context);

	protected virtual bool IsAffectedByTrigger(TDataBinding binding, DeviceStateTriggerData stateTriggerData)
	{
		return false;
	}
}
