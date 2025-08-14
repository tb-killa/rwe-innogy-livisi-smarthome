using System;

namespace onrkn;

internal class ozsgc : Exception
{
	public ozsgc(string message)
		: base(message)
	{
	}

	public ozsgc(string message, Exception inner)
		: base(message, inner)
	{
	}
}
