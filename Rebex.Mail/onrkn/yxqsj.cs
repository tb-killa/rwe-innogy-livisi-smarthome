using System;
using System.Text;

namespace onrkn;

internal class yxqsj
{
	private enum wplhv
	{
		lcyng,
		fwhir
	}

	private string xzqoo;

	private int dajhl;

	private StringBuilder evkgb;

	private wplhv nftzm;

	private bool msjhi => dajhl >= xzqoo.Length;

	public yxqsj(string html)
	{
		xzqoo = html;
		evkgb = new StringBuilder();
	}

	public jwatd dzjsj()
	{
		if (msjhi && 0 == 0)
		{
			return new znhux();
		}
		evkgb.Length = 0;
		if (nftzm == wplhv.lcyng || 1 == 0)
		{
			esvdk('<');
			dajhl++;
			nftzm = wplhv.fwhir;
			return new xbknn(evkgb.ToString());
		}
		esvdk('>');
		string text = evkgb.ToString();
		bool flag = false;
		if (text.Length >= 5)
		{
			flag = text.StartsWith("!--", StringComparison.Ordinal);
			if (flag && 0 == 0)
			{
				bool flag2 = text.EndsWith("--", StringComparison.Ordinal);
				while ((!flag2 || 1 == 0) && !msjhi)
				{
					dajhl++;
					evkgb.Length = 0;
					esvdk('>');
					text = text + '>' + evkgb.ToString();
					flag2 = text.EndsWith("--", StringComparison.Ordinal);
				}
			}
		}
		dajhl++;
		nftzm = wplhv.lcyng;
		if (flag && 0 == 0)
		{
			return new jbhbq(text);
		}
		return new ljegp(text);
	}

	private void esvdk(char p0)
	{
		bool flag = false;
		if (flag)
		{
			goto IL_0009;
		}
		goto IL_006a;
		IL_0009:
		char c = xzqoo[dajhl];
		if (c == p0)
		{
			return;
		}
		dajhl++;
		if (char.IsWhiteSpace(c) && 0 == 0)
		{
			if (!flag || 1 == 0)
			{
				flag = true;
				evkgb.Append(' ');
			}
		}
		else
		{
			flag = false;
			evkgb.Append(c);
		}
		goto IL_006a;
		IL_006a:
		if (msjhi && 0 == 0)
		{
			return;
		}
		goto IL_0009;
	}
}
