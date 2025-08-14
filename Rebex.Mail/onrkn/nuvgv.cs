using System.Text;

namespace onrkn;

internal class nuvgv : paeay
{
	public const int buaio = 20;

	public const string wftrl = "pt";

	public override string tbugp => "td";

	public int iybtz => bgcjm.tntkx.cqmdm(bgcjm.aelyo);

	public int fscgt => iybtz - jvnbl.xywqi;

	public zbmst jvnbl => (zbmst)base.orfvi;

	public xgoqi bgcjm => (xgoqi)base.gtswz;

	internal nuvgv(fpnng model, paeay parent, xgoqi formatting)
		: base(model, formatting)
	{
		ziwoy(new ujhhh(model, model.izmfa));
	}

	public override string pndmk()
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		if (num != 0)
		{
			goto IL_000f;
		}
		goto IL_0026;
		IL_000f:
		stringBuilder.Append(base[num].pndmk());
		num++;
		goto IL_0026;
		IL_0026:
		if (num >= base.suqdl)
		{
			if (stringBuilder.ToString().Replace("<p>", string.Empty).Replace("</p>", string.Empty)
				.Length == 0 || 1 == 0)
			{
				stringBuilder.Remove(0, stringBuilder.Length);
				stringBuilder.Append("&nbsp;");
			}
			string text = base.gtswz.dsdoz();
			string result = (((text.Length != 0) ? true : false) ? brgjd.edcru("{4}<{0} colspan=\"{3}\" style=\"{2}\">{1}</{0}>{4}", tbugp, stringBuilder, text, fscgt, mnedn.zybru) : brgjd.edcru("{3}<{0} colspan=\"{2}\">{1}</{0}>{3}", tbugp, stringBuilder, fscgt, mnedn.zybru));
			jvnbl.xywqi += fscgt;
			return result;
		}
		goto IL_000f;
	}

	public override ujhhh nysqr()
	{
		return ((paeay)base.fuqfa).nysqr();
	}

	internal override bool taklk(string p0)
	{
		int p1;
		if (p0.StartsWith("cl") && 0 == 0)
		{
			string text;
			if ((text = p0) != null && 0 == 0 && text == "clshdrawnil" && 0 == 0)
			{
				bgcjm.zfnqj = false;
				return true;
			}
			if (p0.StartsWith("clcbpatraw") && 0 == 0)
			{
				return true;
			}
			if (p0.StartsWith("clcbpat") && 0 == 0 && mnedn.rqdck(p0, "clcbpat", out p1) && 0 == 0)
			{
				bgcjm.wudph = p1;
				return true;
			}
			if (p0.StartsWith("clftsWidth") && 0 == 0 && p0.Length == 11)
			{
				switch (p0[10])
				{
				case '0':
				case '1':
					bgcjm.yethr = true;
					return true;
				case '2':
					bgcjm.sgqmr = "%";
					return true;
				case '3':
					bgcjm.sgqmr = "pt";
					return true;
				}
			}
			else if (p0.StartsWith("clwWidth") && 0 == 0 && mnedn.rqdck(p0, "clwWidth", out p1) && 0 == 0)
			{
				if (bgcjm.yethr && 0 == 0)
				{
					return true;
				}
				if (bgcjm.sgqmr == "pt" && 0 == 0)
				{
					bgcjm.tgyfn = jvnbl.xebmg.lzkrd(p1);
					return true;
				}
				bgcjm.tgyfn = p1;
				return true;
			}
			return true;
		}
		if (p0.StartsWith("br") && 0 == 0)
		{
			return true;
		}
		if (p0.StartsWith("cellx") && 0 == 0 && mnedn.rqdck(p0, "cellx", out p1) && 0 == 0)
		{
			bgcjm.kgttg = p1;
			if (jvnbl.rdhhb == 1)
			{
				bgcjm.aelyo = bgcjm.kgttg - jvnbl.ikgkh.oalok;
				bgcjm.qonjr = jvnbl.xebmg.lzkrd(bgcjm.aelyo);
			}
			else
			{
				bgcjm.aelyo = bgcjm.kgttg - jvnbl.ikgkh.oalok;
				xgoqi xgoqi2 = base.orfvi[jvnbl.rdhhb - 2].gtswz as xgoqi;
				bgcjm.qonjr = jvnbl.xebmg.lzkrd(bgcjm.aelyo - xgoqi2.aelyo);
			}
			jvnbl.xebmg.cgujs(bgcjm.aelyo);
			return true;
		}
		return base.orfvi.taklk(p0);
	}
}
