using System;
using System.Runtime.CompilerServices;

namespace onrkn;

internal static class jlfbq
{
	private static Func<string, Exception> lzoit;

	public static ushort izulq(byte[] p0, int p1)
	{
		return (ushort)sunaw(p0, p1);
	}

	public static short sunaw(byte[] p0, int p1)
	{
		return (short)(p0[p1] | (p0[++p1] << 8));
	}

	public static uint ubcts(byte[] p0, int p1)
	{
		return (uint)nlfaq(p0, p1);
	}

	public static int nlfaq(byte[] p0, int p1)
	{
		return p0[p1] | (p0[++p1] << 8) | (p0[++p1] << 16) | (p0[++p1] << 24);
	}

	public static ulong icraz(byte[] p0, int p1)
	{
		return (ulong)epwpf(p0, p1);
	}

	public static long epwpf(byte[] p0, int p1)
	{
		return (long)(ubcts(p0, p1) | ((ulong)ubcts(p0, p1 + 4) << 32));
	}

	public static ushort kougd(byte[] p0, int p1)
	{
		return (ushort)bzdmz(p0, p1);
	}

	public static short bzdmz(byte[] p0, int p1)
	{
		return (short)((p0[p1] << 8) | p0[++p1]);
	}

	public static uint vtwgv(byte[] p0, int p1)
	{
		return (uint)yyxrz(p0, p1);
	}

	public static int yyxrz(byte[] p0, int p1)
	{
		return (p0[p1] << 24) | (p0[++p1] << 16) | (p0[++p1] << 8) | p0[++p1];
	}

	public static ulong ffnnw(byte[] p0, int p1)
	{
		return (ulong)zdgzv(p0, p1);
	}

	public static long zdgzv(byte[] p0, int p1)
	{
		return (long)(((ulong)vtwgv(p0, p1) << 32) | vtwgv(p0, p1 + 4));
	}

	public static uint ucuck(byte[] p0, int p1, int p2)
	{
		return p2 switch
		{
			0 => 0u, 
			1 => p0[p1], 
			2 => (uint)(p0[p1] + (p0[p1 + 1] << 8)), 
			3 => (uint)(p0[p1] + (p0[p1 + 1] << 8) + (p0[p1 + 2] << 16)), 
			4 => ubcts(p0, p1), 
			_ => throw new ArgumentException(), 
		};
	}

	public static float tcekw(byte[] p0, int p1)
	{
		return BitConverter.ToSingle(p0, p1);
	}

	public static double iydrg(byte[] p0, int p1)
	{
		return BitConverter.ToDouble(p0, p1);
	}

	public static void ampcm(byte[] p0, int p1, ushort p2)
	{
		dauol(p0, p1, (short)p2);
	}

	public static void dauol(byte[] p0, int p1, short p2)
	{
		p0[p1] = (byte)(p2 & 0xFF);
		p0[++p1] = (byte)((p2 >> 8) & 0xFF);
	}

	public static void mfljd(byte[] p0, int p1, uint p2)
	{
		cdcfa(p0, p1, (int)p2);
	}

	public static void cdcfa(byte[] p0, int p1, int p2)
	{
		p0[p1] = (byte)(p2 & 0xFF);
		p0[++p1] = (byte)((p2 >> 8) & 0xFF);
		p0[++p1] = (byte)((p2 >> 16) & 0xFF);
		p0[++p1] = (byte)((p2 >> 24) & 0xFF);
	}

	public static void ewwft(byte[] p0, int p1, ulong p2)
	{
		wgqao(p0, p1, (long)p2);
	}

	public static void wgqao(byte[] p0, int p1, long p2)
	{
		p0[p1] = (byte)(p2 & 0xFF);
		p0[++p1] = (byte)((p2 >> 8) & 0xFF);
		p0[++p1] = (byte)((p2 >> 16) & 0xFF);
		p0[++p1] = (byte)((p2 >> 24) & 0xFF);
		p0[++p1] = (byte)((p2 >> 32) & 0xFF);
		p0[++p1] = (byte)((p2 >> 40) & 0xFF);
		p0[++p1] = (byte)((p2 >> 48) & 0xFF);
		p0[++p1] = (byte)((p2 >> 56) & 0xFF);
	}

	public static void bktwx(byte[] p0, int p1, ushort p2)
	{
		lasnw(p0, p1, (short)p2);
	}

	public static void lasnw(byte[] p0, int p1, short p2)
	{
		p0[p1++] = (byte)((p2 >> 8) & 0xFF);
		p0[p1] = (byte)(p2 & 0xFF);
	}

	public static void dtrgu(byte[] p0, int p1, uint p2)
	{
		lyicr(p0, p1, (int)p2);
	}

	public static void lyicr(byte[] p0, int p1, int p2)
	{
		p0[p1++] = (byte)((p2 >> 24) & 0xFF);
		p0[p1++] = (byte)((p2 >> 16) & 0xFF);
		p0[p1++] = (byte)((p2 >> 8) & 0xFF);
		p0[p1] = (byte)(p2 & 0xFF);
	}

	public static void wlrvd(byte[] p0, int p1, ulong p2)
	{
		emxnl(p0, p1, (long)p2);
	}

	public static void emxnl(byte[] p0, int p1, long p2)
	{
		p0[p1++] = (byte)((p2 >> 56) & 0xFF);
		p0[p1++] = (byte)((p2 >> 48) & 0xFF);
		p0[p1++] = (byte)((p2 >> 40) & 0xFF);
		p0[p1++] = (byte)((p2 >> 32) & 0xFF);
		p0[p1++] = (byte)((p2 >> 24) & 0xFF);
		p0[p1++] = (byte)((p2 >> 16) & 0xFF);
		p0[p1++] = (byte)((p2 >> 8) & 0xFF);
		p0[p1] = (byte)(p2 & 0xFF);
	}

	public static byte[] cnbay(byte[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("source");
		}
		if (p0.Length > 0)
		{
			if (p0[0] != 0 && 0 == 0)
			{
				return p0;
			}
			int num = jlhar(p0);
			byte[] array = new byte[num];
			Array.Copy(p0, p0.Length - num, array, 0, num);
			return array;
		}
		return new byte[1];
	}

	public static byte[] elzlr(byte[] p0, int p1, int p2, Func<string, Exception> p3)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("source");
		}
		if (p0.Length == p1)
		{
			return p0;
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0029;
		}
		goto IL_00cf;
		IL_0029:
		if (p0[num] != 0 && 0 == 0)
		{
			int num2 = Math.Max(1, p0.Length - num);
			if (num2 > p1)
			{
				if (num2 > p2)
				{
					Func<string, Exception> func = p3;
					if (func == null || 1 == 0)
					{
						if (lzoit == null || 1 == 0)
						{
							lzoit = hnham;
						}
						func = lzoit;
					}
					p3 = func;
					throw p3(brgjd.edcru("The integer does not fit into the specified number of bytes ({0}, {1}).", num2, p2));
				}
				p1 = num2;
			}
			byte[] array = new byte[p1];
			Array.Copy(p0, num, array, p1 - num2, num2);
			return array;
		}
		num++;
		goto IL_00cf;
		IL_00cf:
		if (num < p0.Length)
		{
			goto IL_0029;
		}
		return new byte[p1];
	}

	public static int jlhar(byte[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			return 0;
		}
		if (p0[0] != 0 && 0 == 0)
		{
			return p0.Length;
		}
		int num = 0;
		for (int i = num; i < p0.Length; i++)
		{
			if (p0[i] != 0 && 0 == 0)
			{
				return Math.Max(1, p0.Length - i);
			}
		}
		return 1;
	}

	public static byte[] bpeee(byte[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("value");
		}
		if (p0.Length == 0 || 1 == 0)
		{
			return new byte[1];
		}
		if (p0[0] >= 128)
		{
			p0 = tykuz(p0, p0.Length + 1);
		}
		return p0;
	}

	public static byte[] tykuz(byte[] p0, int p1)
	{
		if (p0.Length >= p1)
		{
			return p0;
		}
		byte[] array = new byte[p1];
		p0.CopyTo(array, p1 - p0.Length);
		return array;
	}

	public static byte[] twxvm(byte[] p0)
	{
		if (p0.Length == 0 || 1 == 0)
		{
			return new byte[1];
		}
		byte b = p0[0];
		if (b >= 128)
		{
			byte[] array = new byte[p0.Length + 1];
			p0.CopyTo(array, 1);
			return array;
		}
		if (b > 0)
		{
			return p0;
		}
		int num = 1;
		if (num == 0)
		{
			goto IL_0043;
		}
		goto IL_0081;
		IL_0081:
		if (num < p0.Length)
		{
			goto IL_0043;
		}
		return new byte[1];
		IL_0043:
		b = p0[num];
		if (b != 0 && 0 == 0)
		{
			if (b >= 128)
			{
				if (num == 1)
				{
					return p0;
				}
				num--;
			}
			byte[] array2 = new byte[p0.Length - num];
			Array.Copy(p0, num, array2, 0, array2.Length);
			return array2;
		}
		num++;
		goto IL_0081;
	}

	public static byte[] xphpx(byte[] p0, int p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("value");
		}
		if (p1 < 0)
		{
			throw new ArgumentOutOfRangeException("requiredLength");
		}
		int num = p0.Length - p1;
		int num2;
		if (num > 0)
		{
			num2 = 0;
			if (num2 != 0)
			{
				goto IL_0037;
			}
			goto IL_0078;
		}
		if (num < 0)
		{
			byte[] array = new byte[p1];
			Array.Copy(p0, 0, array, -num, p0.Length);
			return array;
		}
		return p0;
		IL_0078:
		if (num2 >= num)
		{
			byte[] array2 = new byte[p1];
			Array.Copy(p0, num, array2, 0, array2.Length);
			return array2;
		}
		goto IL_0037;
		IL_0037:
		if (p0[num2] != 0 && 0 == 0)
		{
			throw new ArgumentException(brgjd.edcru("Value is too long. Expected {0}, got {1}.", p1, p0.Length));
		}
		num2++;
		goto IL_0078;
	}

	public static byte[] ttbrb(params byte[][] p0)
	{
		int num = 0;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0010;
		}
		goto IL_0022;
		IL_0010:
		byte[] array = p0[num2];
		num += array.Length;
		num2++;
		goto IL_0022;
		IL_0022:
		if (num2 < p0.Length)
		{
			goto IL_0010;
		}
		byte[] array2 = new byte[num];
		num = 0;
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_003d;
		}
		goto IL_0068;
		IL_0068:
		if (num3 < p0.Length)
		{
			goto IL_003d;
		}
		return array2;
		IL_003d:
		byte[] array3 = p0[num3];
		if (array3.Length != 0 && 0 == 0)
		{
			Array.Copy(array3, 0, array2, num, array3.Length);
			num += array3.Length;
		}
		num3++;
		goto IL_0068;
	}

	public static byte[] usqov(byte[] p0, byte[] p1)
	{
		int num = p0.Length;
		int num2 = p1.Length;
		byte[] array = new byte[num + num2];
		p0.CopyTo(array, 0);
		p1.CopyTo(array, num);
		return array;
	}

	public static bool oreja(byte[] p0, byte[] p1)
	{
		if (object.ReferenceEquals(p0, p1) && 0 == 0)
		{
			return true;
		}
		if (p0 == null || false || p1 == null)
		{
			return false;
		}
		if (p0.Length != p1.Length)
		{
			return false;
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_003a;
		}
		goto IL_0048;
		IL_003a:
		if (p0[num] != p1[num])
		{
			return false;
		}
		num++;
		goto IL_0048;
		IL_0048:
		if (num < p0.Length)
		{
			goto IL_003a;
		}
		return true;
	}

	public static bool inlao(byte[] p0, int p1, byte[] p2, int p3, int p4)
	{
		if ((p0 == null || 1 == 0) && (p2 == null || 1 == 0))
		{
			return true;
		}
		if (p0 == null || false || p2 == null)
		{
			return false;
		}
		dahxy.dionp(p0, p1, p4);
		dahxy.dionp(p2, p3, p4);
		int num = p1;
		int num2 = p3;
		for (int num3 = p4; num3 > 0; num3--)
		{
			if (p0[num++] != p2[num2++])
			{
				return false;
			}
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool ccahg(byte[] p0, int p1, byte[] p2, int p3, int p4)
	{
		if ((p0 == null || 1 == 0) && (p2 == null || 1 == 0))
		{
			return true;
		}
		if (p0 == null || false || p2 == null)
		{
			return false;
		}
		dahxy.dionp(p0, p1, p4);
		dahxy.dionp(p2, p3, p4);
		bool flag = false;
		int num = 0;
		if (num != 0)
		{
			goto IL_004c;
		}
		goto IL_006c;
		IL_004c:
		flag |= p0[p1++] != p2[p3++];
		num++;
		goto IL_006c;
		IL_006c:
		if (num < p4)
		{
			goto IL_004c;
		}
		return !flag;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static bool xzsez(nxtme<byte> p0, nxtme<byte> p1)
	{
		if (p0.hvbtp && 0 == 0 && p1.hvbtp && 0 == 0)
		{
			return true;
		}
		if (p0.hvvsm != p1.hvvsm)
		{
			return false;
		}
		if (p0.bopab && 0 == 0 && p1.bopab && 0 == 0)
		{
			return ccahg(p0.lthjd, p0.frlfs, p1.lthjd, p1.frlfs, p1.hvvsm);
		}
		bool flag = false;
		int i = 0;
		if (i != 0)
		{
			goto IL_008e;
		}
		goto IL_024e;
		IL_024e:
		if (i + 15 >= p0.hvvsm)
		{
			for (; i < p0.hvvsm; i++)
			{
				flag |= p0[i] != p1[i];
			}
			return !flag;
		}
		goto IL_008e;
		IL_008e:
		flag |= p0[i] != p1[i++];
		flag |= p0[i] != p1[i++];
		flag |= p0[i] != p1[i++];
		flag |= p0[i] != p1[i++];
		flag |= p0[i] != p1[i++];
		flag |= p0[i] != p1[i++];
		flag |= p0[i] != p1[i++];
		flag |= p0[i] != p1[i++];
		flag |= p0[i] != p1[i++];
		flag |= p0[i] != p1[i++];
		flag |= p0[i] != p1[i++];
		flag |= p0[i] != p1[i++];
		flag |= p0[i] != p1[i++];
		flag |= p0[i] != p1[i++];
		flag |= p0[i] != p1[i++];
		flag |= p0[i] != p1[i++];
		goto IL_024e;
	}

	public static byte[] aqhfc(this byte[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("buffer");
		}
		return (byte[])p0.Clone();
	}

	public static byte[] wwots(this byte[] p0, int p1, int p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("buffer");
		}
		dahxy.dionp(p0, p1, p2);
		byte[] array = new byte[p2];
		Array.Copy(p0, p1, array, 0, p2);
		return array;
	}

	public static byte[] ukqqp(byte[] p0, int p1, int p2)
	{
		if (p1 == 0 || 1 == 0)
		{
			if (p0 == null || 1 == 0)
			{
				throw new ArgumentNullException("buffer");
			}
			if (p0.Length == p2)
			{
				return p0;
			}
		}
		return p0.wwots(p1, p2);
	}

	public static bool kdbec(byte[] p0, int p1, byte[] p2, int p3, int p4)
	{
		if (object.ReferenceEquals(p0, p2) && 0 == 0 && p1 < p3)
		{
			return p1 + p4 > p3;
		}
		return false;
	}

	public static void cfvhy(byte[] p0, int p1, byte[] p2, int p3, byte[] p4, int p5, int p6)
	{
		if ((kdbec(p0, p1, p4, p5, p6) ? true : false) || kdbec(p2, p3, p4, p5, p6))
		{
			suymn(p0, p1, p2, p3, p4, p5, p6);
		}
		else
		{
			ncylc(p0, p1, p2, p3, p4, p5, p6);
		}
	}

	private static void ncylc(byte[] p0, int p1, byte[] p2, int p3, byte[] p4, int p5, int p6)
	{
		int num;
		for (num = p6; num >= 4; num -= 4)
		{
			int num2 = p0[p1++] | (p0[p1++] << 8) | (p0[p1++] << 16) | (p0[p1++] << 24);
			int num3 = p2[p3++] | (p2[p3++] << 8) | (p2[p3++] << 16) | (p2[p3++] << 24);
			int num4 = num2 ^ num3;
			p4[p5++] = (byte)num4;
			p4[p5++] = (byte)(num4 >> 8);
			p4[p5++] = (byte)(num4 >> 16);
			p4[p5++] = (byte)(num4 >> 24);
		}
		while (num > 0)
		{
			p4[p5++] = (byte)(p0[p1++] ^ p2[p3++]);
			num--;
		}
	}

	private static void suymn(byte[] p0, int p1, byte[] p2, int p3, byte[] p4, int p5, int p6)
	{
		int num = p6 - 1;
		int p7 = p1 + num;
		int p8 = p3 + num;
		int p9 = p5 + num;
		ctxkt(p0, p7, p2, p8, p4, p9, p6);
	}

	private static void ctxkt(byte[] p0, int p1, byte[] p2, int p3, byte[] p4, int p5, int p6)
	{
		while (p6 >= 4)
		{
			int num = p0[p1--] | (p0[p1--] << 8) | (p0[p1--] << 16) | (p0[p1--] << 24);
			int num2 = p2[p3--] | (p2[p3--] << 8) | (p2[p3--] << 16) | (p2[p3--] << 24);
			int num3 = num ^ num2;
			p4[p5--] = (byte)num3;
			p4[p5--] = (byte)(num3 >> 8);
			p4[p5--] = (byte)(num3 >> 16);
			p4[p5--] = (byte)(num3 >> 24);
			p6 -= 4;
		}
		while (p6 > 0)
		{
			p4[p5--] = (byte)(p0[p1--] ^ p2[p3--]);
			p6--;
		}
	}

	public static bool uqqcs(byte[] p0)
	{
		if (p0 != null && 0 == 0)
		{
			return p0.Length == 0;
		}
		return true;
	}

	public static int eivls(byte[] p0, byte[] p1, int p2, int p3)
	{
		int num = Array.IndexOf(p0, p1[0], p2, p3);
		if (num < 0)
		{
			return -1;
		}
		int num2 = p2 + p3;
		while (num >= 0)
		{
			int num3 = num;
			int num4 = 0;
			if (num4 != 0)
			{
				goto IL_0020;
			}
			goto IL_0028;
			IL_0020:
			if (p1[num4] == p0[num3])
			{
				goto IL_0028;
			}
			goto IL_003a;
			IL_0028:
			if (++num4 < p1.Length && ++num3 < num2)
			{
				goto IL_0020;
			}
			goto IL_003a;
			IL_003a:
			if (num4 == p1.Length)
			{
				return num;
			}
			if (num3 == num2)
			{
				return -1;
			}
			num++;
			num = Array.IndexOf(p0, p1[0], num, num2 - num);
		}
		return -1;
	}

	private static Exception hnham(string p0)
	{
		return new InvalidOperationException(p0);
	}
}
