using System;
using System.Text;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class ByteArrayExtension
{
	public static string ToReadable(this byte[] me)
	{
		if (me == null)
		{
			return "Null";
		}
		StringBuilder stringBuilder = new StringBuilder(me.Length * 2);
		foreach (byte b in me)
		{
			stringBuilder.Append(b.ToString("X2"));
		}
		return stringBuilder.ToString();
	}

	public static string ToReadableDecimal(this byte[] me)
	{
		if (me == null)
		{
			return "Null";
		}
		StringBuilder stringBuilder = new StringBuilder(me.Length * 2);
		foreach (byte b in me)
		{
			stringBuilder.Append(b + " ");
		}
		return stringBuilder.ToString();
	}

	public static byte[] ToByteArray(this string me)
	{
		byte[] array = new byte[me.Length / 2];
		for (int i = 0; i < me.Length - 1; i += 2)
		{
			array[i / 2] = Convert.ToByte(me.Substring(i, 2), 16);
		}
		return array;
	}

	public static bool Compare(this byte[] me, byte[] other)
	{
		if (me == null && other == null)
		{
			return true;
		}
		if (me == null)
		{
			return false;
		}
		if (other == null)
		{
			return false;
		}
		if (other.Length != me.Length)
		{
			return false;
		}
		for (int i = 0; i < me.Length; i++)
		{
			if (me[i] != other[i])
			{
				return false;
			}
		}
		return true;
	}

	public static bool Compare(this uint[] me, uint[] other)
	{
		if (me == null && other == null)
		{
			return true;
		}
		if (me == null)
		{
			return false;
		}
		if (other == null)
		{
			return false;
		}
		if (other.Length != me.Length)
		{
			return false;
		}
		for (int i = 0; i < me.Length; i++)
		{
			if (me[i] != other[i])
			{
				return false;
			}
		}
		return true;
	}

	public static void CopySubArray(this byte[] me, byte[] other, int offset, int length)
	{
		for (int i = 0; i < length; i++)
		{
			other[i] = me[offset + i];
		}
	}

	public static bool Matches(this byte[] me, byte[] other, int offset1, int offset2, int length)
	{
		try
		{
			for (int i = 0; i < length; i++)
			{
				if (me[offset1 + i] != other[offset2 + i])
				{
					return false;
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("Exception in ByteArrayExtension.Match: " + ex.ToString());
			return false;
		}
		return true;
	}

	public static byte[] GetRow(this byte[,] array, int row)
	{
		byte[] result = null;
		if (array == null)
		{
			return result;
		}
		if (row < array.GetLowerBound(0) || row > array.GetUpperBound(0))
		{
			return result;
		}
		int length = array.GetLength(1);
		result = new byte[length];
		for (int i = 0; i < length; i++)
		{
			result[i] = array[row, i];
		}
		return result;
	}
}
