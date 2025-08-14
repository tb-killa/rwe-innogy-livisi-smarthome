using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

public class ShcAccessRetrictedException : Exception
{
	public ShcAccessRetrictedException()
	{
	}

	public ShcAccessRetrictedException(string message)
		: base(message)
	{
	}
}
