using System;
using System.IO;

namespace onrkn;

internal static class njbal
{
	internal enum rasyd
	{
		qctbz = 1095517517,
		wqqbd = 1967544908
	}

	private const int pvfph = 4096;

	private const int argzc = 17;

	private const int wuyjp = 5;

	private const int fdedt = 32768;

	private const int mjsmw = 32767;

	private static readonly byte[] wfocq = new byte[207]
	{
		123, 92, 114, 116, 102, 49, 92, 97, 110, 115,
		105, 92, 109, 97, 99, 92, 100, 101, 102, 102,
		48, 92, 100, 101, 102, 116, 97, 98, 55, 50,
		48, 123, 92, 102, 111, 110, 116, 116, 98, 108,
		59, 125, 123, 92, 102, 48, 92, 102, 110, 105,
		108, 32, 92, 102, 114, 111, 109, 97, 110, 32,
		92, 102, 115, 119, 105, 115, 115, 32, 92, 102,
		109, 111, 100, 101, 114, 110, 32, 92, 102, 115,
		99, 114, 105, 112, 116, 32, 92, 102, 100, 101,
		99, 111, 114, 32, 77, 83, 32, 83, 97, 110,
		115, 32, 83, 101, 114, 105, 102, 83, 121, 109,
		98, 111, 108, 65, 114, 105, 97, 108, 84, 105,
		109, 101, 115, 32, 78, 101, 119, 32, 82, 111,
		109, 97, 110, 67, 111, 117, 114, 105, 101, 114,
		123, 92, 99, 111, 108, 111, 114, 116, 98, 108,
		92, 114, 101, 100, 48, 92, 103, 114, 101, 101,
		110, 48, 92, 98, 108, 117, 101, 48, 10, 13,
		92, 112, 97, 114, 32, 92, 112, 97, 114, 100,
		92, 112, 108, 97, 105, 110, 92, 102, 48, 92,
		102, 115, 50, 48, 92, 98, 92, 105, 92, 117,
		92, 116, 97, 98, 92, 116, 120
	};

	public static byte[] iohjj(byte[] p0)
	{
		MemoryStream memoryStream = new MemoryStream();
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		try
		{
			gyduy(binaryWriter, rasyd.wqqbd, p0);
		}
		finally
		{
			if (binaryWriter != null && 0 == 0)
			{
				((IDisposable)binaryWriter).Dispose();
			}
		}
		return memoryStream.ToArray();
	}

	public static byte[] ozsrm(byte[] p0, Func<string, Exception, Exception> p1)
	{
		BinaryReader binaryReader = new BinaryReader(new MemoryStream(p0));
		try
		{
			return dfgxm(binaryReader, p1);
		}
		finally
		{
			if (binaryReader != null && 0 == 0)
			{
				((IDisposable)binaryReader).Dispose();
			}
		}
	}

	public static string rzoyw(string p0, string p1)
	{
		if (p0 == null || 1 == 0)
		{
			return null;
		}
		return wbtcx.vfzjk(p0, p1);
	}

	public static string anqsx(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			return null;
		}
		return wbtcx.uwarv(p0);
	}

	internal static void gyduy(BinaryWriter p0, rasyd p1, byte[] p2)
	{
		switch (p1)
		{
		case rasyd.qctbz:
			p0.Write(p2.Length + 12);
			p0.Write(p2.Length);
			p0.Write((int)p1);
			p0.Write(0);
			p0.Write(p2);
			break;
		case rasyd.wqqbd:
		{
			p0.Write(0);
			p0.Write(0);
			p0.Write(0);
			p0.Write(0);
			lpgze(p0, p2);
			MemoryStream memoryStream = (MemoryStream)p0.BaseStream;
			mecsr mecsr2 = new mecsr();
			mecsr2.hxtqz = ~mecsr2.hxtqz;
			mecsr2.Process(memoryStream.GetBuffer(), 16, (int)memoryStream.Length - 16);
			memoryStream.Position = 0L;
			p0.Write((int)memoryStream.Length - 4);
			p0.Write(p2.Length);
			p0.Write((int)p1);
			p0.Write((int)(~mecsr2.hxtqz));
			break;
		}
		default:
			throw new InvalidOperationException("Unknown RTF compression type tag.");
		}
	}

	private static void lpgze(BinaryWriter p0, byte[] p1)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_001a;
		IL_0006:
		p0.Write((byte)0);
		p0.Write(p1, num, 8);
		num += 8;
		goto IL_001a;
		IL_001a:
		if (num + 8 > p1.Length)
		{
			if (num < p1.Length)
			{
				int num2 = p1.Length - num;
				p0.Write((byte)(1 << num2));
				p0.Write(p1, num, num2);
			}
			else
			{
				p0.Write((byte)1);
			}
			int num3 = (wfocq.Length + p1.Length) % 4096;
			p0.Write((byte)(num3 >> 4));
			p0.Write((byte)(num3 << 4));
			return;
		}
		goto IL_0006;
	}

	private static void zqoco(BinaryWriter p0, byte[] p1)
	{
		byte[] array = new byte[wfocq.Length + p1.Length];
		wfocq.CopyTo(array, 0);
		p1.CopyTo(array, wfocq.Length);
		int[] array2 = new int[32768];
		int[] array3 = new int[array.Length];
		int p2 = (array[0] << 5) ^ array[1];
		jiybs(array, 0, wfocq.Length, array2, array3, ref p2);
		int count = 1;
		byte b = 1;
		byte[] array4 = new byte[17];
		int num = wfocq.Length;
		while (num < array.Length)
		{
			vvqcy(array, num, array2, array3, ref p2, out var p3, out var p4);
			if (p4 < 3)
			{
				array4[count++] = array[num++];
			}
			else
			{
				array4[0] |= b;
				array4[count++] = (byte)(p3 >> 4);
				array4[count++] = (byte)((p3 << 4) | (p4 - 2));
				jiybs(array, num + 1, p4 - 1, array2, array3, ref p2);
				num += p4;
			}
			if (b < 128)
			{
				b <<= 1;
				continue;
			}
			p0.Write(array4, 0, count);
			b = 1;
			count = 1;
			array4[0] = 0;
		}
		array4[0] |= b;
		array4[count++] = (byte)(array.Length % 4096 >> 4);
		array4[count++] = (byte)(array.Length % 4096 << 4);
		p0.Write(array4, 0, count);
	}

	private static void vvqcy(byte[] p0, int p1, int[] p2, int[] p3, ref int p4, out int p5, out int p6)
	{
		p5 = 0;
		p6 = 2;
		if (p1 + 3 >= p0.Length)
		{
			return;
		}
		p4 = ((p4 << 5) ^ p0[p1 + 2]) & 0x7FFF;
		int num = Math.Max(p1 - 4096, 0);
		int num2 = (p3[p1] = p2[p4]);
		p2[p4] = p1;
		if (num2 < num)
		{
			return;
		}
		byte b = p0[p1];
		byte b2 = p0[p1 + 1];
		byte b3 = p0[p1 + p6];
		int num3 = Math.Min(p1 + 17, p0.Length - 1);
		do
		{
			int num4 = num2;
			if (p0[num2 + p6] != b3 || p0[num4] != b || p0[++num4] != b2)
			{
				continue;
			}
			num4++;
			int num5 = p1 + 2;
			while (p0[++num4] == p0[++num5] && num5 < num3)
			{
			}
			int num6 = num5 - p1;
			if (num6 > p6)
			{
				p5 = num2 % 4096;
				p6 = num6;
				if (num6 >= 17)
				{
					break;
				}
				b3 = p0[p1 + num6];
			}
		}
		while ((num2 = p3[num2]) >= num && ((num2 != 0) ? true : false));
	}

	private static void jiybs(byte[] p0, int p1, int p2, int[] p3, int[] p4, ref int p5)
	{
		int num = Math.Min(p1 + p2, p0.Length - 3);
		while (p1 < num)
		{
			p5 = ((p5 << 5) ^ p0[p1 + 2]) & 0x7FFF;
			p4[p1] = p3[p5];
			p3[p5] = p1;
			p1++;
		}
	}

	internal static byte[] dfgxm(BinaryReader p0, Func<string, Exception, Exception> p1)
	{
		int num = p0.ReadInt32();
		int num2 = p0.ReadInt32();
		int num3 = p0.ReadInt32();
		p0.ReadInt32();
		byte[] array;
		int num4;
		int num5;
		int num6;
		int num7;
		int num8;
		int num9;
		switch ((rasyd)num3)
		{
		case (rasyd)0:
			if ((num == 0 || 1 == 0) && (num2 == 0 || 1 == 0))
			{
				return new byte[0];
			}
			break;
		case rasyd.qctbz:
			return p0.ReadBytes(num2);
		case rasyd.wqqbd:
			{
				array = new byte[wfocq.Length + num2];
				wfocq.CopyTo(array, 0);
				num4 = wfocq.Length;
				num5 = 0;
				num6 = 0;
				if (num6 != 0)
				{
					goto IL_009b;
				}
				goto IL_0167;
			}
			IL_0167:
			if (num4 >= array.Length)
			{
				byte[] array2 = new byte[num2];
				Array.Copy(array, wfocq.Length, array2, 0, num2);
				return array2;
			}
			goto IL_009b;
			IL_015b:
			if (num7 < num8)
			{
				goto IL_0143;
			}
			goto IL_0161;
			IL_009b:
			num5 = ((((num6 & 7) != 0) ? true : false) ? (num5 >> 1) : p0.ReadByte());
			if ((num5 & 1) == 0 || 1 == 0)
			{
				array[num4] = p0.ReadByte();
				num4++;
			}
			else
			{
				num9 = p0.ReadByte();
				num8 = p0.ReadByte();
				num9 = (num8 >> 4) | (num9 << 4);
				num8 = (num8 & 0xF) + 2;
				num9 += num4 >> 12 << 12;
				if (num9 >= num4)
				{
					num9 -= 4096;
				}
				if (num9 + num8 > num4)
				{
					num7 = 0;
					if (num7 != 0)
					{
						goto IL_0143;
					}
					goto IL_015b;
				}
				Array.Copy(array, num9, array, num4, num8);
				num4 += num8;
			}
			goto IL_0161;
			IL_0161:
			num6++;
			goto IL_0167;
			IL_0143:
			array[num4++] = array[num9++];
			num7++;
			goto IL_015b;
		}
		throw p1("Unknown RTF compression type tag.", null);
	}
}
