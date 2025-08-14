using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace onrkn;

internal sealed class txmde : IDisposable
{
	private samhn htaqn;

	private samhn[] vfwev;

	private int mjxpo;

	private static readonly txmde rghdt = new txmde();

	private bool yurnv;

	public static txmde qwrqa => rghdt;

	public bool wugvj
	{
		get
		{
			return yurnv;
		}
		private set
		{
			yurnv = value;
		}
	}

	private txmde()
	{
		htaqn = null;
		vfwev = new samhn[0];
		wugvj = true;
	}

	public txmde(int count)
	{
		if (count < 1)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		mjxpo = 8 + IntPtr.Size;
		htaqn = new samhn(mjxpo + count * mjxpo);
		htaqn.fpzdi(0, 0);
		htaqn.fpzdi(4, count);
		htaqn.qurik(8, new IntPtr(htaqn.inyna().ToInt64() + mjxpo));
		vfwev = new samhn[count];
		int num = 0;
		if (num != 0)
		{
			goto IL_0093;
		}
		goto IL_00d6;
		IL_00d6:
		if (num < count)
		{
			goto IL_0093;
		}
		wugvj = false;
		return;
		IL_0093:
		int num2 = mjxpo + mjxpo * num;
		htaqn.fpzdi(num2, 0);
		htaqn.fpzdi(num2 + 4, 0);
		htaqn.qurik(num2 + 8, IntPtr.Zero);
		num++;
		goto IL_00d6;
	}

	public IntPtr rivma()
	{
		if (htaqn == null || 1 == 0)
		{
			return IntPtr.Zero;
		}
		return htaqn.inyna();
	}

	public void gyeqw(int p0, int p1, byte[] p2)
	{
		if (p0 < 0 || p0 >= vfwev.Length)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		int p3;
		IntPtr p4;
		if (p2 != null && 0 == 0)
		{
			p3 = p2.Length;
			samhn samhn2 = samhn.ojtyh(p2);
			vfwev[p0] = samhn2;
			p4 = samhn2.inyna();
		}
		else
		{
			p3 = 0;
			vfwev[p0] = null;
			p4 = IntPtr.Zero;
		}
		int num = mjxpo + mjxpo * p0;
		htaqn.fpzdi(num, p3);
		htaqn.fpzdi(num + 4, p1);
		htaqn.qurik(num + 8, p4);
	}

	public void moqxp(int p0, int p1, samhn p2)
	{
		if (p0 < 0 || p0 >= vfwev.Length)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		vfwev[p0] = null;
		int num = mjxpo + mjxpo * p0;
		htaqn.fpzdi(num, p2.enukg);
		htaqn.fpzdi(num + 4, p1);
		htaqn.qurik(num + 8, p2.inyna());
	}

	public byte[] ogwcp(int p0)
	{
		if (p0 < 0 || p0 >= vfwev.Length)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		int num = mjxpo + mjxpo * p0;
		int num2 = htaqn.sqmvb(num);
		IntPtr intPtr = htaqn.weojx(num + 8);
		if (intPtr == IntPtr.Zero && 0 == 0)
		{
			return null;
		}
		byte[] array = new byte[num2];
		Marshal.Copy(intPtr, array, 0, num2);
		GC.KeepAlive(vfwev);
		GC.KeepAlive(htaqn);
		return array;
	}

	public void Dispose()
	{
		if (wugvj && 0 == 0)
		{
			return;
		}
		samhn samhn2 = htaqn;
		if (samhn2 != null && 0 == 0 && (samhn2 = Interlocked.CompareExchange(ref htaqn, null, samhn2)) != null && 0 == 0)
		{
			samhn2.Dispose();
		}
		samhn[] array = vfwev;
		int num;
		if (array != null && 0 == 0 && (array = Interlocked.CompareExchange(ref vfwev, null, array)) != null)
		{
			num = 0;
			if (num != 0)
			{
				goto IL_0075;
			}
			goto IL_009a;
		}
		return;
		IL_0075:
		samhn samhn3 = array[num];
		if (samhn3 != null && 0 == 0)
		{
			samhn3.Dispose();
		}
		num++;
		goto IL_009a;
		IL_009a:
		if (num >= array.Length)
		{
			return;
		}
		goto IL_0075;
	}
}
