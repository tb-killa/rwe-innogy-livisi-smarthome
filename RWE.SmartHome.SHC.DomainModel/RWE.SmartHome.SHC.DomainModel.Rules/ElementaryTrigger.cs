using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.SHC.DomainModel.Rules;

public class ElementaryTrigger
{
	public readonly Trigger Trigger;

	public readonly CustomTrigger CustomTrigger;

	public ElementaryTrigger(Trigger trigger)
	{
		Trigger = trigger;
		CustomTrigger = null;
	}

	public ElementaryTrigger(CustomTrigger customTrigger)
	{
		Trigger = null;
		CustomTrigger = customTrigger;
	}

	public bool HasComplexCondition()
	{
		if (Trigger != null && Trigger.TriggerConditions != null)
		{
			return Trigger.TriggerConditions.Any(ConditionHasCustomLogic);
		}
		return false;
	}

	private static bool ConditionHasCustomLogic(Condition aCondition)
	{
		if (!OperandsHaveCustomLogic(aCondition.LeftHandOperand, aCondition.RightHandOperand))
		{
			return OperandsHaveCustomLogic(aCondition.RightHandOperand, aCondition.LeftHandOperand);
		}
		return true;
	}

	private static bool OperandsHaveCustomLogic(DataBinding firstOperand, DataBinding secondOperand)
	{
		if (firstOperand is FunctionBinding functionBinding)
		{
			if (functionBinding.Function != FunctionIdentifier.GetEventProperty)
			{
				return true;
			}
			if (functionBinding.Parameters.Any((Parameter p) => p.Value is FunctionBinding))
			{
				return true;
			}
			if (!(secondOperand is IConstantBinding))
			{
				return true;
			}
		}
		return false;
	}
}
