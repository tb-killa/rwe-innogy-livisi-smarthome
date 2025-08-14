using System;
using System.Threading;

namespace onrkn;

internal abstract class fpwmx
{
	private int qxjpr;

	public int uzmnu => qxjpr;

	protected fpwmx(int initialCount)
	{
		if (initialCount < 0)
		{
			throw new ArgumentOutOfRangeException("initialCount");
		}
		qxjpr = initialCount;
	}

	protected abstract void mvhmz();

	public int ilued()
	{
		return Interlocked.Increment(ref qxjpr);
	}

	public int yayaz()
	{
		if (qxjpr == 0 || 1 == 0)
		{
			return 0;
		}
		int num = Interlocked.Decrement(ref qxjpr);
		if (num == 0 || 1 == 0)
		{
			mvhmz();
		}
		return num;
	}

	public int byhxs(int p0)
	{
		int num = qxjpr;
		int num2;
		int num3;
		do
		{
			num2 = num;
			num3 = num2 + p0;
			num = Interlocked.CompareExchange(ref qxjpr, num3, num);
		}
		while (num != num2);
		return num3;
	}

	public override string ToString()
	{
		return brgjd.edcru("Counter: {0}", qxjpr);
	}

	public static skzdp<TDisposable> xrlpw<TDisposable>(TDisposable p0) where TDisposable : class, IDisposable
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("disposableInstance");
		}
		return new skzdp<TDisposable>(1, p0);
	}
}
