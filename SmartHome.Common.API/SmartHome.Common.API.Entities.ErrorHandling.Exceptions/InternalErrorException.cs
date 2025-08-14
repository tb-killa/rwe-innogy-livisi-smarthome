using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

[Serializable]
public class InternalErrorException : Exception
{
	public InternalErrorException()
	{
	}

	public InternalErrorException(string message)
		: base(message)
	{
	}
}
