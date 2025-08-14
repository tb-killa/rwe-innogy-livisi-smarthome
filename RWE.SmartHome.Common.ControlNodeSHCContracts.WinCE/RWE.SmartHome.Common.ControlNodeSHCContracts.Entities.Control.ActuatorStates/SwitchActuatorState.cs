using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;

public class SwitchActuatorState : LogicalDeviceState, IEquatable<SwitchActuatorState>
{
	private const string ON_STATE_PROP_NAME = "OnState";

	private BooleanProperty onStateProperty = new BooleanProperty
	{
		Name = "OnState"
	};

	[XmlIgnore]
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

	[XmlAttribute(AttributeName = "OnState")]
	public string OnStateString
	{
		get
		{
			return OnStateProperty.ValueStr;
		}
		set
		{
			OnStateProperty.ValueStr = value;
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
		if (value is SwitchActuatorState switchActuatorState)
		{
			OnStateProperty.SetValueWithTimestampUpdateIfNotNull(switchActuatorState.OnStateProperty, timestamp);
		}
	}

	public override List<Property> GetProperties()
	{
		List<Property> list = new List<Property>();
		list.Add(OnStateProperty);
		return list;
	}

	protected override LogicalDeviceState CreateClone()
	{
		return new SwitchActuatorState();
	}

	protected override void TransferProperties(LogicalDeviceState clone)
	{
		base.TransferProperties(clone);
		SwitchActuatorState switchActuatorState = (SwitchActuatorState)clone;
		switchActuatorState.OnStateProperty.CopyValueAndTimestamp(OnStateProperty);
	}

	public bool Equals(SwitchActuatorState other)
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
		if ((object)obj.GetType() != typeof(SwitchActuatorState))
		{
			return false;
		}
		return Equals((SwitchActuatorState)obj);
	}

	public override int GetHashCode()
	{
		return OnStateProperty.GetHashCode();
	}

	public override string ToString()
	{
		return string.Format("IsOn = {0}", OnStateProperty.Value.HasValue ? OnStateProperty.ValueStr : "null");
	}

	public override LogicalDeviceState FromGeneric(GenericDeviceState genericState)
	{
		SwitchActuatorState switchActuatorState = new SwitchActuatorState();
		switchActuatorState.LogicalDeviceId = genericState.LogicalDeviceId;
		if (genericState.Properties.Where((Property p) => p.Name == "OnState").FirstOrDefault() is BooleanProperty source)
		{
			switchActuatorState.OnStateProperty.CopyValueAndTimestamp(source);
		}
		return switchActuatorState;
	}
}
