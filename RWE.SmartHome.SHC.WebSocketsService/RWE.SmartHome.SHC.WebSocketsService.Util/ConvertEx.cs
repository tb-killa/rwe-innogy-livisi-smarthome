using System;

namespace RWE.SmartHome.SHC.WebSocketsService.Util;

public static class ConvertEx
{
	private const int CCH_B64_IN_QUARTET = 4;

	private const int CB_B64_OUT_TRIO = 3;

	private static char[] s_rgchBase64EncodingRFC4648 = new char[64]
	{
		'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
		'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
		'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
		'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
		'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
		'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7',
		'8', '9', '+', '/'
	};

	public static string ToBase64String(byte[] inArray)
	{
		return ToBase64String(inArray, 0, inArray.Length);
	}

	public static string ToBase64String(byte[] inArray, int offset, int length)
	{
		if (inArray == null)
		{
			throw new ArgumentNullException();
		}
		if (length == 0)
		{
			return "";
		}
		if (offset + length > inArray.Length)
		{
			throw new ArgumentOutOfRangeException();
		}
		int base64EncodedLength = GetBase64EncodedLength(length);
		char[] array = new char[base64EncodedLength];
		int num = offset + (base64EncodedLength / 4 - 1) * 3;
		int num2 = offset;
		int num3 = 0;
		byte b = 0;
		byte b2 = 0;
		byte b3 = 0;
		while (num2 < num)
		{
			b = inArray[num2];
			b2 = inArray[num2 + 1];
			b3 = inArray[num2 + 2];
			array[num3] = s_rgchBase64EncodingRFC4648[b >> 2];
			array[num3 + 1] = s_rgchBase64EncodingRFC4648[((b << 4) & 0x30) | ((b2 >> 4) & 0xF)];
			array[num3 + 2] = s_rgchBase64EncodingRFC4648[((b2 << 2) & 0x3C) | ((b3 >> 6) & 3)];
			array[num3 + 3] = s_rgchBase64EncodingRFC4648[b3 & 0x3F];
			num2 += 3;
			num3 += 4;
		}
		b = inArray[num2];
		b2 = (byte)((num2 + 1 < offset + length) ? inArray[num2 + 1] : 0);
		b3 = (byte)((num2 + 2 < offset + length) ? inArray[num2 + 2] : 0);
		array[num3] = s_rgchBase64EncodingRFC4648[b >> 2];
		array[num3 + 1] = s_rgchBase64EncodingRFC4648[((b << 4) & 0x30) | ((b2 >> 4) & 0xF)];
		array[num3 + 2] = s_rgchBase64EncodingRFC4648[((b2 << 2) & 0x3C) | ((b3 >> 6) & 3)];
		array[num3 + 3] = s_rgchBase64EncodingRFC4648[b3 & 0x3F];
		switch (length % 3)
		{
		case 1:
			array[base64EncodedLength - 2] = '=';
			goto case 2;
		case 2:
			array[base64EncodedLength - 1] = '=';
			break;
		}
		return new string(array);
	}

	private static int GetBase64EncodedLength(int binaryLen)
	{
		return (binaryLen / 3 + ((binaryLen % 3 != 0) ? 1 : 0)) * 4;
	}
}
