using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

public class NoPermissionsException : Exception
{
	public NoPermissionsException()
	{
	}

	public NoPermissionsException(string message)
		: base(message)
	{
	}
}
