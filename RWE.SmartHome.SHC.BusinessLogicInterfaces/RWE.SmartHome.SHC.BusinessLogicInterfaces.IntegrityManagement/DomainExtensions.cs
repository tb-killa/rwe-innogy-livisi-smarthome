using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.IntegrityManagement;

public static class DomainExtensions
{
	public static bool RemoveLogicalDevice(this Interaction interaction, Guid logicalDeviceId)
	{
		bool wasRemoved = false;
		foreach (Rule rule in interaction.Rules)
		{
			if (rule.Triggers != null)
			{
				rule.Triggers.ForEach(delegate(Trigger t)
				{
					if (t.TriggerConditions != null)
					{
						wasRemoved |= t.TriggerConditions.RemoveAll((Condition c) => c.Contains(logicalDeviceId)) > 0;
					}
				});
				wasRemoved |= rule.Triggers.RemoveAll((Trigger t) => t.Entity.LinkBoundToLogicalDevice(logicalDeviceId)) > 0;
			}
			if (rule.CustomTriggers != null)
			{
				wasRemoved |= rule.CustomTriggers.RemoveAll((CustomTrigger ct) => ct.Entity.LinkBoundToLogicalDevice(logicalDeviceId)) > 0;
			}
			if (rule.Conditions != null)
			{
				wasRemoved |= rule.Conditions.RemoveAll((Condition c) => c.Contains(logicalDeviceId)) > 0;
			}
			if (rule.Actions == null)
			{
				continue;
			}
			wasRemoved |= rule.Actions.RemoveAll((ActionDescription a) => a.Target.LinkBoundToLogicalDevice(logicalDeviceId) || (a.Data != null && a.Data.Any((Parameter p) => p != null && p.Value is FunctionBinding && (p.Value as FunctionBinding).Contains(logicalDeviceId)))) > 0;
		}
		return wasRemoved;
	}

	private static bool LinkBoundToLogicalDevice(this LinkBinding link, Guid logicalDevice)
	{
		if (link != null && link.LinkType == EntityType.LogicalDevice)
		{
			return link.EntityIdAsGuid() == logicalDevice;
		}
		return false;
	}

	public static bool RemoveEmptyRules(this Interaction interaction)
	{
		return interaction.Rules.RemoveAll((Rule r) => (r.Actions == null || r.Actions.Count == 0) && (r.CustomTriggers == null || r.CustomTriggers.Count == 0) && (r.Triggers == null || r.Triggers.Count == 0) && (r.Conditions == null || r.Conditions.Count == 0)) > 0;
	}

	public static bool HasCustomLogic(this Rule rule)
	{
		if (rule.ConditionEvaluationDelay <= 0 && (rule.Conditions == null || rule.Conditions.Count <= 0))
		{
			if (rule.Triggers != null)
			{
				return (from tc in rule.Triggers.Where((Trigger t) => t != null && t.TriggerConditions != null).SelectMany((Trigger t) => t.TriggerConditions)
					where tc != null
					select tc).Any((Condition c) => ConditionHasCustomLogic(c));
			}
			return false;
		}
		return true;
	}

	public static bool Contains(this FunctionBinding function, Guid logicalDeviceId)
	{
		if (function == null)
		{
			return false;
		}
		if (function.Function == FunctionIdentifier.GetEntityStateProperty || function.Function == FunctionIdentifier.GetMinutesSinceLastChange)
		{
			Parameter parameter = function.Parameters.SingleOrDefault((Parameter p) => p.Name == "EntityId");
			if (parameter == null)
			{
				return false;
			}
			try
			{
				if ((parameter.Value as LinkBinding).EntityIdAsGuid() == logicalDeviceId)
				{
					return true;
				}
			}
			catch
			{
				return false;
			}
		}
		foreach (Parameter parameter2 in function.Parameters)
		{
			if (parameter2.Value is FunctionBinding function2 && function2.Contains(logicalDeviceId))
			{
				return true;
			}
		}
		return false;
	}

	public static bool Contains(this Condition condition, Guid logicalDeviceId)
	{
		logicalDeviceId.ToString("N");
		if (condition.LeftHandOperand is FunctionBinding function && function.Contains(logicalDeviceId))
		{
			return true;
		}
		if (condition.RightHandOperand is FunctionBinding function2 && function2.Contains(logicalDeviceId))
		{
			return true;
		}
		return false;
	}

	public static Guid ToGuid(this string s)
	{
		try
		{
			return new Guid(s);
		}
		catch
		{
			return Guid.Empty;
		}
	}

	public static bool Contains(this DataBinding dataBinding, Guid logicalDeviceId)
	{
		bool result = false;
		if (dataBinding is FunctionBinding)
		{
			result = (dataBinding as FunctionBinding).Parameters.Select((Parameter param) => param.Value).Any(delegate(DataBinding db)
			{
				if (db is ConstantStringBinding && (db as ConstantStringBinding).Value.ToGuid() == logicalDeviceId)
				{
					return true;
				}
				return db is FunctionBinding && db.Contains(logicalDeviceId);
			});
		}
		return result;
	}

	public static bool HasDynamicSettings(this Interaction interaction)
	{
		return interaction.Rules.SelectMany((Rule r) => r.Actions).SelectMany((ActionDescription a) => a.Data).Any((Parameter p) => p.Value is FunctionBinding && ((p.Value as FunctionBinding).Function == FunctionIdentifier.GetEntityStateProperty || (p.Value as FunctionBinding).Function == FunctionIdentifier.GetMinutesSinceLastChange));
	}

	public static bool IsEmpty(this Interaction interaction)
	{
		if (interaction.Rules == null || !interaction.Rules.Any())
		{
			return true;
		}
		bool flag = true;
		foreach (Rule rule in interaction.Rules)
		{
			flag = rule.IsEmpty();
			if (!flag)
			{
				break;
			}
		}
		return flag;
	}

	public static bool IsEmpty(this Rule rule)
	{
		if ((rule.Actions == null || !rule.Actions.Any()) && (rule.Triggers == null || !rule.Triggers.Any()) && (rule.CustomTriggers == null || !rule.CustomTriggers.Any()))
		{
			if (rule.Conditions != null)
			{
				return !rule.Conditions.Any();
			}
			return true;
		}
		return false;
	}

	public static List<BaseDevice> GetReferencedDevices(this Condition condition, IRepository configRepository)
	{
		List<BaseDevice> list = new List<BaseDevice>();
		list.AddRange(GetReferencedDevices(condition.LeftHandOperand, configRepository));
		list.AddRange(GetReferencedDevices(condition.RightHandOperand, configRepository));
		return list;
	}

	public static List<BaseDevice> GetReferencedDevices(this ActionDescription action, IRepository configRepository)
	{
		return action.Data.Select((Parameter p) => p.Value).SelectMany((DataBinding db) => GetReferencedDevices(db, configRepository)).ToList();
	}

	private static List<BaseDevice> GetReferencedDevices(DataBinding data, IRepository configRepository)
	{
		List<BaseDevice> list = new List<BaseDevice>();
		if (data is FunctionBinding functionBinding && (functionBinding.Function == FunctionIdentifier.GetEntityStateProperty || functionBinding.Function == FunctionIdentifier.GetMinutesSinceLastChange))
		{
			Parameter parameter = functionBinding.Parameters.SingleOrDefault((Parameter p) => p.Name == "EntityId" && p.Value is LinkBinding);
			if (parameter != null)
			{
				BaseDevice baseDevice = null;
				LinkBinding linkBinding = parameter.Value as LinkBinding;
				if (linkBinding.LinkType == EntityType.BaseDevice)
				{
					baseDevice = configRepository.GetBaseDevice(linkBinding.EntityIdAsGuid());
				}
				else if (linkBinding.LinkType == EntityType.LogicalDevice)
				{
					LogicalDevice logicalDevice = configRepository.GetLogicalDevice(linkBinding.EntityIdAsGuid());
					if (logicalDevice != null)
					{
						baseDevice = logicalDevice.BaseDevice;
					}
				}
				if (baseDevice != null)
				{
					list.Add(baseDevice);
				}
			}
		}
		return list;
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

	public static bool IsInteractionValid(this Interaction interaction)
	{
		DateTime interactionValidFromDate = interaction.GetInteractionValidFromDate();
		DateTime interactionValidToDate = interaction.GetInteractionValidToDate();
		DateTime utcNow = DateTime.UtcNow;
		if (interactionValidFromDate <= utcNow)
		{
			return utcNow < interactionValidToDate;
		}
		return false;
	}

	public static bool IsInteractionFrozen(this Interaction interaction, DateTime lastExecution)
	{
		return lastExecution.AddSeconds(interaction.Freezetime) > DateTime.UtcNow;
	}

	public static DateTime GetInteractionValidFromDate(this Interaction interaction)
	{
		if (!interaction.ValidFrom.HasValue)
		{
			return DateTime.MinValue;
		}
		return interaction.ValidFrom.Value;
	}

	public static DateTime GetInteractionValidToDate(this Interaction interaction)
	{
		if (!interaction.ValidTo.HasValue)
		{
			return DateTime.MaxValue;
		}
		return interaction.ValidTo.Value;
	}

	public static bool IsCustomTimeTrigger(this CustomTrigger ct)
	{
		if (!(ct.Type == "BySecondTrigger") && !(ct.Type == "HourlyTrigger") && !(ct.Type == "DailyTrigger") && !(ct.Type == "WeeklyTrigger") && !(ct.Type == "DayOfWeekMonthlyTrigger"))
		{
			return ct.Type == "DayOfMonthTrigger";
		}
		return true;
	}
}
