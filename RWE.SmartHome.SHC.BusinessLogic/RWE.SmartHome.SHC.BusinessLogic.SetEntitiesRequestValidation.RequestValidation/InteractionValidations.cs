using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.RequestValidation;

public class InteractionValidations
{
	private IRepository repository;

	private RequestValidationRulesProvider rulesProvider;

	public InteractionValidations(IRepository repository, RequestValidationRulesProvider rulesProvider)
	{
		this.repository = repository;
		this.rulesProvider = rulesProvider;
	}

	public ValidationResult ValidateInteractions(IEnumerable<Interaction> interactions)
	{
		ValidationResult validationResult = new ValidationResult();
		foreach (Interaction interaction2 in interactions)
		{
			if (interaction2 != null)
			{
				Interaction interaction = repository.GetInteraction(interaction2.Id);
				validationResult.Add(rulesProvider.GetCommonRules().Validate(interaction2, interaction));
			}
		}
		return validationResult;
	}
}
