using System.Collections.Generic;

namespace onrkn;

internal class ujhhh : paeay
{
	public override string tbugp
	{
		get
		{
			if (base.orfvi != null && 0 == 0)
			{
				return "div";
			}
			return "body";
		}
	}

	public override bool aseow => tbugp != "div";

	public vdlbe cogdi => (vdlbe)base.gtswz;

	public ujhhh(fpnng model, vdlbe formatting)
		: base(model, formatting)
	{
		ziwoy(new qsfrt("", model, model.uyyni));
	}

	public override ujhhh nysqr()
	{
		return this;
	}

	internal override bool taklk(string p0)
	{
		string key;
		if ((key = p0) != null && 0 == 0)
		{
			if (czzgh.oftyv == null || 1 == 0)
			{
				czzgh.oftyv = new Dictionary<string, int>(10)
				{
					{ "intbl", 0 },
					{ "trowd", 1 },
					{ "row", 2 },
					{ "cell", 3 },
					{ "qc", 4 },
					{ "ql", 5 },
					{ "qr", 6 },
					{ "qd", 7 },
					{ "qj", 8 },
					{ "pard", 9 }
				};
			}
			if (czzgh.oftyv.TryGetValue(key, out var value))
			{
				switch (value)
				{
				case 0:
				{
					vdlbe izmfa6 = moion.izmfa;
					bool wjruz = (cogdi.wjruz = true);
					izmfa6.wjruz = wjruz;
					goto case 1;
				}
				case 1:
					if (!(base.orfvi is nuvgv) || 1 == 0)
					{
						cmjgd cmjgd2 = new cmjgd(moion, base.orfvi);
						base.orfvi.ziwoy(cmjgd2);
						moion.swkfa = cmjgd2.fuqfa as zbmst;
					}
					return true;
				case 2:
					if (base.orfvi is nuvgv nuvgv4 && 0 == 0)
					{
						nuvgv4.jvnbl.xebmg.ziwoy(new zbmst(moion, nuvgv4.jvnbl.ikgkh));
						moion.swkfa = nuvgv4.jvnbl.xebmg;
					}
					return true;
				case 3:
					if (base.orfvi is nuvgv nuvgv2 && 0 == 0)
					{
						nuvgv2.jvnbl.mncnv();
					}
					return true;
				case 4:
				{
					vdlbe izmfa5 = moion.izmfa;
					bdhck pvocn5 = (cogdi.pvocn = bdhck.wnclf);
					izmfa5.pvocn = pvocn5;
					return true;
				}
				case 5:
				{
					vdlbe izmfa4 = moion.izmfa;
					bdhck pvocn4 = (cogdi.pvocn = bdhck.berjj);
					izmfa4.pvocn = pvocn4;
					return true;
				}
				case 6:
				{
					vdlbe izmfa3 = moion.izmfa;
					bdhck pvocn3 = (cogdi.pvocn = bdhck.ifynk);
					izmfa3.pvocn = pvocn3;
					return true;
				}
				case 7:
				{
					vdlbe izmfa2 = moion.izmfa;
					bdhck pvocn2 = (cogdi.pvocn = bdhck.llnyg);
					izmfa2.pvocn = pvocn2;
					return true;
				}
				case 8:
				{
					vdlbe izmfa = moion.izmfa;
					bdhck pvocn = (cogdi.pvocn = bdhck.yeujp);
					izmfa.pvocn = pvocn;
					return true;
				}
				case 9:
					break;
				default:
					goto IL_02b7;
				}
				moion.jydeo();
				base.gtswz = new vdlbe(moion);
				goto IL_032b;
			}
		}
		goto IL_02b7;
		IL_032b:
		if (base.taklk(p0) && 0 == 0)
		{
			return true;
		}
		return base.fuqfa.taklk(p0);
		IL_02b7:
		if (p0.StartsWith("sb") && 0 == 0 && mnedn.rqdck(p0, "sb", out var p1))
		{
			cogdi.uwzfn = p1;
		}
		else if (p0.StartsWith("sa") && 0 == 0 && mnedn.rqdck(p0, "sa", out p1) && 0 == 0)
		{
			cogdi.boeuk = p1;
		}
		goto IL_032b;
	}

	public override void qehlq()
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_000c;
		}
		goto IL_0098;
		IL_000c:
		if (!(base[num] is qsfrt qsfrt2) || 1 == 0)
		{
			base[num].qehlq();
		}
		else
		{
			if (num == base.suqdl - 1)
			{
				return;
			}
			if (base[num + 1] is qsfrt qsfrt3 && 0 == 0 && qsfrt2.gtswz.Equals(qsfrt3.gtswz) && 0 == 0)
			{
				qsfrt2.hquev(qsfrt3.iabtc());
				akccy(num + 1);
				num--;
			}
		}
		num++;
		goto IL_0098;
		IL_0098:
		if (num >= base.suqdl)
		{
			return;
		}
		goto IL_000c;
	}
}
