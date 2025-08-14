using System;
using System.Collections.Generic;

namespace onrkn;

internal class jicrh<T0>
{
	public static readonly Func<jicrh<T0>> sbxti;

	private dvxgu<T0> nxwba;

	private readonly nrrft<T0> nbyer;

	private static Func<jicrh<T0>> uzdxa;

	public njvzu<T0> wmfea => nxwba.dioyl;

	public jicrh()
	{
		nxwba = new dvxgu<T0>();
		nbyer = wootk<T0>.ouxvq;
	}

	public bool ydtga(Exception p0)
	{
		bool flag = nbyer.ulpbb(p0, nxwba);
		if (flag && 0 == 0 && nxwba.dioyl.mnscz != null && 0 == 0)
		{
			ksleb(nxwba.dioyl.mnscz);
		}
		return flag;
	}

	public bool feggc(IEnumerable<Exception> p0)
	{
		bool flag = nbyer.dliqg(p0, nxwba);
		if (flag && 0 == 0 && nxwba.dioyl.mnscz != null && 0 == 0)
		{
			ksleb(nxwba.dioyl.mnscz);
		}
		return flag;
	}

	public void bntzg(Exception p0)
	{
		nbyer.wxgri(p0, nxwba);
		if (nxwba.dioyl.mnscz != null && 0 == 0)
		{
			ksleb(nxwba.dioyl.mnscz);
		}
	}

	public void ycqyq(IEnumerable<Exception> p0)
	{
		nbyer.aiubf(p0, nxwba);
		if (nxwba.dioyl.mnscz != null && 0 == 0)
		{
			ksleb(nxwba.dioyl.mnscz);
		}
	}

	public bool spaxx(T0 p0)
	{
		return nbyer.hkxwo(p0, nxwba);
	}

	public void rhkwc(T0 p0)
	{
		nbyer.mhgdh(p0, nxwba);
	}

	public void wkdam()
	{
		nbyer.xundl(nxwba);
		if (nxwba.dioyl.mnscz != null && 0 == 0)
		{
			ksleb(nxwba.dioyl.mnscz);
		}
	}

	public bool ajblh()
	{
		bool flag = nbyer.ayihm(nxwba);
		if (flag && 0 == 0 && nxwba.dioyl.mnscz != null && 0 == 0)
		{
			ksleb(nxwba.dioyl.mnscz);
		}
		return flag;
	}

	public void buehd()
	{
		if (!nxwba.dioyl.IsCompleted || 1 == 0)
		{
			nxwba.kcuac();
		}
		nxwba = new dvxgu<T0>();
	}

	internal static void ksleb(Exception p0)
	{
	}

	static jicrh()
	{
		if (uzdxa == null || 1 == 0)
		{
			uzdxa = hnnli;
		}
		sbxti = uzdxa;
	}

	private static jicrh<T0> hnnli()
	{
		return new jicrh<T0>();
	}
}
