using System;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class jrbbo
{
	private readonly byte[] jkvmk;

	public jrbbo(byte[] key)
	{
		if (key == null || 1 == 0)
		{
			throw new ArgumentNullException("key");
		}
		if (key.Length > 64)
		{
			key = HashingAlgorithm.ComputeHash(HashingAlgorithmId.MD5, key);
		}
		jkvmk = key;
	}

	public byte[] eyirg(byte[] p0)
	{
		HashingAlgorithm hashingAlgorithm = new HashingAlgorithm(HashingAlgorithmId.MD5);
		byte[] array = new byte[64 + p0.Length];
		byte[] array2 = new byte[64 + hashingAlgorithm.HashSize / 8];
		int num = 0;
		if (num != 0)
		{
			goto IL_002d;
		}
		goto IL_003b;
		IL_002d:
		array[num] = 54;
		array2[num] = 92;
		num++;
		goto IL_003b;
		IL_003b:
		if (num < 64)
		{
			goto IL_002d;
		}
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0047;
		}
		goto IL_0089;
		IL_0089:
		if (num2 >= jkvmk.Length)
		{
			p0.CopyTo(array, 64);
			p0 = hashingAlgorithm.ComputeHash(array);
			p0.CopyTo(array2, 64);
			return hashingAlgorithm.ComputeHash(array2);
		}
		goto IL_0047;
		IL_0047:
		array[num2] ^= jkvmk[num2];
		array2[num2] ^= jkvmk[num2];
		num2++;
		goto IL_0089;
	}
}
