using System;
using System.Globalization;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

public class NumericProperty : Property
{
	[XmlIgnore]
	public decimal? Value { get; set; }

	[XmlAttribute(AttributeName = "Value")]
	public string ValueStr
	{
		get
		{
			return Value.HasValue ? Value.Value.ToString(CultureInfo.InvariantCulture) : null;
		}
		set
		{
			Value = (string.IsNullOrEmpty(value) ? ((decimal?)null) : new decimal?(Convert.ToDecimal(value, CultureInfo.InvariantCulture)));
		}
	}

	public override IComparable GetValueAsComparable()
	{
		return (IComparable)(object)Value;
	}

	public override string GetValueAsString()
	{
		return Value.HasValue ? string.Format(CultureInfo.InvariantCulture, "{0:0.##}", new object[1] { Value }) : "";
	}

	public override bool Equals(Property other)
	{
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		NumericProperty numericProperty = other as NumericProperty;
		if (object.ReferenceEquals(null, numericProperty))
		{
			return false;
		}
		return numericProperty.Name == base.Name && numericProperty.Value == Value;
	}

	public new NumericProperty Clone()
	{
		return (NumericProperty)base.Clone();
	}

	protected override Property CreateClone()
	{
		return new NumericProperty();
	}

	protected override void TransferProperties(Property clone)
	{
		base.TransferProperties(clone);
		NumericProperty numericProperty = (NumericProperty)clone;
		numericProperty.Value = Value;
	}
}
