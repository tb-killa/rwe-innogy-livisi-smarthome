using System;
using System.Threading;

namespace onrkn;

internal class jjerb
{
	private class orqnk : IDisposable
	{
		private jjerb dcdrd;

		public orqnk(jjerb provider)
		{
			dcdrd = provider;
		}

		public void Dispose()
		{
			if (dcdrd == null || 1 == 0)
			{
				throw new InvalidOperationException("Lock unlocked twice.");
			}
			dcdrd.hjvir();
			dcdrd = null;
			GC.SuppressFinalize(this);
		}
	}

	private readonly object okdyp;

	private int gqgzq;

	public bool yqdmw => gqgzq > 0;

	public IDisposable ixtmk()
	{
		if (!sdmfe(out var p) || 1 == 0)
		{
			throw new InvalidOperationException("Another operation is pending.");
		}
		return p;
	}

	public bool sdmfe(out IDisposable p0)
	{
		if (!Monitor.TryEnter(okdyp) || 1 == 0)
		{
			p0 = null;
			return false;
		}
		gqgzq++;
		p0 = new orqnk(this);
		return true;
	}

	private void hjvir()
	{
		gqgzq--;
		Monitor.Exit(okdyp);
	}

	public jjerb()
	{
		okdyp = new object();
	}
}
