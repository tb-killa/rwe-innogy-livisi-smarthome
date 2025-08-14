using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

public class NoControllerAccessException : Exception
{
	public NoControllerAccessException()
	{
	}

	public NoControllerAccessException(string message)
		: base(message)
	{
	}
}
