using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public class WindowDoorSensorConfiguration : SensorConfiguration
{
	private class DeviceInteraction
	{
		public Trigger Trigger { get; set; }

		public ActionDescription Action { get; set; }
	}

	private List<DeviceInteraction> deviceInteractions;

	public WindowDoorSensorChannel WindowDoorSensorChannel { get; private set; }

	public WindowDoorSensorConfiguration(LogicalDevice logicalDevice, byte[] address)
		: base(logicalDevice, address)
	{
		WindowDoorSensorChannel = new WindowDoorSensorChannel(1, byte.MaxValue);
		deviceInteractions = new List<DeviceInteraction>();
	}

	public override void InitializeConfiguration(IList<byte[]> shcAddresses, IDictionary<Guid, SensorConfiguration> sensors, IDictionary<Guid, ActuatorConfiguration> actuators)
	{
		base.InitializeConfiguration(shcAddresses, sensors, actuators);
		if (!(base.LogicalDeviceContract is WindowDoorSensor windowDoorSensor))
		{
			throw new ArgumentException("LogicalDevice type doesn't match WindowDoorSensorConfiguration: " + base.LogicalDeviceContract.GetType().Name);
		}
		WindowDoorSensorChannel.EventFilterTime = windowDoorSensor.EventFilterTime;
		base.GlobalStatusInfo.StatusInfoDestinationAddress = shcAddresses[0];
	}

	public override bool CreateLinks(Trigger trigger, ActionDescription action, ActuatorConfiguration actuatorConfiguration, Rule rule)
	{
		if (IsStateReportingLink(trigger, actuatorConfiguration))
		{
			return CreateStateReportingLink(trigger, actuatorConfiguration);
		}
		if (IsWindowReduction(action))
		{
			return CreateWindowReductionLink(trigger, actuatorConfiguration, action, rule);
		}
		bool? flag = IsOpenEvent(trigger);
		if (!flag.HasValue)
		{
			return false;
		}
		bool value = flag.Value;
		ProfileAction profileAction = ProfileAction.NoAction;
		ProfileAction profileAction2 = ProfileAction.NoAction;
		profileAction = ((!value) ? ProfileAction.NoAction : ProfileAction.On);
		profileAction2 = (value ? ProfileAction.NoAction : ProfileAction.On);
		if (action != null)
		{
			deviceInteractions.Add(new DeviceInteraction
			{
				Action = action,
				Trigger = trigger
			});
		}
		CreateLinkToLinkPartner(trigger, actuatorConfiguration, rule, profileAction, profileAction2, action);
		return true;
	}

	private void CreateLinkToLinkPartner(Trigger trigger, ActuatorConfiguration actuatorConfiguration, Rule rule, ProfileAction aboveAction, ProfileAction belowAction, ActionDescription deviceAction)
	{
		IEnumerable<LinkPartner> enumerable = CreateActuatorLinks(WindowDoorSensorChannel.ChannelIndex, actuatorConfiguration, trigger.Entity.EntityIdAsGuid(), deviceAction, ProfileAction.NoAction, aboveAction, belowAction, 50, rule);
		foreach (LinkPartner item in enumerable)
		{
			if (!WindowDoorSensorChannel.Links.ContainsKey(item))
			{
				WindowDoorSensorChannel.Links[item] = new BaseLink();
			}
		}
	}

	private bool CreateWindowReductionLink(Trigger trigger, ActuatorConfiguration actuatorConfiguration, ActionDescription action, Rule rule)
	{
		CreateLinkToLinkPartner(trigger, actuatorConfiguration, rule, ProfileAction.NoAction, ProfileAction.NoAction, action);
		return true;
	}

	private bool CreateStateReportingLink(Trigger trigger, ActuatorConfiguration actuatorConfiguration)
	{
		CreateLinkToLinkPartner(trigger, actuatorConfiguration, null, ProfileAction.NoAction, ProfileAction.NoAction, null);
		return true;
	}

	public override IEnumerable<int> GetUsedChannels(Trigger trigger)
	{
		ICollection<int> collection = new List<int>();
		bool? flag = IsOpenEvent(trigger);
		if (flag.HasValue)
		{
			collection.Add(flag.Value ? 1 : 2);
		}
		return collection;
	}

	public override void SaveConfiguration(DeviceConfiguration configuration)
	{
		base.GlobalStatusInfo.SaveConfiguration(configuration.Channels);
		WindowDoorSensorChannel.SaveConfiguration(configuration.Channels);
	}

	private bool? IsOpenEvent(Trigger aTrigger)
	{
		if (aTrigger.TriggerConditions == null || aTrigger.TriggerConditions.Count != 1 || aTrigger.TriggerConditions[0].Operator != ConditionOperator.Equal)
		{
			return null;
		}
		if (IsWdsEventOperand(aTrigger.TriggerConditions[0].LeftHandOperand) && aTrigger.TriggerConditions[0].RightHandOperand is ConstantBooleanBinding)
		{
			return (aTrigger.TriggerConditions[0].RightHandOperand as ConstantBooleanBinding).Value;
		}
		if (IsWdsEventOperand(aTrigger.TriggerConditions[0].RightHandOperand) && aTrigger.TriggerConditions[0].LeftHandOperand is ConstantBooleanBinding)
		{
			return (aTrigger.TriggerConditions[0].LeftHandOperand as ConstantBooleanBinding).Value;
		}
		return null;
	}

	private bool IsWdsEventOperand(DataBinding operand)
	{
		if (operand is FunctionBinding && (operand as FunctionBinding).Function == FunctionIdentifier.GetEventProperty && (operand as FunctionBinding).Parameters.Count == 1 && (operand as FunctionBinding).Parameters[0].Name == "EventPropertyName" && (operand as FunctionBinding).Parameters[0].Value is ConstantStringBinding)
		{
			return "IsOpen".Equals(((operand as FunctionBinding).Parameters[0].Value as ConstantStringBinding).Value, StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	private bool IsWindowReduction(ActionDescription action)
	{
		return action.ActionType == "WindowReduction";
	}

	private bool IsStateReportingLink(Trigger trigger, ActuatorConfiguration actuatorConfiguration)
	{
		return actuatorConfiguration is ShcActuatorConfiguration;
	}
}
