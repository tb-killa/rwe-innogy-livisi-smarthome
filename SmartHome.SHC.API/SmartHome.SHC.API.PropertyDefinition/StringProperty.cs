namespace SmartHome.SHC.API.PropertyDefinition;

public class StringProperty : Property
{
	public string Name { get; private set; }

	public string Value { get; set; }

	public StringProperty(string name, string value)
	{
		Name = name;
		Value = value;
	}

	public override int GetHashCode()
	{
		return (((Name != null) ? Name.GetHashCode() : 0) * 397) ^ ((!string.IsNullOrEmpty(Value)) ? Value.GetHashCode() : 0);
	}

	public bool Equals(StringProperty other)
	{
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		if (object.ReferenceEquals(other, null))
		{
			return false;
		}
		return other.Name == Name && Value == other.Value;
	}

	public override bool Equals(object other)
	{
		return Equals(other as StringProperty);
	}

	public static bool operator ==(StringProperty first, StringProperty second)
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

	public static bool operator !=(StringProperty first, StringProperty second)
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

	public string GetValueAsString()
	{
		return Value;
	}
}
