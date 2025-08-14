using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

public class TokenExpiredException : Exception
{
	public TokenExpiredException()
	{
	}

	public TokenExpiredException(string message)
		: base(message)
	{
	}
}
