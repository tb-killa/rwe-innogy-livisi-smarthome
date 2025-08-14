using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.EntityRules;

public class InteractionHasValidDevicesRule : EntityRule<Interaction>
{
	private readonly IRepository repository;

	public InteractionHasValidDevicesRule(IRepository repository)
	{
		this.repository = repository;
	}

	public override ValidationResult Check(Interaction newInteraction, Interaction oldInteraction)
	{
		ValidationResult validationResult = new ValidationResult();
		if (newInteraction == null || newInteraction.Rules == null || newInteraction.Rules.Count == 0)
		{
			return validationResult;
		}
		List<Rule> rules = newInteraction.Rules;
		foreach (Rule item in rules)
		{
			validationResult.Add(ValidateRule(item), $"Rule {item.Id} validation failed");
		}
		return validationResult;
	}

	private ValidationResult ValidateRule(Rule rule)
	{
		ValidationResult validationResult = new ValidationResult();
		validationResult.Add(ValidateTriggers(rule.Triggers));
		validationResult.Add(ValidateCustomTriggers(rule.CustomTriggers));
		validationResult.Add(ValidateRuleConditions(rule.Conditions));
		validationResult.Add(ValidateActions(rule.Actions));
		return validationResult;
	}

	private ValidationResult ValidateRuleConditions(IEnumerable<Condition> conditions)
	{
		ValidationResult validationResult = new ValidationResult();
		if (conditions == null)
		{
			return validationResult;
		}
		foreach (Condition condition in conditions)
		{
			validationResult.Add(ValidateRuleCondition(condition), "Rule condition validation error");
		}
		return validationResult;
	}

	private ValidationResult ValidateRuleCondition(Condition condition)
	{
		ValidationResult validationResult = new ValidationResult();
		if (condition == null)
		{
			return validationResult;
		}
		validationResult.Add(ValidateDataBinding(condition.LeftHandOperand), "Left hand operand validation error");
		validationResult.Add(ValidateDataBinding(condition.RightHandOperand), "Right hand operand validation error");
		return validationResult;
	}

	private ValidationResult ValidateDataBinding(DataBinding dataBinding)
	{
		ValidationResult validationResult = new ValidationResult();
		if (dataBinding == null)
		{
			validationResult.Add("Operand is null");
			return validationResult;
		}
		if (dataBinding is FunctionBinding fnBinding)
		{
			ValidateFunctionBinding(fnBinding);
		}
		return validationResult;
	}

	private ValidationResult ValidateFunctionBinding(FunctionBinding fnBinding)
	{
		ValidationResult validationResult = new ValidationResult();
		if (fnBinding == null || fnBinding.Parameters == null)
		{
			return validationResult;
		}
		foreach (Parameter parameter in fnBinding.Parameters)
		{
			if (parameter.Value is LinkBinding linkBinding)
			{
				validationResult.Add(CheckDeviceExists(linkBinding), FormatInvalidTarget(linkBinding));
			}
			if (parameter.Value is FunctionBinding fnBinding2)
			{
				validationResult.Add(ValidateFunctionBinding(fnBinding2));
			}
		}
		return validationResult;
	}

	private ValidationResult ValidateCustomTriggers(IEnumerable<CustomTrigger> customTriggers)
	{
		ValidationResult validationResult = new ValidationResult();
		if (customTriggers == null)
		{
			return validationResult;
		}
		foreach (CustomTrigger customTrigger in customTriggers)
		{
			validationResult.Add(ValidateCustomTrigger(customTrigger), $"Custom trigger {customTrigger.Id} validation error");
		}
		return validationResult;
	}

	private ValidationResult ValidateCustomTrigger(CustomTrigger trigger)
	{
		ValidationResult validationResult = new ValidationResult();
		validationResult.Add(CheckDeviceExists(trigger.Entity), FormatInvalidTarget(trigger.Entity));
		return validationResult;
	}

	private ValidationResult ValidateTriggers(IEnumerable<Trigger> triggers)
	{
		ValidationResult validationResult = new ValidationResult();
		if (triggers == null)
		{
			return validationResult;
		}
		foreach (Trigger trigger in triggers)
		{
			validationResult.Add(ValidateTrigger(trigger), $"Trigger {trigger.Id} validation error");
		}
		return validationResult;
	}

	private ValidationResult ValidateTrigger(Trigger trigger)
	{
		ValidationResult validationResult = new ValidationResult();
		validationResult.Add(CheckDeviceExists(trigger.Entity), FormatInvalidTarget(trigger.Entity));
		return validationResult;
	}

	private ValidationResult ValidateActions(IEnumerable<ActionDescription> actions)
	{
		ValidationResult validationResult = new ValidationResult();
		if (actions == null)
		{
			return validationResult;
		}
		foreach (ActionDescription action in actions)
		{
			validationResult.Add(ValidateAction(action), $"Action {action.Id} validation error");
		}
		return validationResult;
	}

	private ValidationResult ValidateAction(ActionDescription action)
	{
		ValidationResult validationResult = new ValidationResult();
		validationResult.Add(CheckDeviceExists(action.Target), FormatInvalidTarget(action.Target));
		return validationResult;
	}

	private bool CheckDeviceExists(LinkBinding linkBinding)
	{
		if (linkBinding == null)
		{
			return false;
		}
		Entity entity = null;
		switch (linkBinding.LinkType)
		{
		case EntityType.BaseDevice:
			entity = repository.GetBaseDevice(linkBinding.EntityIdAsGuid());
			break;
		case EntityType.LogicalDevice:
			entity = repository.GetLogicalDevice(linkBinding.EntityIdAsGuid());
			break;
		}
		return entity != null;
	}

	private string FormatInvalidTarget(LinkBinding linkBinding)
	{
		return $"Invalid target [{linkBinding.LinkType.ToString()}:{linkBinding.EntityId}].";
	}
}
