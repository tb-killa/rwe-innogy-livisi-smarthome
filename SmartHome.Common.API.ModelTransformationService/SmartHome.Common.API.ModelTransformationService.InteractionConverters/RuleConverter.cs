using System;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.ModelTransformationService.ActionConverters.Generic;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.InteractionConverters;

public class RuleConverter : IRuleConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(RuleConverter));

	private readonly IActionDescriptionConverter actionDescriptionConverter = new ActionDescriptionConverter();

	private readonly IConditionConverter conditionConverter = new ConditionConverter();

	private readonly ITriggerConverter triggerConverter = new TriggerConverter();

	private readonly ICustomTriggerConverter customTriggerConverter = new CustomTriggerConverter();

	public SmartHome.Common.API.Entities.Entities.Rule FromSmartHomeRule(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Rule shRule)
	{
		SmartHome.Common.API.Entities.Entities.Rule rule = new SmartHome.Common.API.Entities.Entities.Rule();
		rule.Id = shRule.Id.ToString("N");
		rule.ConditionEvaluationDelay = shRule.ConditionEvaluationDelay;
		SmartHome.Common.API.Entities.Entities.Rule rule2 = rule;
		if (shRule.Actions != null)
		{
			rule2.Actions = shRule.Actions.ConvertAll((ActionDescription ruleAction) => actionDescriptionConverter.FromSmartHomeActionDescription(ruleAction));
		}
		if (shRule.Conditions != null)
		{
			rule2.Constraints = shRule.Conditions.ConvertAll((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Condition condition) => conditionConverter.FromSmartHomeCondition(condition));
		}
		if (shRule.Triggers != null)
		{
			rule2.Triggers = shRule.Triggers.ConvertAll((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Trigger trigger) => triggerConverter.FromSmartHomeTrigger(trigger));
		}
		if (shRule.CustomTriggers != null)
		{
			rule2.CustomTriggers = shRule.CustomTriggers.ConvertAll((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.CustomTrigger trigger) => customTriggerConverter.FromSmartHomeCustomTrigger(trigger));
		}
		if (shRule.Tags != null && shRule.Tags.Any())
		{
			rule2.Tags = shRule.Tags.ConvertAll((Tag t) => new SmartHome.Common.API.Entities.Entities.Property
			{
				Name = t.Name,
				Value = t.Value
			});
		}
		return rule2;
	}

	public RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Rule ToSmartHomeRule(SmartHome.Common.API.Entities.Entities.Rule apiRule, Guid interactionId)
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Rule rule = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Rule();
		rule.Id = apiRule.Id.ToGuid();
		rule.InteractionId = interactionId;
		rule.ConditionEvaluationDelay = apiRule.ConditionEvaluationDelay.GetValueOrDefault();
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Rule rule2 = rule;
		if (apiRule.Actions != null)
		{
			rule2.Actions = apiRule.Actions.ConvertAll((SmartHome.Common.API.Entities.Entities.Action action) => actionDescriptionConverter.ToSmartHomeActionDescription(action));
		}
		if (apiRule.Constraints != null)
		{
			rule2.Conditions = apiRule.Constraints.ConvertAll((SmartHome.Common.API.Entities.Entities.Condition condition) => conditionConverter.ToSmartHomeCondition(condition));
		}
		if (apiRule.Triggers != null)
		{
			rule2.Triggers = apiRule.Triggers.ConvertAll((SmartHome.Common.API.Entities.Entities.Trigger trigger) => triggerConverter.ToSmartHomeTrigger(trigger));
		}
		if (apiRule.CustomTriggers != null)
		{
			rule2.CustomTriggers = apiRule.CustomTriggers.ConvertAll((SmartHome.Common.API.Entities.Entities.CustomTrigger trigger) => customTriggerConverter.ToSmartHomeCustomTrigger(trigger));
		}
		if (apiRule.Tags != null)
		{
			rule2.Tags = apiRule.Tags.ConvertAll((SmartHome.Common.API.Entities.Entities.Property tag) => new Tag
			{
				Name = tag.Name,
				Value = tag.Value.ToString()
			});
		}
		return rule2;
	}
}
