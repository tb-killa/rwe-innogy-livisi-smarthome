using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

[Serializable]
public class ConnectionTerminatedException : Exception
{
	public ConnectionTerminatedException()
	{
	}

	public ConnectionTerminatedException(string message)
		: base(message)
	{
	}
}
