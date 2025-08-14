using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;

public class ThermostatActuatorState : LogicalDeviceState, IEquatable<ThermostatActuatorState>
{
	private const string POINT_TEMP_PROP_NAME = "PointTemperature";

	private const string OP_MODE_PROP_NAME = "OperationMode";

	private const string WIND_RED_PROP_NAME = "WindowReductionActive";

	private NumericProperty pointTemperatureProperty = new NumericProperty
	{
		Name = "PointTemperature"
	};

	private StringProperty operationModeProperty = new StringProperty
	{
		Name = "OperationMode"
	};

	private BooleanProperty windowReductionActiveProperty = new BooleanProperty
	{
		Name = "WindowReductionActive"
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

	[XmlIgnore]
	public OperationMode? OperationMode
	{
		get
		{
			if (operationModeProperty.Value != null)
			{
				return (OperationMode)Enum.Parse(typeof(OperationMode), operationModeProperty.Value, ignoreCase: true);
			}
			return null;
		}
		set
		{
			operationModeProperty.Value = (value.HasValue ? value.ToString() : null);
		}
	}

	[XmlAttribute(AttributeName = "OpnMd")]
	public string OperationModeString
	{
		get
		{
			return OperationModeProperty.Value;
		}
		set
		{
			OperationModeProperty.Value = value;
		}
	}

	public DateTime? OperationModeUpdateTimestamp
	{
		get
		{
			return OperationModeProperty.UpdateTimestamp;
		}
		set
		{
			OperationModeProperty.UpdateTimestamp = value;
		}
	}

	[XmlIgnore]
	public bool? WindowReductionActive
	{
		get
		{
			return windowReductionActiveProperty.Value;
		}
		set
		{
			windowReductionActiveProperty.Value = value;
		}
	}

	[XmlAttribute(AttributeName = "WRAc")]
	public string WindowReductionActiveString
	{
		get
		{
			return WindowReductionActiveProperty.ValueStr;
		}
		set
		{
			WindowReductionActiveProperty.ValueStr = value;
		}
	}

	public DateTime? WindowReductionActiveUpdateTimestamp
	{
		get
		{
			return WindowReductionActiveProperty.UpdateTimestamp;
		}
		set
		{
			windowReductionActiveProperty.UpdateTimestamp = value;
		}
	}

	public NumericProperty PointTemperatureProperty => pointTemperatureProperty;

	public StringProperty OperationModeProperty => operationModeProperty;

	public BooleanProperty WindowReductionActiveProperty => windowReductionActiveProperty;

	public override void UpdateFrom(LogicalDeviceState value, DateTime timestamp)
	{
		if (value is ThermostatActuatorState thermostatActuatorState)
		{
			PointTemperatureProperty.SetValueWithTimestampUpdateIfNotNull(thermostatActuatorState.PointTemperatureProperty, timestamp);
			OperationModeProperty.SetValueWithTimestampUpdateIfNotNull(thermostatActuatorState.OperationModeProperty, timestamp);
			WindowReductionActiveProperty.SetValueWithTimestampUpdateIfNotNull(thermostatActuatorState.WindowReductionActiveProperty, timestamp);
		}
	}

	public override List<Property> GetProperties()
	{
		List<Property> list = new List<Property>();
		list.Add(PointTemperatureProperty);
		list.Add(WindowReductionActiveProperty);
		list.Add(OperationModeProperty);
		return list;
	}

	public override string ToString()
	{
		return string.Format("PointTemperature = {0}, OperationMode = {1}, WindowReductionActive = {2}", PointTemperatureProperty.Value.HasValue ? PointTemperatureProperty.ValueStr : "null", (!string.IsNullOrEmpty(OperationModeProperty.Value)) ? OperationModeProperty.Value : "null", WindowReductionActiveProperty.Value.HasValue ? WindowReductionActiveProperty.ValueStr : "null");
	}

	protected override LogicalDeviceState CreateClone()
	{
		return new ThermostatActuatorState();
	}

	protected override void TransferProperties(LogicalDeviceState clone)
	{
		base.TransferProperties(clone);
		ThermostatActuatorState thermostatActuatorState = (ThermostatActuatorState)clone;
		thermostatActuatorState.PointTemperatureProperty.CopyValueAndTimestamp(PointTemperatureProperty);
		thermostatActuatorState.OperationModeProperty.CopyValueAndTimestamp(OperationModeProperty);
		thermostatActuatorState.WindowReductionActiveProperty.CopyValueAndTimestamp(WindowReductionActiveProperty);
	}

	public bool Equals(ThermostatActuatorState other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		return other.LogicalDeviceId.Equals(base.LogicalDeviceId) && other.PointTemperatureProperty.Equals(PointTemperatureProperty) && other.OperationModeProperty.Equals(OperationModeProperty) && other.WindowReductionActiveProperty.Equals(WindowReductionActiveProperty);
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
		if ((object)obj.GetType() != typeof(ThermostatActuatorState))
		{
			return false;
		}
		return Equals((ThermostatActuatorState)obj);
	}

	public override int GetHashCode()
	{
		int hashCode = PointTemperatureProperty.GetHashCode();
		hashCode = (hashCode * 397) ^ OperationModeProperty.GetHashCode();
		return (hashCode * 397) ^ WindowReductionActiveProperty.GetHashCode();
	}

	public override LogicalDeviceState FromGeneric(GenericDeviceState genericState)
	{
		ThermostatActuatorState thermostatActuatorState = new ThermostatActuatorState();
		thermostatActuatorState.LogicalDeviceId = genericState.LogicalDeviceId;
		if (genericState.Properties.Where((Property p) => p.Name == "PointTemperature").FirstOrDefault() is NumericProperty source)
		{
			thermostatActuatorState.PointTemperatureProperty.CopyValueAndTimestamp(source);
		}
		if (genericState.Properties.Where((Property p) => p.Name == "OperationMode").FirstOrDefault() is StringProperty source2)
		{
			thermostatActuatorState.OperationModeProperty.CopyValueAndTimestamp(source2);
		}
		if (genericState.Properties.Where((Property p) => p.Name == "WindowReductionActive").FirstOrDefault() is BooleanProperty source3)
		{
			thermostatActuatorState.WindowReductionActiveProperty.CopyValueAndTimestamp(source3);
		}
		return thermostatActuatorState;
	}
}
