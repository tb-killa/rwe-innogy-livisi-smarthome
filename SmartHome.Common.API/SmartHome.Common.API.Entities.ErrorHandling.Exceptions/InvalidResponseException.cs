using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

public class InvalidResponseException : Exception
{
	public override string Message => "Invalid Response Exception";

	public InvalidResponseException(string message)
		: base(message)
	{
	}
}
