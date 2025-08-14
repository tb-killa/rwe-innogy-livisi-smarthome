using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;

namespace RWE.SmartHome.SHC.BusinessLogic.DeleteEntitiesRequestValidation.DeleteRequestValidation;

public class DeleteDeviceValidations
{
	private const string SHCBaseDeviceType = "SHC";

	private IRepository repository;

	public DeleteDeviceValidations(IRepository repository)
	{
		this.repository = repository;
	}

	public ValidationResult ValidateDeleteBaseDevices(IEnumerable<EntityMetadata> entities)
	{
		ValidationResult validationResult = new ValidationResult();
		ValidationResult validationResult2 = new ValidationResult();
		foreach (EntityMetadata entity in entities)
		{
			BaseDevice baseDevice = repository.GetBaseDevice(entity.Id);
			if (baseDevice != null)
			{
				validationResult2.Add(CheckNoSHCBaseDevice(baseDevice));
				validationResult.Add(validationResult2);
			}
		}
		return validationResult;
	}

	private ValidationResult CheckNoSHCBaseDevice(BaseDevice entity)
	{
		ValidationResult validationResult = new ValidationResult();
		if (entity.DeviceType == "SHC")
		{
			validationResult.Add("Validate delete error: cannot delete SHC");
		}
		return validationResult;
	}
}
