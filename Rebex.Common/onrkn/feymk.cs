using System;
using Rebex;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class feymk : DeriveBytes
{
	private const int bzcpt = 32;

	private readonly byte[] hcvrd;

	private readonly int pqkum;

	private byte[] iegon;

	private int qpocs;

	private byte[] ndygk;

	private int yokrc;

	public byte[] nnkdm
	{
		get
		{
			return ndygk;
		}
		private set
		{
			ndygk = value;
		}
	}

	public int saopy
	{
		get
		{
			return yokrc;
		}
		private set
		{
			yokrc = value;
		}
	}

	public feymk(string password, byte[] salt, int iterations, int keySize)
		: this((password != null) ? EncodingTools.UTF8.GetBytes(password) : null, salt, iterations, keySize)
	{
	}

	public feymk(byte[] password, byte[] salt, int iterations, int keySize)
	{
		if (password == null || 1 == 0)
		{
			throw new ArgumentNullException("password");
		}
		if (salt == null || 1 == 0)
		{
			throw new ArgumentNullException("salt");
		}
		if (salt.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Salt cannot be empty.");
		}
		if (iterations < 1)
		{
			throw new ArgumentOutOfRangeException("iterations", "Iterations parameter must be greater than zero.");
		}
		if (keySize < 1)
		{
			throw new ArgumentOutOfRangeException("keySize", "Key size must be a positive number.");
		}
		hcvrd = password;
		nnkdm = salt;
		saopy = iterations;
		pqkum = keySize;
	}

	private static HashingAlgorithm wfwkr()
	{
		return new HashingAlgorithm(HashingAlgorithmId.SHA512);
	}

	public static byte[] vmpsb(byte[] p0, byte[] p1, int p2, int p3)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("password");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("salt");
		}
		if (p1.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Salt cannot be empty.");
		}
		if (p2 < 1)
		{
			throw new ArgumentOutOfRangeException("keySize", "Key size must be greater than zero.");
		}
		if (p3 < 1)
		{
			throw new ArgumentOutOfRangeException("rounds", "Rounds parameter must be greater than zero.");
		}
		byte[] array = new byte[p2];
		byte[] array2 = new byte[32];
		int num = (array.Length - 1) / array2.Length + 1;
		int num2 = (array.Length - 1) / num + 1;
		byte[] array3 = new byte[4];
		byte[] p4 = wfwkr().ComputeHash(p0);
		int num3 = p2;
		int num4 = 1;
		if (num4 == 0)
		{
			goto IL_00b6;
		}
		goto IL_01b6;
		IL_01a4:
		int num5;
		if (num5 < num2)
		{
			goto IL_0186;
		}
		goto IL_01a9;
		IL_0186:
		int num6 = num5 * num + (num4 - 1);
		if (num6 <= p2)
		{
			array[num6] = array2[num5];
			num5++;
			goto IL_01a4;
		}
		goto IL_01a9;
		IL_01a9:
		num3 -= num5;
		num4++;
		goto IL_01b6;
		IL_01b6:
		if (num3 > 0)
		{
			goto IL_00b6;
		}
		return array;
		IL_00b6:
		jlfbq.lyicr(array3, 0, num4);
		HashingAlgorithm hashingAlgorithm = wfwkr();
		byte[] p5;
		try
		{
			IHashTransform hashTransform = hashingAlgorithm.CreateTransform();
			hashTransform.Process(p1, 0, p1.Length);
			hashTransform.Process(array3, 0, array3.Length);
			p5 = hashTransform.GetHash();
		}
		finally
		{
			if (hashingAlgorithm != null && 0 == 0)
			{
				((IDisposable)hashingAlgorithm).Dispose();
			}
		}
		byte[] array4 = csemh(p4, p5);
		Array.Copy(array4, array2, array2.Length);
		int num7 = 1;
		while (num7 < p3)
		{
			p5 = wfwkr().ComputeHash(array4);
			array4 = csemh(p4, p5);
			int num8 = 0;
			if (num8 != 0)
			{
				goto IL_0144;
			}
			goto IL_0164;
			IL_0144:
			array2[num8] ^= array4[num8];
			num8++;
			goto IL_0164;
			IL_0164:
			if (num8 >= array2.Length)
			{
				num7++;
				continue;
			}
			goto IL_0144;
		}
		num2 = Math.Min(num2, num3);
		num5 = 0;
		if (num5 != 0)
		{
			goto IL_0186;
		}
		goto IL_01a4;
	}

	public static byte[] csemh(byte[] p0, byte[] p1)
	{
		byte[] array = new byte[32]
		{
			79, 120, 121, 99, 104, 114, 111, 109, 97, 116,
			105, 99, 66, 108, 111, 119, 102, 105, 115, 104,
			83, 119, 97, 116, 68, 121, 110, 97, 109, 105,
			116, 101
		};
		fficc fficc2 = new fficc();
		sgdyb p2 = new sgdyb(p1);
		sgdyb sgdyb2 = new sgdyb(p0);
		fficc2.mcbkd(p2, sgdyb2);
		int num = 0;
		if (num != 0)
		{
			goto IL_0038;
		}
		goto IL_004c;
		IL_0038:
		fficc2.itgrz(p2);
		fficc2.itgrz(sgdyb2);
		num++;
		goto IL_004c;
		IL_004c:
		if (num < 64)
		{
			goto IL_0038;
		}
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0059;
		}
		goto IL_007d;
		IL_008a:
		int num3;
		byte b = array[num3];
		array[num3] = array[num3 + 3];
		array[num3 + 3] = b;
		b = array[num3 + 1];
		array[num3 + 1] = array[num3 + 2];
		array[num3 + 2] = b;
		num3 += 4;
		goto IL_00c4;
		IL_0059:
		int num4 = 0;
		if (num4 != 0)
		{
			goto IL_0060;
		}
		goto IL_0070;
		IL_0060:
		fficc2.ihrwr(array, num4, p2: false);
		num4 += 8;
		goto IL_0070;
		IL_0070:
		if (num4 < array.Length)
		{
			goto IL_0060;
		}
		num2++;
		goto IL_007d;
		IL_00c4:
		if (num3 < array.Length)
		{
			goto IL_008a;
		}
		return array;
		IL_007d:
		if (num2 < 64)
		{
			goto IL_0059;
		}
		num3 = 0;
		if (num3 != 0)
		{
			goto IL_008a;
		}
		goto IL_00c4;
	}

	public override byte[] GetBytes(int cb)
	{
		if (cb > pqkum - qpocs)
		{
			throw new ArgumentException("Not enough bytes left.", "cb");
		}
		if (iegon == null || 1 == 0)
		{
			iegon = vmpsb(hcvrd, nnkdm, pqkum, saopy);
		}
		byte[] array = new byte[cb];
		Array.Copy(iegon, qpocs, array, 0, cb);
		qpocs += cb;
		return array;
	}

	public override void Reset()
	{
		qpocs = 0;
	}
}
