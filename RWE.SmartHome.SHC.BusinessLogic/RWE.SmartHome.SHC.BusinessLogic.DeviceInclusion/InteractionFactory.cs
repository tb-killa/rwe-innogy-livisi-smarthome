using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.BusinessLogic.DeviceInclusion;

public static class InteractionFactory
{
	private enum SwitchState
	{
		SwitchOn,
		SwitchOff
	}

	private static class CompositePushButtons
	{
		public static int BUTTON_UP = 0;

		public static int BUTTON_DOWN = 1;
	}

	private const string LoggingSource = "InteractionFactory";

	public static Interaction CreateDefaultInteractionForDevice(BuiltinPhysicalDeviceType deviceType, List<LogicalDevice> logicalDevices)
	{
		return deviceType switch
		{
			BuiltinPhysicalDeviceType.ISD2 => CreateDefaultIsdInteraction(logicalDevices), 
			BuiltinPhysicalDeviceType.ISR2 => CreateDefaultIsrInteraction(logicalDevices), 
			BuiltinPhysicalDeviceType.ISS2 => CreateDefaultIssInteraction(logicalDevices), 
			_ => null, 
		};
	}

	private static Interaction CreateDefaultIsrInteraction(List<LogicalDevice> logicalDevices)
	{
		Interaction interaction = new Interaction();
		interaction.Id = Guid.NewGuid();
		interaction.Rules = new List<Rule>();
		PushButtonSensor logicalOfType = GetLogicalOfType<PushButtonSensor>(logicalDevices);
		RollerShutterActuator logicalOfType2 = GetLogicalOfType<RollerShutterActuator>(logicalDevices);
		if (logicalOfType == null || logicalOfType2 == null)
		{
			Log.Error(Module.BusinessLogic, "InteractionFactory", "Malformed ISR device.");
			return null;
		}
		interaction.Rules.Add(new Rule
		{
			InteractionId = interaction.Id,
			Actions = new List<ActionDescription> { CreateDefaultRollerShutterAction(logicalOfType2.Id, 0m) },
			Conditions = new List<Condition>(),
			Triggers = new List<Trigger> { CreatePushButtonTrigger(logicalOfType.Id, CompositePushButtons.BUTTON_DOWN) }
		});
		interaction.Rules.Add(new Rule
		{
			InteractionId = interaction.Id,
			Actions = new List<ActionDescription> { CreateDefaultRollerShutterAction(logicalOfType2.Id, 100m) },
			Conditions = new List<Condition>(),
			Triggers = new List<Trigger> { CreatePushButtonTrigger(logicalOfType.Id, CompositePushButtons.BUTTON_UP) }
		});
		return interaction;
	}

	private static Interaction CreateDefaultIsdInteraction(List<LogicalDevice> logicalDevices)
	{
		Interaction interaction = new Interaction();
		interaction.Id = Guid.NewGuid();
		interaction.Rules = new List<Rule>();
		PushButtonSensor logicalOfType = GetLogicalOfType<PushButtonSensor>(logicalDevices);
		DimmerActuator logicalOfType2 = GetLogicalOfType<DimmerActuator>(logicalDevices);
		if (logicalOfType == null || logicalOfType2 == null)
		{
			Log.Error(Module.BusinessLogic, "InteractionFactory", "Malformed ISD device.");
			return null;
		}
		interaction.Rules = new List<Rule>
		{
			new Rule
			{
				InteractionId = interaction.Id,
				Actions = new List<ActionDescription> { CreateDefaultDimmerAction(logicalOfType2.Id, 100) },
				Conditions = new List<Condition>(),
				Triggers = new List<Trigger> { CreatePushButtonTrigger(logicalOfType.Id, 0) }
			},
			new Rule
			{
				InteractionId = interaction.Id,
				Actions = new List<ActionDescription> { CreateDefaultDimmerAction(logicalOfType2.Id, 0) },
				Conditions = new List<Condition>(),
				Triggers = new List<Trigger> { CreatePushButtonTrigger(logicalOfType.Id, 1) }
			}
		};
		return interaction;
	}

	private static Interaction CreateDefaultIssInteraction(List<LogicalDevice> logicalDevices)
	{
		Interaction interaction = new Interaction();
		interaction.Id = Guid.NewGuid();
		Interaction interaction2 = interaction;
		PushButtonSensor logicalOfType = GetLogicalOfType<PushButtonSensor>(logicalDevices);
		SwitchActuator logicalOfType2 = GetLogicalOfType<SwitchActuator>(logicalDevices);
		if (logicalOfType == null || logicalOfType2 == null)
		{
			Log.Error(Module.BusinessLogic, "InteractionFactory", "Malformed ISS device.");
			return null;
		}
		interaction2.Rules = new List<Rule>
		{
			new Rule
			{
				InteractionId = interaction2.Id,
				Triggers = new List<Trigger>
				{
					CreatePushButtonTrigger(logicalOfType.Id, 0),
					CreatePushButtonTrigger(logicalOfType.Id, 1)
				},
				Conditions = new List<Condition>(),
				Actions = new List<ActionDescription> { GetToggleAction(logicalOfType2.Id) }
			}
		};
		return interaction2;
	}

	private static Trigger CreatePushButtonTrigger(Guid pushbuttonCapabilityId, int buttonIndex)
	{
		Trigger trigger = new Trigger();
		trigger.Entity = new LinkBinding(EntityType.LogicalDevice, pushbuttonCapabilityId);
		trigger.EventType = "ButtonPressed";
		trigger.TriggerConditions = new List<Condition>
		{
			new Condition
			{
				LeftHandOperand = new FunctionBinding
				{
					Function = FunctionIdentifier.GetEventProperty,
					Parameters = new List<Parameter>
					{
						new Parameter
						{
							Name = "EventPropertyName",
							Value = new ConstantStringBinding
							{
								Value = "index"
							}
						}
					}
				},
				Operator = ConditionOperator.Equal,
				RightHandOperand = new ConstantNumericBinding
				{
					Value = buttonIndex
				}
			}
		};
		return trigger;
	}

	private static ActionDescription GetToggleAction(Guid switchCapabilityId)
	{
		ActionDescription actionDescription = new ActionDescription();
		actionDescription.Target = new LinkBinding(EntityType.LogicalDevice, switchCapabilityId);
		actionDescription.ActionType = "Toggle";
		actionDescription.Namespace = "core.RWE";
		actionDescription.Data = new List<Parameter>();
		return actionDescription;
	}

	private static T GetLogicalOfType<T>(List<LogicalDevice> logicalDevices) where T : LogicalDevice
	{
		return logicalDevices.Where((LogicalDevice ld) => ld is T).FirstOrDefault() as T;
	}

	private static ActionDescription CreateDefaultRollerShutterAction(Guid rollerShutterCapabilityId, decimal targetShutterLevel)
	{
		ActionDescription actionDescription = new ActionDescription();
		actionDescription.Target = new LinkBinding(EntityType.LogicalDevice, rollerShutterCapabilityId);
		actionDescription.ActionType = "SetStateWithBehavior";
		actionDescription.Namespace = "core.RWE";
		actionDescription.Data = new List<Parameter>
		{
			new Parameter
			{
				Name = "StepDriveTime",
				Value = new ConstantNumericBinding
				{
					Value = 5m
				}
			},
			new Parameter
			{
				Name = "ShutterControlBehavior",
				Value = new ConstantStringBinding
				{
					Value = "Normal"
				}
			},
			new Parameter
			{
				Name = "ShutterLevel",
				Value = new ConstantNumericBinding
				{
					Value = targetShutterLevel
				}
			}
		};
		return actionDescription;
	}

	private static ActionDescription CreateDefaultDimmerAction(Guid dimmerCapabilityId, int targetLevel)
	{
		ActionDescription actionDescription = new ActionDescription();
		actionDescription.Target = new LinkBinding(EntityType.LogicalDevice, dimmerCapabilityId);
		actionDescription.ActionType = "SetState";
		actionDescription.Namespace = "core.RWE";
		actionDescription.Data = new List<Parameter>
		{
			new Parameter
			{
				Name = "DimLevel",
				Value = new ConstantNumericBinding
				{
					Value = targetLevel
				}
			}
		};
		return actionDescription;
	}
}
