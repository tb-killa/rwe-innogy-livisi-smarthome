using System;
using System.Text;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.Ras;

internal static class ByteArrayExtension
{
	public static uint GetUInt(this byte[] buffer, int offset)
	{
		return (uint)BitConverter.ToInt32(buffer, offset);
	}

	public static void SetUInt(this byte[] buffer, int offset, uint value)
	{
		BitConverter.GetBytes(value).CopyTo(buffer, offset);
	}

	public static string GetString(this byte[] buffer, int offset, int charCount)
	{
		string text = Encoding.Unicode.GetString(buffer, offset, charCount * 2);
		int num = text.IndexOf('\0');
		if (num != -1)
		{
			text = text.Substring(0, num);
		}
		return text;
	}

	public static void SetString(this byte[] buffer, int offset, string value)
	{
		Encoding.Unicode.GetBytes(value + '\0').CopyTo(buffer, offset);
	}
}
