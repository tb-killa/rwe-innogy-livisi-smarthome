using System.Net;

namespace RWE.SmartHome.SHC.ApplicationsHost.SystemServices.mDNS;

public class MDnsARecord
{
	public IPAddress IPAddress;

	public override string ToString()
	{
		return "A " + IPAddress.ToString();
	}
}
