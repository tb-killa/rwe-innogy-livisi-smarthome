using System;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;

namespace onrkn;

internal abstract class jtxhe
{
	private static class wagab
	{
		public static RandomNumberGenerator holra = CryptoHelper.CreateRandomNumberGenerator();

		public static byte[] whebf = new byte[1024];

		public static int qdrcj = 0;
	}

	private const int xukbi = 1024;

	private jtxhe()
	{
	}

	public static uint lhazq(byte[] p0, int p1)
	{
		return (uint)((p0[p1] << 24) + (p0[p1 + 1] << 16) + (p0[p1 + 2] << 8) + p0[p1 + 3]);
	}

	public static void ubsib(byte[] p0, int p1, int p2)
	{
		lock (wagab.holra)
		{
			while (p2 > 0)
			{
				if (wagab.qdrcj == 0 || 1 == 0)
				{
					wagab.holra.GetBytes(wagab.whebf);
					wagab.qdrcj = 1024;
				}
				int num = p2;
				if (num > wagab.qdrcj)
				{
					num = wagab.qdrcj;
				}
				Array.Copy(wagab.whebf, 1024 - wagab.qdrcj, p0, p1, num);
				p2 -= num;
				p1 += num;
				wagab.qdrcj -= num;
			}
		}
	}

	public static int kexzb(byte[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			return 0;
		}
		int num = p0.Length;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0016;
		}
		goto IL_002b;
		IL_0039:
		return num;
		IL_002b:
		if (num2 >= p0.Length)
		{
			goto IL_0039;
		}
		goto IL_0016;
		IL_0016:
		if (p0[num2] == 0 || 1 == 0)
		{
			num--;
			num2++;
			goto IL_002b;
		}
		goto IL_0039;
	}

	public static bool hbsgb(byte[] p0, byte[] p1)
	{
		if (p0 == null || false || p1 == null)
		{
			return false;
		}
		if (p0.Length != p1.Length)
		{
			return false;
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0027;
		}
		goto IL_0035;
		IL_0027:
		if (p0[num] != p1[num])
		{
			return false;
		}
		num++;
		goto IL_0035;
		IL_0035:
		if (num < p0.Length)
		{
			goto IL_0027;
		}
		return true;
	}
}
