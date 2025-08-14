using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

public class NotSupportedActionException : Exception
{
	public NotSupportedActionException()
	{
	}

	public NotSupportedActionException(string message)
		: base(message)
	{
	}
}
