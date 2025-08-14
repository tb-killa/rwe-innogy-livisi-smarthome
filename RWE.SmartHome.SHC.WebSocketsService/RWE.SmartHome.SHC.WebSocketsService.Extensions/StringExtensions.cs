using System.Text;
using System.Text.RegularExpressions;

namespace RWE.SmartHome.SHC.WebSocketsService.Extensions;

public static class StringExtensions
{
	private static string hexValues = "0123456789ABCDEF";

	public static byte[] ToByteArray(this string s)
	{
		return Encoding.UTF8.GetBytes(s);
	}

	public static bool ParseBool(this string s, bool defaultValue)
	{
		if (s == null)
		{
			return defaultValue;
		}
		s = s.Trim();
		if (s.Length == 0)
		{
			return defaultValue;
		}
		s = s.ToLower();
		switch (s)
		{
		case "1":
		case "true":
			return true;
		case "0":
		case "false":
			return false;
		default:
			return defaultValue;
		}
	}

	public static byte ParseByte(this string s, byte defaultValue)
	{
		if (s == null)
		{
			return defaultValue;
		}
		s = s.Trim();
		if (s.Length != 2)
		{
			return defaultValue;
		}
		s = s.ToUpper();
		int num = hexValues.IndexOf(s[0]);
		int num2 = hexValues.IndexOf(s[1]);
		if (num < 0 || num2 < 0)
		{
			return defaultValue;
		}
		return (byte)((num << 4) + num2);
	}

	public static short ParseInt16(this string s, short defaultValue)
	{
		if (s == null)
		{
			return defaultValue;
		}
		s = s.Trim();
		if (s.Length != 4)
		{
			return defaultValue;
		}
		s = s.ToUpper();
		int num = hexValues.IndexOf(s[0]);
		int num2 = hexValues.IndexOf(s[1]);
		int num3 = hexValues.IndexOf(s[2]);
		int num4 = hexValues.IndexOf(s[3]);
		if (num < 0 || num2 < 0 || num3 < 0 || num4 < 0)
		{
			return defaultValue;
		}
		return (short)((num << 12) + (num2 << 8) + (num3 << 4) + num4);
	}

	public static int ParseInt32(this string s, int defaultValue)
	{
		if (s == null)
		{
			return defaultValue;
		}
		s = s.Trim();
		if (s.Length != 8)
		{
			return defaultValue;
		}
		s = s.ToUpper();
		int num = hexValues.IndexOf(s[0]);
		int num2 = hexValues.IndexOf(s[1]);
		int num3 = hexValues.IndexOf(s[2]);
		int num4 = hexValues.IndexOf(s[3]);
		int num5 = hexValues.IndexOf(s[4]);
		int num6 = hexValues.IndexOf(s[5]);
		int num7 = hexValues.IndexOf(s[6]);
		int num8 = hexValues.IndexOf(s[7]);
		if (num < 0 || num2 < 0 || num3 < 0 || num4 < 0 || num5 < 0 || num6 < 0 || num7 < 0 || num8 < 0)
		{
			return defaultValue;
		}
		return (num << 28) + (num2 << 24) + (num3 << 20) + (num4 << 16) + (num5 << 12) + (num6 << 8) + (num7 << 4) + num8;
	}

	public static ushort ParseUInt16(this string s, ushort defaultValue)
	{
		if (s == null)
		{
			return defaultValue;
		}
		s = s.Trim();
		if (s.Length != 4)
		{
			return defaultValue;
		}
		s = s.ToUpper();
		int num = hexValues.IndexOf(s[0]);
		int num2 = hexValues.IndexOf(s[1]);
		int num3 = hexValues.IndexOf(s[2]);
		int num4 = hexValues.IndexOf(s[3]);
		if (num < 0 || num2 < 0 || num3 < 0 || num4 < 0)
		{
			return defaultValue;
		}
		return (ushort)((num << 12) + (num2 << 8) + (num3 << 4) + num4);
	}

	public static uint ParseUInt32(this string s, uint defaultValue)
	{
		if (s == null)
		{
			return defaultValue;
		}
		s = s.Trim();
		if (s.Length != 8)
		{
			return defaultValue;
		}
		s = s.ToUpper();
		int num = hexValues.IndexOf(s[0]);
		int num2 = hexValues.IndexOf(s[1]);
		int num3 = hexValues.IndexOf(s[2]);
		int num4 = hexValues.IndexOf(s[3]);
		int num5 = hexValues.IndexOf(s[4]);
		int num6 = hexValues.IndexOf(s[5]);
		int num7 = hexValues.IndexOf(s[6]);
		int num8 = hexValues.IndexOf(s[7]);
		if (num < 0 || num2 < 0 || num3 < 0 || num4 < 0 || num5 < 0 || num6 < 0 || num7 < 0 || num8 < 0)
		{
			return defaultValue;
		}
		return (uint)((num << 28) + (num2 << 24) + (num3 << 20) + (num4 << 16) + (num5 << 12) + (num6 << 8) + (num7 << 4) + num8);
	}

	public static byte[] ParseMACAddress(this string s)
	{
		string[] array = s.Split('-');
		if (s.Length != 17 || array.Length != 6)
		{
			return null;
		}
		return new byte[6]
		{
			array[0].ParseByte(0),
			array[1].ParseByte(0),
			array[2].ParseByte(0),
			array[3].ParseByte(0),
			array[4].ParseByte(0),
			array[5].ParseByte(0)
		};
	}

	public static byte[] ParseIPAddress(this string s)
	{
		string[] array = s.Split('.');
		if (s.Length < 7 || s.Length > 15 || array.Length != 4)
		{
			return null;
		}
		return new byte[4]
		{
			(byte)int.Parse(array[0]),
			(byte)int.Parse(array[1]),
			(byte)int.Parse(array[2]),
			(byte)int.Parse(array[3])
		};
	}

	public static bool TryParse(this string s, out bool value)
	{
		value = false;
		if (s == null)
		{
			return false;
		}
		s = s.Trim();
		if (s.Length == 0)
		{
			return false;
		}
		s = s.ToLower();
		switch (s)
		{
		case "1":
		case "true":
			value = true;
			return true;
		case "0":
		case "false":
			value = false;
			return true;
		default:
			return false;
		}
	}

	public static bool TryParseHex(this string s, out byte value)
	{
		value = 0;
		if (s == null)
		{
			return false;
		}
		s = s.Trim();
		if (s.Length != 2)
		{
			return false;
		}
		s = s.ToUpper();
		int num = hexValues.IndexOf(s[0]);
		int num2 = hexValues.IndexOf(s[1]);
		if (num < 0 || num2 < 0)
		{
			return false;
		}
		value = (byte)((num << 4) + num2);
		return true;
	}

	public static bool TryParseHex(this string s, out short value)
	{
		value = 0;
		if (s == null)
		{
			return false;
		}
		s = s.Trim();
		if (s.Length != 4)
		{
			return false;
		}
		s = s.ToUpper();
		int num = hexValues.IndexOf(s[0]);
		int num2 = hexValues.IndexOf(s[1]);
		int num3 = hexValues.IndexOf(s[2]);
		int num4 = hexValues.IndexOf(s[3]);
		if (num < 0 || num2 < 0 || num3 < 0 || num4 < 0)
		{
			return false;
		}
		value = (short)((num << 12) + (num2 << 8) + (num3 << 4) + num4);
		return true;
	}

	public static bool TryParseHex(this string s, out int value)
	{
		value = 0;
		if (s == null)
		{
			return false;
		}
		s = s.Trim();
		if (s.Length != 8)
		{
			return false;
		}
		s = s.ToUpper();
		int num = hexValues.IndexOf(s[0]);
		int num2 = hexValues.IndexOf(s[1]);
		int num3 = hexValues.IndexOf(s[2]);
		int num4 = hexValues.IndexOf(s[3]);
		int num5 = hexValues.IndexOf(s[4]);
		int num6 = hexValues.IndexOf(s[5]);
		int num7 = hexValues.IndexOf(s[6]);
		int num8 = hexValues.IndexOf(s[7]);
		if (num < 0 || num2 < 0 || num3 < 0 || num4 < 0 || num5 < 0 || num6 < 0 || num7 < 0 || num8 < 0)
		{
			return false;
		}
		value = (num << 28) + (num2 << 24) + (num3 << 20) + (num4 << 16) + (num5 << 12) + (num6 << 8) + (num7 << 4) + num8;
		return true;
	}

	public static bool TryParseHex(this string s, out ushort value)
	{
		value = 0;
		if (s == null)
		{
			return false;
		}
		s = s.Trim();
		if (s.Length != 4)
		{
			return false;
		}
		s = s.ToUpper();
		int num = hexValues.IndexOf(s[0]);
		int num2 = hexValues.IndexOf(s[1]);
		int num3 = hexValues.IndexOf(s[2]);
		int num4 = hexValues.IndexOf(s[3]);
		if (num < 0 || num2 < 0 || num3 < 0 || num4 < 0)
		{
			return false;
		}
		value = (ushort)((num << 12) + (num2 << 8) + (num3 << 4) + num4);
		return true;
	}

	public static bool TryParseHex(this string s, out uint value)
	{
		value = 0u;
		if (s == null)
		{
			return false;
		}
		s = s.Trim();
		if (s.Length != 8)
		{
			return false;
		}
		s = s.ToUpper();
		int num = hexValues.IndexOf(s[0]);
		int num2 = hexValues.IndexOf(s[1]);
		int num3 = hexValues.IndexOf(s[2]);
		int num4 = hexValues.IndexOf(s[3]);
		int num5 = hexValues.IndexOf(s[4]);
		int num6 = hexValues.IndexOf(s[5]);
		int num7 = hexValues.IndexOf(s[6]);
		int num8 = hexValues.IndexOf(s[7]);
		if (num < 0 || num2 < 0 || num3 < 0 || num4 < 0 || num5 < 0 || num6 < 0 || num7 < 0 || num8 < 0)
		{
			return false;
		}
		value = (uint)((num << 28) + (num2 << 24) + (num3 << 20) + (num4 << 16) + (num5 << 12) + (num6 << 8) + (num7 << 4) + num8);
		return true;
	}

	public static string Replace(this string s, string oldValue, string newValue)
	{
		string pattern = "(" + oldValue + "?)";
		Regex regex = new Regex(pattern);
		return regex.Replace(s, newValue);
	}
}
