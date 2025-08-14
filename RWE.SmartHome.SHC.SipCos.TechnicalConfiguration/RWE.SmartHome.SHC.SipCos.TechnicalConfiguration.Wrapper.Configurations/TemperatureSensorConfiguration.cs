using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public class TemperatureSensorConfiguration : IndirectSensorConfiguration
{
	private byte sensorChannelIndex = byte.MaxValue;

	public TemperatureSensorConfiguration(LogicalDevice logicalDevice, byte[] address)
		: base(logicalDevice, address)
	{
		if (!(logicalDevice is TemperatureSensor))
		{
			throw new ArgumentException("LogicalDevice type doesn't match RoomTemperatureSensorConfiguration: " + logicalDevice.GetType().Name);
		}
		BuiltinPhysicalDeviceType builtinDeviceDeviceType = logicalDevice.BaseDevice.GetBuiltinDeviceDeviceType();
		if (builtinDeviceDeviceType == BuiltinPhysicalDeviceType.RST || builtinDeviceDeviceType == BuiltinPhysicalDeviceType.WRT || builtinDeviceDeviceType == BuiltinPhysicalDeviceType.RST2)
		{
			sensorChannelIndex = 1;
		}
	}

	public override bool CreateLinks(Trigger trigger, ActionDescription action, ActuatorConfiguration actuatorConfiguration, Rule rule)
	{
		bool result = false;
		if (action != null && trigger != null && action.ActionType == "ControlValve")
		{
			result = CreateFSCLinks(trigger, action, actuatorConfiguration, rule);
		}
		return result;
	}

	private bool CreateFSCLinks(Trigger trigger, ActionDescription action, ActuatorConfiguration actuatorConfiguration, Rule rule)
	{
		if (trigger.TriggerConditions != null && trigger.TriggerConditions.Count > 0)
		{
			throw new ArgumentException("Trigger conditons not allowed for the ControlValve action");
		}
		bool result = false;
		try
		{
			IEnumerable<LinkPartner> enumerable = CreateActuatorLinks(sensorChannelIndex, actuatorConfiguration, trigger.Entity.EntityIdAsGuid(), action, ProfileAction.NoAction, ProfileAction.NoAction, ProfileAction.NoAction, null, rule);
			if (enumerable != null && enumerable.Count() > 0)
			{
				result = true;
			}
		}
		catch (Exception ex)
		{
			Log.Exception(Module.SipCosProtocolAdapter, GetType().ToString(), ex, "Error occured while creating links {0}", ex.Message);
		}
		return result;
	}

	public override void InitializeConfiguration(IList<byte[]> shcAddresses, IDictionary<Guid, SensorConfiguration> sensors, IDictionary<Guid, ActuatorConfiguration> actuators)
	{
		TemperatureSensor temperatureSensor = base.LogicalDeviceContract as TemperatureSensor;
		ThermostatActuatorConfiguration thermostatActuatorConfiguration = (from ta in actuators.Values.OfType<ThermostatActuatorConfiguration>()
			where ta.PhysicalDeviceId == base.LogicalDeviceContract.BaseDeviceId
			select ta).FirstOrDefault();
		if (thermostatActuatorConfiguration != null)
		{
			thermostatActuatorConfiguration.ThermostatSensor.IsFreezeProtectionActivated = temperatureSensor.IsFreezeProtectionActivated;
			thermostatActuatorConfiguration.ThermostatSensor.AntiFreezeCentigrade = temperatureSensor.FreezeProtection;
		}
	}
}
