using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.SHC.BusinessLogic.DeleteEntitiesRequestValidation.DeleteRequestValidation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeleteEntitiesRequestValidation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;

namespace RWE.SmartHome.SHC.BusinessLogic.DeleteEntitiesRequestValidation;

public class DeleteEntitiesRequestValidator : IDeleteEntitiesRequestValidator
{
	private readonly IRepository repository;

	private readonly DeleteDeviceValidations deleteDeviceValidations;

	public DeleteEntitiesRequestValidator(IRepository repository)
	{
		this.repository = repository;
		deleteDeviceValidations = new DeleteDeviceValidations(repository);
	}

	public ValidationResult ValidateDeleteRequest(IEnumerable<EntityMetadata> entities)
	{
		ValidationResult validationResult = new ValidationResult();
		validationResult.Add(deleteDeviceValidations.ValidateDeleteBaseDevices(entities));
		return validationResult;
	}
}
