using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;

public class RoomSetpointState : LogicalDeviceState, IEquatable<RoomSetpointState>
{
	private const string POINT_TEMP_PROP_NAME = "PointTemperature";

	private NumericProperty pointTemperatureProperty = new NumericProperty
	{
		Name = "PointTemperature"
	};

	[XmlIgnore]
	public decimal? PointTemperature
	{
		get
		{
			return pointTemperatureProperty.Value;
		}
		set
		{
			pointTemperatureProperty.Value = value;
		}
	}

	[XmlAttribute(AttributeName = "PtTmp")]
	public string PointTemperatureString
	{
		get
		{
			return PointTemperatureProperty.ValueStr;
		}
		set
		{
			PointTemperatureProperty.ValueStr = value;
		}
	}

	public DateTime? PointTemperatureUpdateTimestamp
	{
		get
		{
			return PointTemperatureProperty.UpdateTimestamp;
		}
		set
		{
			PointTemperatureProperty.UpdateTimestamp = value;
		}
	}

	public NumericProperty PointTemperatureProperty => pointTemperatureProperty;

	public override void UpdateFrom(LogicalDeviceState value, DateTime timestamp)
	{
		if (value is RoomSetpointState roomSetpointState)
		{
			PointTemperatureProperty.SetValueWithTimestampUpdateIfNotNull(roomSetpointState.PointTemperatureProperty, timestamp);
		}
	}

	public override List<Property> GetProperties()
	{
		List<Property> list = new List<Property>();
		list.Add(PointTemperatureProperty);
		return list;
	}

	public override string ToString()
	{
		return string.Format("PointTemperature = {0}", PointTemperatureProperty.Value.HasValue ? PointTemperatureProperty.ValueStr : "null");
	}

	protected override LogicalDeviceState CreateClone()
	{
		return new RoomSetpointState();
	}

	protected override void TransferProperties(LogicalDeviceState clone)
	{
		base.TransferProperties(clone);
		RoomSetpointState roomSetpointState = (RoomSetpointState)clone;
		roomSetpointState.PointTemperatureProperty.CopyValueAndTimestamp(PointTemperatureProperty);
	}

	public bool Equals(RoomSetpointState other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		return other.LogicalDeviceId.Equals(base.LogicalDeviceId) && other.PointTemperatureProperty.Equals(PointTemperatureProperty);
	}

	public override bool Equals(object obj)
	{
		if (object.ReferenceEquals(null, obj))
		{
			return false;
		}
		if (object.ReferenceEquals(this, obj))
		{
			return true;
		}
		if ((object)obj.GetType() != typeof(RoomSetpointState))
		{
			return false;
		}
		return Equals((RoomSetpointState)obj);
	}

	public override int GetHashCode()
	{
		return PointTemperatureProperty.Value.GetHashCode();
	}

	public override LogicalDeviceState FromGeneric(GenericDeviceState genericState)
	{
		RoomSetpointState roomSetpointState = new RoomSetpointState();
		roomSetpointState.LogicalDeviceId = genericState.LogicalDeviceId;
		if (genericState.Properties.Where((Property p) => p.Name == "PointTemperature").FirstOrDefault() is NumericProperty source)
		{
			roomSetpointState.PointTemperatureProperty.CopyValueAndTimestamp(source);
		}
		return roomSetpointState;
	}
}
