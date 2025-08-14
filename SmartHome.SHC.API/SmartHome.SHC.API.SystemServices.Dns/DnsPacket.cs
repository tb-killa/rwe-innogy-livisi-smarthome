namespace SmartHome.SHC.API.SystemServices.Dns;

public struct DnsPacket
{
	public DnsQuery[] Queries;

	public DnsResourceRecord[] Answers;

	public DnsResourceRecord[] Authority;

	public DnsResourceRecord[] AdditionalInformation;
}
