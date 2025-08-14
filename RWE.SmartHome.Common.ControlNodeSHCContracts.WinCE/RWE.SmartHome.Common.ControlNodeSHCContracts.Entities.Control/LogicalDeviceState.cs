using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.API;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.SensorStates;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;

[XmlInclude(typeof(TemperatureSensorState))]
[XmlInclude(typeof(RoomSetpointState))]
[XmlInclude(typeof(DimmerActuatorState))]
[XmlInclude(typeof(LuminanceSensorState))]
[XmlInclude(typeof(SmokeDetectionSensorState))]
[XmlInclude(typeof(RoomTemperatureState))]
[XmlInclude(typeof(HumiditySensorState))]
[XmlInclude(typeof(RoomHumidityState))]
[XmlInclude(typeof(RollerShutterActuatorState))]
[XmlInclude(typeof(SwitchActuatorState))]
[XmlInclude(typeof(ThermostatActuatorState))]
[XmlInclude(typeof(GenericDeviceState))]
[XmlInclude(typeof(WindowDoorSensorState))]
[XmlInclude(typeof(AlarmActuatorState))]
public abstract class LogicalDeviceState
{
	private Guid logicalDeviceId;

	[XmlAttribute(AttributeName = "LID")]
	public Guid LogicalDeviceId
	{
		get
		{
			return logicalDeviceId;
		}
		set
		{
			logicalDeviceId = value;
		}
	}

	[XmlIgnore]
	public LogicalDevice LogicalDevice
	{
		get
		{
			return Resolver.ToLogicalDevice(null, logicalDeviceId);
		}
		set
		{
			logicalDeviceId = Resolver.FromLogicalDevice(value);
		}
	}

	public virtual LogicalDeviceState Clone()
	{
		LogicalDeviceState logicalDeviceState = CreateClone();
		TransferProperties(logicalDeviceState);
		return logicalDeviceState;
	}

	public abstract void UpdateFrom(LogicalDeviceState value, DateTime timestamp);

	public abstract List<Property> GetProperties();

	public abstract override string ToString();

	public virtual LogicalDeviceState FromGeneric(GenericDeviceState genericState)
	{
		return null;
	}

	protected virtual LogicalDeviceState CreateClone()
	{
		throw new NotImplementedException();
	}

	protected virtual void TransferProperties(LogicalDeviceState clone)
	{
		clone.LogicalDeviceId = LogicalDeviceId;
	}
}
