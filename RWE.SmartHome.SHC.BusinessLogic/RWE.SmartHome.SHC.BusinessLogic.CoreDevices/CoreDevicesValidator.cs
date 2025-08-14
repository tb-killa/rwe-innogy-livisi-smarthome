using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.DomainModel;

namespace RWE.SmartHome.SHC.BusinessLogic.CoreDevices;

public class CoreDevicesValidator : IConfigurationValidator
{
	internal static class ISRErrors
	{
		public static string MissingTarget = "MissingTarget";

		public static string WrongTargetType = "WrongTargetType";

		public static string IncompatibleTargetDeviceType = "IncompatibleTargetDeviceType";

		public static string WrongControlBehavior = "UnknownOrMissingControlBehavior";

		public static string WrongDriveTime = "WrongOrMissingDriveTime";

		public static string WrongShutterLevel = "WrongOrMissingShutterLEvel";
	}

	public IEnumerable<ErrorEntry> GetConfigurationErrors(IRepository configuration, RepositoryUpdateContextData updateContextData)
	{
		List<ErrorEntry> validationErrors = new List<ErrorEntry>();
		configuration.GetInteractions().ForEach(delegate(Interaction interaction)
		{
			interaction.Rules.Where((Rule rule) => rule.Actions.Any((ActionDescription actn) => actn.ActionType == "ControlValve")).ToList().ForEach(delegate(Rule rule)
			{
				CheckFSC8Interaction(configuration, rule, interaction.Id, validationErrors);
			});
			validationErrors.AddRange(CheckISR2Interaction(configuration, interaction));
			validationErrors.AddRange(ValidateWindowReduction(configuration, interaction));
		});
		return validationErrors;
	}

	private void CheckFSC8Interaction(IRepository configRepo, Rule rule, Guid interactionId, List<ErrorEntry> validationErrors)
	{
		CheckControlValveTrigger(rule, interactionId, validationErrors, configRepo);
		CheckControlValveAction(rule, interactionId, validationErrors, configRepo);
	}

	private void CheckControlValveAction(Rule rule, Guid interactionId, List<ErrorEntry> validationErrors, IRepository configRepo)
	{
		ActionDescription actionDescription = rule.Actions.Where((ActionDescription actn) => actn.Target.LinkType == EntityType.LogicalDevice && actn.ActionType == "ControlValve").FirstOrDefault();
		if (actionDescription != null)
		{
			LogicalDevice logicalDevice = configRepo.GetLogicalDevice(actionDescription.Target.EntityIdAsGuid());
			if (!(logicalDevice is ValveActuator))
			{
				AddValidationError(interactionId, EntityType.Interaction, ValidationErrorCode.MissingActuator, validationErrors);
			}
		}
	}

	private void CheckControlValveTrigger(Rule rule, Guid interactionId, List<ErrorEntry> validationErrors, IRepository configRepo)
	{
		if (rule.Triggers == null || rule.Triggers.Count <= 0)
		{
			return;
		}
		if (rule.Triggers.Count > 1)
		{
			AddValidationError(interactionId, EntityType.Interaction, ValidationErrorCode.InvalidInteraction, validationErrors);
		}
		Trigger trigger = rule.Triggers.Where((Trigger tg) => tg.Entity.LinkType == EntityType.LogicalDevice).FirstOrDefault();
		if (trigger != null)
		{
			LogicalDevice logicalDevice = configRepo.GetLogicalDevice(trigger.Entity.EntityIdAsGuid());
			if (!(logicalDevice is TemperatureSensor))
			{
				AddValidationError(logicalDevice.Id, EntityType.LogicalDevice, ValidationErrorCode.IncompatibleSensorsInProfile, validationErrors);
			}
			if (trigger.TriggerConditions != null && trigger.TriggerConditions.Count > 0)
			{
				AddValidationError(interactionId, EntityType.Interaction, ValidationErrorCode.InvalidInteraction, validationErrors);
			}
		}
		else
		{
			AddValidationError(rule.InteractionId, EntityType.Interaction, ValidationErrorCode.IncompatibleSensorsInProfile, validationErrors);
		}
	}

	private void AddValidationError(Guid entityId, EntityType entityType, ValidationErrorCode validationError, List<ErrorEntry> errors)
	{
		errors.Add(new ErrorEntry
		{
			AffectedEntity = new EntityMetadata
			{
				Id = entityId,
				EntityType = entityType
			},
			ErrorCode = validationError,
			ErrorParameters = new List<ErrorParameter>()
		});
	}

	private bool IsWindowReductionAction(string actionType)
	{
		return actionType == "WindowReduction";
	}

	private List<ErrorEntry> ValidateWindowReduction(IRepository configuration, Interaction interaction)
	{
		List<ErrorEntry> list = new List<ErrorEntry>();
		List<ErrorParameter> list2 = new List<ErrorParameter>();
		foreach (Rule item in interaction.Rules.Where((Rule rl) => rl.Actions.Any((ActionDescription acn) => IsWindowReductionAction(acn.ActionType))))
		{
			bool flag = item.Triggers.Count() <= 8 && item.Triggers.All((Trigger t) => IsWindowReductionTriggerValid(configuration, t));
			bool flag2 = item.CustomTriggers == null || item.CustomTriggers.Count == 0;
			bool flag3 = item.Conditions == null || item.Conditions.Count == 0;
			if (!flag)
			{
				list2.Add(new ErrorParameter
				{
					Key = ErrorParameterKey.ParameterName,
					Value = "Triggers"
				});
				break;
			}
			if (!flag2)
			{
				list2.Add(new ErrorParameter
				{
					Key = ErrorParameterKey.ParameterName,
					Value = "CustomTriggers"
				});
				break;
			}
			if (!flag3)
			{
				list2.Add(new ErrorParameter
				{
					Key = ErrorParameterKey.ParameterName,
					Value = "Conditions"
				});
				break;
			}
			foreach (ActionDescription action in item.Actions)
			{
				if (!IsWindowReductionTargetValid(configuration, action.ActionType, action.Target))
				{
					list2.Add(new ErrorParameter
					{
						Key = ErrorParameterKey.ParameterName,
						Value = "Target"
					});
					break;
				}
			}
			if (list2.Count > 0)
			{
				break;
			}
		}
		if (list2.Count > 0)
		{
			list.Add(new ErrorEntry
			{
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.Interaction,
					Id = interaction.Id
				},
				ErrorCode = ValidationErrorCode.InvalidInteraction,
				ErrorParameters = list2
			});
		}
		return list;
	}

	private bool IsWindowReductionTriggerValid(IRepository configuration, Trigger trigger)
	{
		LogicalDevice logicalDeviceFromEntityDesc = GetLogicalDeviceFromEntityDesc(configuration, trigger.Entity);
		if (trigger.EventType.Contains("StateChanged") && logicalDeviceFromEntityDesc != null)
		{
			return logicalDeviceFromEntityDesc.DeviceType == "WindowDoorSensor";
		}
		return false;
	}

	private bool IsWindowReductionTargetValid(IRepository configuration, string actionType, LinkBinding target)
	{
		LogicalDevice logicalDeviceFromEntityDesc = GetLogicalDeviceFromEntityDesc(configuration, target);
		if (!(logicalDeviceFromEntityDesc is ThermostatActuator))
		{
			return false;
		}
		BaseDevice baseDevice = logicalDeviceFromEntityDesc.BaseDevice;
		List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
		list.Add(new KeyValuePair<string, string>(BuiltinPhysicalDeviceType.RST.ToString(), "1.0"));
		list.Add(new KeyValuePair<string, string>(BuiltinPhysicalDeviceType.RST.ToString(), "1.1"));
		list.Add(new KeyValuePair<string, string>(BuiltinPhysicalDeviceType.WRT.ToString(), "1.0"));
		list.Add(new KeyValuePair<string, string>(BuiltinPhysicalDeviceType.WRT.ToString(), "1.1"));
		list.Add(new KeyValuePair<string, string>(BuiltinPhysicalDeviceType.RST2.ToString(), "1.0"));
		List<KeyValuePair<string, string>> source = list;
		return source.Any((KeyValuePair<string, string> m) => m.Key == baseDevice.DeviceType && m.Value == baseDevice.DeviceVersion);
	}

	private LogicalDevice GetLogicalDeviceFromEntityDesc(IRepository configuration, LinkBinding linkBinding)
	{
		if (linkBinding.LinkType != EntityType.LogicalDevice)
		{
			return null;
		}
		return configuration.GetLogicalDevice(linkBinding.EntityIdAsGuid());
	}

	private List<ErrorEntry> CheckISR2Interaction(IRepository configRepo, Interaction interaction)
	{
		List<ErrorEntry> list = new List<ErrorEntry>();
		List<ErrorParameter> errorParameters = new List<ErrorParameter>();
		interaction.Rules.Where((Rule rule) => rule.Actions.Any((ActionDescription actn) => actn.ActionType == "SetStateWithBehavior")).ToList().ForEach(delegate(Rule rule)
		{
			errorParameters.AddRange(CheckISR2Rule(configRepo, rule));
		});
		if (errorParameters.Count > 0)
		{
			list.Add(new ErrorEntry
			{
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.Interaction,
					Id = interaction.Id
				},
				ErrorCode = ValidationErrorCode.InvalidInteraction,
				ErrorParameters = errorParameters
			});
		}
		return list;
	}

	private List<ErrorParameter> CheckISR2Rule(IRepository config, Rule rule)
	{
		List<ErrorParameter> errorParams = new List<ErrorParameter>();
		rule.Actions.ForEach(delegate(ActionDescription acn)
		{
			if (acn.ActionType == "SetStateWithBehavior")
			{
				errorParams.AddRange(CheckActionIntegrity(config, acn));
			}
		});
		return errorParams;
	}

	private List<ErrorParameter> CheckActionIntegrity(IRepository cfgRepo, ActionDescription action)
	{
		List<ErrorParameter> list = new List<ErrorParameter>();
		CheckTarget(cfgRepo, action.Target, list);
		CheckActionParams(action.Data, list);
		return list;
	}

	private void CheckActionParams(List<Parameter> actionData, List<ErrorParameter> errors)
	{
		if (InvalidDriveTime(actionData))
		{
			errors.Add(new ErrorParameter
			{
				Key = ErrorParameterKey.ParameterName,
				Value = ISRErrors.WrongDriveTime
			});
		}
		if (InvalidControlBehavior(actionData))
		{
			errors.Add(new ErrorParameter
			{
				Key = ErrorParameterKey.ParameterName,
				Value = ISRErrors.WrongControlBehavior
			});
		}
		if (InvalidShutterLevel(actionData))
		{
			errors.Add(new ErrorParameter
			{
				Key = ErrorParameterKey.ParameterName,
				Value = ISRErrors.WrongShutterLevel
			});
		}
	}

	private bool InvalidShutterLevel(List<Parameter> actionData)
	{
		decimal? numericValue = actionData.GetNumericValue("ShutterLevel");
		if (numericValue.HasValue && !(numericValue.Value < 0m))
		{
			return numericValue.Value > 100m;
		}
		return true;
	}

	private bool InvalidDriveTime(List<Parameter> actionData)
	{
		decimal? numericValue = actionData.GetNumericValue("StepDriveTime");
		if (numericValue.HasValue)
		{
			decimal? num = numericValue;
			if (num.GetValueOrDefault() < 1m)
			{
				return num.HasValue;
			}
			return false;
		}
		return true;
	}

	private bool InvalidControlBehavior(List<Parameter> actionData)
	{
		string stringValue = actionData.GetStringValue("ShutterControlBehavior");
		if (stringValue != "Inverted")
		{
			return stringValue != "Normal";
		}
		return false;
	}

	private void CheckTarget(IRepository cfgRepo, LinkBinding target, List<ErrorParameter> errorParams)
	{
		if (target != null)
		{
			if (target.LinkType == EntityType.LogicalDevice)
			{
				if (!(GetLogicalDeviceFromEntityDesc(cfgRepo, target) is RollerShutterActuator))
				{
					errorParams.Add(new ErrorParameter
					{
						Key = ErrorParameterKey.ParameterName,
						Value = ISRErrors.IncompatibleTargetDeviceType
					});
				}
			}
			else
			{
				errorParams.Add(new ErrorParameter
				{
					Key = ErrorParameterKey.ParameterName,
					Value = ISRErrors.WrongTargetType
				});
			}
		}
		else
		{
			errorParams.Add(new ErrorParameter
			{
				Key = ErrorParameterKey.ParameterName,
				Value = ISRErrors.MissingTarget
			});
		}
	}
}
