using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.SensorStates;

public class LuminanceSensorState : LogicalDeviceState, IEquatable<LuminanceSensorState>
{
	private NumericProperty luminanceProperty = new NumericProperty
	{
		Name = "Luminance"
	};

	public int? Luminance
	{
		get
		{
			if (!luminanceProperty.Value.HasValue)
			{
				return null;
			}
			return Convert.ToInt32(luminanceProperty.Value);
		}
		set
		{
			luminanceProperty.Value = value;
		}
	}

	public DateTime? LuminanceUpdateTimestamp
	{
		get
		{
			return LuminanceProperty.UpdateTimestamp;
		}
		set
		{
			LuminanceProperty.UpdateTimestamp = value;
		}
	}

	public NumericProperty LuminanceProperty
	{
		get
		{
			return luminanceProperty;
		}
		set
		{
			if (!value.Value.HasValue)
			{
				luminanceProperty.Value = null;
			}
			else
			{
				luminanceProperty.Value = value.Value;
			}
		}
	}

	public override void UpdateFrom(LogicalDeviceState value, DateTime timestamp)
	{
		if (value is LuminanceSensorState luminanceSensorState)
		{
			LuminanceProperty.SetValueWithTimestampUpdateIfNotNull(luminanceSensorState.LuminanceProperty, timestamp);
		}
	}

	public override List<Property> GetProperties()
	{
		List<Property> list = new List<Property>();
		list.Add(LuminanceProperty);
		return list;
	}

	public override string ToString()
	{
		return string.Format("Luminance = {0}", LuminanceProperty.Value.HasValue ? LuminanceProperty.ValueStr : "null");
	}

	protected override LogicalDeviceState CreateClone()
	{
		return new LuminanceSensorState();
	}

	protected override void TransferProperties(LogicalDeviceState clone)
	{
		base.TransferProperties(clone);
		LuminanceSensorState luminanceSensorState = (LuminanceSensorState)clone;
		luminanceSensorState.LuminanceProperty.CopyValueAndTimestamp(LuminanceProperty);
	}

	public bool Equals(LuminanceSensorState other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		return other.LogicalDeviceId.Equals(base.LogicalDeviceId) && other.LuminanceProperty.Equals(LuminanceProperty);
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
		if ((object)obj.GetType() != typeof(LuminanceSensorState))
		{
			return false;
		}
		return Equals((LuminanceSensorState)obj);
	}

	public override int GetHashCode()
	{
		return LuminanceProperty.GetHashCode();
	}
}
