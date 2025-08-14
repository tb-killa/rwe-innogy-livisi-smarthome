using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.RequestValidation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation;

public class SetEntitiesRequestValidator : ISetEntitiesRequestValidator
{
	private readonly IRepository repository;

	private readonly RequestValidationRulesProvider rulesProvider;

	private readonly DeviceValidations deviceValidations;

	private readonly InteractionValidations interactionValidations;

	private readonly HomeSetupValidations homeSetupValidations;

	public SetEntitiesRequestValidator(IRepository repository, RequestValidationRulesProvider rulesProvider)
	{
		this.repository = repository;
		this.rulesProvider = rulesProvider;
		deviceValidations = new DeviceValidations(repository, rulesProvider);
		interactionValidations = new InteractionValidations(repository, rulesProvider);
		homeSetupValidations = new HomeSetupValidations(repository);
	}

	public ValidationResult ValidateRequest(IEnumerable<Interaction> interactions, IEnumerable<Location> locations, IEnumerable<BaseDevice> baseDevices, IEnumerable<LogicalDevice> logicalDevices, IEnumerable<HomeSetup> homeSetups)
	{
		ValidationResult validationResult = new ValidationResult();
		validationResult.Add(deviceValidations.ValidateBaseDevices(baseDevices));
		validationResult.Add(deviceValidations.ValidateLogicalDevices(logicalDevices));
		validationResult.Add(interactionValidations.ValidateInteractions(interactions));
		validationResult.Add(homeSetupValidations.ValidateHomeSetupUpdates(homeSetups));
		return validationResult;
	}
}
