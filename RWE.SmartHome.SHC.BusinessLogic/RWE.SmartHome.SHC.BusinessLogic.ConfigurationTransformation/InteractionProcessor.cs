using System;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.SHC.BusinessLogic.ConfigurationTransformation;

internal class InteractionProcessor
{
	public Action<Guid> OnBaseDeviceFound { get; set; }

	public Action<Guid> OnLogicalDeviceFound { get; set; }

	public Action<ActionDescription> OnActionFound { get; set; }

	public Action<Trigger> OnTriggerFound { get; set; }

	public Action<CustomTrigger> OnCustomTriggerFound { get; set; }

	public void Process(Interaction interaction)
	{
		foreach (Rule rule in interaction.Rules)
		{
			Process(rule);
		}
	}

	private void Process(Rule rule)
	{
		if (rule.Triggers != null)
		{
			rule.Triggers.ForEach(Process);
		}
		if (rule.CustomTriggers != null)
		{
			rule.CustomTriggers.ForEach(Process);
		}
		if (rule.Conditions != null)
		{
			rule.Conditions.ForEach(Process);
		}
		if (rule.Actions != null)
		{
			rule.Actions.ForEach(Process);
		}
	}

	private void Process(Trigger trigger)
	{
		if (trigger != null)
		{
			if (OnTriggerFound != null)
			{
				OnTriggerFound(trigger);
			}
			OnEntityLinkFound(trigger.Entity);
			if (trigger.TriggerConditions != null)
			{
				trigger.TriggerConditions.ForEach(Process);
			}
		}
	}

	private void Process(CustomTrigger customTrigger)
	{
		if (customTrigger != null)
		{
			if (OnCustomTriggerFound != null)
			{
				OnCustomTriggerFound(customTrigger);
			}
			OnEntityLinkFound(customTrigger.Entity);
		}
	}

	private void Process(Condition condition)
	{
		Process(condition.LeftHandOperand);
		Process(condition.RightHandOperand);
	}

	private void Process(ActionDescription action)
	{
		if (action == null)
		{
			return;
		}
		if (OnActionFound != null)
		{
			OnActionFound(action);
		}
		OnEntityLinkFound(action.Target);
		if (action.Data != null)
		{
			action.Data.ForEach(delegate(Parameter x)
			{
				Process(x.Value);
			});
		}
	}

	private void Process(DataBinding dataBinding)
	{
		if (dataBinding is IConstantBinding || !(dataBinding is FunctionBinding { Function: var function } functionBinding))
		{
			return;
		}
		switch (function)
		{
		case FunctionIdentifier.GetEntityStateProperty:
		case FunctionIdentifier.GetMinutesSinceLastChange:
			OnEntityLinkFound(GetEntityStateEntity(functionBinding));
			return;
		case FunctionIdentifier.GetEventProperty:
			return;
		}
		functionBinding.Parameters.ForEach(delegate(Parameter x)
		{
			Process(x.Value);
		});
	}

	private void OnEntityLinkFound(LinkBinding linkBinding)
	{
		if (linkBinding != null)
		{
			if (linkBinding.LinkType == EntityType.LogicalDevice && OnLogicalDeviceFound != null)
			{
				OnLogicalDeviceFound(linkBinding.EntityIdAsGuid());
			}
			if (linkBinding.LinkType == EntityType.BaseDevice && OnBaseDeviceFound != null)
			{
				OnBaseDeviceFound(linkBinding.EntityIdAsGuid());
			}
		}
	}

	private LinkBinding GetEntityStateEntity(FunctionBinding functionBinding)
	{
		Parameter parameter = functionBinding.Parameters.FirstOrDefault((Parameter x) => x.Name == "EntityId");
		if (parameter == null)
		{
			return null;
		}
		return parameter.Value as LinkBinding;
	}
}
