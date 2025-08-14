using System;
using System.Threading;

namespace onrkn;

internal class pbpve
{
	private readonly Thread puvwn;

	private readonly AutoResetEvent rdeny = new AutoResetEvent(initialState: false);

	private volatile bool xrtaw;

	private Action pwjev;

	private readonly Action ispkr;

	private static Action fxpdu;

	public pbpve(Action onTaskFinished)
	{
		if (onTaskFinished == null || 1 == 0)
		{
			throw new ArgumentNullException("onTaskFinished");
		}
		ispkr = onTaskFinished;
		puvwn = new Thread(hqfby);
		puvwn.IsBackground = true;
		puvwn.Start();
	}

	public bool jzbvv(Action p0)
	{
		djmxe();
		Action action = Interlocked.CompareExchange(ref pwjev, p0, null);
		if (action != null && 0 == 0)
		{
			return false;
		}
		rdeny.Set();
		return true;
	}

	public bool matht(bool p0)
	{
		if (xrtaw && 0 == 0)
		{
			return true;
		}
		if (!p0 || 1 == 0)
		{
			if (fxpdu == null || 1 == 0)
			{
				fxpdu = dyobd;
			}
			Action value = fxpdu;
			if (Interlocked.CompareExchange(ref pwjev, value, null) == null || 1 == 0)
			{
				xrtaw = true;
				rdeny.Set();
				return true;
			}
			return false;
		}
		xrtaw = true;
		rdeny.Set();
		return true;
	}

	private void djmxe()
	{
		if (xrtaw && 0 == 0)
		{
			throw new ObjectDisposedException("ThreadPoolCompact");
		}
	}

	private void hqfby()
	{
		while (true)
		{
			rdeny.WaitOne();
			Action action = Interlocked.CompareExchange(ref pwjev, null, null);
			if (action != null && 0 == 0)
			{
				try
				{
					action();
				}
				catch
				{
				}
			}
			if (xrtaw && 0 == 0)
			{
				break;
			}
			pwjev = null;
			ispkr();
		}
	}

	private static void dyobd()
	{
	}
}
