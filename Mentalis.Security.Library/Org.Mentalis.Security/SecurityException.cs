using System;

namespace Org.Mentalis.Security;

[Serializable]
public class SecurityException : Exception
{
	public SecurityException()
	{
	}

	public SecurityException(string message)
		: base(message)
	{
	}

	public SecurityException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
