using System;

namespace Org.BouncyCastle.Security;

public class InvalidParameterException : KeyException
{
	public InvalidParameterException()
	{
	}

	public InvalidParameterException(string message)
		: base(message)
	{
	}

	public InvalidParameterException(string message, Exception exception)
		: base(message, exception)
	{
	}
}
