using System;
using System.Threading;

namespace onrkn;

internal class njvzu<T0> : exkzi
{
	private sealed class gosjh
	{
		public njvzu<T0> mniou;

		public Action<njvzu<T0>> dkzkw;

		public void vvqen()
		{
			dkzkw(mniou);
		}
	}

	private sealed class ntekb<T1>
	{
		public njvzu<T0> sgjpf;

		public Func<njvzu<T0>, T1> ttwpd;

		public T1 tghxu()
		{
			return ttwpd(sgjpf);
		}
	}

	private static readonly WaitCallback pljlb = eskvs;

	private static readonly Action<exkzi, T0> ecdkr;

	private T0 rtkgl;

	private Func<T0> vlysk;

	private Func<object, T0> osdlc;

	private static Action<exkzi, T0> gxjmv;

	public T0 islme
	{
		get
		{
			txebj();
			return rtkgl;
		}
	}

	public njvzu(Func<T0> func)
		: base(gmpgj.cepok)
	{
		uclqa(func);
	}

	public njvzu(Func<object, T0> func, object state)
		: base(gmpgj.cepok)
	{
		ysdia(func, state);
	}

	internal njvzu(gmpgj status)
		: base(status)
	{
	}

	internal njvzu(object state, gmpgj status)
		: base(state, status)
	{
	}

	public exkzi tsplf(Action<njvzu<T0>> p0)
	{
		gosjh gosjh = new gosjh();
		gosjh.dkzkw = p0;
		gosjh.mniou = this;
		if (gosjh.dkzkw == null || 1 == 0)
		{
			throw new ArgumentNullException("continuation");
		}
		exkzi exkzi2 = new exkzi(gosjh.vvqen);
		eadna(exkzi2);
		return exkzi2;
	}

	public exkzi dlxia(Action<njvzu<T0>> p0, arvtx p1)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("scheduler");
		}
		return tsplf(p0);
	}

	public njvzu<TNewResult> amcit<TNewResult>(Func<njvzu<T0>, TNewResult> p0)
	{
		ntekb<TNewResult> ntekb = new ntekb<TNewResult>();
		ntekb.ttwpd = p0;
		ntekb.sgjpf = this;
		if (ntekb.ttwpd == null || 1 == 0)
		{
			throw new ArgumentNullException("continuation");
		}
		njvzu<TNewResult> njvzu2 = new njvzu<TNewResult>(ntekb.tghxu);
		eadna(njvzu2);
		return njvzu2;
	}

	public njvzu<TNewResult> yvphh<TNewResult>(Func<njvzu<T0>, TNewResult> p0, arvtx p1)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("scheduler");
		}
		return amcit(p0);
	}

	internal bool asybg(T0 p0, bool p1)
	{
		return gsfkm(gmpgj.pcduu, ecdkr, null, null, p0, p1);
	}

	protected override void nsuyd()
	{
		if (vlysk != null || osdlc != null)
		{
			bvilq.cphqj(pljlb, this);
		}
	}

	private void uclqa(Func<T0> p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("func");
		}
		vlysk = p0;
		base.qrmlc = null;
		base.kyejg = wnrns.ljqcx;
	}

	private void ysdia(Func<object, T0> p0, object p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("func");
		}
		osdlc = p0;
		base.qrmlc = p1;
		base.kyejg = wnrns.gfcit;
	}

	private static void eskvs(object p0)
	{
		njvzu<T0> njvzu2 = p0 as njvzu<T0>;
		njvzu2.zjfvk();
		T0 p1;
		try
		{
			p1 = ((njvzu2.kyejg != wnrns.ljqcx) ? njvzu2.osdlc(njvzu2.qrmlc) : njvzu2.vlysk());
		}
		catch (Exception p2)
		{
			njvzu2.zpszo(p2, p1: true);
			return;
		}
		njvzu2.asybg(p1, p1: true);
	}

	static njvzu()
	{
		if (gxjmv == null || 1 == 0)
		{
			gxjmv = dsyux;
		}
		ecdkr = gxjmv;
	}

	private static void dsyux(exkzi p0, T0 p1)
	{
		((njvzu<T0>)p0).rtkgl = p1;
	}
}
