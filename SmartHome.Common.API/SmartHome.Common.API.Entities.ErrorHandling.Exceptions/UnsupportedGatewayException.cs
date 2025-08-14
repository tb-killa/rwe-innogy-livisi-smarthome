using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

[Serializable]
public class UnsupportedGatewayException : Exception
{
	public UnsupportedGatewayException()
	{
	}

	public UnsupportedGatewayException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
