using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation;

public class SetEntitiesConfigurationValidator : ISetEntitiesConfigurationValidator, IConfigurationValidator
{
	private readonly RequestValidationRulesProvider rulesProvider;

	public SetEntitiesConfigurationValidator(RequestValidationRulesProvider rulesProvider)
	{
		this.rulesProvider = rulesProvider;
	}

	public IEnumerable<ErrorEntry> GetConfigurationErrors(IRepository configuration, RepositoryUpdateContextData updateContextData)
	{
		List<ErrorEntry> list = new List<ErrorEntry>();
		foreach (BaseDevice modifiedBaseDevice in configuration.GetModifiedBaseDevices())
		{
			BaseDevice originalBaseDevice = configuration.GetOriginalBaseDevice(modifiedBaseDevice.Id);
			ValidationResult validationResult = ValidateDeviceUpdate(modifiedBaseDevice, originalBaseDevice);
			if (!validationResult.Valid)
			{
				list.Add(ValidationResultToErrorEntry(EntityType.BaseDevice, modifiedBaseDevice.Id, validationResult));
			}
		}
		foreach (LogicalDevice modifiedLogicalDevice in configuration.GetModifiedLogicalDevices())
		{
			LogicalDevice originalLogicalDevice = configuration.GetOriginalLogicalDevice(modifiedLogicalDevice.Id);
			ValidationResult validationResult2 = ValidateCapabilityUpdate(modifiedLogicalDevice, originalLogicalDevice);
			if (!validationResult2.Valid)
			{
				list.Add(ValidationResultToErrorEntry(EntityType.LogicalDevice, modifiedLogicalDevice.Id, validationResult2));
			}
		}
		return list;
	}

	private ValidationResult ValidateDeviceUpdate(BaseDevice newDevice, BaseDevice oldDevice)
	{
		ValidationResult validationResult = new ValidationResult();
		validationResult.Add(rulesProvider.GetCommonRules().CheckValueConstraints(newDevice, oldDevice));
		AppRulesSet rulesSetForEntity = rulesProvider.GetRulesSetForEntity(newDevice);
		if (rulesSetForEntity != null)
		{
			validationResult.Add(rulesSetForEntity.CheckValueConstraints(newDevice, oldDevice));
		}
		return validationResult;
	}

	private ValidationResult ValidateCapabilityUpdate(LogicalDevice newDevice, LogicalDevice oldDevice)
	{
		ValidationResult validationResult = new ValidationResult();
		validationResult.Add(rulesProvider.GetCommonRules().CheckValueConstraints(newDevice, oldDevice));
		AppRulesSet rulesSetForEntity = rulesProvider.GetRulesSetForEntity(newDevice);
		if (rulesSetForEntity != null)
		{
			validationResult.Add(rulesSetForEntity.CheckValueConstraints(newDevice, oldDevice));
		}
		return validationResult;
	}

	private ErrorEntry ValidationResultToErrorEntry(EntityType entityType, Guid id, ValidationResult validationResult)
	{
		ErrorEntry errorEntry = new ErrorEntry();
		errorEntry.ErrorCode = ValidationErrorCode.Unknown;
		errorEntry.AffectedEntity = new EntityMetadata
		{
			EntityType = entityType,
			Id = id
		};
		errorEntry.ErrorParameters = new List<ErrorParameter>
		{
			new ErrorParameter
			{
				Key = ErrorParameterKey.ParameterValue,
				Value = validationResult.Reason
			}
		};
		return errorEntry;
	}
}
