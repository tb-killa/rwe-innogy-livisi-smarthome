using System;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class meytk
{
	private HashingAlgorithm lkfvn;

	public meytk(HashingAlgorithm hashAlg)
	{
		lkfvn = hashAlg;
	}

	public byte[] yldsk(byte[] p0, int p1)
	{
		return gjgvl(p0, 0, p0.Length, p1);
	}

	public byte[] gjgvl(byte[] p0, int p1, int p2, int p3)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("seed", "Value cannot be null.");
		}
		if (p3 < 0)
		{
			throw hifyx.nztrs("outputLength", p3, "Argument must not be negative number.");
		}
		byte[] array = new byte[p3];
		IHashTransform hashTransform = lkfvn.CreateTransform();
		try
		{
			byte[] array2 = new byte[4];
			int num = lkfvn.HashSize / 8;
			int num2 = 0;
			int num3 = 0;
			if (num3 != 0)
			{
				goto IL_0070;
			}
			goto IL_00be;
			IL_0070:
			efmgw(num3++, array2);
			hashTransform.Process(p0, p1, p2);
			hashTransform.Process(array2, 0, 4);
			byte[] hash = hashTransform.GetHash();
			hashTransform.Reset();
			int count = Math.Min(num, p3 - num2);
			Buffer.BlockCopy(hash, 0, array, num2, count);
			num2 += num;
			goto IL_00be;
			IL_00be:
			if (num2 >= p3)
			{
				return array;
			}
			goto IL_0070;
		}
		finally
		{
			if (hashTransform != null && 0 == 0)
			{
				hashTransform.Dispose();
			}
		}
	}

	private void efmgw(int p0, byte[] p1)
	{
		p1[3] = (byte)p0;
		p1[2] = (byte)(p0 >> 8);
		p1[1] = (byte)(p0 >> 16);
		p1[0] = (byte)(p0 >> 24);
	}

	public static byte[] trfox(HashingAlgorithmId p0, byte[] p1, int p2)
	{
		HashingAlgorithm hashingAlgorithm = new HashingAlgorithm(p0);
		try
		{
			meytk meytk2 = new meytk(hashingAlgorithm);
			return meytk2.yldsk(p1, p2);
		}
		finally
		{
			if (hashingAlgorithm != null && 0 == 0)
			{
				((IDisposable)hashingAlgorithm).Dispose();
			}
		}
	}
}
