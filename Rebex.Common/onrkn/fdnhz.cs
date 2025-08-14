using System;

namespace onrkn;

internal class fdnhz
{
	private class onici : IDisposable
	{
		private readonly ogeww uftsz;

		private readonly bool wsbxx;

		public onici(ogeww asyncSemaphore, bool isCachedInstance)
		{
			if (asyncSemaphore == null || 1 == 0)
			{
				throw new ArgumentNullException("asyncSemaphore");
			}
			uftsz = asyncSemaphore;
			wsbxx = isCachedInstance;
		}

		public void Dispose()
		{
			uftsz.zvrwx();
		}
	}

	public const string fejej = "Async wait failed.";

	public const int wcmvk = 1;

	private readonly ogeww qkbzw;

	private readonly njvzu<IDisposable> pynxi;

	public fdnhz()
	{
		qkbzw = new ogeww(1, 1);
		pynxi = rxpjc.caxut((IDisposable)new onici(qkbzw, isCachedInstance: true));
	}

	public njvzu<IDisposable> uqlnf(ddmlv p0)
	{
		exkzi exkzi2 = qkbzw.qlvza(p0);
		jwdyh(exkzi2);
		if (exkzi2.IsCompleted && 0 == 0)
		{
			return pynxi;
		}
		return exkzi2.nsehe(pmzwl);
	}

	public njvzu<IDisposable> uivze()
	{
		return uqlnf(ddmlv.prdik);
	}

	private static void jwdyh(exkzi p0)
	{
		if ((p0.lctag ? true : false) || p0.ijeei)
		{
			throw new hyeis("Async wait failed.", p0.mnscz);
		}
	}

	private IDisposable pmzwl(exkzi p0)
	{
		jwdyh(p0);
		return new onici(qkbzw, isCachedInstance: false);
	}
}
