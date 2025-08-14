using System.Text;

namespace onrkn;

internal class wbtcx
{
	public static string uwarv(string p0)
	{
		return mrlfp("text", inqsk(p0));
	}

	public static string vfzjk(string p0, string p1)
	{
		if ((string.IsNullOrEmpty(p1) ? true : false) || p1.Trim() == "")
		{
			p1 = xaexg.uleoi(p0);
		}
		p1 = inqsk(p1);
		p0 = inqsk(p0);
		p0 = eztco(p0);
		return mrlfp("html1", brgjd.edcru("{0}\\htmlrtf {1}\\htmlrtf0", p0, p1));
	}

	private static string mrlfp(string p0, string p1)
	{
		string text = "{\\rtf1\\ansi\\ansicpg#CODE_PAGE#\\from#FROM_FORMAT# \\fbidis \\deff0{\\fonttbl\r\n{\\f0\\fswiss Arial;}\r\n{\\f1\\fmodern Courier New;}\r\n{\\f2\\fnil\\fcharset2 Symbol;}\r\n{\\f3\\fmodern\\fcharset0 Courier New;}}\r\n{\\colortbl\\red0\\green0\\blue0;\\red0\\green0\\blue255;}\r\n\\uc1\\pard\\plain\\deftab360 \\f0\\fs24 \r\n#TEXT_TO_REPLACE#\r\n}";
		return text.Replace("\r\n", "\n").Replace("\n", "\r\n").Replace("#CODE_PAGE#", "1252")
			.Replace("#FROM_FORMAT#", p0)
			.Replace("#TEXT_TO_REPLACE#", p1);
	}

	private static string eztco(string p0)
	{
		return brgjd.edcru("{0}\\*\\htmltag64 {1}{2}", '{', p0, '}');
	}

	private static string inqsk(string p0)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		if (num != 0)
		{
			goto IL_0012;
		}
		goto IL_0143;
		IL_0012:
		switch (p0[num])
		{
		case '\t':
			stringBuilder.Append("\\tab ");
			break;
		case '\r':
			if (num + 1 < p0.Length && p0[num + 1] == '\n')
			{
				num++;
			}
			stringBuilder.Append("\\par\r\n");
			break;
		case '\n':
			if (num + 1 < p0.Length && p0[num + 1] == '\r')
			{
				num++;
			}
			stringBuilder.Append("\\par\r\n");
			break;
		case '\\':
		case '{':
		case '}':
			stringBuilder.Append('\\');
			stringBuilder.Append(p0[num]);
			break;
		default:
			if (p0[num] < '\u0080')
			{
				stringBuilder.Append(p0[num]);
			}
			else if (p0[num] < '耀')
			{
				stringBuilder.dlvlk("\\u{0}?", (int)p0[num]);
			}
			else
			{
				stringBuilder.dlvlk("\\u{0}?", p0[num] - 65536);
			}
			break;
		}
		num++;
		goto IL_0143;
		IL_0143:
		if (num < p0.Length)
		{
			goto IL_0012;
		}
		return stringBuilder.ToString();
	}
}
