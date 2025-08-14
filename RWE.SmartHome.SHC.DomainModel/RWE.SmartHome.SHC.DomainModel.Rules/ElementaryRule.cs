using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.SHC.DomainModel.Rules;

public class ElementaryRule
{
	public readonly Guid Id;

	public readonly RestrictionType Restriction;

	public readonly ElementaryTrigger Trigger;

	public readonly IEnumerable<Condition> Conditions;

	public readonly TimeSpan ConditionEvaluationDelay;

	public readonly ActionDescription Action;

	public readonly Interaction SourceInteraction;

	public readonly Rule SourceRule;

	public RuleHandlingState HandlingState { get; private set; }

	public bool IsHandled => HandlingState != RuleHandlingState.Unhandled;

	public Guid SourceInteractionId => SourceInteraction.Id;

	public Guid SourceRuleId => SourceRule.Id;

	public bool IsRestricted()
	{
		return Restriction != RestrictionType.NoRestriction;
	}

	internal ElementaryRule(Interaction interaction, Rule rule, ElementaryTrigger trigger, IEnumerable<Condition> conditions, TimeSpan conditionEvaluationDelay, ActionDescription action)
	{
		Id = Guid.NewGuid();
		SourceInteraction = interaction;
		SourceRule = rule;
		HandlingState = RuleHandlingState.Unhandled;
		Trigger = trigger;
		Conditions = conditions;
		ConditionEvaluationDelay = conditionEvaluationDelay;
		Action = action;
		Restriction = GetRestrictionType(interaction, rule);
	}

	private RestrictionType GetRestrictionType(Interaction interaction, Rule rule)
	{
		if (interaction.ValidFrom.HasValue || interaction.ValidTo.HasValue || interaction.Freezetime > 0)
		{
			return RestrictionType.InteractionRestriction;
		}
		if (rule.ConditionEvaluationDelay > 0 || (rule.Conditions != null && rule.Conditions.Any()))
		{
			return RestrictionType.RuleRestriction;
		}
		return RestrictionType.NoRestriction;
	}

	public void MarkAsHandled()
	{
		HandlingState = RuleHandlingState.Accelerated;
	}

	internal static IEnumerable<ElementaryRule> SplitRule(Interaction interaction, Rule rule)
	{
		TimeSpan conditionEvaluationDelay = new TimeSpan(0, 0, 0, 0, rule.ConditionEvaluationDelay);
		List<Condition> conditions = rule.Conditions ?? new List<Condition>();
		foreach (ActionDescription action in rule.Actions)
		{
			if (rule.Triggers != null)
			{
				foreach (Trigger trigger in rule.Triggers)
				{
					yield return new ElementaryRule(interaction, rule, new ElementaryTrigger(trigger), conditions, conditionEvaluationDelay, action);
				}
			}
			if (rule.CustomTriggers == null)
			{
				continue;
			}
			foreach (CustomTrigger trigger2 in rule.CustomTriggers)
			{
				yield return new ElementaryRule(interaction, rule, new ElementaryTrigger(trigger2), conditions, conditionEvaluationDelay, action);
			}
		}
	}

	public static IEnumerable<ElementaryRule> SplitInteraction(Interaction interaction)
	{
		return interaction.Rules.SelectMany((Rule x) => SplitRule(interaction, x));
	}

	public Rule ToRule()
	{
		Rule rule = new Rule();
		rule.Id = Id;
		rule.InteractionId = SourceInteraction.Id;
		rule.Triggers = ((Trigger.Trigger != null) ? new List<Trigger> { Trigger.Trigger } : new List<Trigger>());
		rule.CustomTriggers = ((Trigger.CustomTrigger != null) ? new List<CustomTrigger> { Trigger.CustomTrigger } : new List<CustomTrigger>());
		rule.Conditions = Conditions.ToList();
		rule.ConditionEvaluationDelay = (int)ConditionEvaluationDelay.TotalMilliseconds;
		rule.Actions = new List<ActionDescription> { Action };
		rule.Tags = new List<Tag>
		{
			new Tag
			{
				Name = "SourceRuleId",
				Value = SourceRuleId.ToString()
			}
		};
		return rule;
	}
}
