using System;

namespace onrkn;

internal class ufgee : Exception
{
	public ufgee(string message)
		: base(message)
	{
	}

	public ufgee(string message, Exception inner)
		: base(message, inner)
	{
	}
}
