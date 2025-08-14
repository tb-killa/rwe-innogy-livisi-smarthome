using System.Text;

namespace onrkn;

internal class zbmst : paeay
{
	private int uuuiw;

	private int fowjf;

	private int seuhs;

	public override string tbugp => "tr";

	public cmjgd xebmg => (cmjgd)base.orfvi;

	public eixjz ikgkh => (eixjz)base.gtswz;

	public int lqtdq
	{
		get
		{
			return uuuiw;
		}
		private set
		{
			uuuiw = value;
		}
	}

	public int rdhhb
	{
		get
		{
			return fowjf;
		}
		private set
		{
			fowjf = value;
		}
	}

	public int xywqi
	{
		get
		{
			return seuhs;
		}
		internal set
		{
			seuhs = value;
		}
	}

	internal zbmst(fpnng model, eixjz formatting)
		: base(model, formatting)
	{
		ziwoy(new nuvgv(model, this, new xgoqi(model, this)));
	}

	public override string pndmk()
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		if (num != 0)
		{
			goto IL_000c;
		}
		goto IL_0023;
		IL_000c:
		stringBuilder.Append(base[num].pndmk());
		num++;
		goto IL_0023;
		IL_0023:
		if (num >= rdhhb)
		{
			string text = base.gtswz.dsdoz();
			if (text.Length == 0 || 1 == 0)
			{
				return brgjd.edcru("{2}<{0}>{2}{1}{2}</{0}>{2}", tbugp, stringBuilder, mnedn.zybru);
			}
			return brgjd.edcru("{3}<{0} style=\"{2}\">{3}{1}{3}</{0}>{3}", tbugp, stringBuilder, text, mnedn.zybru);
		}
		goto IL_000c;
	}

	public override ujhhh nysqr()
	{
		while (base.suqdl <= lqtdq)
		{
			ziwoy(new nuvgv(moion, this, new xgoqi(moion, this)));
		}
		return ((paeay)base[lqtdq]).nysqr();
	}

	public void mncnv()
	{
		while (base.suqdl <= lqtdq)
		{
			ziwoy(new nuvgv(moion, this, new xgoqi(moion, this)));
		}
		lqtdq++;
		moion.swkfa = this;
	}

	internal override bool taklk(string p0)
	{
		string text;
		if ((text = p0) != null && 0 == 0)
		{
			if (text == "trowd")
			{
				moion.swkfa = this;
				rdhhb = 0;
				return true;
			}
			if (text == "row")
			{
				base.orfvi.ziwoy(new zbmst(moion, ikgkh));
				moion.swkfa = base.orfvi;
				return true;
			}
			if (text == "cell")
			{
				mncnv();
				return true;
			}
			if (text == "intbl")
			{
				vdlbe izmfa = moion.izmfa;
				bool wjruz = (nysqr().cogdi.wjruz = true);
				izmfa.wjruz = wjruz;
				return true;
			}
		}
		if ((p0.StartsWith("tr") ? true : false) || (p0.StartsWith("tp") ? true : false) || p0.StartsWith("td"))
		{
			string text2;
			if ((text2 = p0) != null && 0 == 0)
			{
				if (text2 == "trqc")
				{
					ikgkh.aowfa = bdhck.wnclf;
					return true;
				}
				if (text2 == "trql")
				{
					ikgkh.aowfa = bdhck.berjj;
					return true;
				}
				if (text2 == "trqr")
				{
					ikgkh.aowfa = bdhck.ifynk;
					return true;
				}
				if (text2 == "trqd")
				{
					ikgkh.aowfa = bdhck.llnyg;
					return true;
				}
				if (text2 == "trqj")
				{
					ikgkh.aowfa = bdhck.yeujp;
					return true;
				}
			}
			if (p0.StartsWith("trgaph") && 0 == 0 && mnedn.rqdck(p0, "trgaph", out var p1) && 0 == 0)
			{
				ikgkh.ilxzg = p1;
				return true;
			}
			if (p0.StartsWith("trrh") && 0 == 0 && mnedn.rqdck(p0, "trrh", out p1) && 0 == 0)
			{
				ikgkh.beaei = p1 / 20;
				return true;
			}
			if (p0.StartsWith("trleft") && 0 == 0 && mnedn.rqdck(p0, "trleft", out p1) && 0 == 0)
			{
				ikgkh.oalok = p1;
				return true;
			}
		}
		else if ((p0.StartsWith("cl") ? true : false) || (p0.StartsWith("br") ? true : false) || p0.StartsWith("cellx"))
		{
			nuvgv nuvgv2;
			if (rdhhb < base.suqdl)
			{
				nuvgv2 = base[rdhhb] as nuvgv;
			}
			else
			{
				nuvgv2 = new nuvgv(moion, this, new xgoqi(moion, this));
				ziwoy(nuvgv2);
			}
			if (p0.StartsWith("cellx") && 0 == 0)
			{
				rdhhb++;
			}
			return nuvgv2.taklk(p0);
		}
		if (base.taklk(p0) && 0 == 0)
		{
			return true;
		}
		if (nysqr().taklk(p0) && 0 == 0)
		{
			return true;
		}
		return false;
	}
}
