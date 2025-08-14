using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace onrkn;

internal class zlqaj : ICryptoTransform, IDisposable
{
	public const int aqtft = 16;

	public const int ykgbd = 16;

	public const int lrwrf = 15;

	public const int mearg = 15;

	public const int msznd = 16384;

	public const int xyiuk = 4;

	public const int gpegw = 3;

	public const int lerwg = 1024;

	private const string kmczc = "Input size must be divisible by block size";

	private readonly ICryptoTransform wlwht;

	private readonly byte[] svkvr;

	private readonly byte[] vkaut;

	private bool bdfxm;

	private ayjol mpoba;

	public bool CanReuseTransform => wlwht.CanReuseTransform;

	public bool CanTransformMultipleBlocks => wlwht.CanTransformMultipleBlocks;

	public int InputBlockSize => wlwht.InputBlockSize;

	public int OutputBlockSize => wlwht.OutputBlockSize;

	public zlqaj(ICryptoTransform transform, byte[] counter)
	{
		svkvr = counter;
		wlwht = transform;
		vkaut = sxztb<byte>.ahblv.vfhlp(16384);
		mpoba = peekn.nmjtk(vkaut, 0, vkaut.Length, p3: false);
	}

	public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
	{
		if ((inputCount & 0xF) != 0 && 0 == 0)
		{
			throw new ArgumentException("Input size must be divisible by block size");
		}
		if (inputCount == 0 || 1 == 0)
		{
			return inputCount;
		}
		ayjol ayjol2 = peekn.nmjtk(inputBuffer, inputOffset, inputCount, p3: true);
		ayjol ayjol3 = peekn.nmjtk(outputBuffer, inputOffset, inputCount, p3: false);
		try
		{
			int num = ((!ayjol2.ygpwi || 1 == 0) ? (inputOffset >> 3) : 0);
			int num2 = ((!ayjol3.ygpwi || 1 == 0) ? (outputOffset >> 3) : 0);
			int num3 = inputCount;
			int num4 = 15;
			byte b = svkvr[num4];
			while (num3 > 0)
			{
				int num5 = Math.Min(16384, num3);
				int num6 = num5 >> 3;
				int num7 = 0;
				int num8 = num5;
				while (num8 > 0)
				{
					Buffer.BlockCopy(svkvr, 0, vkaut, num7, 16);
					num7 += 16;
					num8 -= 16;
					if (b == byte.MaxValue)
					{
						do
						{
							svkvr[num4] = 0;
							num4--;
							b = svkvr[num4];
						}
						while (b == byte.MaxValue && num4 >= 0);
						b = (svkvr[num4] = (byte)(b + 1));
						num4 = 15;
						b = 0;
						if (b == 0)
						{
							continue;
						}
					}
					b = (svkvr[num4] = (byte)(b + 1));
				}
				wlwht.TransformBlock(vkaut, 0, num5, vkaut, 0);
				if (mpoba.ygpwi && 0 == 0)
				{
					Buffer.BlockCopy(vkaut, 0, mpoba.rjqep, 0, num5);
				}
				int num9 = 0;
				ulong[] rjqep = ayjol3.rjqep;
				ulong[] rjqep2 = ayjol2.rjqep;
				ulong[] rjqep3 = mpoba.rjqep;
				int num10 = num6 - 16;
				while (num9 < num10)
				{
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
				}
				num10 -= 8;
				while (num9 < num10)
				{
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
					rjqep[num2++] = rjqep2[num++] ^ rjqep3[num9++];
				}
				while (num9 < num6)
				{
					rjqep[num++] = rjqep2[num2++] ^ rjqep3[num9++];
				}
				num3 -= num5;
			}
			if (ayjol3.ygpwi && 0 == 0)
			{
				Buffer.BlockCopy(ayjol3.rjqep, 0, outputBuffer, outputOffset, inputCount);
			}
			return inputCount;
		}
		finally
		{
			ayjol2.uqjka();
			ayjol3.uqjka();
		}
	}

	public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
	{
		throw new NotSupportedException();
	}

	public void Dispose()
	{
		if (!bdfxm)
		{
			if (wlwht != null && 0 == 0)
			{
				wlwht.Dispose();
			}
			if (vkaut != null && 0 == 0)
			{
				Array.Clear(vkaut, 0, vkaut.Length);
				sxztb<byte>.ahblv.uqydw(vkaut);
			}
			mpoba.uqjka();
			bdfxm = true;
		}
	}

	[Conditional("DEBUG")]
	private void iawvg()
	{
		if (bdfxm && 0 == 0)
		{
			throw new ObjectDisposedException("AesCtrTransform");
		}
	}
}
