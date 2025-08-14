using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.SensorStates;

public class TemperatureSensorState : LogicalDeviceState, IEquatable<TemperatureSensorState>
{
	private NumericProperty temperatureProperty = new NumericProperty
	{
		Name = "Temperature"
	};

	private BooleanProperty frostWarningProperty = new BooleanProperty
	{
		Name = "FrostWarning"
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

	public bool? FrostWarning
	{
		get
		{
			return frostWarningProperty.Value;
		}
		set
		{
			frostWarningProperty.Value = value;
		}
	}

	public DateTime? FrostWarningUpdateTimestamp
	{
		get
		{
			return FrostWarningProperty.UpdateTimestamp;
		}
		set
		{
			FrostWarningProperty.UpdateTimestamp = value;
		}
	}

	public NumericProperty TemperatureProperty => temperatureProperty;

	public BooleanProperty FrostWarningProperty => frostWarningProperty;

	public override void UpdateFrom(LogicalDeviceState value, DateTime timestamp)
	{
		if (value is TemperatureSensorState temperatureSensorState)
		{
			if (temperatureSensorState.TemperatureProperty.Value.HasValue)
			{
				TemperatureProperty.SetValueWithTimestampUpdate(temperatureSensorState.TemperatureProperty.Value, timestamp);
			}
			if (temperatureSensorState.FrostWarningProperty.Value.HasValue)
			{
				FrostWarningProperty.SetValueWithTimestampUpdate(temperatureSensorState.FrostWarningProperty.Value, timestamp);
			}
		}
	}

	public override List<Property> GetProperties()
	{
		List<Property> list = new List<Property>();
		list.Add(TemperatureProperty);
		list.Add(FrostWarningProperty);
		return list;
	}

	public override string ToString()
	{
		return string.Format("Temperature = {0}, FrostWarning = {1}", TemperatureProperty.Value.HasValue ? TemperatureProperty.ValueStr : "null", FrostWarningProperty.Value.HasValue ? FrostWarningProperty.ValueStr : "null");
	}

	protected override LogicalDeviceState CreateClone()
	{
		return new TemperatureSensorState();
	}

	protected override void TransferProperties(LogicalDeviceState clone)
	{
		base.TransferProperties(clone);
		TemperatureSensorState temperatureSensorState = (TemperatureSensorState)clone;
		temperatureSensorState.TemperatureProperty.CopyValueAndTimestamp(TemperatureProperty);
		temperatureSensorState.FrostWarningProperty.CopyValueAndTimestamp(FrostWarningProperty);
	}

	public bool Equals(TemperatureSensorState other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		return other.LogicalDeviceId.Equals(base.LogicalDeviceId) && other.TemperatureProperty.Equals(TemperatureProperty) && other.FrostWarningProperty.Equals(FrostWarningProperty);
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
		if ((object)obj.GetType() != typeof(TemperatureSensorState))
		{
			return false;
		}
		return Equals((TemperatureSensorState)obj);
	}

	public override int GetHashCode()
	{
		int hashCode = TemperatureProperty.GetHashCode();
		return (hashCode * 397) ^ FrostWarningProperty.GetHashCode();
	}
}
