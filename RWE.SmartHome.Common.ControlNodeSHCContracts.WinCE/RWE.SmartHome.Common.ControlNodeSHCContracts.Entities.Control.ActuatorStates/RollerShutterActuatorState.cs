using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;

public class RollerShutterActuatorState : LogicalDeviceState, IEquatable<RollerShutterActuatorState>
{
	private const int DefaultMinValue = 0;

	private const int DefaultMaxValue = 100;

	private const string SHUTTER_LEVEL_PROP_NAME = "ShutterLevel";

	private NumericProperty shutterLevelProperty = new NumericProperty
	{
		Name = "ShutterLevel"
	};

	public int? ShutterLevel
	{
		get
		{
			if (!shutterLevelProperty.Value.HasValue)
			{
				return null;
			}
			return Convert.ToInt32(shutterLevelProperty.Value);
		}
		set
		{
			shutterLevelProperty.Value = value;
		}
	}

	public DateTime? ShutterLevelUpdateTimestamp
	{
		get
		{
			return ShutterLevelProperty.UpdateTimestamp;
		}
		set
		{
			ShutterLevelProperty.UpdateTimestamp = value;
		}
	}

	public NumericProperty ShutterLevelProperty => shutterLevelProperty;

	public override void UpdateFrom(LogicalDeviceState value, DateTime timestamp)
	{
		if (value is RollerShutterActuatorState rollerShutterActuatorState)
		{
			ShutterLevelProperty.SetValueWithTimestampUpdateIfNotNull(rollerShutterActuatorState.ShutterLevelProperty, timestamp);
		}
	}

	public override List<Property> GetProperties()
	{
		List<Property> list = new List<Property>();
		list.Add(ShutterLevelProperty);
		return list;
	}

	protected override LogicalDeviceState CreateClone()
	{
		return new RollerShutterActuatorState();
	}

	protected override void TransferProperties(LogicalDeviceState clone)
	{
		base.TransferProperties(clone);
		RollerShutterActuatorState rollerShutterActuatorState = (RollerShutterActuatorState)clone;
		rollerShutterActuatorState.ShutterLevelProperty.CopyValueAndTimestamp(ShutterLevelProperty);
	}

	public bool Equals(RollerShutterActuatorState other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		return other.LogicalDeviceId.Equals(base.LogicalDeviceId) && other.ShutterLevelProperty.Equals(ShutterLevelProperty);
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
		if ((object)obj.GetType() != typeof(RollerShutterActuatorState))
		{
			return false;
		}
		return Equals((RollerShutterActuatorState)obj);
	}

	public override int GetHashCode()
	{
		return Convert.ToInt32(ShutterLevelProperty.Value.GetValueOrDefault(), CultureInfo.InvariantCulture);
	}

	public override string ToString()
	{
		return string.Format("ShutterLevel = {0}", ShutterLevelProperty.Value.HasValue ? ShutterLevelProperty.ValueStr : "null");
	}

	public override LogicalDeviceState FromGeneric(GenericDeviceState genericState)
	{
		RollerShutterActuatorState rollerShutterActuatorState = new RollerShutterActuatorState();
		rollerShutterActuatorState.LogicalDeviceId = genericState.LogicalDeviceId;
		if (genericState.Properties.Where((Property p) => p.Name == "ShutterLevel").FirstOrDefault() is NumericProperty source)
		{
			rollerShutterActuatorState.ShutterLevelProperty.CopyValueAndTimestamp(source);
		}
		return rollerShutterActuatorState;
	}
}
