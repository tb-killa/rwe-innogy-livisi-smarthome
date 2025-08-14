using System;

namespace onrkn;

internal class uwkib : Exception
{
	public uwkib(string message)
		: base(message)
	{
	}

	public uwkib(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	internal uwkib(string messageFormat, params object[] args)
		: base(brgjd.edcru(messageFormat, args))
	{
	}
}
