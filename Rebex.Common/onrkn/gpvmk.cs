using System;
using System.Security.Cryptography;

namespace onrkn;

internal class gpvmk : ICryptoTransform, IDisposable
{
	private byte[] dhwvt;

	private byte[] aqojr;

	private readonly PaddingMode hxxiu;

	private readonly CipherMode pafvw;

	private readonly int grmqq;

	private readonly icgnq bmbxc;

	private readonly bool tmyyg;

	private byte[] ojiek;

	private byte[] vsrda;

	private bool grqax;

	public bool CanReuseTransform => true;

	public bool CanTransformMultipleBlocks => true;

	public int InputBlockSize => grmqq;

	public int OutputBlockSize => grmqq;

	public gpvmk(icgnq transform, byte[] rgbIV, CipherMode mode, PaddingMode padding, bool encrypt)
	{
		bmbxc = transform;
		grmqq = transform.gzcbg;
		if (rgbIV != null && 0 == 0)
		{
			dhwvt = new byte[grmqq];
			aqojr = (byte[])rgbIV.Clone();
		}
		pafvw = mode;
		hxxiu = padding;
		tmyyg = encrypt;
		if (padding == PaddingMode.PKCS7)
		{
			ojiek = new byte[grmqq];
			vsrda = new byte[grmqq];
		}
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
		if (((count % grmqq != 0) ? true : false) || count <= 0)
		{
			throw new CryptographicException("Length of the data to transform is invalid.");
		}
		if (input != output)
		{
			Array.Copy(input, inputOffset, output, outputOffset, count);
		}
		if ((!tmyyg || 1 == 0) && ojiek != null)
		{
			if (grqax && 0 == 0)
			{
				Array.Copy(input, inputOffset + count - grmqq, vsrda, 0, grmqq);
				Array.Copy(input, inputOffset, output, outputOffset + grmqq, count - grmqq);
				Array.Copy(ojiek, 0, output, outputOffset, grmqq);
				byte[] array = ojiek;
				ojiek = vsrda;
				vsrda = array;
			}
			else
			{
				Array.Copy(input, inputOffset + count - grmqq, ojiek, 0, grmqq);
				count -= grmqq;
				grqax = true;
				if (count == 0 || 1 == 0)
				{
					return 0;
				}
				if (input != output)
				{
					Array.Copy(input, inputOffset, output, outputOffset, count);
				}
			}
		}
		else if (input != output)
		{
			Array.Copy(input, inputOffset, output, outputOffset, count);
		}
		count = wtvhz(output, outputOffset, count);
		return count;
	}

	private int wtvhz(byte[] p0, int p1, int p2)
	{
		int i = p1;
		int result = p2;
		if (!tmyyg || 1 == 0)
		{
			for (; p2 >= grmqq; i += grmqq, p1 += grmqq, p2 -= grmqq)
			{
				if (pafvw == CipherMode.CBC)
				{
					Array.Copy(p0, p1, dhwvt, 0, grmqq);
				}
				bmbxc.vyoid(p0, p1, p0, i);
				if (pafvw != CipherMode.CBC)
				{
					continue;
				}
				int num = 0;
				if (num != 0)
				{
					goto IL_005c;
				}
				goto IL_007e;
				IL_005c:
				p0[i + num] ^= aqojr[num];
				num++;
				goto IL_007e;
				IL_007e:
				if (num >= grmqq)
				{
					byte[] array = dhwvt;
					dhwvt = aqojr;
					aqojr = array;
					continue;
				}
				goto IL_005c;
			}
		}
		else
		{
			while (p2 > 0)
			{
				int num2;
				if (pafvw == CipherMode.CBC)
				{
					num2 = 0;
					if (num2 != 0)
					{
						goto IL_00e5;
					}
					goto IL_010b;
				}
				bmbxc.zpcqe(p0, p1, p0, i);
				goto IL_013a;
				IL_013a:
				p1 += grmqq;
				if (pafvw == CipherMode.CBC)
				{
					Array.Copy(p0, i, aqojr, 0, grmqq);
				}
				i += grmqq;
				p2 -= grmqq;
				continue;
				IL_00e5:
				aqojr[num2] ^= p0[p1 + num2];
				num2++;
				goto IL_010b;
				IL_010b:
				if (num2 < grmqq)
				{
					goto IL_00e5;
				}
				bmbxc.zpcqe(aqojr, 0, p0, i);
				goto IL_013a;
			}
		}
		return result;
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
		if (tmyyg && 0 == 0)
		{
			byte[] array;
			byte b;
			switch (hxxiu)
			{
			default:
				if (count % grmqq != 0 && 0 == 0)
				{
					throw new CryptographicException("Length of the data to transform is invalid.");
				}
				if (count == 0 || 1 == 0)
				{
					return new byte[0];
				}
				array = new byte[count];
				b = 0;
				if (b == 0)
				{
					break;
				}
				goto case PaddingMode.Zeros;
			case PaddingMode.Zeros:
				if (count == 0 || 1 == 0)
				{
					return new byte[0];
				}
				array = new byte[((count - 1) | (grmqq - 1)) + 1];
				b = 0;
				if (b == 0)
				{
					break;
				}
				goto case PaddingMode.PKCS7;
			case PaddingMode.PKCS7:
				array = new byte[(count | (grmqq - 1)) + 1];
				b = (byte)(array.Length - count);
				break;
			}
			Array.Copy(input, inputOffset, array, 0, count);
			for (int i = count; i < array.Length; i++)
			{
				array[i] = b;
			}
			byte[] array2 = new byte[array.Length];
			TransformBlock(array, 0, array.Length, array2, 0);
			return array2;
		}
		if ((count == 0 || 1 == 0) && (!grqax || 1 == 0))
		{
			if (hxxiu == PaddingMode.PKCS7)
			{
				throw new CryptographicException("Bad data.");
			}
			return new byte[0];
		}
		if (((count % grmqq != 0) ? true : false) || count < 0)
		{
			throw new CryptographicException("Length of the data to transform is invalid.");
		}
		byte[] array3;
		if (!grqax || 1 == 0)
		{
			array3 = new byte[count];
			Array.Copy(input, inputOffset, array3, 0, count);
		}
		else
		{
			array3 = new byte[count + grmqq];
			Array.Copy(ojiek, 0, array3, 0, grmqq);
			Array.Copy(input, inputOffset, array3, grmqq, count);
			count += grmqq;
		}
		wtvhz(array3, 0, count);
		if (hxxiu != PaddingMode.PKCS7)
		{
			return array3;
		}
		int num = array3[count - 1];
		if (count < num || num == 0 || 1 == 0)
		{
			throw new CryptographicException("Invalid padding.");
		}
		count--;
		int num2 = 1;
		if (num2 == 0)
		{
			goto IL_0245;
		}
		goto IL_0265;
		IL_0265:
		if (num2 >= num)
		{
			byte[] array4 = new byte[count];
			Array.Copy(array3, 0, array4, 0, count);
			return array4;
		}
		goto IL_0245;
		IL_0245:
		count--;
		if (array3[count] != num)
		{
			throw new CryptographicException("Invalid padding.");
		}
		num2++;
		goto IL_0265;
	}

	public void Dispose()
	{
	}
}
