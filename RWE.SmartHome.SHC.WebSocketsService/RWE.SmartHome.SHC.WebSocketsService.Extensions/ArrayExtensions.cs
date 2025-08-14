using System;
using System.Collections;
using System.Text;
using RWE.SmartHome.SHC.WebSocketsService.Common;

namespace RWE.SmartHome.SHC.WebSocketsService.Extensions;

public static class ArrayExtensions
{
	private static string hexValues = "0123456789ABCDEF";

	public static string ToHex(this byte[] array)
	{
		if (array == null || array.Length == 0)
		{
			return string.Empty;
		}
		return array.ToHex(0, array.Length);
	}

	public static string ToHex(this byte[] array, int length)
	{
		if (array == null || array.Length == 0 || length == 0)
		{
			return string.Empty;
		}
		return array.ToHex(0, length);
	}

	public static string ToHex(this byte[] array, int offset, int length)
	{
		if (array == null || array.Length == 0 || array.Length < offset + length)
		{
			return string.Empty;
		}
		char[] array2 = new char[length * 3];
		int num = offset + length;
		int num2 = 0;
		for (int i = offset; i < num; i++)
		{
			byte b = array[i];
			array2[num2++] = hexValues[(b >> 4) & 0xF];
			array2[num2++] = hexValues[b & 0xF];
			array2[num2++] = ' ';
		}
		return new string(array2, 0, array2.Length);
	}

	public static short ToInt16(this byte[] array, int offset, JDIConst.ByteOrder byteOrder)
	{
		short result = 0;
		switch (byteOrder)
		{
		case JDIConst.ByteOrder.LittleEndian:
			result = (short)((array[offset + 1] << 8) + array[offset]);
			break;
		case JDIConst.ByteOrder.Network:
		case JDIConst.ByteOrder.BigEndian:
			result = (short)((array[offset] << 8) + array[offset + 1]);
			break;
		}
		return result;
	}

	public static int ToInt32(this byte[] array, int offset, JDIConst.ByteOrder byteOrder)
	{
		int result = 0;
		switch (byteOrder)
		{
		case JDIConst.ByteOrder.LittleEndian:
			result = (array[offset + 3] << 24) + (array[offset + 2] << 16) + (array[offset + 1] << 8) + array[offset];
			break;
		case JDIConst.ByteOrder.Network:
		case JDIConst.ByteOrder.BigEndian:
			result = (array[offset] << 24) + (array[offset + 1] << 16) + (array[offset + 2] << 8) + array[offset + 3];
			break;
		}
		return result;
	}

	public static long ToInt64(this byte[] array, int offset, JDIConst.ByteOrder byteOrder)
	{
		long result = 0L;
		switch (byteOrder)
		{
		case JDIConst.ByteOrder.LittleEndian:
			result = (array[offset + 7] << 24) + (array[offset + 6] << 16) + (array[offset + 5] << 8) + array[offset + 4] + (array[offset + 3] << 24) + (array[offset + 2] << 16) + (array[offset + 1] << 8) + array[offset];
			break;
		case JDIConst.ByteOrder.Network:
		case JDIConst.ByteOrder.BigEndian:
			result = (array[offset] << 24) + (array[offset + 1] << 16) + (array[offset + 2] << 8) + array[offset + 3] + (array[offset + 4] << 24) + (array[offset + 5] << 16) + (array[offset + 6] << 8) + array[offset + 7];
			break;
		}
		return result;
	}

	public static ushort ToUInt16(this byte[] array, int offset, JDIConst.ByteOrder byteOrder)
	{
		ushort result = 0;
		switch (byteOrder)
		{
		case JDIConst.ByteOrder.LittleEndian:
			result = (ushort)((array[offset + 1] << 8) + array[offset]);
			break;
		case JDIConst.ByteOrder.Network:
		case JDIConst.ByteOrder.BigEndian:
			result = (ushort)((array[offset] << 8) + array[offset + 1]);
			break;
		}
		return result;
	}

	public static uint ToUInt32(this byte[] array, int offset, JDIConst.ByteOrder byteOrder)
	{
		uint result = 0u;
		switch (byteOrder)
		{
		case JDIConst.ByteOrder.LittleEndian:
			result = (uint)((array[offset + 3] << 24) + (array[offset + 2] << 16) + (array[offset + 1] << 8) + array[offset]);
			break;
		case JDIConst.ByteOrder.Network:
		case JDIConst.ByteOrder.BigEndian:
			result = (uint)((array[offset] << 24) + (array[offset + 1] << 16) + (array[offset + 2] << 8) + array[offset + 3]);
			break;
		}
		return result;
	}

	public static ulong ToUInt64(this byte[] array, int offset, JDIConst.ByteOrder byteOrder)
	{
		ulong result = 0uL;
		switch (byteOrder)
		{
		case JDIConst.ByteOrder.LittleEndian:
			result = (ulong)((array[offset + 7] << 24) + (array[offset + 6] << 16) + (array[offset + 5] << 8) + array[offset + 4] + (array[offset + 3] << 24) + (array[offset + 2] << 16) + (array[offset + 1] << 8) + array[offset]);
			break;
		case JDIConst.ByteOrder.Network:
		case JDIConst.ByteOrder.BigEndian:
			result = (ulong)((array[offset] << 24) + (array[offset + 1] << 16) + (array[offset + 2] << 8) + array[offset + 3] + (array[offset + 4] << 24) + (array[offset + 5] << 16) + (array[offset + 6] << 8) + array[offset + 7]);
			break;
		}
		return result;
	}

	public static string ToString(this byte[] array)
	{
		if (array == null || array.Length == 0)
		{
			return string.Empty;
		}
		return new string(Encoding.UTF8.GetChars(array));
	}

	public static string ToString(this byte[] array, int offset, int length)
	{
		if (array == null || array.Length == 0 || array.Length < offset + length)
		{
			return string.Empty;
		}
		return new string(Encoding.UTF8.GetChars(array, offset, length));
	}

	public static string ToMACAddress(this byte[] array, int offset)
	{
		if (array == null || array.Length == 0 || offset < 0 || offset >= array.Length)
		{
			return string.Empty;
		}
		string[] array2 = new string[6]
		{
			array[offset].ToHex(),
			array[offset + 1].ToHex(),
			array[offset + 2].ToHex(),
			array[offset + 3].ToHex(),
			array[offset + 4].ToHex(),
			array[offset + 5].ToHex()
		};
		return array2[0] + "-" + array2[1] + "-" + array2[2] + "-" + array2[3] + "-" + array2[4] + "-" + array2[5];
	}

	public static string ToIPAddress(this byte[] array, int offset)
	{
		if (array == null || array.Length == 0 || offset < 0 || offset >= array.Length)
		{
			return string.Empty;
		}
		string[] array2 = new string[4]
		{
			array[offset].ToString(),
			array[offset + 1].ToString(),
			array[offset + 2].ToString(),
			array[offset + 3].ToString()
		};
		return array2[0] + "." + array2[1] + "." + array2[2] + "." + array2[3];
	}

	public static int IndexOf(this byte[] array, string value, int startIndex)
	{
		if (array == null || value == null || value.Length == 0 || startIndex < 0 || startIndex >= array.Length)
		{
			return -1;
		}
		byte[] value2 = value.ToByteArray();
		return IndexOf(array, value2, startIndex);
	}

	public static int IndexOf(this byte[] array, byte[] value, int startIndex)
	{
		if (array == null || value == null || value.Length == 0 || startIndex < 0 || startIndex >= array.Length)
		{
			return -1;
		}
		int i = startIndex;
		int num = 0;
		int num2 = 0;
		for (; i < array.Length; i++)
		{
			num = i;
			for (num2 = 0; num2 < value.Length && array[num] == value[num2]; num2++)
			{
				num++;
			}
			if (num2 == value.Length)
			{
				return i;
			}
		}
		return -1;
	}

	public static ArrayList Split(this byte[] array, string delimiter, int startIndex, int length, bool toLower)
	{
		if (array == null || startIndex < 0 || length < 0 || array.Length < startIndex + length)
		{
			return new ArrayList();
		}
		byte[] delimiter2 = delimiter.ToByteArray();
		return array.Split(delimiter2, startIndex, length, toLower);
	}

	public static ArrayList Split(this byte[] array, char delimiter, int startIndex, int length, bool toLower)
	{
		if (array == null || startIndex < 0 || length < 0 || array.Length < startIndex + length)
		{
			return new ArrayList();
		}
		byte[] delimiter2 = new byte[1] { (byte)delimiter };
		return array.Split(delimiter2, startIndex, length, toLower);
	}

	public static ArrayList Split(this byte[] array, byte[] delimiter, int startIndex, int length, bool toLower)
	{
		if (array == null || startIndex < 0 || length < 0 || array.Length < startIndex + length)
		{
			return new ArrayList();
		}
		ArrayList arrayList = new ArrayList();
		int num = startIndex + length;
		int num2 = 0;
		while (startIndex + delimiter.Length <= num && (num2 = IndexOf(array, delimiter, startIndex)) >= 0)
		{
			if (toLower)
			{
				arrayList.Add(new string(Encoding.UTF8.GetChars(array, startIndex, num2 - startIndex)).ToLower());
			}
			else
			{
				arrayList.Add(new string(Encoding.UTF8.GetChars(array, startIndex, num2 - startIndex)));
			}
			startIndex = num2 + delimiter.Length;
		}
		return arrayList;
	}

	public static byte[] SubArray(this byte[] array, int startIndex, int length)
	{
		if (array == null || array.Length < startIndex + length)
		{
			return new byte[0];
		}
		byte[] array2 = new byte[length];
		Array.Copy(array, startIndex, array2, 0, length);
		return array2;
	}
}
