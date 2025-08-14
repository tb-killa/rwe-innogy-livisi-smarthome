using System;

namespace onrkn;

internal class teymq : InvalidOperationException
{
	public teymq(string message)
		: base(message)
	{
	}
}
