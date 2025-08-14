using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace onrkn;

internal class exkzi : IAsyncResult
{
	protected enum wnrns
	{
		nwpeb,
		udsdb,
		plqlf,
		zlkzc,
		ljqcx,
		gfcit
	}

	private sealed class bcith
	{
		public exkzi xfwke;

		public Action<exkzi> ifcgh;

		public void sdedd()
		{
			ifcgh(xfwke);
		}
	}

	private sealed class fudso<T0>
	{
		public exkzi fjrzu;

		public Func<exkzi, T0> rfbut;

		public T0 rfaxi()
		{
			return rfbut(fjrzu);
		}
	}

	private static readonly Action<exkzi, nagsk> ixlmo;

	private static readonly WaitCallback dxfqx;

	private static int fqyhy;

	protected readonly object qacnx = new object();

	private Action tkect;

	private Action<object> ijktb;

	private Action<exkzi, object> tzyky;

	private List<exkzi> rqssu;

	private ManualResetEvent ybtak;

	private gmpgj midqp;

	private int qpdmy;

	private nagsk lyvbq;

	private object okoln;

	private wnrns ffisc;

	private exkzi edsmu;

	private static Action<exkzi, nagsk> lfpnz;

	public object qrmlc
	{
		get
		{
			return okoln;
		}
		protected set
		{
			okoln = value;
		}
	}

	public gmpgj awssf => midqp;

	public int dcfla => qpdmy;

	public nagsk mnscz => lyvbq;

	protected wnrns kyejg
	{
		get
		{
			return ffisc;
		}
		set
		{
			ffisc = value;
		}
	}

	public bool IsCompleted => kqdnm(awssf);

	public bool lctag => awssf == gmpgj.ejvun;

	public bool ijeei => awssf == gmpgj.uznaq;

	private exkzi sbcml
	{
		get
		{
			return edsmu;
		}
		set
		{
			edsmu = value;
		}
	}

	private WaitHandle futgv => iptwa();

	private object uxtbu => qrmlc;

	private bool llghm => false;

	public exkzi(Action action)
	{
		vpdfk();
		iegvd(action);
	}

	public exkzi(Action<exkzi, object> action, object state)
	{
		vpdfk();
		epmgj(action, state);
	}

	public exkzi(Action<object> action, object state)
	{
		vpdfk();
		tzjzp(action, state);
	}

	internal exkzi(gmpgj status)
	{
		vpdfk();
		midqp = status;
	}

	internal exkzi(object state, gmpgj status)
		: this(status)
	{
		vpdfk();
		qrmlc = state;
	}

	public void dfewl()
	{
		lock (qacnx)
		{
			tsbzz();
			vybdv();
		}
	}

	public exkzi wszna(Action<exkzi> p0)
	{
		bcith bcith = new bcith();
		bcith.ifcgh = p0;
		bcith.xfwke = this;
		if (bcith.ifcgh == null || 1 == 0)
		{
			throw new ArgumentNullException("continuation");
		}
		exkzi exkzi2 = new exkzi(bcith.sdedd);
		eadna(exkzi2);
		return exkzi2;
	}

	public exkzi jyasg(Action<exkzi, object> p0, object p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("continuation");
		}
		exkzi exkzi2 = new exkzi(p0, p1);
		eadna(exkzi2);
		return exkzi2;
	}

	public exkzi qizyz(Action<exkzi> p0, arvtx p1)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("scheduler");
		}
		return wszna(p0);
	}

	public njvzu<TNewResult> gqrnv<TNewResult>(Func<exkzi, TNewResult> p0)
	{
		fudso<TNewResult> fudso = new fudso<TNewResult>();
		fudso.rfbut = p0;
		fudso.fjrzu = this;
		if (fudso.rfbut == null || 1 == 0)
		{
			throw new ArgumentNullException("continuation");
		}
		njvzu<TNewResult> njvzu2 = new njvzu<TNewResult>(fudso.rfaxi);
		eadna(njvzu2);
		return njvzu2;
	}

	public void txebj()
	{
		mtfep(-1);
	}

	public bool mtfep(int p0)
	{
		WaitHandle waitHandle;
		lock (qacnx)
		{
			waitHandle = ((kqdnm(midqp) ? true : false) ? null : iptwa());
		}
		if (waitHandle != null && 0 == 0 && (!waitHandle.WaitOne(p0, exitContext: false) || 1 == 0))
		{
			return false;
		}
		lock (qacnx)
		{
			if (lyvbq != null && 0 == 0)
			{
				throw lyvbq;
			}
			if (midqp == gmpgj.ejvun)
			{
				throw new nagsk(new avzkj());
			}
		}
		return true;
	}

	public bool xfxjm(TimeSpan p0)
	{
		long num = (long)p0.TotalMilliseconds;
		if (num < -1)
		{
			throw new ArgumentOutOfRangeException("timeout");
		}
		return mtfep((int)num);
	}

	protected virtual void nsuyd()
	{
		if (tkect != null || ijktb != null || tzyky != null)
		{
			bvilq.cphqj(dxfqx, this);
		}
	}

	protected void eadna(exkzi p0)
	{
		p0.sbcml = this;
		if (kqdnm(midqp) && 0 == 0)
		{
			p0.dfewl();
			return;
		}
		lock (qacnx)
		{
			if (kqdnm(midqp) && 0 == 0)
			{
				p0.dfewl();
				return;
			}
			p0.tsbzz();
			if (rqssu == null || 1 == 0)
			{
				rqssu = new List<exkzi>();
			}
			rqssu.Add(p0);
		}
	}

	protected WaitHandle iptwa()
	{
		if (ybtak == null || 1 == 0)
		{
			lock (qacnx)
			{
				if (ybtak == null || 1 == 0)
				{
					ybtak = new ManualResetEvent(kqdnm(midqp));
				}
			}
		}
		return ybtak;
	}

	protected bool gsfkm<TResult>(gmpgj p0, Action<exkzi, TResult> p1, Action<exkzi, nagsk> p2, nagsk p3, TResult p4, bool p5)
	{
		IEnumerable<exkzi> enumerable = null;
		bool flag = kqdnm(p0);
		lock (qacnx)
		{
			gmpgj gmpgj2 = midqp;
			if (p0 <= gmpgj2 || kqdnm(gmpgj2))
			{
				if (p5 && 0 == 0)
				{
					throw new InvalidOperationException(brgjd.edcru("Task.Status cannot change form {0} to {1}.", gmpgj2, p0));
				}
				return false;
			}
			midqp = p0;
			if (p1 != null && 0 == 0)
			{
				p1(this, p4);
			}
			if (p2 != null && 0 == 0)
			{
				p2(this, p3);
			}
			if (flag && 0 == 0 && rqssu != null && 0 == 0)
			{
				enumerable = rqssu.ToArray();
			}
		}
		if (ybtak != null && 0 == 0 && flag && 0 == 0)
		{
			ybtak.Set();
		}
		if (enumerable != null && 0 == 0 && enumerable.Any() && 0 == 0)
		{
			IEnumerator<exkzi> enumerator = enumerable.GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					exkzi current = enumerator.Current;
					current.vybdv();
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
		return true;
	}

	protected void zjfvk()
	{
		gsfkm(gmpgj.arcbz, null, null, null, gfqoc.jusmc, p5: true);
	}

	internal bool ddszb(bool p0)
	{
		return gsfkm(gmpgj.ejvun, null, null, null, gfqoc.jusmc, p0);
	}

	internal bool zpszo(Exception p0, bool p1)
	{
		nagsk p2 = new nagsk(p0);
		return gsfkm(gmpgj.uznaq, null, ixlmo, p2, gfqoc.jusmc, p1);
	}

	internal bool lyavn(IEnumerable<Exception> p0, bool p1)
	{
		nagsk p2 = new nagsk(p0);
		return gsfkm(gmpgj.uznaq, null, ixlmo, p2, gfqoc.jusmc, p1);
	}

	internal bool nylkz(bool p0)
	{
		return gsfkm(gmpgj.pcduu, null, null, null, gfqoc.jusmc, p0);
	}

	private void iegvd(Action p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("action");
		}
		tkect = p0;
		qrmlc = null;
		kyejg = wnrns.udsdb;
	}

	private void tzjzp(Action<object> p0, object p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("action");
		}
		ijktb = p0;
		qrmlc = p1;
		kyejg = wnrns.plqlf;
	}

	private void epmgj(Action<exkzi, object> p0, object p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("action");
		}
		tzyky = p0;
		qrmlc = p1;
		kyejg = wnrns.zlkzc;
	}

	private void tsbzz()
	{
		gsfkm(gmpgj.sxstl, null, null, null, gfqoc.jusmc, p5: true);
	}

	private void vybdv()
	{
		gsfkm(gmpgj.azfsx, null, null, null, gfqoc.jusmc, p5: true);
		nsuyd();
	}

	private void vpdfk()
	{
		qpdmy = Interlocked.Increment(ref fqyhy);
	}

	public static bool hihjs(exkzi[] p0, int p1)
	{
		exkzi exkzi2 = rxpjc.ybfqj(p0);
		return exkzi2.mtfep(p1);
	}

	public static void oeors(params exkzi[] p0)
	{
		exkzi exkzi2 = rxpjc.ybfqj(p0);
		exkzi2.txebj();
	}

	public static int veyji(exkzi[] p0, int p1)
	{
		njvzu<exkzi> njvzu2 = rxpjc.veygc(p0);
		if (!njvzu2.mtfep(p1) || 1 == 0)
		{
			return -1;
		}
		return opzyi(p0, njvzu2.islme);
	}

	public static int pmvun(params exkzi[] p0)
	{
		njvzu<exkzi> njvzu2 = rxpjc.veygc(p0);
		njvzu2.txebj();
		return opzyi(p0, njvzu2.islme);
	}

	private static int opzyi(exkzi[] p0, exkzi p1)
	{
		int num = Array.IndexOf(p0, p1);
		if (num < 0)
		{
			throw new InvalidOperationException("Task not found.");
		}
		return num;
	}

	private static void ggihp(object p0)
	{
		exkzi exkzi2 = p0 as exkzi;
		exkzi2.zjfvk();
		try
		{
			switch (exkzi2.kyejg)
			{
			case wnrns.udsdb:
				exkzi2.tkect();
				break;
			case wnrns.plqlf:
				exkzi2.ijktb(exkzi2.qrmlc);
				break;
			case wnrns.zlkzc:
				exkzi2.tzyky(exkzi2.sbcml, exkzi2.qrmlc);
				break;
			default:
				throw new InvalidOperationException();
			}
		}
		catch (Exception p1)
		{
			exkzi2.zpszo(p1, p1: true);
			return;
		}
		exkzi2.nylkz(p0: true);
	}

	private static bool kqdnm(gmpgj p0)
	{
		switch (p0)
		{
		case gmpgj.pcduu:
		case gmpgj.ejvun:
		case gmpgj.uznaq:
			return true;
		default:
			return false;
		}
	}

	static exkzi()
	{
		if (lfpnz == null || 1 == 0)
		{
			lfpnz = fcppf;
		}
		ixlmo = lfpnz;
		dxfqx = ggihp;
		fqyhy = -1;
	}

	private static void fcppf(exkzi p0, nagsk p1)
	{
		p0.lyvbq = p1;
	}
}
