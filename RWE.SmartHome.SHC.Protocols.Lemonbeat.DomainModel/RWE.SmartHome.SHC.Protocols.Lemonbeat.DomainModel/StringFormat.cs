using System;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public class StringFormat : IEquatable<StringFormat>
{
	public uint MaximumLength { get; set; }

	public string[] ValidValues { get; set; }

	public StringFormat()
	{
	}

	public StringFormat(uint maximumLength, string[] validValues)
	{
		MaximumLength = maximumLength;
		ValidValues = validValues;
	}

	public StringFormat(byte maximumLength)
	{
		MaximumLength = maximumLength;
	}

	public override bool Equals(object obj)
	{
		if (obj == null || (object)GetType() != obj.GetType())
		{
			return false;
		}
		StringFormat other = (StringFormat)obj;
		return Equals(other);
	}

	public override int GetHashCode()
	{
		return MaximumLength.GetHashCode();
	}

	public bool Equals(StringFormat other)
	{
		if (other != null && MaximumLength == other.MaximumLength)
		{
			return ValidValues.ElementsEqual(other.ValidValues);
		}
		return false;
	}
}
