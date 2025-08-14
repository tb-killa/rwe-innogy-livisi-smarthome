using System;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class xjmlr
{
	private readonly HashingAlgorithm wwksd;

	private readonly meytk uefoy;

	private readonly int sgfky;

	private readonly int gvgry;

	public xjmlr(HashingAlgorithm hashAlg, meytk mgf, int saltLength)
	{
		wwksd = hashAlg;
		uefoy = mgf;
		sgfky = saltLength;
		gvgry = hashAlg.HashSize / 8;
	}

	public byte[] lnuvw(RSA p0, byte[] p1)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("hash", "Value cannot be null.");
		}
		vriff(p0.KeySize, out var p2, out var p3);
		if (p3 < gvgry + sgfky + 2)
		{
			throw new ArgumentException("Key size and hash size mismatch.", "keySize");
		}
		byte[] array = new byte[p3];
		byte[] randomBytes = CryptoHelper.GetRandomBytes(sgfky);
		byte[] p4 = jlfbq.ttbrb(new byte[8], p1, randomBytes);
		byte[] array2 = nqnlq(p4);
		int num = p3 - sgfky - gvgry - 2;
		byte[] array3 = jlfbq.ttbrb(new byte[num], new byte[1] { 1 }, randomBytes);
		byte[] p5 = uefoy.yldsk(array2, p3 - gvgry - 1);
		pymma(array3, 0, p5, 0, array, 0, array3.Length);
		Buffer.BlockCopy(array2, 0, array, array3.Length, gvgry);
		array[p3 - 1] = 188;
		int num2 = 8 * p3 - p2;
		array[0] = (byte)(((array[0] << num2) & 0xFF) >> num2);
		byte[] p6 = p0.DecryptValue(array);
		return jlfbq.tykuz(p6, p3);
	}

	public bool onzqc(RSA p0, byte[] p1, byte[] p2)
	{
		if (p2 == null || 1 == 0)
		{
			throw new ArgumentNullException("signature", "Value cannot be null.");
		}
		vriff(p0.KeySize, out var p3, out var p4);
		if (p2.Length != p4)
		{
			return false;
		}
		if (p4 < gvgry + sgfky + 2)
		{
			return false;
		}
		byte[] p5 = p0.EncryptValue(p2);
		p5 = jlfbq.tykuz(p5, p4);
		if (p5[p4 - 1] != 188)
		{
			return false;
		}
		byte[] array = p5;
		int num = 8 * p4 - p3;
		if (array[0] >> 8 - num != 0 && 0 == 0)
		{
			return false;
		}
		int num2 = p4 - gvgry - 1;
		byte[] p6 = uefoy.gjgvl(array, num2, gvgry, num2);
		byte[] array2 = new byte[num2];
		pymma(array, 0, p6, 0, array2, 0, num2);
		array2[0] = (byte)(((array2[0] << num) & 0xFF) >> num);
		int num3 = p4 - gvgry - sgfky - 2;
		if (array2[num3] != 1)
		{
			return false;
		}
		int num4 = 0;
		if (num4 != 0)
		{
			goto IL_0104;
		}
		goto IL_011b;
		IL_0104:
		if (array2[num4] != 0 && 0 == 0)
		{
			return false;
		}
		num4++;
		goto IL_011b;
		IL_011b:
		if (num4 >= num3)
		{
			byte[] array3 = array2.wwots(num2 - sgfky, sgfky);
			byte[] p7 = jlfbq.ttbrb(new byte[8], p1, array3);
			byte[] p8 = nqnlq(p7);
			return jlfbq.inlao(array, num2, p8, 0, gvgry);
		}
		goto IL_0104;
	}

	private void pymma(byte[] p0, int p1, byte[] p2, int p3, byte[] p4, int p5, int p6)
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

	private byte[] nqnlq(byte[] p0)
	{
		return wwksd.ComputeHash(p0);
	}

	private void vriff(int p0, out int p1, out int p2)
	{
		p1 = p0 - 1;
		p2 = (p1 + 7) / 8;
	}
}
