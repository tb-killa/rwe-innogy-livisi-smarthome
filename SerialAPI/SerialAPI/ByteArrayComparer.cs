using System.Collections.Generic;

namespace SerialAPI;

internal class ByteArrayComparer : IEqualityComparer<byte[]>
{
	public bool Equals(byte[] x, byte[] y)
	{
		if (x.Length != y.Length)
		{
			return false;
		}
		for (int i = 0; i < x.Length; i++)
		{
			if (x[i] != y[i])
			{
				return false;
			}
		}
		return true;
	}

	public int GetHashCode(byte[] obj)
	{
		int num = 0;
		for (int i = 0; i < obj.Length; i++)
		{
			num <<= 8;
			num += obj[i];
		}
		return num;
	}
}
