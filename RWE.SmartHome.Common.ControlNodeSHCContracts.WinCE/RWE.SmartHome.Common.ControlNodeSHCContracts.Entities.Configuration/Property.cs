using System;
using System.Globalization;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

[XmlInclude(typeof(DateTimeProperty))]
[XmlInclude(typeof(BooleanProperty))]
[XmlInclude(typeof(NumericProperty))]
[XmlInclude(typeof(StringProperty))]
public abstract class Property : IEquatable<Property>
{
	[XmlAttribute]
	public string Name { get; set; }

	[XmlIgnore]
	public DateTime? UpdateTimestamp { get; set; }

	[XmlAttribute(AttributeName = "UpdateTimestamp")]
	public string UpdateTimestampStr
	{
		get
		{
			return UpdateTimestamp.HasValue ? UpdateTimestamp.Value.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture) : null;
		}
		set
		{
			UpdateTimestamp = (string.IsNullOrEmpty(value) ? ((DateTime?)null) : new DateTime?(DateTime.Parse(value, CultureInfo.InvariantCulture)));
		}
	}

	public abstract IComparable GetValueAsComparable();

	public abstract string GetValueAsString();

	public Property Clone()
	{
		Property property = CreateClone();
		TransferProperties(property);
		return property;
	}

	protected abstract Property CreateClone();

	protected virtual void TransferProperties(Property clone)
	{
		clone.Name = Name;
		clone.UpdateTimestamp = (UpdateTimestamp.HasValue ? new DateTime?(UpdateTimestamp.Value.ToUniversalTime()) : UpdateTimestamp);
	}

	public abstract bool Equals(Property other);

	public override string ToString()
	{
		return string.Format("{0} = {1}", Name, GetValueAsString() ?? "null");
	}

	public override int GetHashCode()
	{
		string valueAsString = GetValueAsString();
		int num = ((Name != null) ? Name.GetHashCode() : 0);
		return (num * 397) ^ (valueAsString?.GetHashCode() ?? 0);
	}
}
