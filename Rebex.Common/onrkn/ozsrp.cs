using System;
using System.Collections.Generic;
using System.Linq;

namespace onrkn;

internal static class ozsrp
{
	private sealed class tetjx<T0>
	{
		public sxztb<T0> zpddf;

		public void dzowh(ArraySegment<T0> p0)
		{
			zpddf.uqydw(p0.Array);
		}
	}

	private sealed class fkgxz
	{
		public tndeg oezhm;

		public rhzda<tndeg> skqpp;

		public void euxqt(ArraySegment<byte> p0)
		{
			skqpp.wkkog(oezhm);
		}
	}

	private sealed class ztssv<T0>
	{
		public sxztb<T0> yzloa;

		public void xuxdl(nxtme<T0> p0)
		{
			yzloa.uqydw(p0.lthjd);
		}
	}

	public static ihlqx<ArraySegment<T>> zobvr<T>(params T[][] p0)
	{
		hcqmh<T[], int> hcqmh2 = tblxl(p0);
		return rxdfr(hcqmh2.amanf, 0, hcqmh2.cdois, sxztb<T>.ahblv);
	}

	public static ihlqx<nxtme<T>> qeqpb<T>(params T[][] p0)
	{
		hcqmh<T[], int> hcqmh2 = tblxl(p0);
		return ejuvp(hcqmh2.amanf, 0, hcqmh2.cdois, sxztb<T>.ahblv);
	}

	public static ihlqx<ArraySegment<T>> bpmbv<T>(int p0)
	{
		return sxztb<T>.ahblv.dwplz(p0);
	}

	public static ihlqx<ArraySegment<T>> dwplz<T>(this sxztb<T> p0, int p1)
	{
		T[] p2 = p0.vfhlp(p1);
		return rxdfr(p2, 0, p1, p0);
	}

	public static ihlqx<nxtme<T>> lglpi<T>(int p0)
	{
		return sxztb<T>.ahblv.hmmvo(p0);
	}

	public static ihlqx<nxtme<T>> hmmvo<T>(this sxztb<T> p0, int p1)
	{
		T[] p2 = p0.vfhlp(p1);
		return ejuvp(p2, 0, p1, p0);
	}

	public static ihlqx<ArraySegment<T>> rxdfr<T>(T[] p0, int p1, int p2, sxztb<T> p3)
	{
		tetjx<T> tetjx = new tetjx<T>();
		tetjx.zpddf = p3;
		ArraySegment<T> value = new ArraySegment<T>(p0, p1, p2);
		return new ihlqx<ArraySegment<T>>(value, tetjx.dzowh);
	}

	public static ihlqx<ArraySegment<byte>> pfpps(tndeg p0, rhzda<tndeg> p1)
	{
		fkgxz fkgxz = new fkgxz();
		fkgxz.oezhm = p0;
		fkgxz.skqpp = p1;
		ArraySegment<byte> value = new ArraySegment<byte>(fkgxz.oezhm.GetBuffer(), 0, (int)fkgxz.oezhm.Length);
		return new ihlqx<ArraySegment<byte>>(value, fkgxz.euxqt);
	}

	public static ihlqx<nxtme<T>> ejuvp<T>(T[] p0, int p1, int p2, sxztb<T> p3)
	{
		ztssv<T> ztssv = new ztssv<T>();
		ztssv.yzloa = p3;
		nxtme<T> value = new nxtme<T>(p0, p1, p2);
		return new ihlqx<nxtme<T>>(value, ztssv.xuxdl);
	}

	private static hcqmh<T[], int> tblxl<T>(params T[][] p0)
	{
		int num = ((IEnumerable<T[]>)p0).Sum((Func<T[], int>)yvcgv);
		T[] array = sxztb<T>.ahblv.vfhlp(num);
		int num2 = 0;
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_002c;
		}
		goto IL_004a;
		IL_002c:
		T[] array2 = p0[num3];
		Array.Copy(array2, 0, array, num2, array2.Length);
		num2 += array2.Length;
		num3++;
		goto IL_004a;
		IL_004a:
		if (num3 < p0.Length)
		{
			goto IL_002c;
		}
		return new hcqmh<T[], int>(array, num);
	}

	private static int yvcgv<T>(T[] p0)
	{
		return p0.Length;
	}
}
