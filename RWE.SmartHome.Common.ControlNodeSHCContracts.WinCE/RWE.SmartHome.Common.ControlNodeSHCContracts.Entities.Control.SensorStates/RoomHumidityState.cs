using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.SensorStates;

public class RoomHumidityState : LogicalDeviceState, IEquatable<RoomHumidityState>
{
	private NumericProperty humidityProperty = new NumericProperty
	{
		Name = "Humidity"
	};

	[XmlIgnore]
	public decimal? Humidity
	{
		get
		{
			return humidityProperty.Value;
		}
		set
		{
			humidityProperty.Value = value;
		}
	}

	[XmlAttribute(AttributeName = "Humidity")]
	public string HumidityString
	{
		get
		{
			return HumidityProperty.ValueStr;
		}
		set
		{
			HumidityProperty.ValueStr = value;
		}
	}

	public DateTime? HumidityUpdateTimestamp
	{
		get
		{
			return HumidityProperty.UpdateTimestamp;
		}
		set
		{
			HumidityProperty.UpdateTimestamp = value;
		}
	}

	public NumericProperty HumidityProperty => humidityProperty;

	public override void UpdateFrom(LogicalDeviceState value, DateTime timestamp)
	{
		if (value is RoomHumidityState roomHumidityState && roomHumidityState.HumidityProperty.Value.HasValue)
		{
			HumidityProperty.SetValueWithTimestampUpdate(roomHumidityState.HumidityProperty.Value, timestamp);
		}
	}

	public override List<Property> GetProperties()
	{
		List<Property> list = new List<Property>();
		list.Add(HumidityProperty);
		return list;
	}

	public override string ToString()
	{
		return string.Format("Humidity = {0}", HumidityProperty.Value.HasValue ? HumidityProperty.ValueStr : "null");
	}

	protected override LogicalDeviceState CreateClone()
	{
		return new RoomHumidityState();
	}

	protected override void TransferProperties(LogicalDeviceState clone)
	{
		base.TransferProperties(clone);
		RoomHumidityState roomHumidityState = (RoomHumidityState)clone;
		roomHumidityState.HumidityProperty.CopyValueAndTimestamp(HumidityProperty);
	}

	public bool Equals(RoomHumidityState other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		return other.LogicalDeviceId.Equals(base.LogicalDeviceId) && other.HumidityProperty.Equals(HumidityProperty);
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
		if ((object)obj.GetType() != typeof(RoomHumidityState))
		{
			return false;
		}
		return Equals((RoomHumidityState)obj);
	}

	public override int GetHashCode()
	{
		return HumidityProperty.GetHashCode();
	}
}
