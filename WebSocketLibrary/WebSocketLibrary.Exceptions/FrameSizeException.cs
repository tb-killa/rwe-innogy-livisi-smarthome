using System;

namespace WebSocketLibrary.Exceptions;

public class FrameSizeException : Exception
{
	public FrameSizeException(string text)
		: base(text)
	{
	}
}
