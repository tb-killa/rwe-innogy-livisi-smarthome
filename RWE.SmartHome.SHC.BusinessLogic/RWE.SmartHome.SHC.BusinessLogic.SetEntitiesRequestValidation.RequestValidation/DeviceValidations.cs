using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.RequestValidation;

public class DeviceValidations
{
	private IRepository repository;

	private RequestValidationRulesProvider rulesProvider;

	public DeviceValidations(IRepository repository, RequestValidationRulesProvider rulesProvider)
	{
		this.repository = repository;
		this.rulesProvider = rulesProvider;
	}

	public ValidationResult ValidateBaseDevices(IEnumerable<BaseDevice> baseDevices)
	{
		ValidationResult validationResult = new ValidationResult();
		foreach (BaseDevice baseDevice2 in baseDevices)
		{
			BaseDevice baseDevice = repository.GetBaseDevice(baseDevice2.Id);
			if (baseDevice != null)
			{
				AppRulesSet rulesSetForEntity = rulesProvider.GetRulesSetForEntity(baseDevice);
				validationResult.Add(rulesSetForEntity.Validate(baseDevice2, baseDevice));
			}
		}
		return validationResult;
	}

	public ValidationResult ValidateLogicalDevices(IEnumerable<LogicalDevice> logicalDevices)
	{
		ValidationResult validationResult = new ValidationResult();
		foreach (LogicalDevice logicalDevice2 in logicalDevices)
		{
			LogicalDevice logicalDevice = repository.GetLogicalDevice(logicalDevice2.Id);
			if (logicalDevice != null)
			{
				AppRulesSet rulesSetForEntity = rulesProvider.GetRulesSetForEntity(logicalDevice);
				validationResult.Add(rulesSetForEntity.Validate(logicalDevice2, logicalDevice));
			}
		}
		return validationResult;
	}
}
