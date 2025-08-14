using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;

namespace RWE.SmartHome.SHC.RuleEngine.Repository;

internal class RulesRepository : IRulesRepository
{
	private Dictionary<Guid, List<Rule>> rulesMap = new Dictionary<Guid, List<Rule>>();

	private List<Rule> rules = new List<Rule>();

	private List<Rule> tempRules;

	public IEnumerable<Rule> Rules => rules;

	public IEnumerable<Rule> GetRulesForDevice(Guid deviceId)
	{
		if (!rulesMap.ContainsKey(deviceId))
		{
			return new List<Rule>();
		}
		return rulesMap[deviceId];
	}

	public void Add(Rule rule)
	{
		if (tempRules == null)
		{
			throw new InvalidOperationException("Repository not in transaction.");
		}
		if (DoesRuleContainRuleTriggeringActions(rule))
		{
			throw new ArgumentException("Rule cannot contain rule triggering actions.");
		}
		tempRules.Add(rule);
	}

	public bool Remove(Guid ruleId)
	{
		if (tempRules == null)
		{
			throw new InvalidOperationException("Repository not in transaction.");
		}
		return tempRules.RemoveAll((Rule r) => r.Id == ruleId) > 0;
	}

	public void BeginUpdate()
	{
		tempRules = new List<Rule>();
	}

	public void CommitChanges()
	{
		if (tempRules == null)
		{
			throw new InvalidOperationException("Repository not in transaction.");
		}
		rules = tempRules;
		tempRules = null;
		RefreshMap();
	}

	private void RefreshMap()
	{
		rulesMap.Clear();
		rules.ForEach(AddToMap);
	}

	private void AddToMap(Guid deviceId, Rule rule)
	{
		if (!rulesMap.ContainsKey(deviceId))
		{
			rulesMap.Add(deviceId, new List<Rule>());
		}
		if (!rulesMap[deviceId].Contains(rule))
		{
			rulesMap[deviceId].Add(rule);
		}
	}

	private void AddToMap(Rule rule)
	{
		if (rule.Triggers != null)
		{
			rule.Triggers.ForEach(delegate(Trigger t)
			{
				AddToMap(t.Entity.EntityIdAsGuid(), rule);
			});
		}
		if (rule.Conditions == null)
		{
			return;
		}
		foreach (Condition condition in rule.Conditions)
		{
			AddFunctionToMap(condition.LeftHandOperand as FunctionBinding, rule);
			AddFunctionToMap(condition.RightHandOperand as FunctionBinding, rule);
		}
	}

	private void AddFunctionToMap(FunctionBinding funcBinding, Rule rule)
	{
		if (funcBinding == null)
		{
			return;
		}
		foreach (Parameter parameter in funcBinding.Parameters)
		{
			if (parameter.Name != "EntityId")
			{
				if (!(parameter.Value is FunctionBinding))
				{
					continue;
				}
				AddFunctionToMap(parameter.Value as FunctionBinding, rule);
			}
			if (parameter.Value is LinkBinding linkBinding && (linkBinding.LinkType == EntityType.LogicalDevice || linkBinding.LinkType == EntityType.BaseDevice))
			{
				AddToMap(linkBinding.EntityIdAsGuid(), rule);
			}
		}
	}

	private bool DoesRuleContainRuleTriggeringActions(Rule rule)
	{
		return rule.Actions.Any((ActionDescription x) => x.ActionType == "TriggerRule");
	}
}
