using System;
using System.Text;

namespace onrkn;

internal static class dmwgl
{
	public static bdjih hkocc(this bdjih p0, bdjih p1)
	{
		if (p0 == 0 && 0 == 0)
		{
			return 0;
		}
		if (p1 == 0 && 0 == 0)
		{
			return 1;
		}
		bdjih result = 1;
		while (true)
		{
			result *= p0;
			if (p1 == 1 && 0 == 0)
			{
				break;
			}
			p1 -= (bdjih)1;
		}
		return result;
	}

	public static bool jixdr(this bdjih p0)
	{
		return !p0.acgax;
	}

	public static void bdrka(this bdjih p0, byte[] p1)
	{
		p0.csvpb(p1, 0);
	}

	public static void csvpb(this bdjih p0, byte[] p1, int p2)
	{
		byte[] array = p0.kskce(p0: false);
		int num = array.Length - 1;
		int num2 = p2;
		while (num >= 0)
		{
			p1[num2++] = array[num--];
		}
	}

	public static bdjih tfydq(string p0)
	{
		bdjih bdjih2 = 0;
		int num = 0;
		if (num != 0)
		{
			goto IL_000f;
		}
		goto IL_004c;
		IL_000f:
		char c = p0[num];
		if (c < '0' || c > '9')
		{
			throw new FormatException("Not a base10 number.");
		}
		bdjih2 = bdjih2 * 10 + (c - 48);
		num++;
		goto IL_004c;
		IL_004c:
		if (num < p0.Length)
		{
			goto IL_000f;
		}
		return bdjih2;
	}

	public static string mympd(this bdjih p0, string p1)
	{
		string text;
		if ((text = p1) != null && 0 == 0)
		{
			if (text == "x")
			{
				return p0.ToString().ToLower();
			}
			if (text == "X")
			{
				return p0.ToString().ToUpper();
			}
			if (text == "d" || text == "D")
			{
				return p0.qkaao();
			}
		}
		return p0.ToString();
	}

	private static string qkaao(this bdjih p0)
	{
		if (p0 == 0 && 0 == 0)
		{
			return "0";
		}
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = p0 < 0;
		if (flag && 0 == 0)
		{
			p0 = p0.yleym();
		}
		while ((p0 > 0) ? true : false)
		{
			bdjih.szzlb(p0, 10, out p0, out var p1);
			stringBuilder.Insert(0, p1.ToString());
		}
		if (flag && 0 == 0)
		{
			stringBuilder.Insert(0, "-");
		}
		return stringBuilder.ToString();
	}
}
