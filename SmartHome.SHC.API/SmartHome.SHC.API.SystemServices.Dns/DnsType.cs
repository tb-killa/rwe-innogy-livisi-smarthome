namespace SmartHome.SHC.API.SystemServices.Dns;

public enum DnsType : ushort
{
	A = 1,
	IPV4 = 32769,
	NS = 2,
	CNAME = 5,
	SOA = 6,
	WKS = 11,
	PTR = 12,
	AAAA = 28,
	MX = 15,
	SRV = 33,
	TXT = 16,
	Any = 255
}
