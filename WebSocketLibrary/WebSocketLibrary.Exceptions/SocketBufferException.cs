using System;

namespace WebSocketLibrary.Exceptions;

public class SocketBufferException : Exception
{
	public SocketBufferException(string message)
		: base(message)
	{
	}
}
