using RWE.SmartHome.SHC.WebSocketsService.Common;

namespace RWE.SmartHome.SHC.WebSocketsService.Extensions;

public static class IntExtensions
{
	private static string hexValues = "0123456789ABCDEF";

	public static string ToHex(this byte value)
	{
		return new string(new char[2]
		{
			hexValues[(value >> 4) & 0xF],
			hexValues[value & 0xF]
		});
	}

	public static byte[] ToBytes(this short value, JDIConst.ByteOrder byteOrder)
	{
		byte[] array = new byte[2];
		switch (byteOrder)
		{
		case JDIConst.ByteOrder.LittleEndian:
			array[1] = (byte)((value >> 8) & 0xFF);
			array[0] = (byte)(value & 0xFF);
			break;
		case JDIConst.ByteOrder.Network:
		case JDIConst.ByteOrder.BigEndian:
			array[0] = (byte)((value >> 8) & 0xFF);
			array[1] = (byte)(value & 0xFF);
			break;
		}
		return array;
	}

	public static string ToHex(this short value)
	{
		return new string(new char[4]
		{
			hexValues[(byte)((value >> 12) & 0xF)],
			hexValues[(byte)((value >> 8) & 0xF)],
			hexValues[(byte)((value >> 4) & 0xF)],
			hexValues[(byte)(value & 0xF)]
		});
	}

	public static byte[] ToBytes(this int value, JDIConst.ByteOrder byteOrder)
	{
		byte[] array = new byte[4];
		switch (byteOrder)
		{
		case JDIConst.ByteOrder.LittleEndian:
			array[3] = (byte)((value >> 24) & 0xFF);
			array[2] = (byte)((value >> 16) & 0xFF);
			array[1] = (byte)((value >> 8) & 0xFF);
			array[0] = (byte)(value & 0xFF);
			break;
		case JDIConst.ByteOrder.Network:
		case JDIConst.ByteOrder.BigEndian:
			array[0] = (byte)((value >> 24) & 0xFF);
			array[1] = (byte)((value >> 16) & 0xFF);
			array[2] = (byte)((value >> 8) & 0xFF);
			array[3] = (byte)(value & 0xFF);
			break;
		}
		return array;
	}

	public static string ToHex(this int value)
	{
		return new string(new char[8]
		{
			hexValues[(byte)((value >> 28) & 0xF)],
			hexValues[(byte)((value >> 24) & 0xF)],
			hexValues[(byte)((value >> 20) & 0xF)],
			hexValues[(byte)((value >> 16) & 0xF)],
			hexValues[(byte)((value >> 12) & 0xF)],
			hexValues[(byte)((value >> 8) & 0xF)],
			hexValues[(byte)((value >> 4) & 0xF)],
			hexValues[(byte)(value & 0xF)]
		});
	}

	public static string Format(this int value, string format)
	{
		return value.ToString();
	}

	public static byte[] ToBytes(this ushort value, JDIConst.ByteOrder byteOrder)
	{
		byte[] array = new byte[2];
		switch (byteOrder)
		{
		case JDIConst.ByteOrder.LittleEndian:
			array[1] = (byte)((value >> 8) & 0xFF);
			array[0] = (byte)(value & 0xFF);
			break;
		case JDIConst.ByteOrder.Network:
		case JDIConst.ByteOrder.BigEndian:
			array[0] = (byte)((value >> 8) & 0xFF);
			array[1] = (byte)(value & 0xFF);
			break;
		}
		return array;
	}

	public static string ToHex(this ushort value)
	{
		return new string(new char[4]
		{
			hexValues[(byte)((value >> 12) & 0xF)],
			hexValues[(byte)((value >> 8) & 0xF)],
			hexValues[(byte)((value >> 4) & 0xF)],
			hexValues[(byte)(value & 0xF)]
		});
	}

	public static byte[] ToBytes(this uint value, JDIConst.ByteOrder byteOrder)
	{
		byte[] array = new byte[4];
		switch (byteOrder)
		{
		case JDIConst.ByteOrder.LittleEndian:
			array[3] = (byte)((value >> 24) & 0xFF);
			array[2] = (byte)((value >> 16) & 0xFF);
			array[1] = (byte)((value >> 8) & 0xFF);
			array[0] = (byte)(value & 0xFF);
			break;
		case JDIConst.ByteOrder.Network:
		case JDIConst.ByteOrder.BigEndian:
			array[0] = (byte)((value >> 24) & 0xFF);
			array[1] = (byte)((value >> 16) & 0xFF);
			array[2] = (byte)((value >> 8) & 0xFF);
			array[3] = (byte)(value & 0xFF);
			break;
		}
		return array;
	}

	public static string ToHex(this uint value)
	{
		return new string(new char[8]
		{
			hexValues[(byte)((value >> 28) & 0xF)],
			hexValues[(byte)((value >> 24) & 0xF)],
			hexValues[(byte)((value >> 20) & 0xF)],
			hexValues[(byte)((value >> 16) & 0xF)],
			hexValues[(byte)((value >> 12) & 0xF)],
			hexValues[(byte)((value >> 8) & 0xF)],
			hexValues[(byte)((value >> 4) & 0xF)],
			hexValues[(byte)(value & 0xF)]
		});
	}
}
