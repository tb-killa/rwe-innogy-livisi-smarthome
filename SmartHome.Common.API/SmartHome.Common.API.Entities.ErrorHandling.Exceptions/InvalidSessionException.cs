using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

[Serializable]
public class InvalidSessionException : Exception
{
	public InvalidSessionException()
	{
	}

	public InvalidSessionException(string message)
		: base(message)
	{
	}
}
