using System;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.Exceptions;

public class NotEnoughDataAvailableException : Exception
{
	public NotEnoughDataAvailableException()
	{
	}

	public NotEnoughDataAvailableException(string message)
		: base(message)
	{
	}

	public NotEnoughDataAvailableException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
