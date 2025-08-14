using System;

namespace onrkn;

internal class hyeis : Exception
{
	public hyeis()
	{
	}

	public hyeis(string message)
		: base(message)
	{
	}

	public hyeis(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
