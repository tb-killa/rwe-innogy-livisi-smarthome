using System;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Attributes;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.SensorStates;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;

[LogicalDeviceStateType(typeof(RoomHumidityState))]
public class RoomHumidity : LogicalDevice
{
	[XmlIgnore]
	public override string PrimaryPropertyName
	{
		get
		{
			return "Humidity";
		}
		set
		{
		}
	}

	public new RoomHumidity Clone()
	{
		return (RoomHumidity)base.Clone();
	}

	public new RoomHumidity Clone(Guid tag)
	{
		return (RoomHumidity)base.Clone(tag);
	}

	protected override Entity CreateClone()
	{
		return new RoomHumidity();
	}
}
