using System;

namespace onrkn;

internal class lmqll : Exception
{
	public lmqll()
	{
	}

	public lmqll(string message)
		: base(message)
	{
	}

	public lmqll(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
