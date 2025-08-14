namespace SmartHome.SHC.API.SystemServices.Dns;

public struct DnsQuery
{
	public ushort DnsType;

	public ushort DnsClass;

	public string Query;
}
