using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.EntityRules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration;

public class AppRulesSet
{
	public string AppId { get; set; }

	public List<GenericDeviceRule<BaseDevice>> BaseDeviceRules { get; set; }

	public List<GenericDeviceRule<LogicalDevice>> LogicalDeviceRules { get; set; }

	public List<EntityRule<Interaction>> InteractionRules { get; set; }

	public AppRulesSet()
	{
		BaseDeviceRules = new List<GenericDeviceRule<BaseDevice>>();
		LogicalDeviceRules = new List<GenericDeviceRule<LogicalDevice>>();
		InteractionRules = new List<EntityRule<Interaction>>();
	}

	public void AddRulesSet(AppRulesSet rulesSet)
	{
		if (rulesSet != null)
		{
			if (rulesSet.BaseDeviceRules != null)
			{
				BaseDeviceRules.AddRange(rulesSet.BaseDeviceRules);
			}
			if (rulesSet.LogicalDeviceRules != null)
			{
				LogicalDeviceRules.AddRange(rulesSet.LogicalDeviceRules);
			}
			if (rulesSet.InteractionRules != null)
			{
				InteractionRules.AddRange(rulesSet.InteractionRules);
			}
		}
	}

	public ValidationResult Validate(BaseDevice requestEntity, BaseDevice repoEntity)
	{
		ValidationResult validationResult = new ValidationResult();
		foreach (GenericDeviceRule<BaseDevice> baseDeviceRule in BaseDeviceRules)
		{
			validationResult.Add(baseDeviceRule.Check(requestEntity, repoEntity));
		}
		return validationResult;
	}

	public ValidationResult CheckValueConstraints(BaseDevice newEntity, BaseDevice oldEntity)
	{
		ValidationResult validationResult = new ValidationResult();
		foreach (GenericDeviceRule<BaseDevice> baseDeviceRule in BaseDeviceRules)
		{
			validationResult.Add(baseDeviceRule.CheckValuesRange(newEntity, oldEntity));
		}
		return validationResult;
	}

	public ValidationResult Validate(LogicalDevice requestEntity, LogicalDevice repoEntity)
	{
		ValidationResult validationResult = new ValidationResult();
		foreach (GenericDeviceRule<LogicalDevice> logicalDeviceRule in LogicalDeviceRules)
		{
			validationResult.Add(logicalDeviceRule.Check(requestEntity, repoEntity));
		}
		return validationResult;
	}

	public ValidationResult CheckValueConstraints(LogicalDevice newEntity, LogicalDevice oldEntity)
	{
		ValidationResult validationResult = new ValidationResult();
		foreach (GenericDeviceRule<LogicalDevice> logicalDeviceRule in LogicalDeviceRules)
		{
			validationResult.Add(logicalDeviceRule.CheckValuesRange(newEntity, oldEntity));
		}
		return validationResult;
	}

	internal ValidationResult Validate(Interaction requestEntity, Interaction repoEntity)
	{
		ValidationResult validationResult = new ValidationResult();
		foreach (EntityRule<Interaction> interactionRule in InteractionRules)
		{
			validationResult.Add(interactionRule.Check(requestEntity, repoEntity));
		}
		return validationResult;
	}
}
