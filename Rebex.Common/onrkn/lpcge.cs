using System;
using System.Security.Cryptography;

namespace onrkn;

internal static class lpcge
{
	public static int yzaxk(string p0)
	{
		int num = bpkgq.kgsco(p0);
		if (num == 0 || 1 == 0)
		{
			throw new CryptographicException("Unsupported curve '" + p0 + "'.");
		}
		return (num + 7) / 8;
	}

	public static bgosr kkyit(byte[] p0)
	{
		int num = gfdoq(p0);
		byte[] array = new byte[num];
		byte[] array2 = new byte[num];
		Array.Copy(p0, 1, array, 0, num);
		Array.Copy(p0, 1 + num, array2, 0, num);
		return new bgosr
		{
			clbuc = array,
			egjif = array2
		};
	}

	public static byte[] spbnp(bgosr p0, string p1)
	{
		if (p0.clbuc == null || false || p0.egjif == null)
		{
			throw new CryptographicException("Missing EC public key.");
		}
		int num = yzaxk(p1);
		if (p0.clbuc.Length != num || p0.egjif.Length != num)
		{
			throw new CryptographicException("Unsupported EC key.");
		}
		byte[] array = new byte[1 + num * 2];
		array[0] = 4;
		p0.clbuc.CopyTo(array, 1);
		p0.egjif.CopyTo(array, 1 + num);
		return array;
	}

	public static int gfdoq(byte[] p0)
	{
		if (p0.Length == 0 || 1 == 0)
		{
			throw new CryptographicException("Invalid EC public key.");
		}
		int num = p0.Length / 2;
		if (p0[0] != 4 || num * 2 + 1 != p0.Length)
		{
			throw new CryptographicException("Unsupported EC key.");
		}
		return num;
	}
}
