using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public class MotionDetectorConfiguration : SensorConfiguration
{
	private MotionDetectorChannel MotionDetector { get; set; }

	public MotionDetectorConfiguration(LogicalDevice logicalDevice, byte[] address)
		: base(logicalDevice, address)
	{
		MotionDetector = new MotionDetectorChannel(1, byte.MaxValue);
	}

	public override void InitializeConfiguration(IList<byte[]> shcAddresses, IDictionary<Guid, SensorConfiguration> sensors, IDictionary<Guid, ActuatorConfiguration> actuators)
	{
		base.InitializeConfiguration(shcAddresses, sensors, actuators);
		base.GlobalStatusInfo.StatusInfoDestinationAddress = shcAddresses[0];
		MotionDetector.FilterInterval = 0;
		MotionDetector.NumberOfMovements = 1;
		MotionDetector.IntervalToNextEvent = 1;
		MotionDetector.NumberOfBrightnessMeasurements = 2;
	}

	public override bool CreateLinks(Trigger trigger, ActionDescription action, ActuatorConfiguration actuatorConfiguration, Rule rule)
	{
		int num = 100;
		if (trigger.TriggerConditions != null && trigger.TriggerConditions.Count == 1)
		{
			num = GetLuminanceThresholdFromCondition(trigger.TriggerConditions[0]);
			if (num == -1)
			{
				return false;
			}
		}
		IEnumerable<LinkPartner> enumerable = CreateActuatorLinks(MotionDetector.ChannelIndex, actuatorConfiguration, trigger.Entity.EntityIdAsGuid(), action, ProfileAction.NoAction, ProfileAction.NoAction, ProfileAction.On, num, rule);
		foreach (LinkPartner item in enumerable)
		{
			MotionDetector.Links[item] = new BaseLink();
		}
		return true;
	}

	private int GetLuminanceThresholdFromCondition(Condition aCondition)
	{
		if (aCondition.Operator != ConditionOperator.Less && aCondition.Operator != ConditionOperator.LessOrEqual)
		{
			return -1;
		}
		if (IsLuminanceOperand(aCondition.LeftHandOperand))
		{
			return GetOperandIntegerValue(aCondition.RightHandOperand);
		}
		if (IsLuminanceOperand(aCondition.RightHandOperand))
		{
			return GetOperandIntegerValue(aCondition.LeftHandOperand);
		}
		return -1;
	}

	private bool IsLuminanceOperand(DataBinding operand)
	{
		if (operand is FunctionBinding && (operand as FunctionBinding).Function == FunctionIdentifier.GetEventProperty && (operand as FunctionBinding).Parameters.Count == 1 && (operand as FunctionBinding).Parameters[0].Name == "EventPropertyName" && (operand as FunctionBinding).Parameters[0].Value is ConstantStringBinding)
		{
			return "Luminance".Equals(((operand as FunctionBinding).Parameters[0].Value as ConstantStringBinding).Value, StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	public override void SaveConfiguration(DeviceConfiguration configuration)
	{
		base.GlobalStatusInfo.SaveConfiguration(configuration.Channels);
		MotionDetector.SaveConfiguration(configuration.Channels);
	}

	public override IEnumerable<int> GetUsedChannels(Trigger trigger)
	{
		return new int[0];
	}
}
