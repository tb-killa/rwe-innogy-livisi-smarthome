using System;

namespace onrkn;

internal class pwzds : Exception
{
	public pwzds(Exception inner)
		: this(inner.Message, inner)
	{
	}

	public pwzds(string message, Exception inner)
		: base(message, inner)
	{
	}
}
