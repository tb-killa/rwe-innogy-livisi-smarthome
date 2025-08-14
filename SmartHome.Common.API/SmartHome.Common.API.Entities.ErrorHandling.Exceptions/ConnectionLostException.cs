using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

[Serializable]
public class ConnectionLostException : Exception
{
	public ConnectionLostException()
	{
	}

	public ConnectionLostException(string message)
		: base(message)
	{
	}
}
