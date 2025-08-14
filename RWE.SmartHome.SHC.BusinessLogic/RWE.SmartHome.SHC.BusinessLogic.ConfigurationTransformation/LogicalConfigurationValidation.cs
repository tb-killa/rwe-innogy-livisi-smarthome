using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.IntegrityManagement;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;

namespace RWE.SmartHome.SHC.BusinessLogic.ConfigurationTransformation;

public class LogicalConfigurationValidation
{
	private IRepository configurationRepository;

	private IEnumerable<IConfigurationValidator> validators;

	private static readonly List<string> eventTypeNames = GenValidEventTypes();

	public LogicalConfigurationValidation(IRepository repository, IEnumerable<IConfigurationValidator> validators)
	{
		configurationRepository = repository;
		this.validators = validators;
	}

	public IEnumerable<ErrorEntry> ValidateConfiguration(uint physicalDevicesCountLimit, IEnumerable<Guid> knownPhysicalDevices, RepositoryUpdateContextData updateContextData)
	{
		List<ErrorEntry> list = new List<ErrorEntry>();
		list.AddRange(ValidateConfigurationIntegrity());
		list.AddRange(RunValidators(configurationRepository, updateContextData));
		list.AddRange(DetectTooManyPhysicalDevices(physicalDevicesCountLimit));
		list.AddRange(DetectUnknownPhysicalDevices(knownPhysicalDevices));
		list.AddRange(DetectTooManyRouters());
		list.AddRange(ValidateInteractions());
		return list;
	}

	private IEnumerable<ErrorEntry> ValidateConfigurationIntegrity()
	{
		List<ErrorEntry> list = new List<ErrorEntry>();
		Dictionary<Guid, BaseDevice> dictionary;
		Dictionary<Guid, Location> dictionary2;
		try
		{
			dictionary = configurationRepository.GetBaseDevices().ToDictionary((BaseDevice bd) => bd.Id);
			configurationRepository.GetLogicalDevices().ToDictionary((LogicalDevice d) => d.Id);
			dictionary2 = configurationRepository.GetLocations().ToDictionary((Location l) => l.Id);
		}
		catch (Exception)
		{
			list.Add(new ErrorEntry
			{
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.Configuration,
					Id = Guid.Empty
				},
				ErrorCode = ValidationErrorCode.DuplicateEntity,
				ErrorParameters = new List<ErrorParameter>()
			});
			return list;
		}
		foreach (BaseDevice value in dictionary.Values)
		{
			if (value.LocationId.HasValue && !dictionary2.ContainsKey(value.LocationId.Value))
			{
				list.Add(new ErrorEntry
				{
					AffectedEntity = new EntityMetadata
					{
						EntityType = EntityType.BaseDevice,
						Id = value.Id
					},
					ErrorCode = ValidationErrorCode.MissingLocation,
					ErrorParameters = new List<ErrorParameter>()
				});
			}
			if (IsSmokeDetector(value.DeviceType) && !value.LocationId.HasValue)
			{
				list.Add(new ErrorEntry
				{
					AffectedEntity = new EntityMetadata
					{
						EntityType = EntityType.BaseDevice,
						Id = value.Id
					},
					ErrorCode = ValidationErrorCode.MissingLocation,
					ErrorParameters = new List<ErrorParameter>()
				});
			}
		}
		return list;
	}

	private IEnumerable<ErrorEntry> DetectUnknownPhysicalDevices(IEnumerable<Guid> knownPhysicalDevices)
	{
		List<ErrorEntry> list = new List<ErrorEntry>();
		BaseDevice physicalDevice;
		foreach (BaseDevice item in from pd in configurationRepository.GetBaseDevices()
			where pd.ProtocolId != ProtocolIdentifier.Virtual && !knownPhysicalDevices.Contains(pd.Id)
			select pd)
		{
			physicalDevice = item;
			if (!configurationRepository.GetModifiedBaseDevices().Any((BaseDevice bd) => bd.Id == physicalDevice.Id))
			{
				list.Add(new ErrorEntry
				{
					AffectedEntity = new EntityMetadata
					{
						EntityType = EntityType.BaseDevice,
						Id = physicalDevice.Id
					},
					ErrorCode = ValidationErrorCode.UnknownPhysicalDevice
				});
			}
		}
		return list;
	}

	private IEnumerable<ErrorEntry> DetectTooManyPhysicalDevices(uint physicalDevicesCountLimit)
	{
		List<ErrorEntry> list = new List<ErrorEntry>();
		IEnumerable<BaseDevice> source = from pd in configurationRepository.GetBaseDevices()
			where pd.ProtocolId != ProtocolIdentifier.Virtual
			select pd;
		if (source.Count() > physicalDevicesCountLimit)
		{
			Guid id = Guid.Empty;
			BaseDevice baseDevice = source.OrderByDescending((BaseDevice d) => d.TimeOfAcceptance).FirstOrDefault();
			if (baseDevice != null)
			{
				id = baseDevice.Id;
			}
			list.Add(new ErrorEntry
			{
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.BaseDevice,
					Id = id
				},
				ErrorCode = ValidationErrorCode.MaximumNumberOfDevicesReached,
				ErrorParameters = new List<ErrorParameter>
				{
					new ErrorParameter
					{
						Key = ErrorParameterKey.LicenseState,
						Value = $"{source.Count()}/{physicalDevicesCountLimit}"
					}
				}
			});
		}
		return list;
	}

	private IEnumerable<ErrorEntry> ValidateInteractions()
	{
		List<ErrorEntry> list = new List<ErrorEntry>();
		foreach (Interaction interaction in configurationRepository.GetInteractions())
		{
			list.AddRange(ValidateInteraction(interaction));
		}
		return list;
	}

	internal IEnumerable<ErrorEntry> ValidateInteraction(Interaction interaction)
	{
		List<ErrorEntry> errors = new List<ErrorEntry>();
		if (interaction.Rules == null || !interaction.Rules.Any())
		{
			AddValidationError(interaction.Id, EntityType.Interaction, ValidationErrorCode.InvalidInteraction, interaction.Id, errors);
			return errors;
		}
		if (interaction.Rules.Any((Rule rule) => rule?.IsEmpty() ?? true))
		{
			AddValidationError(interaction.Id, EntityType.Interaction, ValidationErrorCode.InvalidInteraction, interaction.Id, errors);
			return errors;
		}
		(from t in interaction.Rules.Where((Rule r) => r.Triggers != null).SelectMany((Rule r) => r.Triggers)
			where t != null && t.TriggerConditions != null
			select t).SelectMany((Trigger t) => t.TriggerConditions).SelectMany((Condition tc) => new List<DataBinding> { tc.LeftHandOperand, tc.RightHandOperand }).OfType<FunctionBinding>()
			.ToList()
			.ForEach(delegate(FunctionBinding i)
			{
				ValidateFunction(i, interaction.Id, errors);
			});
		foreach (Rule item in interaction.Rules.Where((Rule r) => r.Actions != null))
		{
			IEnumerable<string> enumerable = from a in item.Actions
				where a.Target.LinkType == EntityType.LogicalDevice || a.Target.LinkType == EntityType.BaseDevice
				select new
				{
					a.Target.EntityId,
					a.ActionType
				} into acn
				group acn by new { acn.EntityId, acn.ActionType } into g
				where g.Count() > 1
				select g.Key.EntityId;
			foreach (string item2 in enumerable)
			{
				AddValidationError(interaction.Id, EntityType.Interaction, ValidationErrorCode.DuplicateActuators, errors, new ErrorParameter
				{
					Key = ErrorParameterKey.ActuatorId,
					Value = item2.ToString()
				});
			}
			if (item.InteractionId != interaction.Id)
			{
				AddValidationError(interaction.Id, EntityType.Interaction, ValidationErrorCode.InvalidInteraction, errors);
			}
		}
		foreach (ActionDescription item3 in interaction.Rules.SelectMany((Rule r) => r.Actions))
		{
			if (item3 == null)
			{
				AddValidationError(interaction.Id, EntityType.Interaction, ValidationErrorCode.InvalidInteraction, interaction.Id, errors);
			}
			else
			{
				ValidateActionParameters(interaction.Id, item3, errors);
			}
		}
		foreach (Condition item4 in interaction.Rules.Where((Rule r) => r.Triggers != null).SelectMany((Rule r) => r.Triggers.Where((Trigger t) => t.TriggerConditions != null).SelectMany((Trigger t) => t.TriggerConditions)).Union(interaction.Rules.Where((Rule r) => r.Conditions != null).SelectMany((Rule r) => r.Conditions)))
		{
			if (item4 == null)
			{
				AddValidationError(interaction.Id, EntityType.Interaction, ValidationErrorCode.InvalidInteraction, interaction.Id, errors);
				continue;
			}
			CheckStatelessSensorInStateArea(item4.LeftHandOperand, interaction.Id, errors);
			CheckStatelessSensorInStateArea(item4.RightHandOperand, interaction.Id, errors);
		}
		errors.AddRange(ValidateEventTypes(interaction));
		return errors;
	}

	private IEnumerable<ErrorEntry> ValidateEventTypes(Interaction interaction)
	{
		List<ErrorEntry> list = new List<ErrorEntry>();
		IEnumerable<Trigger> source = interaction.Rules.Where((Rule r) => r.Triggers != null).SelectMany((Rule r) => r.Triggers);
		List<string> list2 = source.Select((Trigger m) => m.EventType).ToList();
		foreach (string item in list2)
		{
			if (!eventTypeNames.Contains(item))
			{
				AddValidationError(interaction.Id, EntityType.Interaction, ValidationErrorCode.InvalidInteraction, list, new ErrorParameter
				{
					Key = ErrorParameterKey.ParameterValue,
					Value = item
				}, new ErrorParameter
				{
					Key = ErrorParameterKey.ProfileId,
					Value = interaction.Id.ToString()
				});
			}
		}
		return list;
	}

	private static List<string> GenValidEventTypes()
	{
		List<string> list = new List<string>();
		list.Add("ButtonPressed");
		list.Add("MotionDetected");
		list.Add("StateChanged");
		list.Add("product/core.RWE/1.0/customtrigger/StateMonitoringTrigger");
		list.Add("TimeEvent");
		return list;
	}

	private static IEnumerable<string> GetDuplicateTargets(IEnumerable<ActionDescription> actions)
	{
		return from a in actions
			where a.Target.LinkType == EntityType.LogicalDevice || a.Target.LinkType == EntityType.BaseDevice
			select a.Target.EntityId into id
			group id by id into g
			where g.Count() > 1
			select g.Key;
	}

	private void ValidateActionParameters(Guid profileId, ActionDescription action, List<ErrorEntry> errors)
	{
		foreach (Parameter datum in action.Data)
		{
			if (datum.Value is FunctionBinding)
			{
				ValidateFunction(datum.Value as FunctionBinding, profileId, errors);
			}
		}
	}

	private void ValidateFunction(FunctionBinding functionBinding, Guid profileId, List<ErrorEntry> errors)
	{
		if (functionBinding.Parameters == null)
		{
			AddValidationError(profileId, EntityType.Interaction, ValidationErrorCode.IncorrectFunctionBinding, profileId, errors);
			return;
		}
		switch (functionBinding.Function)
		{
		case FunctionIdentifier.Add:
		case FunctionIdentifier.Multiply:
		case FunctionIdentifier.Min:
		case FunctionIdentifier.Max:
		case FunctionIdentifier.Average:
			ValidateFunctionWithOneOperatorMinimum(functionBinding.Parameters.Select((Parameter param) => param.Value).ToList(), profileId, errors);
			break;
		case FunctionIdentifier.And:
		case FunctionIdentifier.Or:
			ValidatePoliFunction(functionBinding.Parameters.Select((Parameter param) => param.Value).ToList(), profileId, errors);
			break;
		case FunctionIdentifier.Subtract:
		case FunctionIdentifier.Divide:
		case FunctionIdentifier.Modulo:
		case FunctionIdentifier.Equal:
		case FunctionIdentifier.NotEqual:
		case FunctionIdentifier.Smaller:
		case FunctionIdentifier.Greater:
		case FunctionIdentifier.SmallerOrEqual:
		case FunctionIdentifier.GreaterOrEqual:
		case FunctionIdentifier.Pow:
		case FunctionIdentifier.BitwiseAnd:
		case FunctionIdentifier.BitwiseOr:
		case FunctionIdentifier.BitwiseXOR:
		case FunctionIdentifier.BitwiseLeftShift:
		case FunctionIdentifier.BitwiseRightShift:
			ValidateBinaryFunction(functionBinding.Parameters.Select((Parameter param) => param.Value).ToList(), profileId, errors);
			break;
		case FunctionIdentifier.Exp:
		case FunctionIdentifier.Abs:
		case FunctionIdentifier.Round:
		case FunctionIdentifier.GetEventProperty:
		case FunctionIdentifier.BitwiseNot:
		case FunctionIdentifier.GetMinute:
		case FunctionIdentifier.GetHour:
		case FunctionIdentifier.GetDayOfWeek:
		case FunctionIdentifier.GetDayOfMonth:
		case FunctionIdentifier.GetWeekdayOfMonth:
		case FunctionIdentifier.GetMonth:
		case FunctionIdentifier.GetYear:
		case FunctionIdentifier.GetDayOfCentury:
		case FunctionIdentifier.GetWeekOfCentury:
		case FunctionIdentifier.GetMonthOfCentury:
		case FunctionIdentifier.GetCurrentDateTime:
			ValidateUnaryFunction(functionBinding.Parameters.Select((Parameter param) => param.Value).ToList(), profileId, errors);
			break;
		case FunctionIdentifier.Log:
			ValidateLogFunction(functionBinding.Parameters.Select((Parameter param) => param.Value).ToList(), profileId, errors);
			break;
		case FunctionIdentifier.GetEntityStateProperty:
		case FunctionIdentifier.GetMinutesSinceLastChange:
			ValidateTargetPropertyFunction(functionBinding.Parameters, profileId, errors);
			break;
		case FunctionIdentifier.HasEventProperty:
			ValidateHasEventPropertyFunction(functionBinding.Parameters, profileId, errors);
			break;
		default:
			AddValidationError(profileId, EntityType.Interaction, ValidationErrorCode.UnsupportedFunction, profileId, errors);
			break;
		}
	}

	private void ValidateHasEventPropertyFunction(List<Parameter> operands, Guid interactionId, List<ErrorEntry> errors)
	{
		if (operands.Count != 1)
		{
			AddValidationError(interactionId, EntityType.Interaction, ValidationErrorCode.IncorrectFunctionBinding, interactionId, errors);
			return;
		}
		Parameter parameter = operands.FirstOrDefault((Parameter p) => p.Name == "EventPropertyName" && p.Value is ConstantStringBinding);
		if (parameter == null)
		{
			AddValidationError(interactionId, EntityType.Interaction, ValidationErrorCode.IncorrectFunctionBinding, interactionId, errors);
		}
		else if (!(parameter.Value is ConstantStringBinding constantStringBinding) || string.IsNullOrEmpty(constantStringBinding.Value))
		{
			AddValidationError(interactionId, EntityType.Interaction, ValidationErrorCode.IncorrectFunctionBinding, interactionId, errors);
		}
	}

	private void ValidateTargetPropertyFunction(List<Parameter> operands, Guid profileId, List<ErrorEntry> errors)
	{
		if (operands.Count() != 2)
		{
			AddValidationError(profileId, EntityType.Interaction, ValidationErrorCode.IncorrectFunctionBinding, profileId, errors);
			return;
		}
		Parameter parameter = operands.FirstOrDefault((Parameter p) => p.Name == "EntityId" && p.Value is LinkBinding);
		Parameter parameter2 = operands.FirstOrDefault((Parameter p) => p.Name == "TargetPropertyName" && p.Value is ConstantStringBinding);
		if (parameter == null || parameter2 == null)
		{
			AddValidationError(profileId, EntityType.Interaction, ValidationErrorCode.IncorrectFunctionBinding, profileId, errors);
			return;
		}
		LinkBinding linkBinding = parameter.Value as LinkBinding;
		switch ((parameter.Value as LinkBinding).LinkType)
		{
		case EntityType.LogicalDevice:
			if (configurationRepository.GetLogicalDevice(linkBinding.EntityIdAsGuid()) == null)
			{
				AddValidationError(profileId, EntityType.Interaction, ValidationErrorCode.IncorrectFunctionBinding, profileId, errors);
			}
			break;
		case EntityType.BaseDevice:
			if (configurationRepository.GetBaseDevice(linkBinding.EntityIdAsGuid()) == null)
			{
				AddValidationError(profileId, EntityType.Interaction, ValidationErrorCode.IncorrectFunctionBinding, profileId, errors);
			}
			break;
		default:
			AddValidationError(profileId, EntityType.Interaction, ValidationErrorCode.IncorrectFunctionBinding, profileId, errors);
			break;
		}
	}

	private void ValidateFunctionWithOneOperatorMinimum(List<DataBinding> operands, Guid profileId, List<ErrorEntry> errors)
	{
		if (operands.Count() < 1)
		{
			AddValidationError(profileId, EntityType.Interaction, ValidationErrorCode.IncorrectFunctionBinding, profileId, errors);
		}
		else
		{
			ValidateFunctionRecursively(operands, profileId, errors);
		}
	}

	private void ValidatePoliFunction(List<DataBinding> operands, Guid profileId, List<ErrorEntry> errors)
	{
		if (operands.Count() < 2)
		{
			AddValidationError(profileId, EntityType.Interaction, ValidationErrorCode.IncorrectFunctionBinding, profileId, errors);
		}
		else
		{
			ValidateFunctionRecursively(operands, profileId, errors);
		}
	}

	private void ValidateBinaryFunction(List<DataBinding> operands, Guid profileId, List<ErrorEntry> errors)
	{
		if (operands.Count() != 2)
		{
			AddValidationError(profileId, EntityType.Interaction, ValidationErrorCode.IncorrectFunctionBinding, profileId, errors);
		}
		else
		{
			ValidateFunctionRecursively(operands, profileId, errors);
		}
	}

	private void ValidateUnaryFunction(List<DataBinding> operands, Guid profileId, List<ErrorEntry> errors)
	{
		if (operands.Count() != 1)
		{
			AddValidationError(profileId, EntityType.Interaction, ValidationErrorCode.IncorrectFunctionBinding, profileId, errors);
		}
		else if (operands[0] is FunctionBinding)
		{
			ValidateFunction(operands[0] as FunctionBinding, profileId, errors);
		}
	}

	private void ValidateLogFunction(List<DataBinding> operands, Guid profileId, List<ErrorEntry> errors)
	{
		if (operands.Count() == 0 || operands.Count() > 2)
		{
			AddValidationError(profileId, EntityType.Interaction, ValidationErrorCode.IncorrectFunctionBinding, profileId, errors);
		}
		else
		{
			ValidateFunctionRecursively(operands, profileId, errors);
		}
	}

	private void ValidateFunctionRecursively(List<DataBinding> operands, Guid profileId, List<ErrorEntry> errors)
	{
		foreach (DataBinding operand in operands)
		{
			if (operand is FunctionBinding)
			{
				ValidateFunction(operand as FunctionBinding, profileId, errors);
			}
		}
	}

	private void CheckStatelessSensorInStateArea(DataBinding binding, Guid profileId, IList<ErrorEntry> errors)
	{
		if (!(binding is FunctionBinding { Function: FunctionIdentifier.GetEntityStateProperty } functionBinding))
		{
			return;
		}
		Guid? guid = null;
		try
		{
			LinkBinding linkBinding = functionBinding.Parameters.First((Parameter p) => p.Name == "EntityId").Value as LinkBinding;
			if (linkBinding.LinkType == EntityType.LogicalDevice)
			{
				guid = linkBinding.EntityIdAsGuid();
				LogicalDevice logicalDevice = configurationRepository.GetLogicalDevice(guid.Value);
				if (logicalDevice is MotionDetectionSensor || logicalDevice is PushButtonSensor)
				{
					AddValidationError(profileId, EntityType.Interaction, ValidationErrorCode.StatelessSensorInStateArea, guid.Value, errors);
				}
			}
			else
			{
				AddValidationError(profileId, EntityType.Interaction, ValidationErrorCode.IncorrectFunctionBinding, profileId, errors);
			}
		}
		catch
		{
			AddValidationError(profileId, EntityType.Interaction, ValidationErrorCode.IncorrectFunctionBinding, profileId, errors);
		}
	}

	private void AddValidationError(Guid entityId, EntityType entityType, ValidationErrorCode validationError, Guid profileId, IList<ErrorEntry> errors)
	{
		errors.Add(CreateErrorEntry(entityId, entityType, validationError, profileId));
	}

	private void AddValidationError(Guid entityId, EntityType entityType, ValidationErrorCode validationError, IList<ErrorEntry> errors, params ErrorParameter[] parameters)
	{
		errors.Add(CreateErrorEntry(entityId, entityType, validationError, parameters));
	}

	private static ErrorEntry CreateErrorEntry(Guid entityId, EntityType entityType, ValidationErrorCode validationError, Guid profileId)
	{
		return CreateErrorEntry(entityId, entityType, validationError, new ErrorParameter
		{
			Key = ErrorParameterKey.ProfileId,
			Value = profileId.ToString()
		});
	}

	private static ErrorEntry CreateErrorEntry(Guid entityId, EntityType entityType, ValidationErrorCode validationError, params ErrorParameter[] parameters)
	{
		ErrorEntry errorEntry = new ErrorEntry();
		errorEntry.AffectedEntity = new EntityMetadata
		{
			EntityType = entityType,
			Id = entityId
		};
		errorEntry.ErrorCode = validationError;
		ErrorEntry errorEntry2 = errorEntry;
		errorEntry2.ErrorParameters.AddRange(parameters);
		return errorEntry2;
	}

	private IEnumerable<ErrorEntry> RunValidators(IRepository repository, RepositoryUpdateContextData updateContextData)
	{
		List<ErrorEntry> list = new List<ErrorEntry>();
		if (validators == null)
		{
			return list;
		}
		foreach (IConfigurationValidator validator in validators)
		{
			IEnumerable<ErrorEntry> configurationErrors = validator.GetConfigurationErrors(repository, updateContextData);
			if (configurationErrors != null)
			{
				list.AddRange(configurationErrors);
			}
		}
		return list;
	}

	private IEnumerable<ErrorEntry> DetectTooManyRouters()
	{
		List<ErrorEntry> list = new List<ErrorEntry>();
		IEnumerable<BaseDevice> source = from bd in configurationRepository.GetBaseDevices()
			where bd.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.PSR
			select bd;
		if (source.Count() > 1)
		{
			Guid empty = Guid.Empty;
			BaseDevice baseDevice = source.OrderByDescending((BaseDevice d) => d.TimeOfAcceptance).First();
			empty = baseDevice.Id;
			list.Add(new ErrorEntry
			{
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.BaseDevice,
					Id = empty
				},
				ErrorCode = ValidationErrorCode.TooManyRouters,
				ErrorParameters = new List<ErrorParameter>
				{
					new ErrorParameter
					{
						Key = ErrorParameterKey.PhysicalDeviceId,
						Value = string.Format("Only one router allowed per configuration! Remove {0}.", empty.ToString("N"))
					}
				}
			});
		}
		return list;
	}

	private bool IsSmokeDetector(string deviceType)
	{
		if (!(deviceType == BuiltinPhysicalDeviceType.WSD.ToString()))
		{
			return deviceType == BuiltinPhysicalDeviceType.WSD2.ToString();
		}
		return true;
	}
}
