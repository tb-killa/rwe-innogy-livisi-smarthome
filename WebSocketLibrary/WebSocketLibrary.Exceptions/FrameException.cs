using System;

namespace WebSocketLibrary.Exceptions;

public class FrameException : Exception
{
	public FrameException(string message)
		: base(message)
	{
	}
}
