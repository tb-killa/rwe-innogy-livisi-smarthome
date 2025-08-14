using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

public class InvalidActionPermissionException : Exception
{
	public InvalidActionPermissionException()
	{
	}

	public InvalidActionPermissionException(string message)
		: base(message)
	{
	}
}
