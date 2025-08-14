using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace onrkn;

internal static class dtjod
{
	private sealed class guogx
	{
		public Stream befnt;

		public Encoding npklt;

		public void vkuts(char p0)
		{
			befnt.WriteByte((byte)p0);
		}

		public void jdrfl(string p0)
		{
			byte[] array = cfeey(p0, npklt);
			befnt.Write(array, 0, array.Length);
		}
	}

	private sealed class mrhcu
	{
		public long xdlan;

		public Encoding ttfig;

		public void pztcq(char p0)
		{
			xdlan++;
		}

		public void vdgit(string p0)
		{
			xdlan += cfeey(p0, ttfig).Length;
		}
	}

	private static readonly string[] wqqrf = new string[12]
	{
		"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct",
		"Nov", "Dec"
	};

	public static bool rvufw(string p0, out DateTime p1)
	{
		p1 = default(DateTime);
		object obj = p0;
		if (obj == null || 1 == 0)
		{
			obj = "";
		}
		p0 = ((string)obj).Trim();
		if (p0.Length == 0 || 1 == 0)
		{
			return false;
		}
		qqxey qqxey2 = new qqxey(p0);
		bool flag = false;
		string text = null;
		int num = 0;
		TimeSpan value = TimeSpan.Zero;
		try
		{
			if (!brgjd.kmfkd(qqxey2.eelpd) || 1 == 0)
			{
				string text2 = qqxey2.odgye(p0: true);
				if (text2[text2.Length - 1] != ',')
				{
					flag = true;
					text = ((!brgjd.kmfkd(qqxey2.eelpd)) ? qqxey2.odgye(p0: true) : text2);
				}
				if (!brgjd.kmfkd(qqxey2.eelpd) || 1 == 0)
				{
					return false;
				}
			}
			int num2 = qqxey2.vuszx(2);
			qqxey2.buoqv('-');
			if (!flag || 1 == 0)
			{
				text = qqxey2.chaht(3);
				qqxey2.buoqv('-');
				num = qqxey2.ivcgj(70);
				qqxey2.oqtdm();
			}
			int num3 = qqxey2.vwuje(2);
			qqxey2.cycsv(':');
			int num4 = qqxey2.vwuje(2);
			qqxey2.cycsv(':');
			int num5 = qqxey2.vwuje(2);
			qqxey2.oqtdm();
			if (num3 > 23 || num4 > 59 || num5 > 59)
			{
				return false;
			}
			if (flag && 0 == 0)
			{
				num = qqxey2.ivcgj(70);
				qqxey2.oqtdm();
			}
			int num6 = 0;
			int num7 = 0;
			if (num7 != 0)
			{
				goto IL_0183;
			}
			goto IL_01aa;
			IL_0183:
			if (text.Equals(wqqrf[num7], StringComparison.OrdinalIgnoreCase))
			{
				num6 = num7 + 1;
				goto IL_01b5;
			}
			num7++;
			goto IL_01aa;
			IL_01aa:
			if (num7 < wqqrf.Length)
			{
				goto IL_0183;
			}
			goto IL_01b5;
			IL_01b5:
			if (num < 1 || num6 < 1 || num2 < 1 || num2 > 31)
			{
				return false;
			}
			if ((!flag || 1 == 0) && (!qqxey2.tkduk || 1 == 0))
			{
				value = qqxey2.jykum();
			}
			p1 = new DateTime(num, num6, num2, num3, num4, num5, DateTimeKind.Utc).Subtract(value);
			return true;
		}
		catch (pqotq)
		{
		}
		catch (FormatException)
		{
		}
		catch (OverflowException)
		{
		}
		catch (ArgumentException)
		{
		}
		return false;
	}

	public static void kbrxv(NameValueCollection p0, Stream p1, Encoding p2)
	{
		guogx guogx = new guogx();
		guogx.befnt = p1;
		guogx.npklt = p2;
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("values");
		}
		if (guogx.befnt == null || 1 == 0)
		{
			throw new ArgumentNullException("stream");
		}
		if (!guogx.befnt.CanWrite || 1 == 0)
		{
			throw new ArgumentException("Stream is not writeable.");
		}
		if ((p0.Count != 0) ? true : false)
		{
			ixjxt(p0, guogx.vkuts, guogx.jdrfl);
		}
	}

	public static long kkxgc(NameValueCollection p0, Encoding p1)
	{
		mrhcu mrhcu = new mrhcu();
		mrhcu.ttfig = p1;
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("values");
		}
		if (p0.Count == 0 || 1 == 0)
		{
			return 0L;
		}
		mrhcu.xdlan = 0L;
		ixjxt(p0, mrhcu.pztcq, mrhcu.vdgit);
		return mrhcu.xdlan;
	}

	private static void ixjxt(NameValueCollection p0, Action<char> p1, Action<string> p2)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_003d;
		IL_0006:
		if (num > 0)
		{
			p1('&');
		}
		p2(p0.GetKey(num));
		p1('=');
		p2(p0[num]);
		num++;
		goto IL_003d;
		IL_003d:
		if (num >= p0.Count)
		{
			return;
		}
		goto IL_0006;
	}

	public static byte[] cfeey(string p0, Encoding p1)
	{
		if (p0 == null || 1 == 0)
		{
			return null;
		}
		byte[] bytes = p1.GetBytes(p0);
		return dhhdy(bytes, 0, bytes.Length);
	}

	private static byte[] dhhdy(byte[] p0, int p1, int p2)
	{
		if (p2 == 0 || 1 == 0)
		{
			return p0;
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_0019;
		}
		goto IL_0042;
		IL_0077:
		int num4;
		byte b = p0[p1 + num4];
		char c = (char)b;
		byte[] array;
		int num5;
		if (gozeo(c) && 0 == 0)
		{
			array[num5++] = b;
		}
		else if (c == ' ')
		{
			array[num5++] = 43;
		}
		else
		{
			array[num5++] = 37;
			array[num5++] = (byte)wpgdx((b >> 4) & 0xF);
			array[num5++] = (byte)wpgdx(b & 0xF);
		}
		num4++;
		goto IL_00f4;
		IL_00f4:
		if (num4 < p2)
		{
			goto IL_0077;
		}
		return array;
		IL_0019:
		char c2 = (char)p0[p1 + num3];
		if (c2 == ' ')
		{
			num++;
		}
		else if (!gozeo(c2) || 1 == 0)
		{
			num2++;
		}
		num3++;
		goto IL_0042;
		IL_0042:
		if (num3 < p2)
		{
			goto IL_0019;
		}
		if ((num == 0 || 1 == 0) && (num2 == 0 || 1 == 0))
		{
			return p0;
		}
		array = new byte[p2 + 2 * num2];
		num5 = 0;
		num4 = 0;
		if (num4 != 0)
		{
			goto IL_0077;
		}
		goto IL_00f4;
	}

	private static char wpgdx(int p0)
	{
		if (p0 <= 9)
		{
			return (char)(p0 + 48);
		}
		return (char)(p0 - 10 + 97);
	}

	private static bool gozeo(char p0)
	{
		if ((p0 >= 'a' && p0 <= 'z') || (p0 >= 'A' && p0 <= 'Z') || (p0 >= '0' && p0 <= '9'))
		{
			return true;
		}
		switch (p0)
		{
		case '!':
		case '\'':
		case '(':
		case ')':
		case '*':
		case '-':
		case '.':
		case '_':
			return true;
		default:
			return false;
		}
	}
}
