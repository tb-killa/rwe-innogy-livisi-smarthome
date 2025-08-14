using System;

namespace WebSocketLibrary.Exceptions;

public class ConnectionFailedException : Exception
{
	public ConnectionFailedException(string message)
		: base(message)
	{
	}
}
