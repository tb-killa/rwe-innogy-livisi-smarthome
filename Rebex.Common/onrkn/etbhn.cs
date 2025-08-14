using System;

namespace onrkn;

internal static class etbhn
{
	private sealed class gzuuo<T0, T1, T2>
	{
		public Func<T1, T2> sdgig;

		public lpmle<T0, T2> owrzd(T1 p0)
		{
			return sdgig(p0);
		}
	}

	private sealed class aqwfn<T0, T1, T2, T3>
	{
		public Func<T0, T2> wumsb;

		public Func<T1, T3> sfdne;

		public lpmle<T2, T3> nrdxh(T0 p0)
		{
			return wumsb(p0);
		}

		public lpmle<T2, T3> zoohx(T1 p0)
		{
			return sfdne(p0);
		}
	}

	private sealed class ipgea<T0, T1, T2, T3>
	{
		public lpmle<T0, T1> zxtft;

		public Func<T1, T2, T3> cttip;

		public lpmle<T0, T3> lplsg(T2 p0)
		{
			return new lpmle<T0, T3>(cttip(zxtft.obvbo, p0));
		}
	}

	public static lpmle<TL, TRR> qoubt<TL, TR, TRR>(this lpmle<TL, TR> p0, Func<TR, TRR> p1)
	{
		gzuuo<TL, TR, TRR> gzuuo = new gzuuo<TL, TR, TRR>();
		gzuuo.sdgig = p1;
		if (p0 == null && 0 == 0)
		{
			throw new ArgumentNullException("either");
		}
		if (gzuuo.sdgig == null || 1 == 0)
		{
			throw new ArgumentNullException("selectRightFunc");
		}
		return p0.kmgai<lpmle<TL, TRR>>(volxl<TL, TR, TRR>, gzuuo.owrzd);
	}

	public static lpmle<TRL, TRR> beped<TL, TR, TRL, TRR>(this lpmle<TL, TR> p0, Func<TL, TRL> p1, Func<TR, TRR> p2)
	{
		aqwfn<TL, TR, TRL, TRR> aqwfn = new aqwfn<TL, TR, TRL, TRR>();
		aqwfn.wumsb = p1;
		aqwfn.sfdne = p2;
		if (p0 == null && 0 == 0)
		{
			throw new ArgumentNullException("either");
		}
		if (aqwfn.wumsb == null || 1 == 0)
		{
			throw new ArgumentNullException("selectLeftFunc");
		}
		if (aqwfn.sfdne == null || 1 == 0)
		{
			throw new ArgumentNullException("selectRightFunc");
		}
		return p0.kmgai<lpmle<TRL, TRR>>(aqwfn.nrdxh, aqwfn.zoohx);
	}

	public static lpmle<TL, TRR> hxxzg<TL, TR, TRR>(this lpmle<TL, TR> p0, Func<TR, lpmle<TL, TRR>> p1)
	{
		if (p0 == null && 0 == 0)
		{
			throw new ArgumentNullException("either");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("eitherSelector");
		}
		return p0.kmgai(gfisx<TL, TR, TRR>, p1);
	}

	public static lpmle<TL, TRR> pmaat<TL, TR, TRR>(this lpmle<TL, TR> p0, Func<TR, lpmle<TL, TRR>> p1)
	{
		return p0.hxxzg(p1);
	}

	public static lpmle<TL, TRRR> dtxes<TL, TR, TRR, TRRR>(this lpmle<TL, TR> p0, Func<TR, lpmle<TL, TRR>> p1, Func<TR, TRR, TRRR> p2)
	{
		ipgea<TL, TR, TRR, TRRR> ipgea = new ipgea<TL, TR, TRR, TRRR>();
		ipgea.zxtft = p0;
		ipgea.cttip = p2;
		if (ipgea.zxtft == null && 0 == 0)
		{
			throw new ArgumentNullException("either");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("eitherSelector");
		}
		if (ipgea.cttip == null || 1 == 0)
		{
			throw new ArgumentNullException("resultSelector");
		}
		return ipgea.zxtft.hxxzg(p1).hxxzg(ipgea.lplsg);
	}

	private static lpmle<TL, TRR> volxl<TL, TR, TRR>(TL p0)
	{
		return p0;
	}

	private static lpmle<TL, TRR> gfisx<TL, TR, TRR>(TL p0)
	{
		return p0;
	}
}
