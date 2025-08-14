using System.Security.Cryptography;

namespace onrkn;

internal class pucov : icgnq
{
	private const int sbybe = 8;

	private int[] uibnn;

	private static readonly byte[] yhius = new byte[256]
	{
		217, 120, 249, 196, 25, 221, 181, 237, 40, 233,
		253, 121, 74, 160, 216, 157, 198, 126, 55, 131,
		43, 118, 83, 142, 98, 76, 100, 136, 68, 139,
		251, 162, 23, 154, 89, 245, 135, 179, 79, 19,
		97, 69, 109, 141, 9, 129, 125, 50, 189, 143,
		64, 235, 134, 183, 123, 11, 240, 149, 33, 34,
		92, 107, 78, 130, 84, 214, 101, 147, 206, 96,
		178, 28, 115, 86, 192, 20, 167, 140, 241, 220,
		18, 117, 202, 31, 59, 190, 228, 209, 66, 61,
		212, 48, 163, 60, 182, 38, 111, 191, 14, 218,
		70, 105, 7, 87, 39, 242, 29, 155, 188, 148,
		67, 3, 248, 17, 199, 246, 144, 239, 62, 231,
		6, 195, 213, 47, 200, 102, 30, 215, 8, 232,
		234, 222, 128, 82, 238, 247, 132, 170, 114, 172,
		53, 77, 106, 42, 150, 26, 210, 113, 90, 21,
		73, 116, 75, 159, 208, 94, 4, 24, 164, 236,
		194, 224, 65, 110, 15, 81, 203, 204, 36, 145,
		175, 80, 161, 244, 112, 57, 153, 124, 58, 133,
		35, 184, 180, 122, 252, 2, 54, 91, 37, 85,
		151, 49, 45, 93, 250, 152, 227, 138, 146, 174,
		5, 223, 41, 16, 103, 108, 186, 201, 211, 0,
		230, 207, 225, 158, 168, 44, 99, 22, 1, 63,
		88, 226, 137, 169, 13, 56, 52, 27, 171, 51,
		255, 176, 187, 72, 12, 95, 185, 177, 205, 46,
		197, 243, 219, 71, 229, 165, 156, 119, 10, 166,
		32, 104, 254, 127, 193, 173
	};

	public int gzcbg => 8;

	internal static KeySizes[] carfl()
	{
		return new KeySizes[1]
		{
			new KeySizes(8, 1024, 8)
		};
	}

	internal static KeySizes[] cucpv()
	{
		return new KeySizes[1]
		{
			new KeySizes(64, 64, 0)
		};
	}

	public pucov(byte[] rgbKey, int effectiveKeySize)
	{
		if (effectiveKeySize == 0 || 1 == 0)
		{
			effectiveKeySize = rgbKey.Length * 8;
		}
		uibnn = jfzib(rgbKey, effectiveKeySize);
	}

	private int[] jfzib(byte[] p0, int p1)
	{
		int num = (p1 + 7) / 8;
		int num2 = 255 >> (7 & -p1);
		int[] array = new int[128];
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_0028;
		}
		goto IL_0032;
		IL_0028:
		array[num3] = p0[num3];
		num3++;
		goto IL_0032;
		IL_0032:
		if (num3 < p0.Length)
		{
			goto IL_0028;
		}
		int num4 = 0;
		for (int i = p0.Length; i < array.Length; i++)
		{
			array[i] = yhius[(array[i - 1] + array[num4]) & 0xFF];
			num4++;
		}
		array[128 - num] = yhius[array[128 - num] & num2];
		for (int num5 = 127 - num; num5 >= 0; num5--)
		{
			array[num5] = yhius[array[num5 + 1] ^ array[num5 + num]];
		}
		int[] array2 = new int[64];
		int num6 = 0;
		if (num6 != 0)
		{
			goto IL_00c4;
		}
		goto IL_00e4;
		IL_00e4:
		if (num6 < array2.Length)
		{
			goto IL_00c4;
		}
		return array2;
		IL_00c4:
		array2[num6] = array[2 * num6] + 256 * array[2 * num6 + 1];
		num6++;
		goto IL_00e4;
	}

	private int xcxxu(int p0, int p1)
	{
		p0 &= 0xFFFF;
		return (p0 << p1) | (p0 >> 16 - p1);
	}

	public void zpcqe(byte[] p0, int p1, byte[] p2, int p3)
	{
		int[] array = new int[4]
		{
			p0[p1] + (p0[p1 + 1] << 8),
			p0[p1 + 2] + (p0[p1 + 3] << 8),
			p0[p1 + 4] + (p0[p1 + 5] << 8),
			p0[p1 + 6] + (p0[p1 + 7] << 8)
		};
		int num = 0;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0053;
		}
		goto IL_0103;
		IL_0053:
		array[0] = xcxxu(array[0] + (array[1] & ~array[3]) + (array[2] & array[3]) + uibnn[num], 1);
		num++;
		array[1] = xcxxu(array[1] + (array[2] & ~array[0]) + (array[3] & array[0]) + uibnn[num], 2);
		num++;
		array[2] = xcxxu(array[2] + (array[3] & ~array[1]) + (array[0] & array[1]) + uibnn[num], 3);
		num++;
		array[3] = xcxxu(array[3] + (array[0] & ~array[2]) + (array[1] & array[2]) + uibnn[num], 5);
		num++;
		num2++;
		goto IL_0103;
		IL_0103:
		if (num2 < 5)
		{
			goto IL_0053;
		}
		array[0] += uibnn[array[3] & 0x3F];
		array[1] += uibnn[array[0] & 0x3F];
		array[2] += uibnn[array[1] & 0x3F];
		array[3] += uibnn[array[2] & 0x3F];
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_0192;
		}
		goto IL_0242;
		IL_02d3:
		array[0] = xcxxu(array[0] + (array[1] & ~array[3]) + (array[2] & array[3]) + uibnn[num], 1);
		num++;
		array[1] = xcxxu(array[1] + (array[2] & ~array[0]) + (array[3] & array[0]) + uibnn[num], 2);
		num++;
		array[2] = xcxxu(array[2] + (array[3] & ~array[1]) + (array[0] & array[1]) + uibnn[num], 3);
		num++;
		array[3] = xcxxu(array[3] + (array[0] & ~array[2]) + (array[1] & array[2]) + uibnn[num], 5);
		num++;
		int num4 = num4 + 1;
		goto IL_0385;
		IL_0192:
		array[0] = xcxxu(array[0] + (array[1] & ~array[3]) + (array[2] & array[3]) + uibnn[num], 1);
		num++;
		array[1] = xcxxu(array[1] + (array[2] & ~array[0]) + (array[3] & array[0]) + uibnn[num], 2);
		num++;
		array[2] = xcxxu(array[2] + (array[3] & ~array[1]) + (array[0] & array[1]) + uibnn[num], 3);
		num++;
		array[3] = xcxxu(array[3] + (array[0] & ~array[2]) + (array[1] & array[2]) + uibnn[num], 5);
		num++;
		num3++;
		goto IL_0242;
		IL_0242:
		if (num3 < 6)
		{
			goto IL_0192;
		}
		array[0] += uibnn[array[3] & 0x3F];
		array[1] += uibnn[array[0] & 0x3F];
		array[2] += uibnn[array[1] & 0x3F];
		array[3] += uibnn[array[2] & 0x3F];
		num4 = 0;
		if (num4 != 0)
		{
			goto IL_02d3;
		}
		goto IL_0385;
		IL_0385:
		if (num4 >= 5)
		{
			p2[p3] = (byte)array[0];
			p2[p3 + 1] = (byte)(array[0] >> 8);
			p2[p3 + 2] = (byte)array[1];
			p2[p3 + 3] = (byte)(array[1] >> 8);
			p2[p3 + 4] = (byte)array[2];
			p2[p3 + 5] = (byte)(array[2] >> 8);
			p2[p3 + 6] = (byte)array[3];
			p2[p3 + 7] = (byte)(array[3] >> 8);
			return;
		}
		goto IL_02d3;
	}

	public void vyoid(byte[] p0, int p1, byte[] p2, int p3)
	{
		int[] array = new int[4]
		{
			p0[p1] + (p0[p1 + 1] << 8),
			p0[p1 + 2] + (p0[p1 + 3] << 8),
			p0[p1 + 4] + (p0[p1 + 5] << 8),
			p0[p1 + 6] + (p0[p1 + 7] << 8)
		};
		int num = 63;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0054;
		}
		goto IL_0108;
		IL_0054:
		array[3] = xcxxu(array[3], 11) - ((array[0] & ~array[2]) + (array[1] & array[2]) + uibnn[num]);
		num--;
		array[2] = xcxxu(array[2], 13) - ((array[3] & ~array[1]) + (array[0] & array[1]) + uibnn[num]);
		num--;
		array[1] = xcxxu(array[1], 14) - ((array[2] & ~array[0]) + (array[3] & array[0]) + uibnn[num]);
		num--;
		array[0] = xcxxu(array[0], 15) - ((array[1] & ~array[3]) + (array[2] & array[3]) + uibnn[num]);
		num--;
		num2++;
		goto IL_0108;
		IL_0108:
		if (num2 < 5)
		{
			goto IL_0054;
		}
		array[3] -= uibnn[array[2] & 0x3F];
		array[2] -= uibnn[array[1] & 0x3F];
		array[1] -= uibnn[array[0] & 0x3F];
		array[0] -= uibnn[array[3] & 0x3F];
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_0197;
		}
		goto IL_024b;
		IL_02dc:
		array[3] = xcxxu(array[3], 11) - ((array[0] & ~array[2]) + (array[1] & array[2]) + uibnn[num]);
		num--;
		array[2] = xcxxu(array[2], 13) - ((array[3] & ~array[1]) + (array[0] & array[1]) + uibnn[num]);
		num--;
		array[1] = xcxxu(array[1], 14) - ((array[2] & ~array[0]) + (array[3] & array[0]) + uibnn[num]);
		num--;
		array[0] = xcxxu(array[0], 15) - ((array[1] & ~array[3]) + (array[2] & array[3]) + uibnn[num]);
		num--;
		int num4 = num4 + 1;
		goto IL_0392;
		IL_0197:
		array[3] = xcxxu(array[3], 11) - ((array[0] & ~array[2]) + (array[1] & array[2]) + uibnn[num]);
		num--;
		array[2] = xcxxu(array[2], 13) - ((array[3] & ~array[1]) + (array[0] & array[1]) + uibnn[num]);
		num--;
		array[1] = xcxxu(array[1], 14) - ((array[2] & ~array[0]) + (array[3] & array[0]) + uibnn[num]);
		num--;
		array[0] = xcxxu(array[0], 15) - ((array[1] & ~array[3]) + (array[2] & array[3]) + uibnn[num]);
		num--;
		num3++;
		goto IL_024b;
		IL_024b:
		if (num3 < 6)
		{
			goto IL_0197;
		}
		array[3] -= uibnn[array[2] & 0x3F];
		array[2] -= uibnn[array[1] & 0x3F];
		array[1] -= uibnn[array[0] & 0x3F];
		array[0] -= uibnn[array[3] & 0x3F];
		num4 = 0;
		if (num4 != 0)
		{
			goto IL_02dc;
		}
		goto IL_0392;
		IL_0392:
		if (num4 >= 5)
		{
			p2[p3] = (byte)array[0];
			p2[p3 + 1] = (byte)(array[0] >> 8);
			p2[p3 + 2] = (byte)array[1];
			p2[p3 + 3] = (byte)(array[1] >> 8);
			p2[p3 + 4] = (byte)array[2];
			p2[p3 + 5] = (byte)(array[2] >> 8);
			p2[p3 + 6] = (byte)array[3];
			p2[p3 + 7] = (byte)(array[3] >> 8);
			return;
		}
		goto IL_02dc;
	}
}
