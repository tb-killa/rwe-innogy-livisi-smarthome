using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;

public class DimmerActuatorState : LogicalDeviceState, IEquatable<DimmerActuatorState>
{
	private const string DIM_LEVEL_PROP_NAME = "DimLevel";

	private NumericProperty dimLevelProperty = new NumericProperty
	{
		Name = "DimLevel"
	};

	[XmlIgnore]
	public int? DimLevel
	{
		get
		{
			if (!dimLevelProperty.Value.HasValue)
			{
				return null;
			}
			return Convert.ToInt32(dimLevelProperty.Value);
		}
		set
		{
			dimLevelProperty.Value = value;
		}
	}

	[XmlAttribute(AttributeName = "DmLvl")]
	public string DimLevelString
	{
		get
		{
			return DimLevelProperty.ValueStr;
		}
		set
		{
			DimLevelProperty.ValueStr = value;
		}
	}

	public DateTime? DimLevelUpdateTimestamp
	{
		get
		{
			return DimLevelProperty.UpdateTimestamp;
		}
		set
		{
			DimLevelProperty.UpdateTimestamp = value;
		}
	}

	public NumericProperty DimLevelProperty => dimLevelProperty;

	public override void UpdateFrom(LogicalDeviceState value, DateTime timestamp)
	{
		if (value is DimmerActuatorState dimmerActuatorState)
		{
			dimLevelProperty.SetValueWithTimestampUpdateIfNotNull(dimmerActuatorState.dimLevelProperty, timestamp);
		}
	}

	public override List<Property> GetProperties()
	{
		List<Property> list = new List<Property>();
		list.Add(DimLevelProperty);
		return list;
	}

	protected override LogicalDeviceState CreateClone()
	{
		return new DimmerActuatorState();
	}

	protected override void TransferProperties(LogicalDeviceState clone)
	{
		base.TransferProperties(clone);
		DimmerActuatorState dimmerActuatorState = (DimmerActuatorState)clone;
		dimmerActuatorState.DimLevelProperty.CopyValueAndTimestamp(DimLevelProperty);
	}

	public bool Equals(DimmerActuatorState other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		return other.LogicalDeviceId.Equals(base.LogicalDeviceId) && other.DimLevelProperty.Equals(DimLevelProperty);
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
		if ((object)obj.GetType() != typeof(DimmerActuatorState))
		{
			return false;
		}
		return Equals((DimmerActuatorState)obj);
	}

	public override int GetHashCode()
	{
		return Convert.ToInt32(DimLevelProperty.Value.GetValueOrDefault(), CultureInfo.InvariantCulture);
	}

	public override string ToString()
	{
		return string.Format("DimLevel = {0}", DimLevelProperty.Value.HasValue ? DimLevelProperty.ValueStr : "null");
	}

	public override LogicalDeviceState FromGeneric(GenericDeviceState genericState)
	{
		DimmerActuatorState dimmerActuatorState = new DimmerActuatorState();
		dimmerActuatorState.LogicalDeviceId = genericState.LogicalDeviceId;
		if (genericState.Properties.Where((Property prop) => prop.Name == "DimLevel").FirstOrDefault() is NumericProperty source)
		{
			dimmerActuatorState.DimLevelProperty.CopyValueAndTimestamp(source);
		}
		return dimmerActuatorState;
	}
}
