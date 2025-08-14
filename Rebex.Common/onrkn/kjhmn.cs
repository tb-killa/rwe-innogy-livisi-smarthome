using System;
using System.IO;
using Rebex;

namespace onrkn;

internal static class kjhmn
{
	public static void scgio(Stream p0, byte[] p1)
	{
		StreamWriter streamWriter = new StreamWriter(p0);
		try
		{
			jsvbw(streamWriter, p1);
		}
		finally
		{
			if (streamWriter != null && 0 == 0)
			{
				((IDisposable)streamWriter).Dispose();
			}
		}
	}

	public static void jsvbw(StreamWriter p0, byte[] p1)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_002b;
		IL_0006:
		string value = Convert.ToBase64String(p1, num, Math.Min(48, p1.Length - num));
		p0.WriteLine(value);
		num += 48;
		goto IL_002b;
		IL_002b:
		if (num >= p1.Length)
		{
			return;
		}
		goto IL_0006;
	}

	public static byte[] qzqgg(byte[] p0, int p1, out string p2)
	{
		string p3 = EncodingTools.dmppd.GetString(p0, 0, p1);
		string p4;
		return qwify(p3, out p2, out p4);
	}

	public static byte[] bhchj(string p0)
	{
		string p1;
		string p2;
		return qwify(p0, out p1, out p2);
	}

	public static byte[] btkpl(string p0, out string p1)
	{
		string p2;
		return qwify(p0, out p1, out p2);
	}

	private static byte[] qwify(string p0, out string p1, out string p2)
	{
		int num = p0.IndexOf("-----BEGIN ", StringComparison.Ordinal);
		if (num < 0)
		{
			throw new FormatException("Invalid header.");
		}
		string text = p0.Substring(num).Replace("\r", "");
		char[] trimChars = new char[1];
		p0 = text.TrimEnd(trimChars).Trim();
		int num2 = p0.IndexOf('\n', 11, 64);
		if (num2 < 8)
		{
			throw new FormatException("Invalid header length.");
		}
		string text2 = p0.Substring(11, num2 - 11);
		text2 = text2.TrimEnd();
		if (!text2.EndsWith("-----", StringComparison.Ordinal) || 1 == 0)
		{
			throw new FormatException("Invalid header length.");
		}
		p1 = text2.Substring(0, text2.Length - 5);
		string text3 = "-----END " + p1 + "-----";
		int num3 = p0.IndexOf(text3, StringComparison.Ordinal);
		if (num3 < 0)
		{
			throw new FormatException("Invalid footer.");
		}
		p0 = p0.Substring(0, num3 + text3.Length);
		int num4 = 0;
		if (num4 != 0)
		{
			goto IL_00fd;
		}
		goto IL_0155;
		IL_0155:
		if (num4 >= p0.Length)
		{
			p0 = p0.Substring(text3.Length + 2, p0.Length - 2 - 2 * text3.Length);
			int num5 = p0.IndexOf("\n\n", StringComparison.Ordinal);
			if (num5 > 0)
			{
				p2 = p0.Substring(0, num5).Trim();
				p0 = p0.Substring(num5 + 2);
			}
			else
			{
				p2 = null;
			}
			try
			{
				return Convert.FromBase64String(p0);
			}
			catch (FormatException innerException)
			{
				throw new FormatException("Invalid Base-64 encoding.", innerException);
			}
		}
		goto IL_00fd;
		IL_00fd:
		int num6 = p0[num4];
		switch (num6)
		{
		default:
			throw new FormatException(brgjd.edcru("Invalid character {0} at position {1}.", num6, num4));
		case 9:
		case 10:
		case 32:
		case 33:
		case 34:
		case 35:
		case 36:
		case 37:
		case 38:
		case 39:
		case 40:
		case 41:
		case 42:
		case 43:
		case 44:
		case 45:
		case 46:
		case 47:
		case 48:
		case 49:
		case 50:
		case 51:
		case 52:
		case 53:
		case 54:
		case 55:
		case 56:
		case 57:
		case 58:
		case 59:
		case 60:
		case 61:
		case 62:
		case 63:
		case 64:
		case 65:
		case 66:
		case 67:
		case 68:
		case 69:
		case 70:
		case 71:
		case 72:
		case 73:
		case 74:
		case 75:
		case 76:
		case 77:
		case 78:
		case 79:
		case 80:
		case 81:
		case 82:
		case 83:
		case 84:
		case 85:
		case 86:
		case 87:
		case 88:
		case 89:
		case 90:
		case 91:
		case 92:
		case 93:
		case 94:
		case 95:
		case 96:
		case 97:
		case 98:
		case 99:
		case 100:
		case 101:
		case 102:
		case 103:
		case 104:
		case 105:
		case 106:
		case 107:
		case 108:
		case 109:
		case 110:
		case 111:
		case 112:
		case 113:
		case 114:
		case 115:
		case 116:
		case 117:
		case 118:
		case 119:
		case 120:
		case 121:
		case 122:
		case 123:
		case 124:
		case 125:
		case 126:
		case 127:
			break;
		}
		num4++;
		goto IL_0155;
	}
}
