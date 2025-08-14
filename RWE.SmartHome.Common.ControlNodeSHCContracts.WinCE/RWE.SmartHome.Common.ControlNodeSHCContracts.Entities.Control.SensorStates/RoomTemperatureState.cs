using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.SensorStates;

public class RoomTemperatureState : LogicalDeviceState, IEquatable<RoomTemperatureState>
{
	private NumericProperty temperatureProperty = new NumericProperty
	{
		Name = "Temperature"
	};

	[XmlIgnore]
	public decimal? Temperature
	{
		get
		{
			return temperatureProperty.Value;
		}
		set
		{
			temperatureProperty.Value = value;
		}
	}

	[XmlAttribute(AttributeName = "Temperature")]
	public string TemperatureString
	{
		get
		{
			return TemperatureProperty.ValueStr;
		}
		set
		{
			TemperatureProperty.ValueStr = value;
		}
	}

	public DateTime? TemperatureUpdateTimestamp
	{
		get
		{
			return TemperatureProperty.UpdateTimestamp;
		}
		set
		{
			TemperatureProperty.UpdateTimestamp = value;
		}
	}

	public NumericProperty TemperatureProperty => temperatureProperty;

	public override void UpdateFrom(LogicalDeviceState value, DateTime timestamp)
	{
		if (value is RoomTemperatureState roomTemperatureState && roomTemperatureState.TemperatureProperty.Value.HasValue)
		{
			TemperatureProperty.SetValueWithTimestampUpdate(roomTemperatureState.TemperatureProperty.Value, timestamp);
		}
	}

	public override List<Property> GetProperties()
	{
		List<Property> list = new List<Property>();
		list.Add(TemperatureProperty);
		return list;
	}

	public override string ToString()
	{
		return string.Format("Temperature = {0}", TemperatureProperty.Value.HasValue ? TemperatureProperty.ValueStr : "null");
	}

	protected override LogicalDeviceState CreateClone()
	{
		return new RoomTemperatureState();
	}

	protected override void TransferProperties(LogicalDeviceState clone)
	{
		base.TransferProperties(clone);
		RoomTemperatureState roomTemperatureState = (RoomTemperatureState)clone;
		roomTemperatureState.TemperatureProperty.CopyValueAndTimestamp(TemperatureProperty);
	}

	public bool Equals(RoomTemperatureState other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		return other.LogicalDeviceId.Equals(base.LogicalDeviceId) && other.TemperatureProperty.Equals(TemperatureProperty);
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
		if ((object)obj.GetType() != typeof(RoomTemperatureState))
		{
			return false;
		}
		return Equals((RoomTemperatureState)obj);
	}

	public override int GetHashCode()
	{
		return TemperatureProperty.GetHashCode();
	}
}
