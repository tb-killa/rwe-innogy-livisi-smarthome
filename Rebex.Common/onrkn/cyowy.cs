using System;
using System.Collections.Generic;
using System.Linq;

namespace onrkn;

internal static class cyowy
{
	public static nxtme<T> cbdmo<T>(this nxtme<T> p0, params nxtme<T>[] p1)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("arrayViews");
		}
		return p1.dwgkr(p0).gwwcy();
	}

	public static nxtme<T> ihuxo<T>(this nxtme<T> p0, nxtme<T> p1)
	{
		if (p0.hvbtp && 0 == 0 && p1.hvbtp && 0 == 0)
		{
			return nxtme<T>.gihlo;
		}
		if (p0.hvbtp && 0 == 0)
		{
			return p1;
		}
		if (p1.hvbtp && 0 == 0)
		{
			return p0;
		}
		return new nxtme<T>(p0, p1);
	}

	public static nxtme<T> gwwcy<T>(this IEnumerable<nxtme<T>> p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("arrayViews");
		}
		return new nxtme<T>(p0.ToArray());
	}

	public static nxtme<T> cmlvg<T>(this nxtme<T> p0, IEnumerable<nxtme<T>> p1)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("arrayViews");
		}
		return p1.dwgkr(p0).gwwcy();
	}

	public static nxtme<byte> holjr(this nxtme<byte> p0)
	{
		if (p0.hvbtp && 0 == 0)
		{
			return p0;
		}
		if (p0.bopab && 0 == 0)
		{
			Array.Clear(p0.lthjd, p0.frlfs, p0.hvvsm);
			return p0;
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0045;
		}
		goto IL_0052;
		IL_0052:
		if (num < p0.tvoem)
		{
			goto IL_0045;
		}
		return p0;
		IL_0045:
		p0[num] = 0;
		num++;
		goto IL_0052;
	}

	public static nxtme<T> qkihq<T>(this nxtme<T> p0)
	{
		if (p0.hvbtp && 0 == 0)
		{
			return p0;
		}
		if (p0.bopab && 0 == 0)
		{
			Array.Clear(p0.lthjd, p0.frlfs, p0.hvvsm);
			return p0;
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0048;
		}
		goto IL_005d;
		IL_005d:
		if (num < p0.tvoem)
		{
			goto IL_0048;
		}
		return p0;
		IL_0048:
		p0[num] = default(T);
		num++;
		goto IL_005d;
	}

	public static nxtme<T> hluyy<T>(this nxtme<T> p0, int p1)
	{
		if (p1 < 0)
		{
			throw new ArgumentOutOfRangeException("numberOfItems");
		}
		if (p1 == 0 || 1 == 0)
		{
			return p0;
		}
		T[] array = new T[p1];
		return p0.omtfq(array);
	}

	public static nxtme<T> szntq<T>(this nxtme<T> p0, int p1)
	{
		if (p1 < 0)
		{
			throw new ArgumentOutOfRangeException("numberOfItems");
		}
		if (p1 == 0 || 1 == 0)
		{
			return p0;
		}
		T[] array = new T[p1];
		return p0.pyfmm(array);
	}

	public static int hchhf(this nxtme<int> p0)
	{
		if (p0.hvbtp && 0 == 0)
		{
			return 0;
		}
		int num = 0;
		int tvoem = p0.tvoem;
		int frlfs = p0.frlfs;
		for (int i = frlfs; i < tvoem; i++)
		{
			num = checked(num + p0[i]);
		}
		return num;
	}

	public static long xfpjh(this nxtme<long> p0)
	{
		if (p0.hvbtp && 0 == 0)
		{
			return 0L;
		}
		long num = 0L;
		int tvoem = p0.tvoem;
		int frlfs = p0.frlfs;
		for (int i = frlfs; i < tvoem; i++)
		{
			num = checked(num + p0[i]);
		}
		return num;
	}
}
