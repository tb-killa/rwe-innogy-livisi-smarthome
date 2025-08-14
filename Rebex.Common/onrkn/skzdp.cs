using System;

namespace onrkn;

internal class skzdp<T0> : rzobj<T0> where T0 : class, IDisposable
{
	private static readonly Action<T0> jqvln;

	private static Action<T0> cjdta;

	public skzdp(int initialCount, T0 disposableResource)
		: base(initialCount, disposableResource, jqvln)
	{
	}

	static skzdp()
	{
		if (cjdta == null || 1 == 0)
		{
			cjdta = gunon;
		}
		jqvln = cjdta;
	}

	private static void gunon(T0 p0)
	{
		p0.Dispose();
	}
}
