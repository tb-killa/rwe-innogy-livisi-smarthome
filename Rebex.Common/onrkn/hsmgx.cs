using System;

namespace onrkn;

internal class hsmgx : Exception
{
	public hsmgx()
		: base("Semaphore would exceed its maximum count.")
	{
	}
}
