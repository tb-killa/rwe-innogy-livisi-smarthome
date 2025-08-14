using System;
using System.IO;
using System.Text;

namespace onrkn;

internal static class zrwmt
{
	public static int ewhcy(Stream p0, byte[] p1, int p2, int p3)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_001c;
		IL_0006:
		int num2 = p0.Read(p1, p2 + num, p3 - num);
		if (num2 > 0)
		{
			num += num2;
			goto IL_001c;
		}
		goto IL_0027;
		IL_0027:
		return num;
		IL_001c:
		if (num >= p3)
		{
			goto IL_0027;
		}
		goto IL_0006;
	}

	public static void izhdo(Stream p0, byte[] p1, int p2, int p3)
	{
		long p4 = 0L;
		qoqca(p0, p1, p2, p3, ref p4);
	}

	public static void qoqca(Stream p0, byte[] p1, int p2, int p3, ref long p4)
	{
		int num = ewhcy(p0, p1, p2, p3);
		p4 -= num;
		if (num >= p3)
		{
			return;
		}
		throw new InvalidOperationException("Not enough data within the stream.");
	}

	public static ushort ivgfe(Stream p0, byte[] p1, int p2)
	{
		izhdo(p0, p1, p2, 2);
		return nvwco(p1, ref p2);
	}

	public static ushort dmlua(Stream p0, byte[] p1, int p2, ref long p3)
	{
		izhdo(p0, p1, p2, 2);
		p3 -= 2L;
		return nvwco(p1, ref p2);
	}

	public static uint keegp(Stream p0, byte[] p1, int p2)
	{
		izhdo(p0, p1, p2, 4);
		return tqkwn(p1, ref p2);
	}

	public static uint qzuim(Stream p0, byte[] p1, int p2, ref long p3)
	{
		izhdo(p0, p1, p2, 4);
		p3 -= 4L;
		return tqkwn(p1, ref p2);
	}

	public static ushort nvwco(byte[] p0, ref int p1)
	{
		return (ushort)(p0[p1++] | (p0[p1++] << 8));
	}

	public static uint tqkwn(byte[] p0, ref int p1)
	{
		return (uint)(p0[p1++] | (p0[p1++] << 8) | (p0[p1++] << 16) | (p0[p1++] << 24));
	}

	public static long rcdmm(byte[] p0, ref int p1)
	{
		return (long)(tqkwn(p0, ref p1) | ((ulong)tqkwn(p0, ref p1) << 32));
	}

	public static void xidph(ushort p0, byte[] p1, ref int p2)
	{
		p1[p2++] = (byte)p0;
		p1[p2++] = (byte)(p0 >> 8);
	}

	public static void bjdpt(uint p0, byte[] p1, ref int p2)
	{
		p1[p2++] = (byte)p0;
		p1[p2++] = (byte)(p0 >> 8);
		p1[p2++] = (byte)(p0 >> 16);
		p1[p2++] = (byte)(p0 >> 24);
	}

	public static void tmgwe(long p0, byte[] p1, ref int p2)
	{
		p1[p2++] = (byte)p0;
		p1[p2++] = (byte)(p0 >> 8);
		p1[p2++] = (byte)(p0 >> 16);
		p1[p2++] = (byte)(p0 >> 24);
		p1[p2++] = (byte)(p0 >> 32);
		p1[p2++] = (byte)(p0 >> 40);
		p1[p2++] = (byte)(p0 >> 48);
		p1[p2++] = (byte)(p0 >> 56);
	}

	public static string uxsjw(byte[] p0, ref int p1, int p2, Encoding p3)
	{
		string result = p3.GetString(p0, p1, p2);
		p1 += p2;
		return result;
	}

	public static void hlfzf(byte[] p0, int p1, int p2, byte p3)
	{
		if (p2 < 4)
		{
			while (p2-- > 0)
			{
				p0[p1++] = p3;
			}
			return;
		}
		p0[p1] = p3;
		p0[p1 + 1] = p3;
		p0[p1 + 2] = p3;
		p0[p1 + 3] = p3;
		int num = 4;
		if (num == 0)
		{
			goto IL_0039;
		}
		goto IL_0056;
		IL_0056:
		if (num >= p2)
		{
			return;
		}
		goto IL_0039;
		IL_0039:
		Array.Copy(p0, p1, p0, p1 + num, Math.Min(num, p2 - num));
		num <<= 1;
		goto IL_0056;
	}
}
