using System;
using System.Text;
using RWE.SmartHome.SHC.WebSocketsService.Common;
using RWE.SmartHome.SHC.WebSocketsService.Extensions;

namespace RWE.SmartHome.SHC.WebSocketsService.Util;

public static class ArrayUtil
{
	public static void Fill(byte[] array, int offset, int count)
	{
		if (array != null && array.Length != 0 && array.Length >= offset + count)
		{
			for (int i = offset; i < offset + count; i++)
			{
				array[i] = 0;
			}
		}
	}

	public static void Fill(byte[] array, int offset, int count, byte fillByte)
	{
		if (array != null && array.Length != 0 && array.Length >= offset + count)
		{
			for (int i = offset; i < offset + count; i++)
			{
				array[i] = fillByte;
			}
		}
	}

	public static byte[] Concat(params byte[][] arrays)
	{
		int num = 0;
		foreach (byte[] array in arrays)
		{
			num += array.Length;
		}
		byte[] array2 = new byte[num];
		if (num > 0)
		{
			int num2 = 0;
			foreach (byte[] array3 in arrays)
			{
				Array.Copy(array3, 0, array2, num2, array3.Length);
				num2 += array3.Length;
			}
		}
		return array2;
	}

	public static byte[] Concat(params object[] list)
	{
		int num = 0;
		int num2 = 0;
		foreach (object obj in list)
		{
			if (obj == null)
			{
				num = 0;
			}
			else if (obj is byte[])
			{
				num = ((byte[])obj).Length;
			}
			else if (obj is string)
			{
				num = ((string)obj).Length;
			}
			else if (obj is byte)
			{
				num = 1;
			}
			else if (obj is short || obj is short || obj is ushort)
			{
				num = 2;
			}
			else
			{
				if (!(obj is int) && !(obj is uint) && !(obj is int) && !(obj is uint))
				{
					throw new ArgumentException("Parameters must be of type 'byte', 'byte[]', 'short', 'uint16', 'int', or 'uint'");
				}
				num = 4;
			}
			num2 += num;
		}
		byte[] array = new byte[num2];
		if (num2 > 0)
		{
			int num3 = 0;
			foreach (object obj2 in list)
			{
				if (obj2 == null)
				{
					num = 0;
				}
				else if (obj2 is byte[])
				{
					num = ((byte[])obj2).Length;
					Array.Copy((byte[])obj2, 0, array, num3, num);
				}
				else if (obj2 is string)
				{
					num = ((string)obj2).Length;
					Array.Copy(Encoding.UTF8.GetBytes((string)obj2), 0, array, num3, num);
				}
				else if (obj2 is byte)
				{
					num = 1;
					array[num3] = (byte)obj2;
				}
				else if (obj2 is short || obj2 is short)
				{
					num = 2;
					byte[] sourceArray = ((short)obj2).ToBytes(JDIConst.ByteOrder.Network);
					Array.Copy(sourceArray, 0, array, num3, num);
				}
				else if (obj2 is ushort)
				{
					num = 2;
					byte[] sourceArray = ((ushort)obj2).ToBytes(JDIConst.ByteOrder.Network);
					Array.Copy(sourceArray, 0, array, num3, num);
				}
				else if (obj2 is int || obj2 is int)
				{
					num = 4;
					byte[] sourceArray = ((int)obj2).ToBytes(JDIConst.ByteOrder.Network);
					Array.Copy(sourceArray, 0, array, num3, num);
				}
				else if (obj2 is uint || obj2 is uint)
				{
					num = 4;
					byte[] sourceArray = ((uint)obj2).ToBytes(JDIConst.ByteOrder.Network);
					Array.Copy(sourceArray, 0, array, num3, num);
				}
				num3 += num;
			}
		}
		return array;
	}
}
