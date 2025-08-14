using System.Globalization;

namespace SmartHome.SHC.API.PropertyDefinition;

public class NumericProperty : Property
{
	public string Name { get; private set; }

	public decimal? Value { get; set; }

	public NumericProperty(string name, decimal? value)
	{
		Name = name;
		Value = value;
	}

	public string GetValueAsString()
	{
		return Value.HasValue ? string.Format(CultureInfo.InvariantCulture, "{0:0.##}", new object[1] { Value }) : "";
	}

	public override int GetHashCode()
	{
		return (((Name != null) ? Name.GetHashCode() : 0) * 397) ^ (Value.HasValue ? Value.GetHashCode() : 0);
	}

	public bool Equals(NumericProperty other)
	{
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		if (object.ReferenceEquals(other, null))
		{
			return false;
		}
		int result;
		if (other.Name == Name && Value.HasValue == other.Value.HasValue)
		{
			if (!Value.HasValue)
			{
				result = 1;
			}
			else
			{
				decimal value = Value.Value;
				decimal? value2 = other.Value;
				result = ((value == value2.GetValueOrDefault() && value2.HasValue) ? 1 : 0);
			}
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public override bool Equals(object other)
	{
		return Equals(other as NumericProperty);
	}

	public static bool operator ==(NumericProperty first, NumericProperty second)
	{
		if (!object.ReferenceEquals(first, null))
		{
			return first.Equals(second);
		}
		if (!object.ReferenceEquals(second, null))
		{
			return second.Equals(first);
		}
		return true;
	}

	public static bool operator !=(NumericProperty first, NumericProperty second)
	{
		if (!object.ReferenceEquals(first, null))
		{
			return !first.Equals(second);
		}
		if (!object.ReferenceEquals(second, null))
		{
			return !second.Equals(first);
		}
		return false;
	}
}
