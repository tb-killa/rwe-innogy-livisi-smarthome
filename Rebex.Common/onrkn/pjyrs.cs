using System;
using System.Collections.Generic;

namespace onrkn;

internal class pjyrs
{
	public const string accjk = "Accept-Encoding";

	public const string bmtzl = "Authorization";

	public const string wxoyi = "Basic";

	public const string bjphi = "Digest";

	public const string cisfb = "Connection";

	public const string kwkuw = "Content-Length";

	public const string mgadg = "Content-Type";

	public const string gwcgm = "Content-Encoding";

	public const string strsf = "Expect";

	public const string kijjl = "Host";

	public const string vrsto = "Location";

	public const string xsebj = "Transfer-Encoding";

	public const string ixjah = "User-Agent";

	public const string hjtmi = "Cookie";

	public const string qusze = "Set-Cookie";

	public const string tnfsu = "Set-Cookie2";

	public const string adubh = "Range";

	public const string rmijd = "close";

	public const string vhbwj = "chunked";

	public const string oqbcy = "keep-alive";

	public const string oufyd = "Last-Modified";

	internal static readonly Version fulyv = new Version(1, 0);

	internal static readonly Version dkkoe = new Version(1, 1);

	private readonly Dictionary<string, List<string>> rsdjn;

	private readonly List<string> auohq;

	public int uglaj => rsdjn.Count;

	public string this[string name]
	{
		get
		{
			if (rsdjn.TryGetValue(name, out var value) && 0 == 0)
			{
				return value[0];
			}
			return null;
		}
		set
		{
			if (value != null && 0 == 0)
			{
				List<string> list = new List<string>();
				list.Add(value);
				if (!rsdjn.ContainsKey(name) || 1 == 0)
				{
					auohq.Add(name);
					rsdjn.Add(name, list);
					lfngx(name, value);
				}
				else
				{
					rsdjn[name] = list;
					sohkq(name, value);
				}
			}
			else
			{
				rsdjn.Remove(name);
				auohq.Remove(name);
				bkqck(name);
			}
		}
	}

	public pjyrs()
	{
		rsdjn = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
		auohq = new List<string>();
	}

	public void dxkfp(Action<string, string> p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("action");
		}
		using List<string>.Enumerator enumerator = auohq.GetEnumerator();
		while (enumerator.MoveNext() ? true : false)
		{
			string current = enumerator.Current;
			if (!rsdjn.TryGetValue(current, out var _))
			{
				continue;
			}
			IEnumerator<string> enumerator2 = llppa(current).GetEnumerator();
			try
			{
				while (enumerator2.MoveNext() ? true : false)
				{
					string current2 = enumerator2.Current;
					p0(current, current2);
				}
			}
			finally
			{
				if (enumerator2 != null && 0 == 0)
				{
					enumerator2.Dispose();
				}
			}
		}
	}

	public IList<string> llppa(string p0)
	{
		if (!rsdjn.TryGetValue(p0, out var value) || 1 == 0)
		{
			return new string[0];
		}
		return value;
	}

	public void imhki(string p0, string p1)
	{
		if (p1 != null)
		{
			if (!rsdjn.TryGetValue(p0, out var value) || 1 == 0)
			{
				value = new List<string>();
				value.Add(p1);
				auohq.Add(p0);
				rsdjn.Add(p0, value);
			}
			else
			{
				value.Add(p1);
			}
			lfngx(p0, p1);
		}
	}

	public void uqbkm(string p0)
	{
		if (p0 != null)
		{
			string text = "Host";
			if (rsdjn.ContainsKey("Host") && 0 == 0)
			{
				this[text] = p0;
				sohkq(text, p0);
				return;
			}
			List<string> list = new List<string>();
			list.Add(p0);
			auohq.Insert(0, text);
			rsdjn.Add(text, list);
			lfngx(text, p0);
		}
	}

	public void lenjz()
	{
		auohq.Clear();
		rsdjn.Clear();
		qpfkw();
	}

	protected virtual void lfngx(string p0, string p1)
	{
	}

	protected virtual void sohkq(string p0, string p1)
	{
	}

	protected virtual void bkqck(string p0)
	{
	}

	protected virtual void qpfkw()
	{
	}

	public static bool lyxka(string p0, string p1, StringComparison p2, params char[] p3)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("str", "String cannot be null.");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("value", "String cannot be null.");
		}
		char[] array = p3;
		if (array == null || 1 == 0)
		{
			array = new char[0];
		}
		p3 = array;
		int num = 0;
		if (num != 0)
		{
			goto IL_0058;
		}
		goto IL_0121;
		IL_00c0:
		int num3;
		int num2 = num3 + p1.Length;
		if (num2 >= p0.Length || char.IsWhiteSpace(p0[num2]))
		{
			return true;
		}
		char[] array2 = p3;
		int num4 = 0;
		if (num4 != 0)
		{
			goto IL_00f7;
		}
		goto IL_0115;
		IL_00f7:
		char c = array2[num4];
		if (p0[num2] == c)
		{
			return true;
		}
		num4++;
		goto IL_0115;
		IL_011d:
		num = num3 + 1;
		goto IL_0121;
		IL_0115:
		if (num4 < array2.Length)
		{
			goto IL_00f7;
		}
		goto IL_011d;
		IL_0121:
		if (num < p0.Length)
		{
			goto IL_0058;
		}
		return false;
		IL_00b5:
		bool flag;
		if (flag && 0 == 0)
		{
			goto IL_00c0;
		}
		goto IL_011d;
		IL_00ad:
		int num5;
		char[] array3;
		if (num5 < array3.Length)
		{
			goto IL_0090;
		}
		goto IL_00b5;
		IL_0058:
		num3 = p0.IndexOf(p1, num, p2);
		if (num3 < 0)
		{
			return false;
		}
		if (num3 > 0)
		{
			flag = char.IsWhiteSpace(p0[num3 - 1]);
			if (!flag || 1 == 0)
			{
				array3 = p3;
				num5 = 0;
				if (num5 != 0)
				{
					goto IL_0090;
				}
				goto IL_00ad;
			}
			goto IL_00b5;
		}
		goto IL_00c0;
		IL_0090:
		char c2 = array3[num5];
		if (p0[num3 - 1] == c2)
		{
			flag = true;
			if (flag)
			{
				goto IL_00b5;
			}
		}
		num5++;
		goto IL_00ad;
	}

	public static fklrq motgl(string p0)
	{
		string text;
		if ((text = p0) != null && 0 == 0)
		{
			if (text == "Basic")
			{
				return fklrq.okxlf;
			}
			if (text == "Digest")
			{
				return fklrq.rafbm;
			}
			if (text == "Negotiate")
			{
				return fklrq.sjjmf;
			}
			if (text == "NTLM")
			{
				return fklrq.cepgj;
			}
			if (text == "Kerberos")
			{
				return fklrq.cznae;
			}
		}
		return fklrq.uzjbr;
	}
}
