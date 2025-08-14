using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.SensorStates;

public class WindowDoorSensorState : LogicalDeviceState, IEquatable<WindowDoorSensorState>
{
	private BooleanProperty isOpenProperty = new BooleanProperty
	{
		Name = "IsOpen"
	};

	public bool? IsOpen
	{
		get
		{
			return isOpenProperty.Value;
		}
		set
		{
			isOpenProperty.Value = value;
		}
	}

	public DateTime? IsOpenUpdateTimestamp
	{
		get
		{
			return IsOpenProperty.UpdateTimestamp;
		}
		set
		{
			IsOpenProperty.UpdateTimestamp = value;
		}
	}

	public BooleanProperty IsOpenProperty => isOpenProperty;

	public override void UpdateFrom(LogicalDeviceState value, DateTime timestamp)
	{
		if (value is WindowDoorSensorState windowDoorSensorState)
		{
			IsOpenProperty.SetValueWithTimestampUpdateIfNotNull(windowDoorSensorState.IsOpenProperty, timestamp);
		}
	}

	public bool Equals(WindowDoorSensorState other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		return other.LogicalDeviceId.Equals(base.LogicalDeviceId) && other.IsOpenProperty.Equals(IsOpenProperty);
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
		if ((object)obj.GetType() != typeof(WindowDoorSensorState))
		{
			return false;
		}
		return Equals((WindowDoorSensorState)obj);
	}

	public override int GetHashCode()
	{
		return IsOpenProperty.GetHashCode();
	}

	public override List<Property> GetProperties()
	{
		List<Property> list = new List<Property>();
		list.Add(IsOpenProperty);
		return list;
	}

	public override string ToString()
	{
		return string.Format("IsOpen = {0},", IsOpenProperty.Value.HasValue ? IsOpenProperty.ValueStr : "null");
	}

	protected override LogicalDeviceState CreateClone()
	{
		return new WindowDoorSensorState();
	}

	protected override void TransferProperties(LogicalDeviceState clone)
	{
		base.TransferProperties(clone);
		WindowDoorSensorState windowDoorSensorState = (WindowDoorSensorState)clone;
		windowDoorSensorState.IsOpenProperty.CopyValueAndTimestamp(IsOpenProperty);
	}
}
