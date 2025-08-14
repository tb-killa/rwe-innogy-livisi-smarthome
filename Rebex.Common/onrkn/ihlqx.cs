using System;
using System.Threading;

namespace onrkn;

internal class ihlqx<T0> : IDisposable
{
	private Action<T0> ggucp;

	private readonly T0 tsaja;

	public T0 wnjdk
	{
		get
		{
			if (ggucp == null || 1 == 0)
			{
				throw new ObjectDisposedException(brgjd.edcru("Pooled<{0}>", typeof(T0).FullName));
			}
			return tsaja;
		}
	}

	public ihlqx(T0 value, Action<T0> returnAction)
	{
		if (returnAction == null || 1 == 0)
		{
			throw new ArgumentNullException("returnAction");
		}
		ggucp = returnAction;
		tsaja = value;
	}

	public void Dispose()
	{
		Action<T0> action = Interlocked.Exchange(ref ggucp, null);
		if (action != null && 0 == 0)
		{
			action(tsaja);
		}
	}
}
