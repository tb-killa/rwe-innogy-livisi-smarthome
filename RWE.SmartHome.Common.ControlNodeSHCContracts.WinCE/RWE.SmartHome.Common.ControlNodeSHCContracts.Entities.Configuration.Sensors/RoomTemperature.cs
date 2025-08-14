using System;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Attributes;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.SensorStates;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;

[LogicalDeviceStateType(typeof(RoomTemperatureState))]
public class RoomTemperature : LogicalDevice
{
	[XmlIgnore]
	public override string PrimaryPropertyName
	{
		get
		{
			return "Temperature";
		}
		set
		{
		}
	}

	public new RoomTemperature Clone()
	{
		return (RoomTemperature)base.Clone();
	}

	public new RoomTemperature Clone(Guid tag)
	{
		return (RoomTemperature)base.Clone(tag);
	}

	protected override Entity CreateClone()
	{
		return new RoomTemperature();
	}
}
