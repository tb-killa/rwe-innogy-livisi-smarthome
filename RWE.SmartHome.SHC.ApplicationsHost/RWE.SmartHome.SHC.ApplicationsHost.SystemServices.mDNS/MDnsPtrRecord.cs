namespace RWE.SmartHome.SHC.ApplicationsHost.SystemServices.mDNS;

public class MDnsPtrRecord
{
	public MDnsDomainName Domain;

	public override string ToString()
	{
		return "PTR " + Domain.Name;
	}
}
