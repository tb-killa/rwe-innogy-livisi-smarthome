namespace Org.Mentalis.Security.Certificates;

public class Extension
{
	public bool Critical;

	public byte[] EncodedValue;

	public string ObjectID;

	public Extension(string oid, bool critical, byte[] val)
	{
		ObjectID = oid;
		Critical = critical;
		EncodedValue = val;
	}
}
