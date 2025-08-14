using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Rebex;

namespace onrkn;

internal static class qnqqf
{
	private sealed class fvcqn<T0> : IEnumerable<T0>, IEnumerable, IEnumerator<T0>, IEnumerator, IDisposable
	{
		private T0 arqev;

		private int ubwdp;

		private int mload;

		public IEnumerable pmqvh;

		public IEnumerable ojedm;

		public object qnbvs;

		public IEnumerator ovmkk;

		public IDisposable atjav;

		private T0 vztkm => arqev;

		private object jabhi => arqev;

		private IEnumerator<T0> trxjg()
		{
			fvcqn<T0> fvcqn;
			if (Thread.CurrentThread.ManagedThreadId == mload && ubwdp == -2)
			{
				ubwdp = 0;
				fvcqn = this;
			}
			else
			{
				fvcqn = new fvcqn<T0>(0);
			}
			fvcqn.pmqvh = ojedm;
			return fvcqn;
		}

		IEnumerator<T0> IEnumerable<T0>.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in trxjg
			return this.trxjg();
		}

		private IEnumerator iijah()
		{
			return trxjg();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in iijah
			return this.iijah();
		}

		private bool lhwvn()
		{
			try
			{
				switch (ubwdp)
				{
				case 0:
					ubwdp = -1;
					if (pmqvh == null || 1 == 0)
					{
						throw new ArgumentNullException("source");
					}
					ovmkk = pmqvh.GetEnumerator();
					ubwdp = 1;
					goto IL_0092;
				case 2:
					{
						ubwdp = 1;
						goto IL_0092;
					}
					IL_0092:
					if (ovmkk.MoveNext() ? true : false)
					{
						qnbvs = ovmkk.Current;
						arqev = (T0)qnbvs;
						ubwdp = 2;
						return true;
					}
					muarj();
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				fshyz();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in lhwvn
			return this.lhwvn();
		}

		private void lbejh()
		{
			throw new NotSupportedException();
		}

		void IEnumerator.Reset()
		{
			//ILSpy generated this explicit interface implementation from .override directive in lbejh
			this.lbejh();
		}

		private void fshyz()
		{
			switch (ubwdp)
			{
			case 1:
			case 2:
				try
				{
					break;
				}
				finally
				{
					muarj();
				}
			}
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in fshyz
			this.fshyz();
		}

		public fvcqn(int _003C_003E1__state)
		{
			ubwdp = _003C_003E1__state;
			mload = Thread.CurrentThread.ManagedThreadId;
		}

		private void muarj()
		{
			ubwdp = -1;
			atjav = ovmkk as IDisposable;
			if (atjav != null && 0 == 0)
			{
				atjav.Dispose();
			}
		}
	}

	private sealed class olqwk<T0>
	{
		public string uoijr;

		public ILogWriter wqvlq;

		public void abtcq(T0 p0)
		{
			string message = uoijr + p0;
			wqvlq.Write(LogLevel.Info, typeof(qnqqf), -1, "LINQEXTENSIONS", message);
		}
	}

	private sealed class scyll<T0, T1> : IEnumerable<T1>, IEnumerable, IEnumerator<T1>, IEnumerator, IDisposable
	{
		private T1 fspqo;

		private int fcmwn;

		private int ovnjw;

		public IEnumerable<T0> awkrr;

		public IEnumerable<T0> erlew;

		public Func<T0, T1> kijes;

		public Func<T0, T1> wmgfl;

		public T0 kbwss;

		public IEnumerator<T0> vnmwh;

		private T1 lzvlb => fspqo;

		private object qzxiz => fspqo;

		private IEnumerator<T1> xnhst()
		{
			scyll<T0, T1> scyll;
			if (Thread.CurrentThread.ManagedThreadId == ovnjw && fcmwn == -2)
			{
				fcmwn = 0;
				scyll = this;
			}
			else
			{
				scyll = new scyll<T0, T1>(0);
			}
			scyll.awkrr = erlew;
			scyll.kijes = wmgfl;
			return scyll;
		}

		IEnumerator<T1> IEnumerable<T1>.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in xnhst
			return this.xnhst();
		}

		private IEnumerator umjsv()
		{
			return xnhst();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in umjsv
			return this.umjsv();
		}

		private bool xnuxj()
		{
			try
			{
				switch (fcmwn)
				{
				case 0:
					fcmwn = -1;
					if (awkrr == null || 1 == 0)
					{
						throw new ArgumentNullException("source");
					}
					if (kijes == null || 1 == 0)
					{
						throw new ArgumentNullException("source");
					}
					vnmwh = awkrr.GetEnumerator();
					fcmwn = 1;
					goto IL_00b3;
				case 2:
					{
						fcmwn = 1;
						goto IL_00b3;
					}
					IL_00b3:
					if (vnmwh.MoveNext() ? true : false)
					{
						kbwss = vnmwh.Current;
						fspqo = kijes(kbwss);
						fcmwn = 2;
						return true;
					}
					kwgft();
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				vmrgt();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in xnuxj
			return this.xnuxj();
		}

		private void bonql()
		{
			throw new NotSupportedException();
		}

		void IEnumerator.Reset()
		{
			//ILSpy generated this explicit interface implementation from .override directive in bonql
			this.bonql();
		}

		private void vmrgt()
		{
			switch (fcmwn)
			{
			case 1:
			case 2:
				try
				{
					break;
				}
				finally
				{
					kwgft();
				}
			}
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in vmrgt
			this.vmrgt();
		}

		public scyll(int _003C_003E1__state)
		{
			fcmwn = _003C_003E1__state;
			ovnjw = Thread.CurrentThread.ManagedThreadId;
		}

		private void kwgft()
		{
			fcmwn = -1;
			if (vnmwh != null && 0 == 0)
			{
				vnmwh.Dispose();
			}
		}
	}

	private sealed class ktrkz<T0> : IEnumerable<T0>, IEnumerable, IEnumerator<T0>, IEnumerator, IDisposable
	{
		private T0 foyjk;

		private int eodbo;

		private int xikwa;

		public IEnumerable<T0> dhqmz;

		public IEnumerable<T0> bmzft;

		public T0 fupxm;

		public T0 pzqis;

		public T0 ezzik;

		public IEnumerator<T0> xdlin;

		private T0 wotyf => foyjk;

		private object ohvdr => foyjk;

		private IEnumerator<T0> xkouy()
		{
			ktrkz<T0> ktrkz;
			if (Thread.CurrentThread.ManagedThreadId == xikwa && eodbo == -2)
			{
				eodbo = 0;
				ktrkz = this;
			}
			else
			{
				ktrkz = new ktrkz<T0>(0);
			}
			ktrkz.dhqmz = bmzft;
			ktrkz.fupxm = pzqis;
			return ktrkz;
		}

		IEnumerator<T0> IEnumerable<T0>.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in xkouy
			return this.xkouy();
		}

		private IEnumerator pwypn()
		{
			return xkouy();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in pwypn
			return this.pwypn();
		}

		private bool xayzu()
		{
			try
			{
				switch (eodbo)
				{
				case 0:
					eodbo = -1;
					xdlin = dhqmz.GetEnumerator();
					eodbo = 1;
					goto IL_0076;
				case 2:
					eodbo = 1;
					goto IL_0076;
				case 3:
					{
						eodbo = -1;
						break;
					}
					IL_0076:
					if (xdlin.MoveNext() ? true : false)
					{
						ezzik = xdlin.Current;
						foyjk = ezzik;
						eodbo = 2;
						return true;
					}
					xmdms();
					foyjk = fupxm;
					eodbo = 3;
					return true;
				}
				return false;
			}
			catch
			{
				//try-fault
				gkzgf();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in xayzu
			return this.xayzu();
		}

		private void woiwa()
		{
			throw new NotSupportedException();
		}

		void IEnumerator.Reset()
		{
			//ILSpy generated this explicit interface implementation from .override directive in woiwa
			this.woiwa();
		}

		private void gkzgf()
		{
			switch (eodbo)
			{
			case 1:
			case 2:
				try
				{
					break;
				}
				finally
				{
					xmdms();
				}
			}
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in gkzgf
			this.gkzgf();
		}

		public ktrkz(int _003C_003E1__state)
		{
			eodbo = _003C_003E1__state;
			xikwa = Thread.CurrentThread.ManagedThreadId;
		}

		private void xmdms()
		{
			eodbo = -1;
			if (xdlin != null && 0 == 0)
			{
				xdlin.Dispose();
			}
		}
	}

	private sealed class krfws<T0> : IEnumerable<T0>, IEnumerable, IEnumerator<T0>, IEnumerator, IDisposable
	{
		private T0 jtohj;

		private int cmlqg;

		private int coewv;

		public IEnumerable<T0> xouvb;

		public IEnumerable<T0> zhpyq;

		public Action<T0> hojjr;

		public Action<T0> atobc;

		public T0 vmghc;

		public IEnumerator<T0> lcowd;

		private T0 hvedo => jtohj;

		private object legru => jtohj;

		private IEnumerator<T0> yghsb()
		{
			krfws<T0> krfws;
			if (Thread.CurrentThread.ManagedThreadId == coewv && cmlqg == -2)
			{
				cmlqg = 0;
				krfws = this;
			}
			else
			{
				krfws = new krfws<T0>(0);
			}
			krfws.xouvb = zhpyq;
			krfws.hojjr = atobc;
			return krfws;
		}

		IEnumerator<T0> IEnumerable<T0>.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in yghsb
			return this.yghsb();
		}

		private IEnumerator twhho()
		{
			return yghsb();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in twhho
			return this.twhho();
		}

		private bool ncgrd()
		{
			try
			{
				switch (cmlqg)
				{
				case 0:
					cmlqg = -1;
					lcowd = xouvb.GetEnumerator();
					cmlqg = 1;
					goto IL_0083;
				case 2:
					{
						cmlqg = 1;
						goto IL_0083;
					}
					IL_0083:
					if (lcowd.MoveNext() ? true : false)
					{
						vmghc = lcowd.Current;
						hojjr(vmghc);
						jtohj = vmghc;
						cmlqg = 2;
						return true;
					}
					rmhxb();
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				jypym();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ncgrd
			return this.ncgrd();
		}

		private void nkdpx()
		{
			throw new NotSupportedException();
		}

		void IEnumerator.Reset()
		{
			//ILSpy generated this explicit interface implementation from .override directive in nkdpx
			this.nkdpx();
		}

		private void jypym()
		{
			switch (cmlqg)
			{
			case 1:
			case 2:
				try
				{
					break;
				}
				finally
				{
					rmhxb();
				}
			}
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in jypym
			this.jypym();
		}

		public krfws(int _003C_003E1__state)
		{
			cmlqg = _003C_003E1__state;
			coewv = Thread.CurrentThread.ManagedThreadId;
		}

		private void rmhxb()
		{
			cmlqg = -1;
			if (lcowd != null && 0 == 0)
			{
				lcowd.Dispose();
			}
		}
	}

	private sealed class eyaxp<T0> : IEnumerable<T0>, IEnumerable, IEnumerator<T0>, IEnumerator, IDisposable
	{
		private T0 uhysm;

		private int uxwnu;

		private int ffmqi;

		public IEnumerable<T0> ekdnw;

		public IEnumerable<T0> tjoiq;

		public T0 mtude;

		public T0 uebcu;

		public T0 kqddq;

		public IEnumerator<T0> wngcz;

		private T0 oppsj => uhysm;

		private object gahlo => uhysm;

		private IEnumerator<T0> hscgf()
		{
			eyaxp<T0> eyaxp;
			if (Thread.CurrentThread.ManagedThreadId == ffmqi && uxwnu == -2)
			{
				uxwnu = 0;
				eyaxp = this;
			}
			else
			{
				eyaxp = new eyaxp<T0>(0);
			}
			eyaxp.ekdnw = tjoiq;
			eyaxp.mtude = uebcu;
			return eyaxp;
		}

		IEnumerator<T0> IEnumerable<T0>.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in hscgf
			return this.hscgf();
		}

		private IEnumerator yzkqx()
		{
			return hscgf();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in yzkqx
			return this.yzkqx();
		}

		private bool zjpyy()
		{
			try
			{
				switch (uxwnu)
				{
				case 0:
					uxwnu = -1;
					uhysm = mtude;
					uxwnu = 1;
					return true;
				case 1:
					uxwnu = -1;
					wngcz = ekdnw.GetEnumerator();
					uxwnu = 2;
					goto IL_0097;
				case 3:
					{
						uxwnu = 2;
						goto IL_0097;
					}
					IL_0097:
					if (wngcz.MoveNext() ? true : false)
					{
						kqddq = wngcz.Current;
						uhysm = kqddq;
						uxwnu = 3;
						return true;
					}
					glpkn();
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				ynjff();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in zjpyy
			return this.zjpyy();
		}

		private void uspfs()
		{
			throw new NotSupportedException();
		}

		void IEnumerator.Reset()
		{
			//ILSpy generated this explicit interface implementation from .override directive in uspfs
			this.uspfs();
		}

		private void ynjff()
		{
			switch (uxwnu)
			{
			case 2:
			case 3:
				try
				{
					break;
				}
				finally
				{
					glpkn();
				}
			}
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ynjff
			this.ynjff();
		}

		public eyaxp(int _003C_003E1__state)
		{
			uxwnu = _003C_003E1__state;
			ffmqi = Thread.CurrentThread.ManagedThreadId;
		}

		private void glpkn()
		{
			uxwnu = -1;
			if (wngcz != null && 0 == 0)
			{
				wngcz.Dispose();
			}
		}
	}

	public static IEnumerable<T> dwgkr<T>(this IEnumerable<T> p0, T p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("values");
		}
		return kwqfg(p0, p1);
	}

	public static IEnumerable<TReturn> eaqmu<TReturn>(this IEnumerable p0)
	{
		fvcqn<TReturn> fvcqn = new fvcqn<TReturn>(-2);
		fvcqn.ojedm = p0;
		return fvcqn;
	}

	public static IEnumerable<T> tvgnw<T>(this IEnumerable<T> p0, T p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("source");
		}
		return sevsh(p0, p1);
	}

	public static IEnumerable<T> icauz<T>(this IEnumerable<T> p0, ILogWriter p1)
	{
		return p0.vsolz(p1, string.Empty);
	}

	public static IEnumerable<T> vsolz<T>(this IEnumerable<T> p0, ILogWriter p1, string p2)
	{
		olqwk<T> olqwk = new olqwk<T>();
		olqwk.wqvlq = p1;
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("source");
		}
		if (olqwk.wqvlq == null || 1 == 0)
		{
			throw new ArgumentNullException("logWriter");
		}
		string text = p2;
		if (text == null || 1 == 0)
		{
			text = string.Empty;
		}
		olqwk.uoijr = text;
		return p0.kgkfm(olqwk.abtcq);
	}

	public static void asaqa<TSource>(this IEnumerable<TSource> p0, Action<TSource> p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("source");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("action");
		}
		kfubl(p0, p1);
	}

	public static IEnumerable<TSource> kgkfm<TSource>(this IEnumerable<TSource> p0, Action<TSource> p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("source");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("action");
		}
		return ivxde(p0, p1);
	}

	public static IEnumerable<TReturn> enqpw<TSource, TReturn>(this IEnumerable<TSource> p0, Func<TSource, TReturn> p1)
	{
		scyll<TSource, TReturn> scyll = new scyll<TSource, TReturn>(-2);
		scyll.erlew = p0;
		scyll.wmgfl = p1;
		return scyll;
	}

	private static IEnumerable<T> sevsh<T>(IEnumerable<T> p0, T p1)
	{
		ktrkz<T> ktrkz = new ktrkz<T>(-2);
		ktrkz.bmzft = p0;
		ktrkz.pzqis = p1;
		return ktrkz;
	}

	private static IEnumerable<TSource> ivxde<TSource>(IEnumerable<TSource> p0, Action<TSource> p1)
	{
		krfws<TSource> krfws = new krfws<TSource>(-2);
		krfws.zhpyq = p0;
		krfws.atobc = p1;
		return krfws;
	}

	private static void kfubl<TSource>(IEnumerable<TSource> p0, Action<TSource> p1)
	{
		IEnumerator<TSource> enumerator = p0.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				TSource current = enumerator.Current;
				p1(current);
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
	}

	private static IEnumerable<T> kwqfg<T>(IEnumerable<T> p0, T p1)
	{
		eyaxp<T> eyaxp = new eyaxp<T>(-2);
		eyaxp.tjoiq = p0;
		eyaxp.uebcu = p1;
		return eyaxp;
	}
}
