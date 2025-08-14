using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace onrkn;

internal static class brgjd
{
	[Flags]
	public enum hkmrp
	{
		aawxp = 0,
		igawx = 1
	}

	internal const int lfvnu = 2048;

	internal const int kiljx = 4096;

	private static readonly char[] kfwbc = new char[33]
	{
		'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
		'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
		'U', 'V', 'W', 'X', 'Y', 'Z', '2', '3', '4', '5',
		'6', '7', '='
	};

	private static readonly long[] aqkef = new long[8] { 1065151889408L, 33285996544L, 1040187392L, 32505856L, 1015808L, 31744L, 992L, 31L };

	private static int wyhid = 65;

	private static int uadlp = 24;

	private static char khgqh = '=';

	public static string thskn(this string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("value");
		}
		int num = p0.Length - 1;
		char[] array = new char[p0.Length];
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0030;
		}
		goto IL_0040;
		IL_0040:
		if (num2 <= num)
		{
			goto IL_0030;
		}
		return new string(array);
		IL_0030:
		array[num - num2] = p0[num2];
		num2++;
		goto IL_0040;
	}

	public static int hhssl(string p0, char p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("str", "String cannot be null.");
		}
		return p0.LastIndexOf(p1);
	}

	public static int pkosy(string p0, params char[] p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("str", "String cannot be null.");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("anyOf", "Value cannot be null.");
		}
		if (p0.Length == 0 || 1 == 0)
		{
			return -1;
		}
		return p0.LastIndexOfAny(p1);
	}

	public static int rsxyq(string p0, params char[] p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("str", "String cannot be null.");
		}
		return p0.IndexOfAny(p1);
	}

	public static bool nzvbl(this string p0, string p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("str", "String cannot be null.");
		}
		return p0.IndexOf(p1) >= 0;
	}

	public static bool aptsd(this string p0, string p1, StringComparison p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("str", "String cannot be null.");
		}
		return p0.IndexOf(p1, p2) >= 0;
	}

	public static bool qwnqu(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			return true;
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0012;
		}
		goto IL_002e;
		IL_002e:
		if (num < p0.Length)
		{
			goto IL_0012;
		}
		return true;
		IL_0012:
		if (!char.IsWhiteSpace(p0[num]) || 1 == 0)
		{
			return false;
		}
		num++;
		goto IL_002e;
	}

	public static string emrfc(this string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("value");
		}
		return p0.ToUpper(CultureInfo.InvariantCulture);
	}

	public static string hewdv(this string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("value");
		}
		return p0.ToLower(CultureInfo.InvariantCulture);
	}

	public static string eexca(string p0, params object[] p1)
	{
		if (p1 == null || false || p1.Length == 0 || 1 == 0)
		{
			return p0;
		}
		return string.Format(CultureInfo.InvariantCulture, p0, p1);
	}

	public static string yfllk(params string[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("values");
		}
		return string.Concat(p0);
	}

	public static string eaguk(params object[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("values");
		}
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		if (num != 0)
		{
			goto IL_0023;
		}
		goto IL_0056;
		IL_0056:
		if (num < p0.Length)
		{
			goto IL_0023;
		}
		return stringBuilder.ToString();
		IL_0023:
		object obj = p0[num];
		if (obj != null && 0 == 0)
		{
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}", new object[1] { obj });
		}
		num++;
		goto IL_0056;
	}

	public static string edcru(string p0, params object[] p1)
	{
		return string.Format(CultureInfo.InvariantCulture, p0, p1);
	}

	public static string mclrt(string p0, string p1, string p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("str", "String cannot be null.");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("oldValue", "Value cannot be null.");
		}
		if (p1.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("oldValue", "Value cannot be empty.");
		}
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		if (num != 0)
		{
			goto IL_0064;
		}
		goto IL_00b0;
		IL_00b0:
		if (num < p0.Length)
		{
			goto IL_0064;
		}
		return stringBuilder.ToString();
		IL_0064:
		if (erjom(p0, num, p1) && 0 == 0)
		{
			if (!string.IsNullOrEmpty(p2) || 1 == 0)
			{
				stringBuilder.arumx(p2);
			}
			num += p1.Length - 1;
		}
		else
		{
			stringBuilder.arumx(p0[num]);
		}
		num++;
		goto IL_00b0;
	}

	private static bool erjom(string p0, int p1, string p2)
	{
		if (p1 + p2.Length > p0.Length)
		{
			return false;
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0017;
		}
		goto IL_002f;
		IL_002f:
		if (num < p2.Length)
		{
			goto IL_0017;
		}
		return true;
		IL_0017:
		if (p0[p1 + num] != p2[num])
		{
			return false;
		}
		num++;
		goto IL_002f;
	}

	public static char ewjli(char p0)
	{
		return char.ToLower(p0, CultureInfo.InvariantCulture);
	}

	public static char rbxxu(char p0)
	{
		return char.ToUpper(p0, CultureInfo.InvariantCulture);
	}

	public static byte[] qaycu(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("data", "String cannot be null.");
		}
		if (p0.Length == 0 || 1 == 0)
		{
			return new byte[0];
		}
		if (p0.Length % 2 == 1)
		{
			throw new ArgumentException("String has invalid length.", "data");
		}
		byte[] array = new byte[p0.Length / 2];
		int num = 0;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0065;
		}
		goto IL_0084;
		IL_0084:
		if (num < array.Length)
		{
			goto IL_0065;
		}
		return array;
		IL_0065:
		array[num] = (byte)((ppuky(p0, num2) << 4) | ppuky(p0, num2 + 1));
		num++;
		num2 += 2;
		goto IL_0084;
	}

	private static int ppuky(string p0, int p1)
	{
		char c = p0[p1];
		if (c >= '0' && c <= '9')
		{
			return c - 48;
		}
		if (c >= 'a' && c <= 'f')
		{
			return c - 97 + 10;
		}
		if (c >= 'A' && c <= 'F')
		{
			return c - 65 + 10;
		}
		throw new FormatException(edcru("Character at position {0} is not a valid hexadecimal digit.", p1));
	}

	public static string wlvqq(byte[] p0)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		if (num != 0)
		{
			goto IL_000c;
		}
		goto IL_0023;
		IL_000c:
		stringBuilder.dlvlk("{0:x2}", p0[num]);
		num++;
		goto IL_0023;
		IL_0023:
		if (num < p0.Length)
		{
			goto IL_000c;
		}
		return stringBuilder.ToString();
	}

	public static bool bnrqx(string p0, out int p1)
	{
		return dahxy.feudq(int.Parse, p1: true, p0, out p1);
	}

	public static bool hujyn(string p0, out long p1)
	{
		return dahxy.feudq(long.Parse, p1: true, p0, out p1);
	}

	public static bool nbusd(string p0, out DateTime p1)
	{
		return dahxy.feudq(DateTime.Parse, p1: false, p0, out p1);
	}

	public static bool nyxjq<TEnum>(string p0, bool p1, out TEnum p2) where TEnum : struct
	{
		try
		{
			if (!string.IsNullOrEmpty(p0) || 1 == 0)
			{
				p2 = (TEnum)Enum.Parse(typeof(TEnum), p0, p1);
				return true;
			}
		}
		catch
		{
		}
		p2 = default(TEnum);
		return false;
	}

	public static TimeSpan weval(string p0)
	{
		string key;
		if ((key = p0.emrfc()) != null && 0 == 0)
		{
			if (fnfqw.gfilb == null || 1 == 0)
			{
				fnfqw.gfilb = new Dictionary<string, int>(14)
				{
					{ "Z", 0 },
					{ "UT", 1 },
					{ "UTC", 2 },
					{ "GMT", 3 },
					{ "AST", 4 },
					{ "ADT", 5 },
					{ "EST", 6 },
					{ "EDT", 7 },
					{ "CST", 8 },
					{ "CDT", 9 },
					{ "MST", 10 },
					{ "MDT", 11 },
					{ "PST", 12 },
					{ "PDT", 13 }
				};
			}
			if (fnfqw.gfilb.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
				case 1:
				case 2:
				case 3:
					return TimeSpan.Zero;
				case 4:
					return new TimeSpan(-4, 0, 0);
				case 5:
					return new TimeSpan(-3, 0, 0);
				case 6:
					return new TimeSpan(-5, 0, 0);
				case 7:
					return new TimeSpan(-4, 0, 0);
				case 8:
					return new TimeSpan(-6, 0, 0);
				case 9:
					return new TimeSpan(-5, 0, 0);
				case 10:
					return new TimeSpan(-7, 0, 0);
				case 11:
					return new TimeSpan(-6, 0, 0);
				case 12:
					return new TimeSpan(-8, 0, 0);
				case 13:
					return new TimeSpan(-7, 0, 0);
				}
			}
		}
		throw new pqotq(edcru("Unknown time zone ({0}).", p0));
	}

	public static bool wsiiz(string p0, out byte p1)
	{
		p1 = 0;
		if (p0 == null || false || p0.Length == 0 || false || p0.Length > 4)
		{
			return false;
		}
		bool flag = false;
		int num = 0;
		if (num != 0)
		{
			goto IL_0034;
		}
		goto IL_007e;
		IL_007a:
		num++;
		goto IL_007e;
		IL_0034:
		char c = p0[num];
		if ((c < '0' || c > '9') && (c < 'a' || c > 'f') && (c < 'A' || c > 'F'))
		{
			if (num == 1 && (c == 'x' || c == 'X') && p0[0] == '0')
			{
				flag = true;
				if (flag)
				{
					goto IL_007a;
				}
			}
			return false;
		}
		goto IL_007a;
		IL_007e:
		if (num >= p0.Length)
		{
			if (flag && 0 == 0)
			{
				if (p0.Length == 2)
				{
					return false;
				}
			}
			else if (p0.Length > 2)
			{
				return false;
			}
			p1 = Convert.ToByte(p0, 16);
			return true;
		}
		goto IL_0034;
	}

	public static bool zjirc(string p0, out int p1)
	{
		p1 = 0;
		if (p0 == null || false || p0.Length == 0 || false || p0.Length > 10)
		{
			return false;
		}
		bool flag = false;
		int num = 0;
		if (num != 0)
		{
			goto IL_0035;
		}
		goto IL_007f;
		IL_007b:
		num++;
		goto IL_007f;
		IL_0035:
		char c = p0[num];
		if ((c < '0' || c > '9') && (c < 'a' || c > 'f') && (c < 'A' || c > 'F'))
		{
			if (num == 1 && (c == 'x' || c == 'X') && p0[0] == '0')
			{
				flag = true;
				if (flag)
				{
					goto IL_007b;
				}
			}
			return false;
		}
		goto IL_007b;
		IL_007f:
		if (num >= p0.Length)
		{
			if (flag && 0 == 0)
			{
				if (p0.Length == 2)
				{
					return false;
				}
			}
			else if (p0.Length > 8)
			{
				return false;
			}
			try
			{
				p1 = Convert.ToInt32(p0, 16);
				return true;
			}
			catch
			{
				return false;
			}
		}
		goto IL_0035;
	}

	public static string cbpto(byte[] p0)
	{
		return jojfk(p0, hkmrp.aawxp);
	}

	public static string jojfk(byte[] p0, hkmrp p1)
	{
		if (p0 == null || 1 == 0)
		{
			if ((p1 & hkmrp.igawx) == 0 || 1 == 0)
			{
				throw new ArgumentNullException("data");
			}
			return "0";
		}
		StringBuilder stringBuilder = new StringBuilder();
		MemoryStream memoryStream = new MemoryStream(p0);
		while (true)
		{
			byte[] array = new byte[5];
			int num = memoryStream.Read(array, 0, array.Length);
			if (num == 0)
			{
				break;
			}
			if (num == array.Length)
			{
				xrcqz(array, stringBuilder);
				continue;
			}
			fgrpp(array, num, stringBuilder);
			break;
		}
		return stringBuilder.ToString();
	}

	private static void xrcqz(byte[] p0, StringBuilder p1)
	{
		long num = p0[0];
		int num2 = 1;
		if (num2 == 0)
		{
			goto IL_000b;
		}
		goto IL_001a;
		IL_000b:
		num <<= 8;
		num += p0[num2];
		num2++;
		goto IL_001a;
		IL_001a:
		if (num2 < 5)
		{
			goto IL_000b;
		}
		int num3 = 35;
		int num4 = 0;
		if (num4 != 0)
		{
			goto IL_0026;
		}
		goto IL_0053;
		IL_0053:
		if (num4 >= 8)
		{
			return;
		}
		goto IL_0026;
		IL_0026:
		int num5 = (int)((num & aqkef[num4]) >> num3);
		p1.Append(kfwbc[num5]);
		num3 -= 5;
		num4++;
		goto IL_0053;
	}

	private static void fgrpp(byte[] p0, int p1, StringBuilder p2)
	{
		long num = 0L;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_000c;
		}
		goto IL_0023;
		IL_000c:
		if (num2 > 0)
		{
			num <<= 8;
		}
		if (num2 < p1)
		{
			num += p0[num2];
		}
		num2++;
		goto IL_0023;
		IL_0023:
		if (num2 < 5)
		{
			goto IL_000c;
		}
		int num3 = p1 << 3;
		int num4 = 0;
		int num5 = 35;
		int num6 = 0;
		if (num6 != 0)
		{
			goto IL_0038;
		}
		goto IL_0081;
		IL_0038:
		if (num4 < num3)
		{
			int num7 = (int)((num & aqkef[num6]) >> num5);
			p2.Append(kfwbc[num7]);
			num4 += 5;
			num5 -= 5;
		}
		else
		{
			p2.Append(khgqh);
		}
		num6++;
		goto IL_0081;
		IL_0081:
		if (num6 >= 8)
		{
			return;
		}
		goto IL_0038;
	}

	public static byte[] rqtls(string p0)
	{
		return jmemt(p0, hkmrp.aawxp);
	}

	public static byte[] jmemt(string p0, hkmrp p1)
	{
		if (p0 == null || 1 == 0)
		{
			if ((p1 & hkmrp.igawx) == 0 || 1 == 0)
			{
				throw new ArgumentNullException("data");
			}
			return new byte[0];
		}
		if (p0.Length == 0 || 1 == 0)
		{
			return new byte[0];
		}
		if (p0.Length == 1)
		{
			if ((p1 & hkmrp.igawx) == 0 || 1 == 0)
			{
				throw new FormatException("Data are not base32 encoded.");
			}
			if (p0[0] == '0')
			{
				return null;
			}
		}
		if ((p0.Length & 7) > 0)
		{
			throw new FormatException("Data are not base32 encoded.");
		}
		List<byte> list = new List<byte>();
		int num = 0;
		if (num != 0)
		{
			goto IL_0093;
		}
		goto IL_00a5;
		IL_00a5:
		if (num < p0.Length)
		{
			goto IL_0093;
		}
		return list.ToArray();
		IL_0093:
		gdsnh(p0.Substring(num, 8), list);
		num += 8;
		goto IL_00a5;
	}

	private static void gdsnh(string p0, List<byte> p1)
	{
		long num = 0L;
		int num2;
		switch (p0.IndexOf(khgqh))
		{
		case -1:
			num2 = 5;
			if (num2 != 0)
			{
				break;
			}
			goto case 2;
		case 2:
			num2 = 1;
			if (num2 != 0)
			{
				break;
			}
			goto case 4;
		case 4:
			num2 = 2;
			if (num2 != 0)
			{
				break;
			}
			goto case 5;
		case 5:
			num2 = 3;
			if (num2 != 0)
			{
				break;
			}
			goto case 7;
		case 7:
			num2 = 4;
			if (num2 != 0)
			{
				break;
			}
			goto default;
		default:
			throw new FormatException("Padding does not match base32 encoding.");
		}
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_006e;
		}
		goto IL_00ca;
		IL_006e:
		if (num3 > 0)
		{
			num <<= 5;
		}
		if (char.IsLetter(p0[num3]) && 0 == 0)
		{
			num += p0[num3] - wyhid;
		}
		else if (char.IsDigit(p0[num3]) && 0 == 0)
		{
			num += p0[num3] - uadlp;
		}
		num3++;
		goto IL_00ca;
		IL_0107:
		int num4;
		if (num4 >= num2)
		{
			return;
		}
		goto IL_00de;
		IL_00de:
		int num5;
		p1.Add((byte)((num >> num5) & 0xFF));
		num5 -= 8;
		num4++;
		goto IL_0107;
		IL_00ca:
		if (num3 < p0.Length)
		{
			goto IL_006e;
		}
		num5 = 32;
		num4 = 0;
		if (num4 != 0)
		{
			goto IL_00de;
		}
		goto IL_0107;
	}

	public static bool mcaae(char p0)
	{
		if (p0 < 'a' || p0 > 'z')
		{
			if (p0 >= 'A')
			{
				return p0 <= 'Z';
			}
			return false;
		}
		return true;
	}

	public static bool kmfkd(char p0)
	{
		if (p0 >= '0')
		{
			return p0 <= '9';
		}
		return false;
	}

	public static bool yxjfm(char p0)
	{
		if (!mcaae(p0) || 1 == 0)
		{
			return kmfkd(p0);
		}
		return true;
	}

	public static bool iyiqi(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("text");
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_001b;
		}
		goto IL_0037;
		IL_0037:
		if (num < p0.Length)
		{
			goto IL_001b;
		}
		return true;
		IL_001b:
		if (!yxjfm(p0[num]) || 1 == 0)
		{
			return false;
		}
		num++;
		goto IL_0037;
	}

	public static bool ynufr(string p0, params char[] p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("text");
		}
		if (p1 == null || false || p1.Length == 0 || 1 == 0)
		{
			return iyiqi(p0);
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_003d;
		}
		goto IL_0069;
		IL_0069:
		if (num < p0.Length)
		{
			goto IL_003d;
		}
		return true;
		IL_003d:
		if ((!yxjfm(p0[num]) || 1 == 0) && Array.IndexOf(p1, p0[num]) < 0)
		{
			return false;
		}
		num++;
		goto IL_0069;
	}

	internal static string qfhzc(Stream p0, Encoding p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("input");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("encoding");
		}
		Decoder decoder = p1.GetDecoder();
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array = new byte[4096];
		char[] array2 = new char[p1.GetMaxCharCount(4096)];
		int byteCount;
		while ((byteCount = p0.Read(array, 0, array.Length)) > 0)
		{
			int chars = decoder.GetChars(array, 0, byteCount, array2, 0);
			stringBuilder.Append(array2, 0, chars);
		}
		return stringBuilder.ToString();
	}

	public static void jqfnk(string p0, Stream p1, Encoding p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("data");
		}
		if (p2 == null || 1 == 0)
		{
			throw new ArgumentNullException("encoding");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("output");
		}
		Encoder encoder = p2.GetEncoder();
		byte[] array = new byte[p2.GetMaxByteCount(2048)];
		char[] array2 = new char[2048];
		int num = 0;
		if (num != 0)
		{
			goto IL_006d;
		}
		goto IL_00b4;
		IL_0074:
		int num2;
		array2[num2] = p0[num];
		num++;
		num2++;
		goto IL_0089;
		IL_006d:
		num2 = 0;
		if (num2 != 0)
		{
			goto IL_0074;
		}
		goto IL_0089;
		IL_0089:
		int num3 = default(int);
		if (num2 < num3)
		{
			goto IL_0074;
		}
		int bytes = encoder.GetBytes(array2, 0, num3, array, 0, flush: false);
		p1.Write(array, 0, bytes);
		goto IL_00b4;
		IL_00b4:
		if ((num3 = Math.Min(2048, p0.Length - num)) <= 0)
		{
			return;
		}
		goto IL_006d;
	}
}
