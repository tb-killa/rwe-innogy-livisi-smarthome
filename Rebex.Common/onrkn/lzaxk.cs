using System;
using System.Security.Cryptography;

namespace onrkn;

internal class lzaxk : ICryptoTransform, IDisposable
{
	private byte[] fubdk;

	private int ycajc;

	private int jnkwl;

	private byte[] okjzv;

	public bool CanReuseTransform => true;

	public bool CanTransformMultipleBlocks => true;

	public int InputBlockSize => 1;

	public int OutputBlockSize => 1;

	public lzaxk(byte[] key)
	{
		okjzv = (byte[])key.Clone();
		fubdk = new byte[256];
		bjlwn();
	}

	private void bjlwn()
	{
		ycajc = 0;
		jnkwl = 0;
		int num = 0;
		if (num != 0)
		{
			goto IL_0013;
		}
		goto IL_0021;
		IL_0013:
		fubdk[num] = (byte)num;
		num++;
		goto IL_0021;
		IL_0021:
		if (num < 256)
		{
			goto IL_0013;
		}
		int num2 = 0;
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_0030;
		}
		goto IL_0082;
		IL_0082:
		if (num3 >= 256)
		{
			return;
		}
		goto IL_0030;
		IL_0030:
		num2 = (okjzv[num3 % okjzv.Length] + fubdk[num3] + num2) & 0xFF;
		byte b = fubdk[num3];
		fubdk[num3] = fubdk[num2];
		fubdk[num2] = b;
		num3++;
		goto IL_0082;
	}

	public int TransformBlock(byte[] input, int inputOffset, int count, byte[] output, int outputOffset)
	{
		if (input == null || 1 == 0)
		{
			throw new ArgumentNullException("input");
		}
		if (output == null || 1 == 0)
		{
			throw new ArgumentNullException("output");
		}
		if (inputOffset > input.Length)
		{
			throw new ArgumentException("Input offset is outside the bounds of an array.");
		}
		if (inputOffset < 0)
		{
			throw hifyx.nztrs("inputOffset", inputOffset, "Input offset is outside the bounds of an array.");
		}
		if (count < 0 || inputOffset + count > input.Length)
		{
			throw new ArgumentException("Count is outside the bounds of an array.", "count");
		}
		if (outputOffset < 0)
		{
			throw new CryptographicException("Output offset is outside the bounds of an array.");
		}
		if (outputOffset > output.Length)
		{
			throw new CryptographicException("Output offset is outside the bounds of an array.");
		}
		if (outputOffset + count > output.Length)
		{
			throw new CryptographicException("Output array is too small.");
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_00b7;
		}
		goto IL_015d;
		IL_015d:
		if (num < count)
		{
			goto IL_00b7;
		}
		return count;
		IL_00b7:
		ycajc = (ycajc + 1) & 0xFF;
		jnkwl = (fubdk[ycajc] + jnkwl) & 0xFF;
		byte b = fubdk[ycajc];
		fubdk[ycajc] = fubdk[jnkwl];
		fubdk[jnkwl] = b;
		int num2 = (fubdk[ycajc] + fubdk[jnkwl]) & 0xFF;
		output[outputOffset + num] = (byte)(input[inputOffset + num] ^ fubdk[num2]);
		num++;
		goto IL_015d;
	}

	public byte[] TransformFinalBlock(byte[] input, int inputOffset, int count)
	{
		if (input == null || 1 == 0)
		{
			throw new ArgumentNullException("input");
		}
		if (inputOffset > input.Length)
		{
			throw new ArgumentException("Input offset is outside the bounds of an array.");
		}
		if (inputOffset < 0)
		{
			throw hifyx.nztrs("inputOffset", inputOffset, "Input offset is outside the bounds of an array.");
		}
		if (count < 0 || inputOffset + count > input.Length)
		{
			throw new ArgumentException("Count is outside the bounds of an array.", "count");
		}
		byte[] array = new byte[count];
		TransformBlock(input, inputOffset, count, array, 0);
		bjlwn();
		return array;
	}

	public void Dispose()
	{
	}
}
