using System;

namespace RWE.SmartHome.SHC.SHCRelayDriver.Exceptions;

public class WebSocketException : Exception
{
	public WebSocketException(string message)
		: base(message)
	{
	}
}
