namespace SmartHome.SHC.API.PropertyDefinition;

public class BooleanProperty : Property
{
	public string Name { get; private set; }

	public bool? Value { get; set; }

	public BooleanProperty(string name, bool? value)
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

	public bool Equals(BooleanProperty other)
	{
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		if (object.ReferenceEquals(other, null))
		{
			return false;
		}
		return other.Name == Name && Value.HasValue == other.Value.HasValue && (!Value.HasValue || Value.Value == other.Value);
	}

	public override bool Equals(object other)
	{
		return Equals(other as BooleanProperty);
	}

	public static bool operator ==(BooleanProperty first, BooleanProperty second)
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

	public static bool operator !=(BooleanProperty first, BooleanProperty second)
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
