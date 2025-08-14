using System;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

public class StringProperty : Property
{
	[XmlAttribute]
	public string Value { get; set; }

	public StringProperty()
	{
	}

	public StringProperty(string name, string value)
	{
		base.Name = name;
		Value = value;
	}

	public override bool Equals(Property other)
	{
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		StringProperty stringProperty = other as StringProperty;
		if (object.ReferenceEquals(null, stringProperty))
		{
			return false;
		}
		return stringProperty.Name == base.Name && stringProperty.Value == Value;
	}

	public override IComparable GetValueAsComparable()
	{
		return Value;
	}

	public override string GetValueAsString()
	{
		return Value;
	}

	public new StringProperty Clone()
	{
		return (StringProperty)base.Clone();
	}

	protected override Property CreateClone()
	{
		return new StringProperty();
	}

	protected override void TransferProperties(Property clone)
	{
		base.TransferProperties(clone);
		StringProperty stringProperty = (StringProperty)clone;
		stringProperty.Value = Value;
	}
}
