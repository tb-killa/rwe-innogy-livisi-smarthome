using System;
using System.Security.Cryptography;

namespace onrkn;

internal class wkxwh : ICryptoTransform, IDisposable
{
	private readonly int kagdd;

	private readonly ICryptoTransform npiqw;

	private readonly byte[] djxmz;

	private readonly byte[] zihvi;

	public bool CanReuseTransform => npiqw.CanReuseTransform;

	public bool CanTransformMultipleBlocks => npiqw.CanTransformMultipleBlocks;

	public int InputBlockSize => npiqw.InputBlockSize;

	public int OutputBlockSize => npiqw.OutputBlockSize;

	public wkxwh(ICryptoTransform transform, byte[] counter)
	{
		kagdd = counter.Length;
		djxmz = counter;
		npiqw = transform;
		zihvi = new byte[4096 / kagdd * kagdd];
	}

	public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
	{
		if (inputCount % kagdd != 0 && 0 == 0)
		{
			throw new ArgumentException("Input size must be divisible by block size");
		}
		int result = inputCount;
		while (inputCount > 0)
		{
			int num = Math.Min(zihvi.Length, inputCount);
			int num2 = 0;
			int num3 = num;
			while (num3 > 0)
			{
				Array.Copy(djxmz, 0, zihvi, num2, kagdd);
				num2 += kagdd;
				num3 -= kagdd;
				int num4 = kagdd;
				while (++djxmz[--num4] == 0 && num4 != 0)
				{
				}
			}
			npiqw.TransformBlock(zihvi, 0, num, zihvi, 0);
			int num5 = 0;
			if (num5 != 0)
			{
				goto IL_00d5;
			}
			goto IL_00fc;
			IL_00d5:
			outputBuffer[outputOffset++] = (byte)(inputBuffer[inputOffset++] ^ zihvi[num5]);
			num5++;
			goto IL_00fc;
			IL_00fc:
			if (num5 >= num)
			{
				inputCount -= num;
				continue;
			}
			goto IL_00d5;
		}
		return result;
	}

	public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
	{
		throw new NotSupportedException();
	}

	public void Dispose()
	{
		if (npiqw != null && 0 == 0)
		{
			npiqw.Dispose();
		}
	}
}
