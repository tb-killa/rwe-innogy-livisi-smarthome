using System;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.Ras;

internal class AdapterException : Exception
{
	public int ErrorCode { get; private set; }

	public AdapterException(int errorCode)
	{
		ErrorCode = errorCode;
	}
}
