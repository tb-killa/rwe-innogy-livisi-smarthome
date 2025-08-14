using System.Net;

namespace RWE.SmartHome.SHC.ApplicationsHost.SystemServices.mDNS;

public class MDnsAAAARecord
{
	public IPAddress IPAddress;

	public override string ToString()
	{
		return "AAAA " + IPAddress.ToString();
	}
}
