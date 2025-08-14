using System.Net;

namespace SmartHome.SHC.API.SystemServices.Dns;

public struct DnsResourceRecord
{
	public string DomainName;

	public ushort Type;

	public ushort Class;

	public uint TimeToLive;

	public byte[] RawData;

	public object TranslatedData;

	public string[] AsTxtData()
	{
		return TranslatedData as string[];
	}

	public IPAddress AsAuthorityData()
	{
		return TranslatedData as IPAddress;
	}

	public string AsDnsPointerRecord()
	{
		return TranslatedData as string;
	}

	public DnsServiceRecordSpecificData AsDnsServiceRecord()
	{
		return TranslatedData as DnsServiceRecordSpecificData;
	}
}
