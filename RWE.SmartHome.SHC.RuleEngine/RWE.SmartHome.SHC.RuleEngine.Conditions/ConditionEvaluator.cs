using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.RuleEngine.DataBinders;
using RWE.SmartHome.SHC.RuleEngine.Triggers;

namespace RWE.SmartHome.SHC.RuleEngine.Conditions;

public class ConditionEvaluator
{
	private class EmptyDataBinder : IDataBinder
	{
		public bool CanEvaluate(DataBinding binding)
		{
			return true;
		}

		public IComparable GetValue(DataBinding binding, EventContext context)
		{
			return null;
		}

		public bool IsAffectedByTrigger(DataBinding binding, DeviceStateTriggerData stateTriggerData)
		{
			return false;
		}

		public DataBinderType GetBinderType(DataBinding binding)
		{
			return DataBinderType.Runtime;
		}
	}

	private readonly List<IDataBinder> binders;

	public ConditionEvaluator(List<IDataBinder> binders)
	{
		this.binders = binders;
	}

	public bool Evaluate(Condition condition, EventContext context)
	{
		bool result = false;
		IComparable value = GetBinder(condition.LeftHandOperand).GetValue(condition.LeftHandOperand, context);
		IComparable value2 = GetBinder(condition.RightHandOperand).GetValue(condition.RightHandOperand, context);
		if (value == null || value2 == null)
		{
			Log.Debug(Module.RuleEngine, "ArithmeticConditionEvaluator: One of the operands is null. Returning false.");
			return false;
		}
		int num = 0;
		try
		{
			num = value.CompareTo(value2);
		}
		catch (ArgumentException ex)
		{
			Log.Error(Module.RuleEngine, "The type of the 2 operands do not match\n" + ex);
			return false;
		}
		if (num < 0)
		{
			if (condition.Operator == ConditionOperator.NotEqual || condition.Operator == ConditionOperator.Less || condition.Operator == ConditionOperator.LessOrEqual)
			{
				result = true;
			}
		}
		else if (num == 0)
		{
			if (condition.Operator == ConditionOperator.Equal || condition.Operator == ConditionOperator.LessOrEqual || condition.Operator == ConditionOperator.GreaterOrEqual)
			{
				result = true;
			}
		}
		else if (condition.Operator == ConditionOperator.NotEqual || condition.Operator == ConditionOperator.Greater || condition.Operator == ConditionOperator.GreaterOrEqual)
		{
			result = true;
		}
		return result;
	}

	public bool IsAffectedByPropertyUpdate(Condition condition, DeviceStateTriggerData stateTriggerData)
	{
		if (GetBinder(condition.LeftHandOperand).IsAffectedByTrigger(condition.LeftHandOperand, stateTriggerData))
		{
			return true;
		}
		if (GetBinder(condition.RightHandOperand).IsAffectedByTrigger(condition.RightHandOperand, stateTriggerData))
		{
			return true;
		}
		return false;
	}

	private IDataBinder GetBinder(DataBinding binding)
	{
		IDataBinder dataBinder = binders.FirstOrDefault((IDataBinder x) => x.CanEvaluate(binding));
		return dataBinder ?? new EmptyDataBinder();
	}
}
