using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

[Serializable]
public class UnsupportedRequestException : Exception
{
	public UnsupportedRequestException()
	{
	}

	public UnsupportedRequestException(string message)
		: base(message)
	{
	}
}
