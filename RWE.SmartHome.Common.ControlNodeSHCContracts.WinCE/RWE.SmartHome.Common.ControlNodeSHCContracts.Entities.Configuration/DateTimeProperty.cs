using System;
using System.Globalization;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

public class DateTimeProperty : Property
{
	[XmlIgnore]
	public DateTime? Value { get; set; }

	[XmlAttribute(AttributeName = "Value")]
	public string ValueStr
	{
		get
		{
			return Value.HasValue ? Value.Value.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture) : null;
		}
		set
		{
			Value = (string.IsNullOrEmpty(value) ? ((DateTime?)null) : new DateTime?(DateTime.Parse(value, CultureInfo.InvariantCulture).ToUniversalTime()));
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
		DateTimeProperty dateTimeProperty = other as DateTimeProperty;
		if (object.ReferenceEquals(null, dateTimeProperty))
		{
			return false;
		}
		return dateTimeProperty.Name == base.Name && dateTimeProperty.Value == Value;
	}

	public new DateTimeProperty Clone()
	{
		return (DateTimeProperty)base.Clone();
	}

	protected override Property CreateClone()
	{
		return new DateTimeProperty();
	}

	protected override void TransferProperties(Property clone)
	{
		base.TransferProperties(clone);
		DateTimeProperty dateTimeProperty = (DateTimeProperty)clone;
		dateTimeProperty.Value = Value;
	}
}
