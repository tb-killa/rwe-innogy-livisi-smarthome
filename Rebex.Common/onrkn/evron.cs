using System;

namespace onrkn;

[AttributeUsage(AttributeTargets.Method)]
internal class evron : Attribute
{
	public bool lpawn;

	public bool gjtim;

	public bool rgaar;

	public evron()
	{
		lpawn = false;
		gjtim = false;
		rgaar = false;
	}
}
