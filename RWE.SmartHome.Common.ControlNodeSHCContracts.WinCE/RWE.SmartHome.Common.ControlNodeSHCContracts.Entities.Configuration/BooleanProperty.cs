using System;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

public class BooleanProperty : Property
{
	[XmlIgnore]
	public bool? Value { get; set; }

	[XmlAttribute(AttributeName = "Value")]
	public string ValueStr
	{
		get
		{
			return Value.HasValue ? Value.ToString() : null;
		}
		set
		{
			Value = ((string.IsNullOrEmpty(value) || value == "null") ? ((bool?)null) : new bool?(Convert.ToBoolean(value)));
		}
	}

	public override IComparable GetValueAsComparable()
	{
		return (IComparable)(object)Value;
	}

	public override string GetValueAsString()
	{
		return Value.ToString();
	}

	public override bool Equals(Property other)
	{
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		BooleanProperty booleanProperty = other as BooleanProperty;
		if (object.ReferenceEquals(null, booleanProperty))
		{
			return false;
		}
		return booleanProperty.Name == base.Name && booleanProperty.Value == Value;
	}

	public new BooleanProperty Clone()
	{
		return (BooleanProperty)base.Clone();
	}

	protected override Property CreateClone()
	{
		return new BooleanProperty();
	}

	protected override void TransferProperties(Property clone)
	{
		base.TransferProperties(clone);
		BooleanProperty booleanProperty = (BooleanProperty)clone;
		booleanProperty.Value = Value;
	}
}
