using System;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Attributes;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.SensorStates;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;

[LogicalDeviceStateType(typeof(SmokeDetectionSensorState))]
public class SmokeDetectorSensor : LogicalDevice
{
	[XmlIgnore]
	public override string PrimaryPropertyName
	{
		get
		{
			return "IsSmokeAlarm";
		}
		set
		{
		}
	}

	protected override Entity CreateClone()
	{
		return new SmokeDetectorSensor();
	}

	public new SmokeDetectorSensor Clone()
	{
		return (SmokeDetectorSensor)base.Clone();
	}

	public new SmokeDetectorSensor Clone(Guid tag)
	{
		return (SmokeDetectorSensor)base.Clone(tag);
	}
}
