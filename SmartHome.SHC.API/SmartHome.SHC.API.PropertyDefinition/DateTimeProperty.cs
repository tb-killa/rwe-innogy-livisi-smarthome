using System;

namespace SmartHome.SHC.API.PropertyDefinition;

public class DateTimeProperty : Property
{
	public string Name { get; private set; }

	public DateTime? Value { get; set; }

	public DateTimeProperty(string name, DateTime? value)
	{
		Name = name;
		Value = value;
	}

	public string GetValueAsString()
	{
		return Value.ToString();
	}

	public override int GetHashCode()
	{
		return (((Name != null) ? Name.GetHashCode() : 0) * 397) ^ (Value.HasValue ? Value.GetHashCode() : 0);
	}

	public bool Equals(DateTimeProperty other)
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
				DateTime value = Value.Value;
				DateTime? value2 = other.Value;
				result = ((value == value2) ? 1 : 0);
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
		return Equals(other as DateTimeProperty);
	}

	public static bool operator ==(DateTimeProperty first, DateTimeProperty second)
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

	public static bool operator !=(DateTimeProperty first, DateTimeProperty second)
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
