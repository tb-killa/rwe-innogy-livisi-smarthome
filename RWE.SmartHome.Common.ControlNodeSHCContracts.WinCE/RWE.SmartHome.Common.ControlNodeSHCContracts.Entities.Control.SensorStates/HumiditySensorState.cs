using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.SensorStates;

public class HumiditySensorState : LogicalDeviceState, IEquatable<HumiditySensorState>
{
	private NumericProperty humidityProperty = new NumericProperty
	{
		Name = "Humidity"
	};

	private BooleanProperty moldWarningProperty = new BooleanProperty
	{
		Name = "MoldWarning"
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

	public bool? MoldWarning
	{
		get
		{
			return moldWarningProperty.Value;
		}
		set
		{
			moldWarningProperty.Value = value;
		}
	}

	public DateTime? MoldWarningUpdateTimestamp
	{
		get
		{
			return MoldWarningProperty.UpdateTimestamp;
		}
		set
		{
			MoldWarningProperty.UpdateTimestamp = value;
		}
	}

	public NumericProperty HumidityProperty => humidityProperty;

	public BooleanProperty MoldWarningProperty => moldWarningProperty;

	public override void UpdateFrom(LogicalDeviceState value, DateTime timestamp)
	{
		if (value is HumiditySensorState humiditySensorState)
		{
			if (humiditySensorState.HumidityProperty.Value.HasValue)
			{
				HumidityProperty.SetValueWithTimestampUpdate(humiditySensorState.HumidityProperty.Value, timestamp);
			}
			if (humiditySensorState.MoldWarningProperty.Value.HasValue)
			{
				MoldWarningProperty.SetValueWithTimestampUpdate(humiditySensorState.MoldWarningProperty.Value, timestamp);
			}
		}
	}

	public override List<Property> GetProperties()
	{
		List<Property> list = new List<Property>();
		list.Add(HumidityProperty);
		list.Add(MoldWarningProperty);
		return list;
	}

	public override string ToString()
	{
		return string.Format("Humidity = {0}, MoldWarning = {1}", HumidityProperty.Value.HasValue ? HumidityProperty.ValueStr : "null", MoldWarningProperty.Value.HasValue ? MoldWarningProperty.ValueStr : "null");
	}

	protected override LogicalDeviceState CreateClone()
	{
		return new HumiditySensorState();
	}

	protected override void TransferProperties(LogicalDeviceState clone)
	{
		base.TransferProperties(clone);
		HumiditySensorState humiditySensorState = (HumiditySensorState)clone;
		humiditySensorState.HumidityProperty.CopyValueAndTimestamp(HumidityProperty);
		humiditySensorState.MoldWarningProperty.CopyValueAndTimestamp(MoldWarningProperty);
	}

	public bool Equals(HumiditySensorState other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		return other.LogicalDeviceId.Equals(base.LogicalDeviceId) && other.HumidityProperty.Equals(HumidityProperty) && other.MoldWarningProperty.Equals(MoldWarningProperty);
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
		if ((object)obj.GetType() != typeof(HumiditySensorState))
		{
			return false;
		}
		return Equals((HumiditySensorState)obj);
	}

	public override int GetHashCode()
	{
		int hashCode = HumidityProperty.GetHashCode();
		return (hashCode * 397) ^ MoldWarningProperty.GetHashCode();
	}
}
