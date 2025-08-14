namespace RWE.SmartHome.SHC.ApplicationsHost.SystemServices.mDNS;

public class MDnsSrvRecord
{
	public ushort Priority;

	public ushort Weight;

	public ushort Port;

	public MDnsDomainName Target;

	public override string ToString()
	{
		return $"SRV Prio: {Priority} Weight: {Weight} Port: {Port} Domain: {Target}";
	}
}
