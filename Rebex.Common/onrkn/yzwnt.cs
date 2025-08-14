using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace onrkn;

internal static class yzwnt
{
	private sealed class nlbvc<T0, T1>
	{
		public Func<T0, T1> ubwly;

		public apajk<T1> ymijb(T0 p0)
		{
			return ubwly(p0);
		}
	}

	private sealed class zwnea<T0>
	{
		public Func<T0, bool> prpuo;

		public apajk<T0> uomzg(T0 p0)
		{
			if (!prpuo(p0) || 1 == 0)
			{
				return vvchs.apcdm<T0>();
			}
			return p0;
		}
	}

	private sealed class gaocu<T0, T1, T2>
	{
		private sealed class cpyvc
		{
			public gaocu<T0, T1, T2> fmvvd;

			public T0 tcngq;

			public apajk<T2> msozt(T1 p0)
			{
				return new apajk<T2>(fmvvd.xmedb(tcngq, p0));
			}
		}

		public Func<T0, apajk<T1>> vmhkr;

		public Func<T0, T1, T2> xmedb;

		public apajk<T2> nthqq(T0 p0)
		{
			cpyvc cpyvc = new cpyvc();
			cpyvc.fmvvd = this;
			cpyvc.tcngq = p0;
			return vmhkr(cpyvc.tcngq).seumy(cpyvc.msozt);
		}
	}

	private sealed class kdgaw<T0> : IEnumerable<T0>, IEnumerable, IEnumerator<T0>, IEnumerator, IDisposable
	{
		private T0 zdqci;

		private int vtyqq;

		private int qvrax;

		public apajk<T0> uvrmm;

		public apajk<T0> dendo;

		private T0 dwoxj => zdqci;

		private object ixxxc => zdqci;

		private IEnumerator<T0> arffr()
		{
			kdgaw<T0> kdgaw;
			if (Thread.CurrentThread.ManagedThreadId == qvrax && vtyqq == -2)
			{
				vtyqq = 0;
				kdgaw = this;
			}
			else
			{
				kdgaw = new kdgaw<T0>(0);
			}
			kdgaw.uvrmm = dendo;
			return kdgaw;
		}

		IEnumerator<T0> IEnumerable<T0>.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in arffr
			return this.arffr();
		}

		private IEnumerator kpxtc()
		{
			return arffr();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in kpxtc
			return this.kpxtc();
		}

		private bool zfdxr()
		{
			switch (vtyqq)
			{
			case 0:
				vtyqq = -1;
				if (!uvrmm.nbnot || 1 == 0)
				{
					zdqci = uvrmm.mzanw();
					vtyqq = 1;
					return true;
				}
				break;
			case 1:
				vtyqq = -1;
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in zfdxr
			return this.zfdxr();
		}

		private void fwpaf()
		{
			throw new NotSupportedException();
		}

		void IEnumerator.Reset()
		{
			//ILSpy generated this explicit interface implementation from .override directive in fwpaf
			this.fwpaf();
		}

		private void gvkae()
		{
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in gvkae
			this.gvkae();
		}

		public kdgaw(int _003C_003E1__state)
		{
			vtyqq = _003C_003E1__state;
			qvrax = Thread.CurrentThread.ManagedThreadId;
		}
	}

	public static apajk<TR> ttrww<TA1, TR>(this apajk<TA1> p0, Func<TA1, TR> p1)
	{
		nlbvc<TA1, TR> nlbvc = new nlbvc<TA1, TR>();
		nlbvc.ubwly = p1;
		if (nlbvc.ubwly == null || 1 == 0)
		{
			throw new ArgumentNullException("selector");
		}
		return p0.jqaxc(nlbvc.ymijb, vvchs.apcdm<TR>);
	}

	public static apajk<TA> ugexw<TA>(this apajk<TA> p0, Func<TA, bool> p1)
	{
		zwnea<TA> zwnea = new zwnea<TA>();
		zwnea.prpuo = p1;
		if (zwnea.prpuo == null || 1 == 0)
		{
			throw new ArgumentNullException("predicate");
		}
		return p0.jqaxc(zwnea.uomzg, vvchs.apcdm<TA>);
	}

	public static apajk<TR> bgsce<TR>(this apajk<apajk<TR>> p0)
	{
		return p0.jqaxc(hassx, iwtgs<TR>);
	}

	public static apajk<TR> seumy<TA1, TR>(this apajk<TA1> p0, Func<TA1, apajk<TR>> p1)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("optionalSelector");
		}
		return p0.jqaxc(p1, vvchs.apcdm<TR>);
	}

	public static apajk<TR> tdzgk<TA1, TR>(this apajk<TA1> p0, Func<TA1, apajk<TR>> p1)
	{
		return p0.seumy(p1);
	}

	public static apajk<TR2> mnzgf<TA1, TR1, TR2>(this apajk<TA1> p0, Func<TA1, apajk<TR1>> p1, Func<TA1, TR1, TR2> p2)
	{
		gaocu<TA1, TR1, TR2> gaocu = new gaocu<TA1, TR1, TR2>();
		gaocu.vmhkr = p1;
		gaocu.xmedb = p2;
		if (gaocu.vmhkr == null || 1 == 0)
		{
			throw new ArgumentNullException("optionalSelector");
		}
		if (gaocu.xmedb == null || 1 == 0)
		{
			throw new ArgumentNullException("resultSelector");
		}
		return p0.jqaxc(gaocu.nthqq, vvchs.apcdm<TR2>);
	}

	public static IEnumerable<T> gusft<T>(this apajk<T> p0)
	{
		kdgaw<T> kdgaw = new kdgaw<T>(-2);
		kdgaw.dendo = p0;
		return kdgaw;
	}

	private static apajk<TR> hassx<TR>(apajk<TR> p0)
	{
		return p0;
	}

	private static apajk<TR> iwtgs<TR>()
	{
		return apajk<TR>.uceou;
	}
}
