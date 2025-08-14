using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

[Serializable]
public class DestinationOfflineException : Exception
{
	public DestinationOfflineException()
	{
	}

	public DestinationOfflineException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
