using System;
using System.Collections.Generic;
using System.Text;

namespace onrkn;

internal abstract class fwjvi
{
	public readonly fpnng moion;

	private readonly List<fwjvi> zthob;

	private paeay nztlp;

	private bgxyp sufhk;

	public abstract string tbugp { get; }

	public virtual bool aseow => true;

	public paeay orfvi
	{
		get
		{
			return nztlp;
		}
		internal set
		{
			nztlp = value;
		}
	}

	public bgxyp gtswz
	{
		get
		{
			return sufhk;
		}
		protected set
		{
			sufhk = value;
		}
	}

	public int suqdl => zthob.Count;

	public fwjvi fuqfa
	{
		get
		{
			if (zthob.Count != 0 && 0 == 0)
			{
				return zthob[zthob.Count - 1];
			}
			return null;
		}
	}

	public virtual qsfrt angkg
	{
		get
		{
			if (zthob.Count != 0 && 0 == 0)
			{
				return zthob[zthob.Count - 1].angkg;
			}
			return null;
		}
	}

	public fwjvi this[int index] => zthob[index];

	public fwjvi(fpnng model, bgxyp formatting)
	{
		moion = model;
		gtswz = formatting.xakax();
		zthob = new List<fwjvi>();
	}

	public virtual void ziwoy(fwjvi p0)
	{
		paeay paeay2 = this as paeay;
		if (paeay2 == null || 1 == 0)
		{
			new InvalidOperationException("Invalid use of method (expected instance of the RtfParagraph).");
		}
		p0.orfvi = paeay2;
		zthob.Add(p0);
	}

	public void akccy(int p0)
	{
		zthob.RemoveAt(p0);
	}

	public virtual void qehlq()
	{
		using List<fwjvi>.Enumerator enumerator = zthob.GetEnumerator();
		while (enumerator.MoveNext() ? true : false)
		{
			fwjvi current = enumerator.Current;
			current.qehlq();
		}
	}

	public virtual string pndmk()
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		if (num != 0)
		{
			goto IL_000f;
		}
		goto IL_0026;
		IL_000f:
		stringBuilder.Append(this[num].pndmk());
		num++;
		goto IL_0026;
		IL_0026:
		if (num >= suqdl)
		{
			if (stringBuilder.Length == 0 || 1 == 0)
			{
				if (this is ujhhh && 0 == 0)
				{
					return brgjd.edcru("{0}<br/>{0}", mnedn.zybru);
				}
				return "";
			}
			string text = gtswz.dsdoz();
			string text2 = ((aseow ? true : false) ? mnedn.zybru : string.Empty);
			if (text.Length == 0 || 1 == 0)
			{
				return brgjd.edcru("{3}<{0}>{2}{1}{2}</{0}>{3}", tbugp, stringBuilder, text2, mnedn.zybru);
			}
			return brgjd.edcru("{4}<{0} style=\"{2}\">{3}{1}{3}</{0}>{4}", tbugp, stringBuilder, text, text2, mnedn.zybru);
		}
		goto IL_000f;
	}

	internal virtual bool taklk(string p0)
	{
		string key;
		if ((key = p0) != null && 0 == 0)
		{
			if (czzgh.obwto == null || 1 == 0)
			{
				czzgh.obwto = new Dictionary<string, int>(23)
				{
					{ "rtf1", 0 },
					{ "ansi", 1 },
					{ "ansicpg1252", 2 },
					{ "intbl", 3 },
					{ "pard", 4 },
					{ "ul", 5 },
					{ "ulnone", 6 },
					{ "i", 7 },
					{ "i0", 8 },
					{ "b", 9 },
					{ "b0", 10 },
					{ "strike", 11 },
					{ "strike0", 12 },
					{ "striked1", 13 },
					{ "striked0", 14 },
					{ "sub", 15 },
					{ "super", 16 },
					{ "embo", 17 },
					{ "impr", 18 },
					{ "plain", 19 },
					{ "par", 20 },
					{ "\r", 21 },
					{ "\n", 22 }
				};
			}
			if (czzgh.obwto.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
				case 1:
				case 2:
				case 3:
					return true;
				case 4:
					moion.jydeo();
					moion.swkfa.nysqr().gtswz = new vdlbe(moion);
					return true;
				case 5:
					moion.uyyni.ozxic.hchkn = true;
					return true;
				case 6:
					moion.uyyni.ozxic.hchkn = false;
					return true;
				case 7:
					moion.uyyni.ozxic.jdcus = true;
					return true;
				case 8:
					moion.uyyni.ozxic.jdcus = false;
					return true;
				case 9:
					moion.uyyni.ozxic.lljxt = true;
					return true;
				case 10:
					moion.uyyni.ozxic.lljxt = false;
					return true;
				case 11:
					moion.uyyni.ozxic.xsiqy = true;
					return true;
				case 12:
					moion.uyyni.ozxic.xsiqy = false;
					return true;
				case 13:
					moion.uyyni.ozxic.ywrnu = true;
					return true;
				case 14:
					moion.uyyni.ozxic.ywrnu = false;
					return true;
				case 15:
					moion.uyyni.ozxic.uqwbx = true;
					return true;
				case 16:
					moion.uyyni.ozxic.qsrru = true;
					return true;
				case 17:
					moion.uyyni.ozxic.geabl = true;
					return true;
				case 18:
					moion.uyyni.ozxic.lfwjp = true;
					return true;
				case 19:
					moion.uyyni.ozxic = new gnadp(moion);
					moion.uyyni.diubo = new qbhyy(-1, moion);
					moion.uyyni.shgkl = new oxbyz(moion);
					return true;
				case 20:
				case 21:
				case 22:
				{
					ujhhh ujhhh2 = moion.swkfa.nysqr();
					ujhhh ujhhh3 = new ujhhh(moion, moion.izmfa);
					ujhhh2.orfvi.ziwoy(ujhhh3);
					moion.swkfa = ujhhh3;
					return true;
				}
				}
			}
		}
		if (p0.StartsWith("deff") && 0 == 0 && mnedn.rqdck(p0, "deff", out var p1) && 0 == 0)
		{
			moion.crrut(p1);
			return true;
		}
		if (p0.StartsWith("cf") && 0 == 0 && mnedn.rqdck(p0, "cf", out p1) && 0 == 0)
		{
			moion.uyyni.shgkl = new oxbyz("c" + p1, p1, moion);
			return true;
		}
		if (p0.StartsWith("cb") && 0 == 0 && mnedn.rqdck(p0, "cb", out p1) && 0 == 0)
		{
			return true;
		}
		if (p0.StartsWith("fs") && 0 == 0 && mnedn.rqdck(p0, "fs", out p1) && 0 == 0)
		{
			moion.uyyni.diubo = new qbhyy(p1 / 2, moion);
			return true;
		}
		if (p0.StartsWith("f") && 0 == 0 && mnedn.rqdck(p0, "f", out p1) && 0 == 0)
		{
			moion.uyyni.cydme = new dmcjx("f" + p1, p1, moion);
			return true;
		}
		if (p0.StartsWith("highlight") && 0 == 0 && mnedn.rqdck(p0, "highlight", out p1) && 0 == 0)
		{
			moion.uyyni.ozxic.iekwa = true;
			return true;
		}
		return false;
	}
}
