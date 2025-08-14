using System;
using System.Collections.Generic;
using System.Text;

namespace onrkn;

internal class cmjgd : paeay
{
	private Dictionary<int, object> zwluh;

	private int[] tbijl;

	private Dictionary<int, int> ebiok;

	public override string tbugp => "table";

	internal cmjgd(fpnng model, paeay parent)
		: base(model, new uuhjg(model))
	{
		ziwoy(new zbmst(model, new eixjz(model)));
		zwluh = new Dictionary<int, object>();
		ebiok = new Dictionary<int, int>();
	}

	public override ujhhh nysqr()
	{
		return ((paeay)base.fuqfa).nysqr();
	}

	public override string pndmk()
	{
		tbijl = new int[zwluh.Count];
		zwluh.Keys.CopyTo(tbijl, 0);
		Array.Sort(tbijl);
		int num = 1;
		int[] array = tbijl;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0049;
		}
		goto IL_0066;
		IL_0049:
		int key = array[num2];
		ebiok.Add(key, num);
		num++;
		num2++;
		goto IL_0066;
		IL_0066:
		if (num2 < array.Length)
		{
			goto IL_0049;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(nzmmy());
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_0086;
		}
		goto IL_009d;
		IL_009d:
		if (num3 >= base.suqdl)
		{
			if (stringBuilder.Length == 0 || 1 == 0)
			{
				return "";
			}
			string text = base.gtswz.dsdoz();
			if (text.Length == 0 || 1 == 0)
			{
				return brgjd.edcru("{2}<{0}>{2}{1}{2}</{0}>{2}", tbugp, stringBuilder, mnedn.zybru);
			}
			return brgjd.edcru("{3}<{0} style=\"{2}\">{3}{1}{3}</{0}>{3}", tbugp, stringBuilder, text, mnedn.zybru);
		}
		goto IL_0086;
		IL_0086:
		stringBuilder.Append(base[num3].pndmk());
		num3++;
		goto IL_009d;
	}

	private string nzmmy()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(brgjd.edcru("{0}<tr height=0>", mnedn.zybru));
		int num = 0;
		int[] array = tbijl;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_003f;
		}
		goto IL_00b1;
		IL_003f:
		int num3 = array[num2];
		string text = brgjd.edcru("width:{0}pt;", lzkrd(num3 - num));
		stringBuilder.Append(brgjd.edcru("{2}<td width=\"{0}\" style=\"{1}\"></td>", uvpea(num3 - num), text, mnedn.zybru));
		num = num3;
		num2++;
		goto IL_00b1;
		IL_00b1:
		if (num2 >= array.Length)
		{
			stringBuilder.Append(brgjd.edcru("{0}</tr>", mnedn.zybru));
			return stringBuilder.ToString();
		}
		goto IL_003f;
	}

	internal int uvpea(double p0)
	{
		return (int)Math.Round(p0 / 15.0, 0);
	}

	internal double lzkrd(double p0)
	{
		return Math.Round(p0 / 20.0, 1);
	}

	internal override bool taklk(string p0)
	{
		string text;
		if ((text = p0) != null && 0 == 0)
		{
			if (text == "intbl")
			{
				moion.swkfa = base.fuqfa as zbmst;
				base.fuqfa.taklk(p0);
				return true;
			}
			if (text == "trowd")
			{
				moion.swkfa = base.fuqfa as zbmst;
				return true;
			}
			if (text == "par" || text == "\r" || text == "\n")
			{
				blgfe();
				return true;
			}
		}
		if (base.taklk(p0) && 0 == 0)
		{
			return true;
		}
		return base.fuqfa.taklk(p0);
	}

	internal void blgfe()
	{
		if (nysqr().cogdi.wjruz && 0 == 0)
		{
			moion.swkfa = base.fuqfa as zbmst;
			return;
		}
		ujhhh ujhhh2 = nysqr();
		base.orfvi.ziwoy(ujhhh2);
		moion.swkfa = ujhhh2;
		akccy(base.suqdl - 1);
	}

	internal void cgujs(int p0)
	{
		if (!zwluh.ContainsKey(p0) || 1 == 0)
		{
			zwluh.Add(p0, null);
		}
	}

	internal int cqmdm(int p0)
	{
		ebiok.TryGetValue(p0, out var value);
		return value;
	}
}
