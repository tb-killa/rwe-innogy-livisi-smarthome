using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.ErrorHandling;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public class PushButtonSensorConfiguration : SensorConfiguration
{
	private readonly byte channelIndexBase;

	private readonly bool swapButtons;

	private readonly bool isIndependentDevice;

	public PushButtonChannel[] PushButtons { get; private set; }

	public PushButtonSensorConfiguration(LogicalDevice logicalDevice, byte[] address)
		: base(logicalDevice, address)
	{
		if (!(logicalDevice is PushButtonSensor pushButtonSensor))
		{
			ErrorEntry error = new ErrorEntry
			{
				ErrorCode = ValidationErrorCode.MissingSensor,
				AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.LogicalDevice,
					Id = logicalDevice.Id
				}
			};
			throw new TransformationException(error);
		}
		isIndependentDevice = IsIndependentDevice(logicalDevice.BaseDevice.GetBuiltinDeviceDeviceType());
		bool needsLinkUpdatePendingFlag = isIndependentDevice;
		channelIndexBase = (byte)(isIndependentDevice ? 1u : 2u);
		int buttonCount = pushButtonSensor.ButtonCount;
		PushButtons = new PushButtonChannel[buttonCount];
		swapButtons = buttonCount == 2 && logicalDevice.BaseDevice.GetBuiltinDeviceDeviceType() != BuiltinPhysicalDeviceType.ISC2;
		for (byte b = 0; b < PushButtons.Length; b++)
		{
			PushButtons[b] = new PushButtonChannel((byte)(channelIndexBase + b), byte.MaxValue, needsLinkUpdatePendingFlag);
		}
	}

	public override bool CreateLinks(Trigger trigger, ActionDescription action, ActuatorConfiguration actuatorConfiguration, Rule rule)
	{
		if (trigger.TriggerConditions == null)
		{
			return false;
		}
		foreach (Condition triggerCondition in trigger.TriggerConditions)
		{
			int buttonIndexFromCondition = GetButtonIndexFromCondition(triggerCondition);
			if (buttonIndexFromCondition < 0)
			{
				return false;
			}
			ProfileAction switchEvent = ProfileAction.On;
			if (action != null && IsToggleAction(action))
			{
				switchEvent = ProfileAction.Toggle;
			}
			int channelIndexFromButtonNo = GetChannelIndexFromButtonNo(buttonIndexFromCondition);
			if (channelIndexFromButtonNo < 0)
			{
				throw CreateSensorValidationException(trigger.Entity.EntityIdAsGuid(), ValidationErrorCode.ButtonsOutOfRange);
			}
			try
			{
				IEnumerable<LinkPartner> enumerable = null;
				enumerable = CreateActuatorLinks((byte)channelIndexFromButtonNo, actuatorConfiguration, trigger.Entity.EntityIdAsGuid(), action, switchEvent, ProfileAction.NoAction, ProfileAction.NoAction, null, rule);
				foreach (LinkPartner item in enumerable)
				{
					PushButtons[channelIndexFromButtonNo - channelIndexBase].Links[item] = new BaseLink();
				}
			}
			catch (UnsupportedLinkException)
			{
				return false;
			}
		}
		return true;
	}

	private bool IsToggleAction(ActionDescription action)
	{
		return action.ActionType.Equals("Toggle");
	}

	private int GetButtonIndexFromCondition(Condition aCondition)
	{
		if (aCondition.Operator != ConditionOperator.Equal)
		{
			return -1;
		}
		if (IsButtonIndexOperand(aCondition.LeftHandOperand))
		{
			return GetOperandIntegerValue(aCondition.RightHandOperand);
		}
		if (IsButtonIndexOperand(aCondition.RightHandOperand))
		{
			return GetOperandIntegerValue(aCondition.LeftHandOperand);
		}
		return -1;
	}

	private bool IsButtonIndexOperand(DataBinding operand)
	{
		if (operand is FunctionBinding && (operand as FunctionBinding).Function == FunctionIdentifier.GetEventProperty && (operand as FunctionBinding).Parameters.Count == 1 && (operand as FunctionBinding).Parameters[0].Name == "EventPropertyName" && (operand as FunctionBinding).Parameters[0].Value is ConstantStringBinding)
		{
			return "index".Equals(((operand as FunctionBinding).Parameters[0].Value as ConstantStringBinding).Value, StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	public override void InitializeConfiguration(IList<byte[]> shcAddresses, IDictionary<Guid, SensorConfiguration> sensors, IDictionary<Guid, ActuatorConfiguration> actuators)
	{
		base.InitializeConfiguration(shcAddresses, sensors, actuators);
		AddCalibrationLinksIfNecessary(shcAddresses, actuators);
	}

	public override void CreateInternalLinks(IEnumerable<ActuatorConfiguration> actuatorConfigurations)
	{
		if (isIndependentDevice)
		{
			return;
		}
		foreach (ActuatorConfiguration item in actuatorConfigurations.Where((ActuatorConfiguration actuator) => actuator.PhysicalDeviceId == base.PhysicalDeviceId))
		{
			foreach (LinkPartner item2 in CreateInternalActuatorLinks(channelIndexBase, item, InternalLinkType.LowerButton))
			{
				PushButtons[0].Links.Add(item2, new BaseLink());
			}
			foreach (LinkPartner item3 in CreateInternalActuatorLinks((byte)(channelIndexBase + 1), item, InternalLinkType.UpperButton))
			{
				PushButtons[1].Links.Add(item3, new BaseLink());
			}
		}
	}

	public override void SaveConfiguration(DeviceConfiguration configuration)
	{
		if (isIndependentDevice)
		{
			base.GlobalStatusInfo.SaveConfiguration(configuration.Channels);
		}
		PushButtonChannel[] pushButtons = PushButtons;
		foreach (PushButtonChannel pushButtonChannel in pushButtons)
		{
			pushButtonChannel.SaveConfiguration(configuration.Channels);
		}
	}

	public override IEnumerable<int> GetUsedChannels(Trigger trigger)
	{
		List<int> list = new List<int>();
		foreach (Condition triggerCondition in trigger.TriggerConditions)
		{
			int buttonIndexFromCondition = GetButtonIndexFromCondition(triggerCondition);
			if (buttonIndexFromCondition >= 0 && !list.Contains(buttonIndexFromCondition))
			{
				list.Add(buttonIndexFromCondition);
			}
		}
		return list;
	}

	public int GetButtonNumberFromChannelIndex(int channelIndex)
	{
		if (swapButtons)
		{
			return Math.Abs(channelIndex - channelIndexBase - 1);
		}
		return channelIndex - channelIndexBase;
	}

	private static bool IsIndependentDevice(BuiltinPhysicalDeviceType deviceType)
	{
		switch (deviceType)
		{
		case BuiltinPhysicalDeviceType.WSC2:
		case BuiltinPhysicalDeviceType.BRC8:
		case BuiltinPhysicalDeviceType.ISC2:
			return true;
		case BuiltinPhysicalDeviceType.PSS:
		case BuiltinPhysicalDeviceType.PSSO:
		case BuiltinPhysicalDeviceType.PSD:
		case BuiltinPhysicalDeviceType.ISS2:
		case BuiltinPhysicalDeviceType.ISD2:
		case BuiltinPhysicalDeviceType.ISR2:
		case BuiltinPhysicalDeviceType.ChargingStation:
			return false;
		default:
			throw new ArgumentOutOfRangeException("deviceType");
		}
	}

	private void AddCalibrationLinksIfNecessary(IList<byte[]> shcAddresses, IDictionary<Guid, ActuatorConfiguration> actuators)
	{
		if (isIndependentDevice)
		{
			return;
		}
		ActuatorConfiguration actuatorConfiguration = actuators.Values.FirstOrDefault((ActuatorConfiguration actuator) => base.PhysicalDeviceId == actuator.PhysicalDeviceId);
		if (actuatorConfiguration != null && actuatorConfiguration is RollerShutterConfiguration && ((RollerShutterActuator)actuatorConfiguration.LogicalDeviceContract).IsCalibrating)
		{
			LinkPartner key = new LinkPartner(Guid.Empty, shcAddresses[1], 63);
			for (byte b = 0; b < PushButtons.Length; b++)
			{
				PushButtons[b].Links.Add(key, new BaseLink());
			}
		}
	}

	private int GetChannelIndexFromButtonNo(int buttonNo)
	{
		if (buttonNo < 0 || buttonNo >= PushButtons.Length)
		{
			return -1;
		}
		if (swapButtons)
		{
			return channelIndexBase + 1 - buttonNo;
		}
		return channelIndexBase + buttonNo;
	}

	private static TransformationException CreateSensorValidationException(Guid sensorId, ValidationErrorCode errorCode)
	{
		ErrorEntry errorEntry = new ErrorEntry();
		errorEntry.ErrorCode = errorCode;
		errorEntry.AffectedEntity = new EntityMetadata
		{
			EntityType = EntityType.LogicalDevice,
			Id = sensorId
		};
		ErrorEntry error = errorEntry;
		return new TransformationException(error);
	}
}
