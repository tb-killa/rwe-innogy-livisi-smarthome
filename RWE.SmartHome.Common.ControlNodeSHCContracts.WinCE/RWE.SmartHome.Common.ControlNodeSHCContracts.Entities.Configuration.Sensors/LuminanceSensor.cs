using System;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Attributes;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.SensorStates;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;

[LogicalDeviceStateType(typeof(LuminanceSensorState))]
public class LuminanceSensor : LogicalDevice
{
	[XmlIgnore]
	public override string PrimaryPropertyName
	{
		get
		{
			return "Luminance";
		}
		set
		{
		}
	}

	public new LuminanceSensor Clone()
	{
		return (LuminanceSensor)base.Clone();
	}

	public new LuminanceSensor Clone(Guid tag)
	{
		return (LuminanceSensor)base.Clone(tag);
	}

	protected override Entity CreateClone()
	{
		return new LuminanceSensor();
	}
}
