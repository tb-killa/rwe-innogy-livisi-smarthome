using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.SensorStates;

public class SmokeDetectionSensorState : LogicalDeviceState, IEquatable<SmokeDetectionSensorState>
{
	private BooleanProperty isSmokeAlarmProperty = new BooleanProperty
	{
		Name = "IsSmokeAlarm"
	};

	public bool? IsSmokeAlarm
	{
		get
		{
			return isSmokeAlarmProperty.Value;
		}
		set
		{
			isSmokeAlarmProperty.Value = value;
		}
	}

	public DateTime? IsSmokeAlarmUpdateTimestamp
	{
		get
		{
			return IsSmokeAlarmProperty.UpdateTimestamp;
		}
		set
		{
			IsSmokeAlarmProperty.UpdateTimestamp = value;
		}
	}

	public BooleanProperty IsSmokeAlarmProperty => isSmokeAlarmProperty;

	public override void UpdateFrom(LogicalDeviceState value, DateTime timestamp)
	{
		if (value is SmokeDetectionSensorState smokeDetectionSensorState && smokeDetectionSensorState.IsSmokeAlarmProperty.Value.HasValue)
		{
			IsSmokeAlarmProperty.SetValueWithTimestampUpdate(smokeDetectionSensorState.IsSmokeAlarmProperty.Value, timestamp);
		}
	}

	public bool Equals(SmokeDetectionSensorState other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		return other.LogicalDeviceId.Equals(base.LogicalDeviceId) && other.IsSmokeAlarmProperty.Equals(IsSmokeAlarmProperty);
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
		if ((object)obj.GetType() != typeof(SmokeDetectionSensorState))
		{
			return false;
		}
		return Equals((SmokeDetectionSensorState)obj);
	}

	public override int GetHashCode()
	{
		return IsSmokeAlarmProperty.GetHashCode();
	}

	public override List<Property> GetProperties()
	{
		List<Property> list = new List<Property>();
		list.Add(IsSmokeAlarmProperty);
		return list;
	}

	public override string ToString()
	{
		return string.Format("IsSmokeAlarm = {0}", IsSmokeAlarmProperty.Value.HasValue ? IsSmokeAlarmProperty.ValueStr : "null");
	}

	protected override LogicalDeviceState CreateClone()
	{
		return new SmokeDetectionSensorState();
	}

	protected override void TransferProperties(LogicalDeviceState clone)
	{
		base.TransferProperties(clone);
		SmokeDetectionSensorState smokeDetectionSensorState = (SmokeDetectionSensorState)clone;
		smokeDetectionSensorState.IsSmokeAlarmProperty.CopyValueAndTimestamp(IsSmokeAlarmProperty);
	}
}
