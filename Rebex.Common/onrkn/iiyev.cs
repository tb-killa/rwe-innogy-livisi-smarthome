using System;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class iiyev
{
	private static readonly byte[] srnge = new byte[0];

	private HashingAlgorithm smgil;

	private meytk yyxpd;

	public iiyev(HashingAlgorithm hashAlg, meytk mgf)
	{
		smgil = hashAlg;
		yyxpd = mgf;
	}

	public byte[] qvkih(int p0, byte[] p1, byte[] p2)
	{
		return qgeir(p0, p1, 0, p1.Length, p2);
	}

	public byte[] qgeir(int p0, byte[] p1, int p2, int p3, byte[] p4)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("message", "Value cannot be null.");
		}
		byte[] array = p4;
		if (array == null || 1 == 0)
		{
			array = srnge;
		}
		p4 = array;
		int num = smgil.HashSize / 8;
		int num2 = p0 - 2 * num - 2 - p3;
		if (p0 < 2 * num + 2)
		{
			throw new ArgumentException("Key size and hash size mismatch.", "keySize");
		}
		if (num2 < 0)
		{
			throw new ArgumentException("Message too long for this key.", "message");
		}
		byte[] array2 = new byte[p0];
		byte[] p5 = smgil.ComputeHash(p4);
		byte[] p6 = new byte[num2];
		byte[] randomBytes = CryptoHelper.GetRandomBytes(num);
		byte[] array3 = yyxpd.yldsk(randomBytes, num + num2 + 1 + p3);
		int num3 = 0;
		int p7 = num + 1;
		jidsu(p5, 0, array3, num3, array2, ref p7, num);
		num3 += num;
		jidsu(p6, 0, array3, num3, array2, ref p7, num2);
		num3 += num2;
		array2[p7++] = (byte)(array3[num3++] ^ 1);
		jidsu(p1, p2, array3, num3, array2, ref p7, p3);
		byte[] p8 = yyxpd.gjgvl(array2, num + 1, array3.Length, num);
		ksywf(randomBytes, 0, p8, 0, array2, 1, num);
		return array2;
	}

	public byte[] mlcrj(int p0, byte[] p1)
	{
		return vkfkg(p0, p1, srnge);
	}

	public byte[] vkfkg(int p0, byte[] p1, byte[] p2)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("encodedMessage", "Value cannot be null.");
		}
		if (p2 == null || 1 == 0)
		{
			throw new ArgumentNullException("label", "Value cannot be null.");
		}
		if (p1.Length != p0)
		{
			throw new ArgumentException("Length of encoded message doesn't match the key size.", "encodedMessage");
		}
		int num = smgil.HashSize / 8;
		if (p0 < 2 * num + 2)
		{
			throw new ArgumentException("Key size and hash size mismatch.", "byteKeySize");
		}
		byte[] p3 = smgil.ComputeHash(p2);
		byte[] p4 = yyxpd.gjgvl(p1, num + 1, p0 - (num + 1), num);
		byte[] array = new byte[num];
		ksywf(p1, 1, p4, 0, array, 0, num);
		byte[] array2 = yyxpd.yldsk(array, p0 - (num + 1));
		byte[] array3 = new byte[array2.Length];
		ksywf(p1, num + 1, array2, 0, array3, 0, array3.Length);
		bool flag = false;
		if (p1[0] != 0 && 0 == 0)
		{
			flag = true;
		}
		if (!jlfbq.ccahg(p3, 0, array3, 0, num) || 1 == 0)
		{
			flag = true;
		}
		int i;
		for (i = num; i < array3.Length && array3[i] != 1; i++)
		{
			if (array3[i] != 0 && 0 == 0)
			{
				flag = true;
			}
		}
		if (++i >= array3.Length)
		{
			flag = true;
		}
		if (flag && 0 == 0)
		{
			throw new CryptographicException("Decryption error.");
		}
		return array3.wwots(i, array3.Length - i);
	}

	private void ksywf(byte[] p0, int p1, byte[] p2, int p3, byte[] p4, int p5, int p6)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_0032;
		IL_0006:
		p4[p5++] = (byte)(p0[p1++] ^ p2[p3++]);
		num++;
		goto IL_0032;
		IL_0032:
		if (num >= p6)
		{
			return;
		}
		goto IL_0006;
	}

	private void jidsu(byte[] p0, int p1, byte[] p2, int p3, byte[] p4, ref int p5, int p6)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_0033;
		IL_0006:
		p4[p5++] = (byte)(p0[p1++] ^ p2[p3++]);
		num++;
		goto IL_0033;
		IL_0033:
		if (num >= p6)
		{
			return;
		}
		goto IL_0006;
	}
}
