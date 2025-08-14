namespace Org.Mentalis.Security.Certificates;

public struct NameAttribute
{
	public string ObjectID;

	public string Value;

	public NameAttribute(string oid, string val)
	{
		ObjectID = oid;
		Value = val;
	}

	public override bool Equals(object obj)
	{
		try
		{
			NameAttribute nameAttribute = (NameAttribute)obj;
			return nameAttribute.ObjectID == ObjectID && nameAttribute.Value == Value;
		}
		catch
		{
			return false;
		}
	}

	public override int GetHashCode()
	{
		if (ObjectID == null && Value == null)
		{
			return 0;
		}
		if (ObjectID == null)
		{
			return Value.GetHashCode();
		}
		if (Value == null)
		{
			return ObjectID.GetHashCode();
		}
		return Value.GetHashCode() ^ ObjectID.GetHashCode();
	}

	public override string ToString()
	{
		if (ObjectID == null && Value == null)
		{
			return "N/A: N/A";
		}
		if (ObjectID == null)
		{
			return "N/A: " + Value;
		}
		if (Value == null)
		{
			return ObjectID + ": N/A";
		}
		return ObjectID + ": " + Value;
	}
}
