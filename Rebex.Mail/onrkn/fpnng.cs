using System.Collections.Generic;

namespace onrkn;

internal class fpnng
{
	private paeay sjpav;

	private paeay rglfx;

	private int axvvx = -1;

	private readonly Stack<iuphh> idnwz;

	private readonly Stack<vdlbe> lbuet;

	public readonly Dictionary<int, ooyms> jwtwk;

	public readonly Dictionary<int, brnhb> ivqtq;

	public string jahzk;

	internal iuphh uyyni => idnwz.Peek();

	internal vdlbe izmfa => lbuet.Peek();

	public paeay swkfa
	{
		get
		{
			return sjpav;
		}
		set
		{
			sjpav = value;
		}
	}

	internal fpnng()
	{
		jwtwk = new Dictionary<int, ooyms>();
		ivqtq = new Dictionary<int, brnhb>();
		idnwz = new Stack<iuphh>();
		idnwz.Push(new iuphh(this));
		lbuet = new Stack<vdlbe>();
		lbuet.Push(new vdlbe(this));
		rglfx = new ujhhh(this, izmfa);
		sjpav = new ujhhh(this, izmfa);
		rglfx.ziwoy(sjpav);
	}

	internal void tqiqh(string p0, bool p1)
	{
		p0 = mnedn.miajk(p0);
		if (p0.Length == 0 || 1 == 0)
		{
			return;
		}
		if (!p1 || 1 == 0)
		{
			p0 = mnedn.rgxzy(p0);
		}
		if (swkfa is cmjgd cmjgd2 && 0 == 0)
		{
			cmjgd2.blgfe();
		}
		ujhhh ujhhh2 = swkfa.nysqr();
		if (ujhhh2 == null)
		{
			return;
		}
		qsfrt angkg = ujhhh2.angkg;
		if (angkg != null && 0 == 0 && angkg.gtswz.Equals(uyyni) && 0 == 0)
		{
			angkg.hquev(p0);
			return;
		}
		if (swkfa is codsj && 0 == 0 && angkg.ToString() == string.Empty && 0 == 0)
		{
			ujhhh2.akccy(ujhhh2.suqdl - 1);
		}
		qsfrt p2 = new qsfrt(p0, this, uyyni);
		ujhhh2.ziwoy(p2);
	}

	public string wdtbt()
	{
		return rglfx.pndmk();
	}

	internal void trlza()
	{
		idnwz.Push((iuphh)uyyni.xakax());
		lbuet.Push((vdlbe)izmfa.xakax());
	}

	internal void cafwu()
	{
		idnwz.Pop();
		lbuet.Pop();
	}

	public void jydeo()
	{
		lbuet.Pop();
		lbuet.Push(new vdlbe(this));
	}

	public void vejtn()
	{
		rglfx.qehlq();
	}

	internal void crrut(int p0)
	{
		axvvx = p0;
		iuphh iuphh2 = new iuphh(this);
		iuphh2.cydme = new dmcjx("f" + p0, p0, this);
		idnwz.Push(iuphh2);
	}

	public string ninlm(int p0)
	{
		if (ivqtq.TryGetValue(p0, out var value) && 0 == 0)
		{
			return value.xtprc();
		}
		value = null;
		if (axvvx != -1 && ivqtq.TryGetValue(axvvx, out value) && 0 == 0)
		{
			return value.xtprc();
		}
		return "";
	}

	public string llbka(int p0)
	{
		if (jwtwk.TryGetValue(p0, out var value) && 0 == 0)
		{
			return "color:#" + value.ykxfe();
		}
		return "";
	}

	public string omlea(int p0)
	{
		if (jwtwk.TryGetValue(p0, out var value) && 0 == 0)
		{
			return "background-color:#" + value.ykxfe();
		}
		return "";
	}
}
