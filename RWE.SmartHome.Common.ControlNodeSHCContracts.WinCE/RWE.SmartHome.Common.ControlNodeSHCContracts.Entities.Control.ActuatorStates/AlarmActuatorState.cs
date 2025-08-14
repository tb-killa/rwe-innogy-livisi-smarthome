using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;

public class AlarmActuatorState : LogicalDeviceState, IEquatable<AlarmActuatorState>
{
	private const string ON_STATE_PROP_NAME = "OnState";

	private BooleanProperty onStateProperty = new BooleanProperty
	{
		Name = "OnState"
	};

	public bool? OnState
	{
		get
		{
			return onStateProperty.Value;
		}
		set
		{
			onStateProperty.Value = value;
		}
	}

	public DateTime? OnStateUpdateTimestamp
	{
		get
		{
			return OnStateProperty.UpdateTimestamp;
		}
		set
		{
			OnStateProperty.UpdateTimestamp = value;
		}
	}

	public BooleanProperty OnStateProperty => onStateProperty;

	public override void UpdateFrom(LogicalDeviceState value, DateTime timestamp)
	{
		if (value is AlarmActuatorState alarmActuatorState)
		{
			OnStateProperty.SetValueWithTimestampUpdateIfNotNull(alarmActuatorState.OnStateProperty, timestamp);
		}
	}

	public override List<Property> GetProperties()
	{
		List<Property> list = new List<Property>();
		list.Add(OnStateProperty);
		return list;
	}

	public override string ToString()
	{
		return string.Format("OnState = {0}", OnStateProperty.Value.HasValue ? OnStateProperty.ValueStr : "null");
	}

	protected override LogicalDeviceState CreateClone()
	{
		return new AlarmActuatorState();
	}

	protected override void TransferProperties(LogicalDeviceState clone)
	{
		base.TransferProperties(clone);
		AlarmActuatorState alarmActuatorState = (AlarmActuatorState)clone;
		alarmActuatorState.OnStateProperty.CopyValueAndTimestamp(OnStateProperty);
	}

	public bool Equals(AlarmActuatorState other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		return other.LogicalDeviceId.Equals(base.LogicalDeviceId) && other.OnStateProperty.Equals(OnStateProperty);
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
		if ((object)obj.GetType() != typeof(AlarmActuatorState))
		{
			return false;
		}
		return Equals((AlarmActuatorState)obj);
	}

	public override int GetHashCode()
	{
		return OnStateProperty.GetHashCode();
	}

	public override LogicalDeviceState FromGeneric(GenericDeviceState genericState)
	{
		AlarmActuatorState alarmActuatorState = new AlarmActuatorState();
		alarmActuatorState.LogicalDeviceId = genericState.LogicalDeviceId;
		if (genericState.Properties.Where((Property prop) => prop.Name == "OnState").FirstOrDefault() is BooleanProperty source)
		{
			alarmActuatorState.OnStateProperty.CopyValueAndTimestamp(source);
		}
		return alarmActuatorState;
	}
}
