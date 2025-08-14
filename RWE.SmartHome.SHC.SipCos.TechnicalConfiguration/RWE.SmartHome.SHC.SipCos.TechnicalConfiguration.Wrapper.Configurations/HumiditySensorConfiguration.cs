using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public class HumiditySensorConfiguration : IndirectSensorConfiguration
{
	public HumiditySensorConfiguration(LogicalDevice logicalDevice, byte[] address)
		: base(logicalDevice, address)
	{
		if (!(logicalDevice is HumiditySensor))
		{
			throw new ArgumentException("LogicalDevice type doesn't match HumiditySensorConfiguration: " + logicalDevice.GetType().Name);
		}
	}

	public override bool CreateLinks(Trigger trigger, ActionDescription action, ActuatorConfiguration actuatorConfiguration, Rule rule)
	{
		return false;
	}

	public override void InitializeConfiguration(IList<byte[]> shcAddresses, IDictionary<Guid, SensorConfiguration> sensors, IDictionary<Guid, ActuatorConfiguration> actuators)
	{
		HumiditySensor humiditySensor = base.LogicalDeviceContract as HumiditySensor;
		ThermostatActuatorConfiguration thermostatActuatorConfiguration = (from ta in actuators.Values.OfType<ThermostatActuatorConfiguration>()
			where ta.PhysicalDeviceId == base.LogicalDeviceContract.BaseDeviceId
			select ta).FirstOrDefault();
		if (thermostatActuatorConfiguration != null)
		{
			thermostatActuatorConfiguration.ThermostatSensor.IsMoldProtectionActivated = humiditySensor.IsMoldProtectionActivated;
			thermostatActuatorConfiguration.ThermostatSensor.AntiMoldPercent = humiditySensor.HumidityMoldProtection;
		}
	}
}
