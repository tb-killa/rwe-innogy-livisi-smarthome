using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using SmartHome.SHC.API.Configuration;
using SmartHome.SHC.API.PropertyDefinition;

namespace RWE.SmartHome.SHC.CoreApiConverters;

public static class TriggerConverters
{
	public static ComparisonOperator ToApiComparisonOperator(this Operator coreOperator)
	{
		return (ComparisonOperator)coreOperator;
	}

	public static ComparisonOperator ToApiComparisonOperator(this ConditionOperator coreOperator)
	{
		return coreOperator switch
		{
			ConditionOperator.Less => ComparisonOperator.Less, 
			ConditionOperator.LessOrEqual => ComparisonOperator.LessOrEqual, 
			ConditionOperator.Equal => ComparisonOperator.Equal, 
			ConditionOperator.GreaterOrEqual => ComparisonOperator.GreaterOrEqual, 
			ConditionOperator.Greater => ComparisonOperator.Greater, 
			ConditionOperator.NotEqual => ComparisonOperator.NotEqual, 
			_ => throw new NotSupportedException("Invalid core condition operator: " + coreOperator), 
		};
	}

	public static Operator ToCoreComparisonOperator(this ComparisonOperator apiOperator)
	{
		return (Operator)apiOperator;
	}

	public static global::SmartHome.SHC.API.Configuration.TriggerAction ToApiTriggerAction(this RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.TriggerAction coreAction)
	{
		return coreAction switch
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.TriggerAction.None => global::SmartHome.SHC.API.Configuration.TriggerAction.None, 
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.TriggerAction.Toggle => global::SmartHome.SHC.API.Configuration.TriggerAction.Toggle, 
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.TriggerAction.TurnOff => global::SmartHome.SHC.API.Configuration.TriggerAction.TurnOff, 
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.TriggerAction.TurnOn => global::SmartHome.SHC.API.Configuration.TriggerAction.TurnOn, 
			_ => throw new ArgumentException("Invalid core action"), 
		};
	}

	public static RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.TriggerAction ToCoreTriggerAction(this global::SmartHome.SHC.API.Configuration.TriggerAction apiAction)
	{
		return apiAction switch
		{
			global::SmartHome.SHC.API.Configuration.TriggerAction.None => RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.TriggerAction.None, 
			global::SmartHome.SHC.API.Configuration.TriggerAction.Toggle => RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.TriggerAction.Toggle, 
			global::SmartHome.SHC.API.Configuration.TriggerAction.TurnOff => RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.TriggerAction.TurnOff, 
			global::SmartHome.SHC.API.Configuration.TriggerAction.TurnOn => RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.TriggerAction.TurnOn, 
			_ => throw new ArgumentException("Invalid API action"), 
		};
	}

	public static global::SmartHome.SHC.API.Configuration.TriggerCondition ToApiTriggerCondition(this RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.TriggerCondition coreCondition)
	{
		return new global::SmartHome.SHC.API.Configuration.TriggerCondition(coreCondition.Operator.ToApiComparisonOperator(), coreCondition.Threshold.ToApiProperty());
	}

	public static bool IsApiCompatible(this Condition condition)
	{
		GetConditionMembers(condition, out var function, out var constant);
		if (function != null)
		{
			return constant != null;
		}
		return false;
	}

	public static global::SmartHome.SHC.API.Configuration.TriggerCondition ToApiTriggerCondition(this Condition condition)
	{
		GetConditionMembers(condition, out var function, out var constant);
		if (function != null && constant != null)
		{
			Parameter parameter = null;
			parameter = ((!IsTimeFunction(function.Function)) ? function.Parameters.Single((Parameter p) => p.Name == "EventPropertyName") : (from p in function.Parameters
				where p.Value is FunctionBinding
				select (p.Value as FunctionBinding).Parameters.Single((Parameter prm) => prm.Name == "EventPropertyName")).FirstOrDefault());
			return new global::SmartHome.SHC.API.Configuration.TriggerCondition(condition.Operator.ToApiComparisonOperator(), BuildApiProperty(constant, (parameter.Value as ConstantStringBinding).Value));
		}
		return null;
	}

	private static void GetConditionMembers(Condition condition, out FunctionBinding function, out IConstantBinding constant)
	{
		if (CheckFunctionValidOperands(condition.LeftHandOperand, condition.RightHandOperand))
		{
			function = condition.RightHandOperand as FunctionBinding;
			constant = condition.LeftHandOperand as IConstantBinding;
		}
		else if (CheckFunctionValidOperands(condition.RightHandOperand, condition.LeftHandOperand))
		{
			function = condition.LeftHandOperand as FunctionBinding;
			constant = condition.RightHandOperand as IConstantBinding;
		}
		else
		{
			function = null;
			constant = null;
		}
	}

	private static bool CheckFunctionValidOperands(DataBinding firstOperand, DataBinding secondOperand)
	{
		if ((firstOperand is IConstantBinding && secondOperand is FunctionBinding && (secondOperand as FunctionBinding).Function == FunctionIdentifier.GetEventProperty && (secondOperand as FunctionBinding).Parameters.Any((Parameter p) => p.Name == "EventPropertyName" && p.Value is ConstantStringBinding)) || (secondOperand is FunctionBinding && IsTimeFunction((secondOperand as FunctionBinding).Function)))
		{
			return true;
		}
		return false;
	}

	private static bool IsTimeFunction(FunctionIdentifier function)
	{
		if (function == FunctionIdentifier.GetCurrentDateTime || function == FunctionIdentifier.GetDayOfCentury || function == FunctionIdentifier.GetDayOfMonth || function == FunctionIdentifier.GetDayOfWeek || function == FunctionIdentifier.GetHour || function == FunctionIdentifier.GetMinute || function == FunctionIdentifier.GetMonth || function == FunctionIdentifier.GetMonthOfCentury || function == FunctionIdentifier.GetWeekdayOfMonth || function == FunctionIdentifier.GetWeekOfCentury || function == FunctionIdentifier.GetYear)
		{
			return true;
		}
		return false;
	}

	private static global::SmartHome.SHC.API.PropertyDefinition.Property BuildApiProperty(IConstantBinding val, string name)
	{
		if (val is ConstantStringBinding)
		{
			return new global::SmartHome.SHC.API.PropertyDefinition.StringProperty(name, (val as ConstantStringBinding).Value);
		}
		if (val is ConstantBooleanBinding)
		{
			return new global::SmartHome.SHC.API.PropertyDefinition.BooleanProperty(name, (val as ConstantBooleanBinding).Value);
		}
		if (val is ConstantDateTimeBinding)
		{
			return new global::SmartHome.SHC.API.PropertyDefinition.DateTimeProperty(name, (val as ConstantDateTimeBinding).Value);
		}
		if (val is ConstantNumericBinding)
		{
			return new global::SmartHome.SHC.API.PropertyDefinition.NumericProperty(name, (val as ConstantNumericBinding).Value);
		}
		throw new NotSupportedException("Constant binding not supported by API: " + val.GetType().Name);
	}

	public static RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.TriggerCondition ToCoreTriggerCondition(this global::SmartHome.SHC.API.Configuration.TriggerCondition apiCondition)
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.TriggerCondition triggerCondition = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.TriggerCondition();
		triggerCondition.Operator = apiCondition.Operator.ToCoreComparisonOperator();
		triggerCondition.Threshold = apiCondition.Threshold.ToCoreProperty(includeTimestamp: false);
		return triggerCondition;
	}

	public static global::SmartHome.SHC.API.Configuration.Trigger ToApiInteractionTrigger(this RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Trigger trigger, InteractionDetails interactionDetails)
	{
		return new global::SmartHome.SHC.API.Configuration.Trigger(Guid.NewGuid(), trigger.Entity.EntityIdAsGuid(), interactionDetails, trigger.EventType, (trigger.TriggerConditions != null) ? trigger.TriggerConditions.ConvertAll((Condition c) => c.ToApiTriggerCondition()) : new List<global::SmartHome.SHC.API.Configuration.TriggerCondition>(), 0);
	}

	public static global::SmartHome.SHC.API.Configuration.CustomTrigger ToApiCustomTrigger(this RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.CustomTrigger trigger)
	{
		global::SmartHome.SHC.API.Configuration.CustomTrigger customTrigger = new global::SmartHome.SHC.API.Configuration.CustomTrigger();
		customTrigger.Id = trigger.Id;
		customTrigger.Entity = trigger.Entity.ToApi();
		customTrigger.Properties = ((trigger.Properties != null) ? trigger.Properties.ConvertAll((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property prop) => prop.ToApiProperty()) : new List<global::SmartHome.SHC.API.PropertyDefinition.Property>());
		customTrigger.TriggerType = trigger.Type;
		return customTrigger;
	}
}
