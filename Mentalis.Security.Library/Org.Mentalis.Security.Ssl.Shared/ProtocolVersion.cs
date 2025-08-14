namespace Org.Mentalis.Security.Ssl.Shared;

internal struct ProtocolVersion
{
	public byte major;

	public byte minor;

	public ProtocolVersion(byte major, byte minor)
	{
		this.major = major;
		this.minor = minor;
	}

	public int GetVersionInt()
	{
		return major * 10 + minor;
	}

	public override string ToString()
	{
		return major + "." + minor;
	}
}
