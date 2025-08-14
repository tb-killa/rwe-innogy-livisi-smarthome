using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

[Serializable]
public class ConnectionAlreadyInitializedException : Exception
{
	public ConnectionAlreadyInitializedException()
	{
	}

	public ConnectionAlreadyInitializedException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
