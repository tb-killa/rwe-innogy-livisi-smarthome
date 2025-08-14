using System;
using System.Collections.Generic;
using System.Text;

namespace onrkn;

internal class codsj : mfths
{
	private ujhhh vehcy;

	private bool ebfpu;

	private bool mglbb;

	public override string tbugp => "a";

	public bool empmv
	{
		get
		{
			return ebfpu;
		}
		set
		{
			ebfpu = value;
		}
	}

	public bool hhrbp
	{
		get
		{
			return mglbb;
		}
		set
		{
			mglbb = value;
		}
	}

	public override ujhhh cjacp => vehcy;

	internal override bool taklk(string p0)
	{
		string text;
		if ((text = p0) != null && 0 == 0)
		{
			if (text == "fldinst")
			{
				empmv = false;
				hhrbp = true;
				return true;
			}
			if (text == "fldrslt")
			{
				hhrbp = false;
				empmv = true;
				return true;
			}
		}
		return cjacp.taklk(p0);
	}

	internal codsj(fpnng model, paeay parent, bgxyp formatting)
		: base(model, formatting)
	{
		base.orfvi = parent;
		vehcy = new ujhhh(model, model.izmfa);
		vehcy.orfvi = this;
	}

	public override string pndmk()
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0009;
		}
		goto IL_0047;
		IL_0009:
		if (cjacp[num].ToString().Trim() == string.Empty && 0 == 0)
		{
			cjacp.akccy(num);
			num--;
		}
		num++;
		goto IL_0047;
		IL_0047:
		if (num >= cjacp.suqdl)
		{
			if (cjacp.suqdl == 0 || 1 == 0)
			{
				return string.Empty;
			}
			if (cjacp.suqdl == 1)
			{
				qsfrt qsfrt2;
				if ((qsfrt2 = cjacp[0].angkg) != null && 0 == 0 && evsjd(qsfrt2.iabtc()) && 0 == 0)
				{
					return kpxhh();
				}
				return cjacp[0].pndmk();
			}
			if (cjacp.suqdl >= 3)
			{
				return cjacp.pndmk();
			}
			return kpxhh();
		}
		goto IL_0009;
	}

	private string kpxhh()
	{
		if (cjacp.suqdl == 2)
		{
			string text = cjacp[0].ToString().Replace("HYPERLINK ", "").Replace("\"", "");
			if (text.IndexOf("mailto:") >= 0)
			{
				text = text.Replace("[mailto:", "");
				text = text.Replace("]", "");
			}
			string text2 = text.Trim();
			text = cjacp[1].pndmk();
			if (text.IndexOf("[mailto:") >= 0)
			{
				text = text.Replace("[mailto:", "");
				text = text.Replace("]", "");
			}
			string text3 = text;
			return brgjd.edcru("<a href=\"{0}\">{1}</a>", text2, text3);
		}
		string text4 = cjacp[0].angkg.iabtc();
		text4 = text4.Replace("\\* MERGEFORMAT \\d", "");
		string[] array;
		List<string> list;
		int num;
		if (text4.IndexOf("HYPERLINK") >= 0)
		{
			array = text4.Split(' ');
			list = new List<string>();
			num = 0;
			if (num != 0)
			{
				goto IL_013a;
			}
			goto IL_0165;
		}
		return cjacp.pndmk();
		IL_0165:
		if (num < array.Length)
		{
			goto IL_013a;
		}
		string text5 = string.Empty;
		string text6 = string.Empty;
		StringBuilder stringBuilder = new StringBuilder();
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_018c;
		}
		goto IL_0240;
		IL_0240:
		if (num2 >= list.Count - 1)
		{
			bool flag = text4.IndexOf("<img border=0 src=\"cid:embedded-rtf-attachment-id") >= 0 && text4.IndexOf("@rebex.net") >= 0;
			if (text4.IndexOf("INCLUDEPICTURE") >= 0)
			{
				return brgjd.edcru("<div><a href=\"{0}\"><img src=\"{1}\"></a></div>", text5, text6);
			}
			if (flag && 0 == 0 && list.Count == 5)
			{
				string text7 = list[4];
				return brgjd.edcru("<div><a href=\"{0}\"><img border=\"0\" {1}</a></div>", text5, text7);
			}
			return brgjd.edcru("<a href={0}>{1}</a>", text5, stringBuilder.ToString());
		}
		goto IL_018c;
		IL_023a:
		num2 += 2;
		goto IL_0240;
		IL_013a:
		if (array[num] != string.Empty && 0 == 0)
		{
			list.Add(array[num]);
		}
		num++;
		goto IL_0165;
		IL_018c:
		string text8;
		if ((text8 = list[num2]) == null)
		{
			goto IL_020f;
		}
		if (!(text8 == "HYPERLINK") || 1 == 0)
		{
			if (!(text8 == "INCLUDEPICTURE") || 1 == 0)
			{
				goto IL_020f;
			}
			text6 = list[num2 + 1].Replace("\"", "");
		}
		else
		{
			text5 = list[num2 + 1].Replace("\"", "");
		}
		goto IL_023a;
		IL_020f:
		stringBuilder.Append(list[num2] + " " + list[num2 + 1] + " ");
		goto IL_023a;
	}

	public static bool hxkbl(List<mmgqv> p0)
	{
		using (List<mmgqv>.Enumerator enumerator = p0.GetEnumerator())
		{
			while (enumerator.MoveNext() ? true : false)
			{
				mmgqv current = enumerator.Current;
				if ((current.jprco == ajuaj.icyep || 1 == 0) && eypon(current, "HYPERLINK") && 0 == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	private static bool eypon(mmgqv p0, string p1)
	{
		List<mmgqv> khhoe = p0.iyvve.khhoe;
		if (khhoe.Count <= 2)
		{
			return false;
		}
		if (khhoe[0].jprco == ajuaj.gdmuq && khhoe[0].iaqgi == "*" && 0 == 0 && khhoe[1].jprco == ajuaj.gdmuq && khhoe[1].iaqgi == "fldinst" && 0 == 0)
		{
			if (khhoe[2].jprco == ajuaj.mbsvn && khhoe[2].iaqgi.IndexOf("HYPERLINK") >= 0 && evsjd(khhoe[2].iaqgi) && 0 == 0)
			{
				return true;
			}
			if (khhoe[2].jprco == ajuaj.icyep || 1 == 0)
			{
				using List<mmgqv>.Enumerator enumerator = khhoe[2].iyvve.khhoe.GetEnumerator();
				while (enumerator.MoveNext() ? true : false)
				{
					mmgqv current = enumerator.Current;
					if (current.jprco == ajuaj.mbsvn && current.iaqgi.IndexOf("HYPERLINK") >= 0 && evsjd(current.iaqgi) && 0 == 0)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	private static bool evsjd(string p0)
	{
		string text = p0.Replace("\\n", "").Replace("\\r", "");
		text = text.Substring(text.IndexOf("HYPERLINK") + 9).Trim(' ', '"', '\'', '\t', '\r', '\n');
		if (text.IndexOf("mailto:", StringComparison.OrdinalIgnoreCase) >= 0)
		{
			return true;
		}
		try
		{
			if (string.IsNullOrEmpty(text) && 0 == 0)
			{
				return false;
			}
			new Uri(text);
			return true;
		}
		catch (UriFormatException)
		{
			return false;
		}
	}
}
