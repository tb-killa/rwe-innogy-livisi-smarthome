using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Attributes;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.SensorStates;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;

[LogicalDeviceStateType(typeof(WindowDoorSensorState))]
public class WindowDoorSensor : LogicalDevice
{
	public int EventFilterTime { get; set; }

	[XmlIgnore]
	public override string PrimaryPropertyName
	{
		get
		{
			return "IsOpen";
		}
		set
		{
		}
	}

	protected override Entity CreateClone()
	{
		return new WindowDoorSensor();
	}

	protected override void TransferProperties(Entity clone)
	{
		base.TransferProperties(clone);
		WindowDoorSensor windowDoorSensor = (WindowDoorSensor)clone;
		windowDoorSensor.EventFilterTime = EventFilterTime;
	}

	public new WindowDoorSensor Clone()
	{
		return (WindowDoorSensor)base.Clone();
	}

	public new WindowDoorSensor Clone(Guid tag)
	{
		return (WindowDoorSensor)base.Clone(tag);
	}

	public override List<Property> GetAllProperties()
	{
		List<Property> allProperties = base.GetAllProperties();
		allProperties.Add(new NumericProperty
		{
			Name = "EventFilterTime",
			Value = EventFilterTime
		});
		return allProperties;
	}
}
