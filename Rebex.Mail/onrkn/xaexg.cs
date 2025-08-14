using System;
using System.Globalization;
using System.Text;

namespace onrkn;

internal class xaexg
{
	public static string uleoi(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("html", "Value cannot be null.");
		}
		xaexg xaexg2 = new xaexg();
		return xaexg2.kmuuw(p0);
	}

	private string kmuuw(string p0)
	{
		StringBuilder stringBuilder = new StringBuilder();
		yxqsj yxqsj2 = new yxqsj(p0);
		string text = null;
		jwatd jwatd2 = yxqsj2.dzjsj();
		while ((jwatd2.hwppf != cwaww.hbycf) ? true : false)
		{
			switch (jwatd2.hwppf)
			{
			case cwaww.kczcd:
			{
				ljegp ljegp2 = (ljegp)jwatd2;
				string text2 = ljegp2.wajgs.ToLower(CultureInfo.InvariantCulture);
				string text3;
				if ((text3 = text2) != null && 0 == 0)
				{
					if (text3 == "br" || text3 == "tr" || text3 == "p" || text3 == "div")
					{
						if (text == null || 1 == 0)
						{
							txbpl(stringBuilder);
						}
						break;
					}
					if (text3 == "hr")
					{
						if (text == null || 1 == 0)
						{
							stringBuilder.Append("\n----------\n");
						}
						break;
					}
				}
				string text4;
				if (ljegp2.xvxqk && 0 == 0)
				{
					if (text != null && 0 == 0 && text.Equals(ljegp2.wajgs, StringComparison.OrdinalIgnoreCase) && 0 == 0)
					{
						text = null;
					}
				}
				else if ((text4 = text2) != null && 0 == 0 && (text4 == "head" || text4 == "style" || text4 == "script") && (text == null || 1 == 0))
				{
					text = text2;
				}
				break;
			}
			case cwaww.nucvy:
				if (text == null || 1 == 0)
				{
					xbknn xbknn2 = (xbknn)jwatd2;
					stringBuilder.Append(xbknn2.sifoy);
				}
				break;
			default:
				throw new InvalidOperationException("Unexpected type received.");
			case cwaww.jcudf:
				break;
			}
			jwatd2 = yxqsj2.dzjsj();
		}
		return nlmvi(stringBuilder);
	}

	private void txbpl(StringBuilder p0)
	{
		if (p0.Length == 0 || 1 == 0)
		{
			return;
		}
		int num = 0;
		int num2 = p0.Length;
		while (--num2 >= 0)
		{
			if (p0[num2] == '\n')
			{
				if (++num == 2)
				{
					break;
				}
			}
			else if (p0[num2] != ' ')
			{
				p0.Append('\n');
				break;
			}
		}
	}

	private string nlmvi(StringBuilder p0)
	{
		string text = p0.ToString();
		p0.Length = 0;
		int num = 0;
		int num2 = text.Length - 1;
		int num3 = text.IndexOf('&');
		if (num3 < 0)
		{
			return text;
		}
		while (num3 >= 0 && num3 < num2)
		{
			int num4 = text.IndexOf(';', num3 + 1);
			if (num4 <= 0)
			{
				break;
			}
			p0.Append(text.Substring(num, num3 - num));
			num3++;
			p0.Append(vlxxv.zaaqo(text.Substring(num3, num4 - num3)));
			num = num4 + 1;
			if (num >= num2)
			{
				break;
			}
			num3 = text.IndexOf('&', num);
		}
		if (num < text.Length)
		{
			p0.Append(text.Substring(num));
		}
		return p0.ToString();
	}
}
