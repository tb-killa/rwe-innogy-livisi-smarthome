using System.Collections.Generic;
using System.Text;

namespace onrkn;

internal class qsfrt : fwjvi
{
	private StringBuilder lmohv;

	public override string tbugp => "span";

	public override qsfrt angkg => this;

	public qsfrt(string text, fpnng model, bgxyp formatting)
		: base(model, formatting)
	{
		lmohv = new StringBuilder(text);
	}

	internal override bool taklk(string p0)
	{
		string key;
		if ((key = p0) != null && 0 == 0)
		{
			if (czzgh.bpyvf == null || 1 == 0)
			{
				czzgh.bpyvf = new Dictionary<string, int>(11)
				{
					{ "line", 0 },
					{ "tab", 1 },
					{ "lquote", 2 },
					{ "rquote", 3 },
					{ "ldblquote", 4 },
					{ "rdblquote", 5 },
					{ "emdash", 6 },
					{ "endash", 7 },
					{ "bullet", 8 },
					{ "-", 9 },
					{ " ", 10 }
				};
			}
			if (czzgh.bpyvf.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
					hquev(brgjd.edcru("{0}<br>{0}", mnedn.zybru));
					return true;
				case 1:
					hquev("&nbsp;&nbsp;&nbsp;&nbsp;");
					return true;
				case 2:
					hquev("‘");
					return true;
				case 3:
					hquev("’");
					return true;
				case 4:
					hquev("“");
					return true;
				case 5:
					hquev("”");
					return true;
				case 6:
					hquev("—");
					return true;
				case 7:
					hquev("–");
					return true;
				case 8:
					hquev("•");
					return true;
				case 9:
					hquev("-");
					return true;
				case 10:
					hquev("&nbsp;");
					return true;
				}
			}
		}
		return false;
	}

	private bool jtaxl(string p0)
	{
		if (p0.Length != 1)
		{
			return false;
		}
		if (p0[0] != ' ')
		{
			return false;
		}
		return true;
	}

	public override string pndmk()
	{
		string p = lmohv.ToString();
		p = ivsvp(p);
		if (p.Length == 0 || 1 == 0)
		{
			return "";
		}
		if (p == "</br>" && 0 == 0)
		{
			return "</br>";
		}
		if (jtaxl(p) && 0 == 0)
		{
			p = "&nbsp;";
		}
		string text = base.gtswz.dsdoz();
		if (text.Length == 0 || 1 == 0)
		{
			return brgjd.edcru("<{0}>{1}</{0}>", tbugp, p);
		}
		return brgjd.edcru("<{0} style=\"{2}\">{1}</{0}>", tbugp, p, text);
	}

	internal void hquev(string p0)
	{
		lmohv.Append(p0);
	}

	public override string ToString()
	{
		return lmohv.ToString();
	}

	public string iabtc()
	{
		return lmohv.ToString();
	}

	private string ivsvp(string p0)
	{
		return p0.Replace("\0", "");
	}
}
