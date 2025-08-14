using System;
using System.Globalization;
using System.IO;
using System.Text;
using Rebex;
using Rebex.Mime;
using Rebex.Security.Cryptography;

namespace onrkn;

internal sealed class kgbvh
{
	internal class oemou
	{
		private readonly StringBuilder jptvl = new StringBuilder();

		private readonly MemoryStream ynolj = new MemoryStream();

		private Encoding lkeye;

		public void jdpgz(string p0, int p1, int p2)
		{
			clkna();
			jptvl.Append(p0, p1, p2);
		}

		public void qftve(Encoding p0, byte[] p1, int p2, int p3)
		{
			if (lkeye != null && 0 == 0 && (!lkeye.WebName.Equals(p0.WebName, StringComparison.OrdinalIgnoreCase) || 1 == 0))
			{
				clkna();
			}
			lkeye = p0;
			ynolj.Write(p1, p2, p3);
		}

		public void clkna()
		{
			if (lkeye != null && 0 == 0)
			{
				string value = lkeye.GetString(ynolj.GetBuffer(), 0, (int)ynolj.Length);
				jptvl.Append(value);
				lkeye = null;
				ynolj.SetLength(0L);
			}
		}

		public override string ToString()
		{
			return jptvl.ToString();
		}
	}

	private static readonly string[] vpajc = new string[18]
	{
		"1", "2", "windows-1252", "windows-1250", "3", "4", "5", "windows-1251", "6", "7",
		"windows-1255", "9", "10", "11", "13", "14", "15", "16"
	};

	private static char[] zzrne;

	private kgbvh()
	{
	}

	public static bool zhxzu(char p0)
	{
		if (p0 != '\t')
		{
			return p0 == ' ';
		}
		return true;
	}

	public static bool ynmbt(char p0)
	{
		if (p0 <= ' ' || p0 == '\u007f')
		{
			return false;
		}
		return "\"(),.:;<>@[\\]".IndexOf(p0) == -1;
	}

	public static bool ayydf(char p0)
	{
		if (p0 == '"')
		{
			return true;
		}
		return ynmbt(p0);
	}

	public static bool uyowq(char p0)
	{
		string text = "()<>@,;:\\\"/[]?=";
		if (text.IndexOf(p0) >= 0)
		{
			return false;
		}
		if (p0 > ' ')
		{
			return p0 < '\u007f';
		}
		return false;
	}

	public static string rqlyl(string p0, char p1, char p2)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(p1);
		int num = 0;
		if (num != 0)
		{
			goto IL_0014;
		}
		goto IL_0044;
		IL_0014:
		char c = p0[num];
		if (c == '\\' || c == p1 || c == p2)
		{
			stringBuilder.Append('\\');
		}
		stringBuilder.Append(p0[num]);
		num++;
		goto IL_0044;
		IL_0044:
		if (num >= p0.Length)
		{
			stringBuilder.Append(p2);
			return stringBuilder.ToString();
		}
		goto IL_0014;
	}

	public static int csiln(string p0, int p1, out string p2)
	{
		if (p1 >= p0.Length || p0[p1] != '"')
		{
			p2 = null;
			return -1;
		}
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		int num = p1 + 1;
		while (num < p0.Length && ((flag ? true : false) || p0[num] != '"'))
		{
			if (flag && 0 == 0)
			{
				flag = false;
				if (!flag)
				{
					goto IL_0047;
				}
			}
			if (p0[num] == '\\')
			{
				flag = true;
			}
			goto IL_0047;
			IL_0047:
			if (!flag || 1 == 0)
			{
				stringBuilder.Append(p0[num]);
			}
			num++;
		}
		p2 = stringBuilder.ToString();
		if (num == p0.Length)
		{
			return num;
		}
		return num + 1;
	}

	public static int bvuap(int p0)
	{
		if (p0 >= 48 && p0 <= 57)
		{
			return p0 - 48;
		}
		if (p0 >= 65 && p0 <= 70)
		{
			return 10 + p0 - 65;
		}
		if (p0 >= 97 && p0 <= 102)
		{
			return 10 + p0 - 97;
		}
		return -1;
	}

	public static string ttsbq(string p0)
	{
		oemou oemou = new oemou();
		int num = 0;
		while (num < p0.Length)
		{
			int num2 = p0.IndexOf("=?", num);
			if (num2 == -1)
			{
				break;
			}
			string text = p0.Substring(num, num2 - num);
			bool flag = true;
			int num3 = 0;
			if (num3 != 0)
			{
				goto IL_0041;
			}
			goto IL_0066;
			IL_0041:
			if (text[num3] != ' ' && text[num3] != '\t')
			{
				flag = false;
				if (!flag)
				{
					goto IL_0070;
				}
			}
			num3++;
			goto IL_0066;
			IL_0070:
			if (!flag || 1 == 0)
			{
				oemou.jdpgz(text, 0, text.Length);
			}
			num = wjqyy(p0, num2, oemou);
			if (num < 0)
			{
				oemou.jdpgz("=?", 0, 2);
				num = num2 + 2;
			}
			continue;
			IL_0066:
			if (num3 < text.Length)
			{
				goto IL_0041;
			}
			goto IL_0070;
		}
		if (num < p0.Length)
		{
			oemou.jdpgz(p0, num, p0.Length - num);
		}
		oemou.clkna();
		return oemou.ToString();
	}

	private static int wjqyy(string p0, int p1, oemou p2)
	{
		int num = p1 + 2;
		int num2 = p0.IndexOf('?', num);
		if (num2 <= num || num2 == p0.Length - 1)
		{
			return -1;
		}
		num2++;
		int num3 = p0.IndexOf('?', num2);
		if (num3 <= num2 || num3 == p0.Length - 1)
		{
			return -1;
		}
		num3++;
		int num4 = p0.IndexOf('?', num3);
		if (num4 < num3 || (num4 < p0.Length - 1 && p0[num4 + 1] != '='))
		{
			return -1;
		}
		if (num4 == num3)
		{
			return num4 + 2;
		}
		string p3 = p0.Substring(num, num2 - num - 1);
		if (num3 != num2 + 2)
		{
			return -1;
		}
		char c = brgjd.rbxxu(p0[num2]);
		int p4 = num4 - num3;
		byte[] array;
		switch (c)
		{
		case 'B':
			array = mlpth(p0, num3, p4);
			break;
		case 'Q':
			array = xzbgg(p0, num3, p4);
			break;
		default:
			return -1;
		}
		if (array == null || 1 == 0)
		{
			return -1;
		}
		Encoding encoding = qirxf(p3);
		if (encoding == null || 1 == 0)
		{
			return -1;
		}
		p2.qftve(encoding, array, 0, array.Length);
		return num4 + 2;
	}

	private static byte[] mlpth(string p0, int p1, int p2)
	{
		try
		{
			p0 = p0.Substring(p1, p2);
			if (p0.IndexOf(' ') >= 0)
			{
				p0 = p0.Replace(" ", "");
			}
			if (p0.IndexOf('\t') >= 0)
			{
				p0 = p0.Replace("\t", "");
			}
			if (p0.Length % 4 != 0 && 0 == 0)
			{
				p0 = p0.TrimEnd('=');
				int num = p0.Length % 4;
				if (num != 0 && 0 == 0)
				{
					p0 = p0.PadRight(p0.Length + 4 - num, '=');
				}
			}
			if (p0.IndexOf('_') >= 0)
			{
				p0 = p0.Replace("_", "/");
			}
			if (p0.IndexOf('-') >= 0)
			{
				p0 = p0.Replace("-", "+");
			}
			return Convert.FromBase64String(p0);
		}
		catch (FormatException p3)
		{
			throw MimeException.zarvw(p1, "Base64 decoding error.", p3);
		}
	}

	private static byte[] xzbgg(string p0, int p1, int p2)
	{
		MemoryStream memoryStream = new MemoryStream();
		int num = 0;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0014;
		}
		goto IL_00fd;
		IL_0014:
		char c = p0[p1 + num2];
		if (c == '_')
		{
			memoryStream.WriteByte(32);
			num2++;
		}
		else if (c == '=')
		{
			while (true)
			{
				if (p1 + num2 + 2 >= p0.Length)
				{
					throw MimeException.dngxr(p1 + num2, "Quoted-printable decoding error.");
				}
				c = p0[p1 + num2 + 1];
				if (c != ' ' && c != '\t')
				{
					break;
				}
				num2++;
			}
			int num3 = bvuap(p0[p1 + num2 + 1]);
			int num4 = bvuap(p0[p1 + num2 + 2]);
			if (num3 < 0)
			{
				num2++;
			}
			else if (num4 < 0)
			{
				num2 += 2;
			}
			else
			{
				memoryStream.WriteByte((byte)(16 * num3 + num4));
				num2 += 3;
			}
		}
		else
		{
			if (c > '~')
			{
				byte[] bytes = EncodingTools.Default.GetBytes(new char[1] { c });
				memoryStream.Write(bytes, 0, bytes.Length);
			}
			else
			{
				memoryStream.WriteByte((byte)c);
			}
			num2++;
		}
		goto IL_00fd;
		IL_00fd:
		if (num2 >= p2)
		{
			p2 = num;
			memoryStream.Close();
			return memoryStream.ToArray();
		}
		goto IL_0014;
	}

	public static Encoding gptax(Encoding p0, Encoding p1, string p2, out byte[] p3)
	{
		if (rsafc(p2) && 0 == 0)
		{
			if (p0 != null && 0 == 0)
			{
				Encoding encoding = uafvj(p2, p0, out p3);
				if (encoding != null && 0 == 0)
				{
					return encoding;
				}
			}
			if (p1 != null && 0 == 0)
			{
				Encoding encoding2 = uafvj(p2, p1, out p3);
				if (encoding2 != null && 0 == 0)
				{
					return encoding2;
				}
			}
			p3 = EncodingTools.ASCII.GetBytes(p2);
			return EncodingTools.ASCII;
		}
		int num = -2;
		if (num == 0)
		{
			goto IL_0071;
		}
		goto IL_00c5;
		IL_00c5:
		if (num >= vpajc.Length)
		{
			p3 = Encoding.UTF8.GetBytes(p2);
			return Encoding.UTF8;
		}
		goto IL_0071;
		IL_0071:
		Encoding encoding3 = num switch
		{
			-2 => p0, 
			-1 => p1, 
			_ => fhqcd(vpajc[num]), 
		};
		if (encoding3 != null && 0 == 0)
		{
			Encoding encoding4 = uafvj(p2, encoding3, out p3);
			if (encoding4 != null && 0 == 0)
			{
				return encoding4;
			}
		}
		num++;
		goto IL_00c5;
	}

	public static bool rsafc(string p0)
	{
		bool flag = true;
		int num = 0;
		if (num != 0)
		{
			goto IL_0008;
		}
		goto IL_001e;
		IL_0008:
		char c = p0[num];
		if (c > '~')
		{
			flag = false;
			if (!flag)
			{
				goto IL_002e;
			}
		}
		num++;
		goto IL_001e;
		IL_002e:
		return flag;
		IL_001e:
		if (num >= p0.Length)
		{
			goto IL_002e;
		}
		goto IL_0008;
	}

	public static Encoding uafvj(string p0, Encoding p1, out byte[] p2)
	{
		if (p1 != null && 0 == 0)
		{
			p2 = p1.GetBytes(p0);
			string text = p1.GetString(p2, 0, p2.Length);
			if (p0 == text && 0 == 0)
			{
				return p1;
			}
		}
		p2 = null;
		return null;
	}

	private static Encoding fhqcd(string p0)
	{
		if (p0.Length > 0 && char.IsDigit(p0[0]) && 0 == 0)
		{
			p0 = "iso-8859-" + p0;
		}
		return qirxf(p0);
	}

	private static bool mrgya(int p0)
	{
		if (p0 <= 32 || p0 >= 127)
		{
			return false;
		}
		string text = "()<>@,;:\"/[]?.=";
		return text.IndexOf((char)p0) < 0;
	}

	public static string qtbqs(byte[] p0, int p1, int p2)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		if (num != 0)
		{
			goto IL_000c;
		}
		goto IL_0057;
		IL_000c:
		int num2 = p0[p1 + num];
		if (num2 == 32)
		{
			stringBuilder.Append('_');
		}
		else if (mrgya(num2) && 0 == 0 && num2 != 95)
		{
			stringBuilder.Append((char)num2);
		}
		else
		{
			stringBuilder.dlvlk("={0:X2}", num2);
		}
		num++;
		goto IL_0057;
		IL_0057:
		if (num < p2)
		{
			goto IL_000c;
		}
		return stringBuilder.ToString();
	}

	public static void wbwli(string p0, string p1)
	{
		if (zzrne == null || 1 == 0)
		{
			zzrne = dahxy.mnelc();
		}
		int num = p1.IndexOfAny(zzrne);
		int num2;
		if (num < 0)
		{
			num2 = 0;
			if (num2 != 0)
			{
				goto IL_002e;
			}
			goto IL_0041;
		}
		goto IL_004a;
		IL_004a:
		if (num < 0)
		{
			return;
		}
		throw new ArgumentException(brgjd.edcru("Invalid character (0x{1:X2}) at position {0}.", num, (int)p0[num]), p1);
		IL_002e:
		if (p0[num2] < ' ')
		{
			num = num2;
			goto IL_004a;
		}
		num2++;
		goto IL_0041;
		IL_0041:
		if (num2 < p0.Length)
		{
			goto IL_002e;
		}
		goto IL_004a;
	}

	public static void zgeyl(string p0, string p1, bool p2)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_0061;
		IL_0006:
		if ((p0[num] != '\t' || !p2 || 1 == 0) && p0[num] < ' ')
		{
			throw new ArgumentException(brgjd.edcru("Invalid character (0x{1:X2}) at position {0}.", num, (int)p0[num]), p1);
		}
		num++;
		goto IL_0061;
		IL_0061:
		if (num >= p0.Length)
		{
			return;
		}
		goto IL_0006;
	}

	public static void hdlkr(string p0, string p1, bool p2)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_000c;
		}
		goto IL_007d;
		IL_000c:
		if (((p0[num] != '\t' && p0[num] != ' ') || !p2 || 1 == 0) && (p0[num] <= ' ' || p0[num] >= '\u007f'))
		{
			throw new ArgumentException(brgjd.edcru("Invalid character (0x{1:X2}) at position {0}.", num, (int)p0[num]), p1);
		}
		num++;
		goto IL_007d;
		IL_007d:
		if (num >= p0.Length)
		{
			return;
		}
		goto IL_000c;
	}

	public static Encoding qirxf(string p0)
	{
		p0 = p0.ToLower(CultureInfo.InvariantCulture);
		if (p0.Length == 4)
		{
			if (p0.StartsWith("125") && 0 == 0 && char.IsDigit(p0[3]) && 0 == 0)
			{
				p0 = "windows-" + p0;
			}
			if (p0 == "utf8" && 0 == 0)
			{
				p0 = "utf-8";
			}
		}
		else if (p0.Length == 6 && p0.StartsWith("cp125") && 0 == 0 && char.IsDigit(p0[5]) && 0 == 0)
		{
			p0 = "windows-" + p0.Substring(2);
		}
		try
		{
			return EncodingTools.GetEncoding(p0);
		}
		catch
		{
			return null;
		}
	}

	public static byte[] sjmog()
	{
		byte[] array = new byte[16];
		CryptoHelper.CreateRandomNumberGenerator().GetBytes(array);
		return array;
	}

	public static string nzgih(string p0, string p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException(p1);
		}
		p0 = p0.Trim();
		return p0;
	}
}
