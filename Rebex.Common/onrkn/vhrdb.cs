using System;
using System.Collections.Generic;
using System.Threading;

namespace onrkn;

internal static class vhrdb
{
	private abstract class qkobe
	{
		public enum imdfb
		{
			cgops,
			kvemp,
			hwfiz,
			pgzpq,
			dkfer,
			acasc,
			seerj,
			oidow,
			rvoef
		}

		protected const int mnize = 10;

		public abstract bool btlgc();

		public abstract void vsnet();

		public static uilqz<T> orzzh<T>(dvxgu<T> p0, T p1)
		{
			uilqz<T> uilqz = rynhf(p0);
			uilqz.czayc = p1;
			uilqz.dautg = imdfb.kvemp;
			return uilqz;
		}

		public static uilqz<T> hwfrk<T>(dvxgu<T> p0, T p1)
		{
			uilqz<T> uilqz = rynhf(p0);
			uilqz.czayc = p1;
			uilqz.dautg = imdfb.hwfiz;
			return uilqz;
		}

		public static uilqz<T> bhesp<T>(dvxgu<T> p0, Exception p1)
		{
			uilqz<T> uilqz = rynhf(p0);
			uilqz.pvdfm = p1;
			uilqz.dautg = imdfb.pgzpq;
			return uilqz;
		}

		public static uilqz<T> qbfqh<T>(dvxgu<T> p0, Exception p1)
		{
			uilqz<T> uilqz = rynhf(p0);
			uilqz.pvdfm = p1;
			uilqz.dautg = imdfb.dkfer;
			return uilqz;
		}

		public static uilqz<T> ktrfr<T>(dvxgu<T> p0, IEnumerable<Exception> p1)
		{
			uilqz<T> uilqz = rynhf(p0);
			uilqz.weziu = p1;
			uilqz.dautg = imdfb.acasc;
			return uilqz;
		}

		public static uilqz<T> gceso<T>(dvxgu<T> p0, IEnumerable<Exception> p1)
		{
			uilqz<T> uilqz = rynhf(p0);
			uilqz.weziu = p1;
			uilqz.dautg = imdfb.seerj;
			return uilqz;
		}

		public static uilqz<T> ntqlv<T>(dvxgu<T> p0)
		{
			uilqz<T> uilqz = rynhf(p0);
			uilqz.dautg = imdfb.oidow;
			return uilqz;
		}

		public static uilqz<T> oxwpa<T>(dvxgu<T> p0)
		{
			uilqz<T> uilqz = rynhf(p0);
			uilqz.dautg = imdfb.rvoef;
			return uilqz;
		}

		public abstract void djabc();

		private static uilqz<T> rynhf<T>(dvxgu<T> p0)
		{
			uilqz<T> uilqz = uilqz<T>.pzpis.tbdcs();
			uilqz.wlhph = p0;
			return uilqz;
		}
	}

	private class uilqz<T0> : qkobe
	{
		private static eswqz<uilqz<T0>> svart;

		private dvxgu<T0> ybphm;

		private T0 asltj;

		private imdfb oqazi;

		private Exception cccxp;

		private IEnumerable<Exception> ngyvh;

		private static Func<uilqz<T0>> slkhm;

		private static Action<uilqz<T0>> opgkw;

		private static Action<uilqz<T0>> ttqlg;

		public dvxgu<T0> wlhph
		{
			get
			{
				return ybphm;
			}
			set
			{
				ybphm = value;
			}
		}

		public T0 czayc
		{
			get
			{
				return asltj;
			}
			set
			{
				asltj = value;
			}
		}

		public imdfb dautg
		{
			get
			{
				return oqazi;
			}
			set
			{
				oqazi = value;
			}
		}

		public static eswqz<uilqz<T0>> pzpis => svart;

		public Exception pvdfm
		{
			get
			{
				return cccxp;
			}
			set
			{
				cccxp = value;
			}
		}

		public IEnumerable<Exception> weziu
		{
			get
			{
				return ngyvh;
			}
			set
			{
				ngyvh = value;
			}
		}

		public override bool btlgc()
		{
			switch (dautg)
			{
			case imdfb.kvemp:
				return wlhph.lxwus(czayc);
			case imdfb.hwfiz:
				wlhph.cbgge(czayc);
				return false;
			case imdfb.pgzpq:
				return wlhph.ggmwx(pvdfm);
			case imdfb.dkfer:
				wlhph.dassc(pvdfm);
				return false;
			case imdfb.acasc:
				return wlhph.qstcu(weziu);
			case imdfb.seerj:
				wlhph.omxrj(weziu);
				return false;
			case imdfb.oidow:
				return wlhph.kcuac();
			case imdfb.rvoef:
				wlhph.gxdnj();
				return false;
			default:
				throw new InvalidOperationException("Invalid operation.");
			}
		}

		public override void vsnet()
		{
			dautg = imdfb.cgops;
			wlhph = null;
			czayc = default(T0);
			pvdfm = null;
			weziu = null;
		}

		public override void djabc()
		{
			svart.wkkog(this);
		}

		static uilqz()
		{
			if (slkhm == null || 1 == 0)
			{
				slkhm = pzgcj;
			}
			Func<uilqz<T0>> create = slkhm;
			if (opgkw == null || 1 == 0)
			{
				opgkw = rlrog;
			}
			Action<uilqz<T0>> cleanUp = opgkw;
			if (ttqlg == null || 1 == 0)
			{
				ttqlg = vkuah;
			}
			svart = new eswqz<uilqz<T0>>(10, create, cleanUp, ttqlg);
		}

		private static uilqz<T0> pzgcj()
		{
			return new uilqz<T0>();
		}

		private static void rlrog(uilqz<T0> p0)
		{
			p0.vsnet();
		}

		private static void vkuah(uilqz<T0> p0)
		{
		}
	}

	public const string vttvv = "An attempt was made to transition a task to a final state when it had already completed.";

	private const int xgmvl = 1;

	private static readonly sroer<int, int> pykil = new sroer<int, int>();

	private static readonly Func<int, int> cjrox;

	private static readonly Func<int, int, int> pmqax;

	private static readonly WaitCallback aypwx;

	private static Func<int, int> tyxkw;

	private static Func<int, int, int> scjzl;

	private static WaitCallback hdnpi;

	public static bool tkbaj<T>(this dvxgu<T> p0, T p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("taskCompletionSource");
		}
		return cwxdf(qkobe.orzzh(p0, p1), p1: false);
	}

	public static void rfuxn<T>(this dvxgu<T> p0, T p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("taskCompletionSource");
		}
		cwxdf(qkobe.hwfrk(p0, p1), p1: true);
	}

	public static bool aibvq<T>(this dvxgu<T> p0, Exception p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("taskCompletionSource");
		}
		return cwxdf(qkobe.bhesp(p0, p1), p1: false);
	}

	public static void bvadh<T>(this dvxgu<T> p0, Exception p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("taskCompletionSource");
		}
		cwxdf(qkobe.qbfqh(p0, p1), p1: true);
	}

	public static bool uknyc<T>(this dvxgu<T> p0, IEnumerable<Exception> p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("taskCompletionSource");
		}
		return cwxdf(qkobe.ktrfr(p0, p1), p1: false);
	}

	public static void dqvdn<T>(this dvxgu<T> p0, IEnumerable<Exception> p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("taskCompletionSource");
		}
		cwxdf(qkobe.gceso(p0, p1), p1: true);
	}

	public static bool uroed<T>(this dvxgu<T> p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("taskCompletionSource");
		}
		return cwxdf(qkobe.ntqlv(p0), p1: false);
	}

	public static void yefft<T>(this dvxgu<T> p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("taskCompletionSource");
		}
		cwxdf(qkobe.oxwpa(p0), p1: true);
	}

	public static void wulbt<TR, TRR>(this dvxgu<TR> p0, njvzu<TRR> p1) where TRR : TR
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("tcs");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("sourceTask");
		}
		if (!p1.IsCompleted || 1 == 0)
		{
			throw new ArgumentException("sourceTask");
		}
		if (!p0.lrrqo(p1))
		{
			p0.lxwus((TR)(object)p1.islme);
		}
	}

	public static void czprg<TR>(this dvxgu<TR> p0, exkzi p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("tcs");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("sourceTask");
		}
		if (!p1.IsCompleted || 1 == 0)
		{
			throw new ArgumentException("sourceTask");
		}
		if (!p0.lrrqo(p1))
		{
			p0.lxwus(default(TR));
		}
	}

	private static bool lrrqo<TR>(this dvxgu<TR> p0, exkzi p1)
	{
		if (p1.ijeei && 0 == 0)
		{
			p0.qstcu(p1.mnscz.mfkfw);
			return true;
		}
		if (p1.lctag && 0 == 0)
		{
			p0.kcuac();
			return true;
		}
		return false;
	}

	private static bool cwxdf<T>(uilqz<T> p0, bool p1)
	{
		try
		{
			return p0.btlgc();
		}
		catch (InvalidOperationException)
		{
			throw;
		}
		catch (Exception p2)
		{
			pcnje(p2);
		}
		finally
		{
			p0.djabc();
		}
		return false;
	}

	private static void pcnje(Exception p0)
	{
	}

	static vhrdb()
	{
		if (tyxkw == null || 1 == 0)
		{
			tyxkw = ktegx;
		}
		cjrox = tyxkw;
		if (scjzl == null || 1 == 0)
		{
			scjzl = maqbj;
		}
		pmqax = scjzl;
		if (hdnpi == null || 1 == 0)
		{
			hdnpi = qxobu;
		}
		aypwx = hdnpi;
	}

	private static int ktegx(int p0)
	{
		return 1;
	}

	private static int maqbj(int p0, int p1)
	{
		return ++p1;
	}

	private static void qxobu(object p0)
	{
		((qkobe)p0).btlgc();
	}
}
