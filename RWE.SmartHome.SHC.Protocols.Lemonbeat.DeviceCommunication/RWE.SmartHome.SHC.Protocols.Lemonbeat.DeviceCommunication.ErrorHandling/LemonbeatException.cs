using System;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.ErrorHandling;

public class LemonbeatException : Exception
{
	public LemonbeatException()
	{
	}

	public LemonbeatException(string message)
		: base(message)
	{
	}

	public LemonbeatException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
