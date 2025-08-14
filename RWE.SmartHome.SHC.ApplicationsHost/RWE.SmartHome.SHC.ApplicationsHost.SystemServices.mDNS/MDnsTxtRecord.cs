using System.Collections.Generic;

namespace RWE.SmartHome.SHC.ApplicationsHost.SystemServices.mDNS;

public class MDnsTxtRecord
{
	public List<string> TxtLines;

	public override string ToString()
	{
		return "TXT [" + string.Join("] [", TxtLines.ToArray()) + "]";
	}
}
