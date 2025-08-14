using System;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;

public class MotionDetectionSensor : LogicalDevice
{
	[XmlIgnore]
	public override string PrimaryPropertyName
	{
		get
		{
			return "MotionDetected";
		}
		set
		{
		}
	}

	public new MotionDetectionSensor Clone()
	{
		return (MotionDetectionSensor)base.Clone();
	}

	public new MotionDetectionSensor Clone(Guid tag)
	{
		return (MotionDetectionSensor)base.Clone(tag);
	}

	protected override Entity CreateClone()
	{
		return new MotionDetectionSensor();
	}
}
