using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using onrkn;

namespace Rebex;

public static class EncodingTools
{
	private static Encoding jnagn;

	private static Encoding wwyjl;

	private static Encoding tpwgl;

	private static Encoding odckb;

	private static readonly string[] njcia = new string[22]
	{
		"windows-1250", "windows-1251", "windows-1252", "windows-1253", "windows-1254", "windows-1255", "windows-1256", "windows-1257", "windows-1258", "iso-8859-1",
		"iso-8859-2", "iso-8859-3", "iso-8859-4", "iso-8859-5", "iso-8859-6", "iso-8859-7", "iso-8859-8", "iso-8859-9", "iso-8859-11", "iso-8859-13",
		"iso-8859-15", "IBM437"
	};

	private static readonly string[] vnbey = new string[22]
	{
		"Central European", "Cyrillic", "Western European", "Greek", "Turkish", "Hebrew", "Arabic", "Baltic", "Vietnamese", "Western European",
		"Central European", "South European", "North European", "Cyrillic", "Arabic", "Greek", "Hebrew", "Turkish", "Thai", "Estonian",
		"Latin-9", "OEM 437"
	};

	private static readonly int[] ipcyp = new int[22]
	{
		1250, 1251, 1252, 1253, 1254, 1255, 1256, 1257, 1258, 28591,
		28592, 28593, 28594, 28595, 28596, 28597, 28598, 28599, 874, 28603,
		28605, 437
	};

	private static byte[] nfopg;

	public static Encoding ASCII
	{
		get
		{
			Encoding encoding = jnagn;
			if (encoding == null || 1 == 0)
			{
				encoding = (jnagn = GetEncoding("us-ascii"));
			}
			return encoding;
		}
	}

	public static Encoding Default
	{
		get
		{
			Encoding encoding = wwyjl;
			if (encoding == null || 1 == 0)
			{
				encoding = Encoding.Default;
				if (string.Equals(encoding.WebName, "utf-8", StringComparison.OrdinalIgnoreCase) && 0 == 0)
				{
					encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
				}
				wwyjl = encoding;
			}
			return encoding;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			wwyjl = value;
		}
	}

	internal static Encoding dmppd
	{
		get
		{
			if (tpwgl == null || 1 == 0)
			{
				tpwgl = new qrrje("eightbit", "8-bit", 0, 255);
			}
			return tpwgl;
		}
	}

	public static Encoding UTF8
	{
		get
		{
			if (odckb == null || 1 == 0)
			{
				odckb = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
			}
			return odckb;
		}
	}

	private static byte[] dqvqw
	{
		get
		{
			byte[] array;
			int num;
			if (nfopg == null || 1 == 0)
			{
				array = new byte[256];
				num = 0;
				if (num != 0)
				{
					goto IL_001f;
				}
				goto IL_0028;
			}
			goto IL_003c;
			IL_003c:
			return nfopg;
			IL_0028:
			if (num < 256)
			{
				goto IL_001f;
			}
			nfopg = array;
			goto IL_003c;
			IL_001f:
			array[num] = (byte)num;
			num++;
			goto IL_0028;
		}
	}

	public static string[] GetEncodingNames()
	{
		List<string> list = new List<string>();
		list.AddRange(njcia);
		return list.ToArray();
	}

	internal static Encoding vtjfm(string p0)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0009;
		}
		goto IL_004a;
		IL_0009:
		if (string.Equals(p0, njcia[num], StringComparison.OrdinalIgnoreCase) && 0 == 0)
		{
			p0 = njcia[num];
			string title = vnbey[num];
			int codePage = ipcyp[num];
			return new tdgni(num, p0, title, codePage);
		}
		num++;
		goto IL_004a;
		IL_004a:
		if (num >= njcia.Length)
		{
			if (string.Equals(p0, "eightbit", StringComparison.OrdinalIgnoreCase) && 0 == 0)
			{
				return new qrrje("eightbit", "8-bit", 0, 255);
			}
			if ((string.Equals(p0, "us-ascii", StringComparison.OrdinalIgnoreCase) ? true : false) || string.Equals(p0, "ascii", StringComparison.OrdinalIgnoreCase))
			{
				return new qrrje("us-ascii", "ASCII", 20127, 127);
			}
			return null;
		}
		goto IL_0009;
	}

	public static Encoding GetEncoding(string name)
	{
		try
		{
			return Encoding.GetEncoding(name);
		}
		catch
		{
		}
		Encoding encoding = vtjfm(name);
		if (encoding == null || 1 == 0)
		{
			throw new ArgumentException("Encoding '" + name + "' is not supported.", "name");
		}
		return encoding;
	}

	private static Encoding ozamc(int p0)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_0039;
		IL_0006:
		if (ipcyp[num] == p0)
		{
			string name = njcia[num];
			string title = vnbey[num];
			p0 = ipcyp[num];
			return new tdgni(num, name, title, p0);
		}
		num++;
		goto IL_0039;
		IL_0039:
		if (num >= ipcyp.Length)
		{
			if (p0 == 20127)
			{
				return new qrrje("us-ascii", "ASCII", p0, 127);
			}
			return null;
		}
		goto IL_0006;
	}

	public static Encoding GetEncoding(int codepage)
	{
		try
		{
			return Encoding.GetEncoding(codepage);
		}
		catch
		{
		}
		Encoding encoding = ozamc(codepage);
		if (encoding == null || 1 == 0)
		{
			throw new NotSupportedException("Encoding codepage '" + codepage + "' is not supported.");
		}
		return encoding;
	}

	internal static string mohvk(this Encoding p0, byte[] p1)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("bytes");
		}
		return p0.GetString(p1, 0, p1.Length);
	}

	internal static bool dbhus(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("value");
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_001b;
		}
		goto IL_003a;
		IL_003a:
		if (num < p0.Length)
		{
			goto IL_001b;
		}
		return true;
		IL_001b:
		if (p0[num] < ' ' || p0[num] >= '\u0080')
		{
			return false;
		}
		num++;
		goto IL_003a;
	}

	internal static string juvnv(Encoding p0, byte[] p1)
	{
		return yyyrx(p0, p1, 0, p1.Length);
	}

	internal static string yyyrx(Encoding p0, byte[] p1, int p2, int p3)
	{
		char[] chars = p0.GetChars(p1, p2, p3);
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		if (num != 0)
		{
			goto IL_0019;
		}
		goto IL_0074;
		IL_0019:
		char c = chars[num];
		if (c == '\r')
		{
			stringBuilder.Append("<CR>");
		}
		else if (c == '\n')
		{
			stringBuilder.Append("<LF>");
		}
		else if (c < ' ' || (c >= '\u007f' && c < '\u00a0'))
		{
			stringBuilder.dlvlk("<{0:X2}>", (int)c);
		}
		else
		{
			stringBuilder.Append(c);
		}
		num++;
		goto IL_0074;
		IL_0074:
		if (num < chars.Length)
		{
			goto IL_0019;
		}
		return stringBuilder.ToString();
	}

	internal static bool nzaah(byte[] p0)
	{
		int num = p0.Length;
		int num2 = 0;
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_0012;
		}
		goto IL_0084;
		IL_0012:
		byte b = p0[num3];
		if (num2 == 0 || 1 == 0)
		{
			if (b < 127)
			{
				num2 = 1;
				if (num2 != 0)
				{
					goto IL_007c;
				}
			}
			if (b >= 194 && b <= 223)
			{
				num2 = 2;
				if (num2 != 0)
				{
					goto IL_007c;
				}
			}
			if (b >= 224 && b <= 239)
			{
				num2 = 3;
				if (num2 != 0)
				{
					goto IL_007c;
				}
			}
			if (b >= 240 && b <= 244)
			{
				num2 = 4;
				if (num2 != 0)
				{
					goto IL_007c;
				}
			}
			return false;
		}
		if ((b & 0xC0) != 128)
		{
			return false;
		}
		goto IL_007c;
		IL_007c:
		num2--;
		num3++;
		goto IL_0084;
		IL_0084:
		if (num3 >= num)
		{
			if (num2 != 0 && 0 == 0)
			{
				return false;
			}
			return true;
		}
		goto IL_0012;
	}

	internal static bool azzem(byte[] p0)
	{
		int num = p0.Length;
		if (num >= 2 && (p0[num - 2] == 0 || 1 == 0))
		{
			return p0[num - 1] == 0;
		}
		return false;
	}

	internal static bool bzzgu(byte[] p0, out Encoding p1)
	{
		int num = 0;
		int num2 = Math.Min(p0.Length, 1024);
		p1 = null;
		if (num2 <= 0)
		{
			return false;
		}
		int num3 = num2 / 2;
		int num4 = 0;
		int num5 = 0;
		bool flag = true;
		int num6 = num;
		while (num6 < num2)
		{
			if (p0[num6] == 0 || 1 == 0)
			{
				if (flag && 0 == 0)
				{
					num4++;
				}
				else
				{
					num5++;
				}
			}
			num6++;
			flag = !flag;
		}
		double num7 = num4 / num3;
		double num8 = num5 / num3;
		if (num7 < 0.05 && num8 < 0.05)
		{
			return false;
		}
		if (num7 > num8)
		{
			p1 = Encoding.BigEndianUnicode;
		}
		else
		{
			p1 = Encoding.Unicode;
		}
		return true;
	}

	internal static bool jgvsv(Encoding p0)
	{
		if (yuhur(p0) && 0 == 0)
		{
			return false;
		}
		string text = p0.WebName.ToLower(CultureInfo.InvariantCulture);
		string text2;
		if ((text2 = text) != null && 0 == 0 && (text2 == "utf-8" || text2 == "utf-7" || text2 == "shift_jis" || text2 == "big5"))
		{
			return false;
		}
		return true;
	}

	internal static bool qoyra(Encoding p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("encoding");
		}
		return p0.WebName.ToLower(CultureInfo.InvariantCulture) == "utf-8";
	}

	internal static bool yuhur(Encoding p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("encoding");
		}
		return gepeu.qdrhv(p0);
	}

	internal static faifm ncjak(Encoding p0, char[] p1)
	{
		if (yuhur(p0) && 0 == 0)
		{
			p0.GetChars(dqvqw, 0, 256, p1, 0);
			return faifm.warpv;
		}
		if (p0.WebName == Encoding.UTF8.WebName && 0 == 0)
		{
			return faifm.sxrrs;
		}
		return faifm.qtkwo;
	}
}
