using System;

namespace RWE.SmartHome.SHC.ApplicationsHostInterfaces;

public class AddinNotSignedException : Exception
{
	public AddinNotSignedException(string message)
		: base(message)
	{
	}
}
