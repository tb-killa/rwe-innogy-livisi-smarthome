using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace onrkn;

internal static class rxpjc
{
	private sealed class ifijz
	{
		public dvxgu<object> kkxiq;

		public void dslsw()
		{
			kkxiq.cbgge(null);
		}

		public void sfkbu(Exception p0)
		{
			kkxiq.dassc(p0);
		}
	}

	private sealed class wmyre<T0> where T0 : exkzi
	{
		public dvxgu<T0> gfqwm;

		public int cvkak;

		public void xliim(exkzi p0)
		{
			if (Interlocked.Increment(ref cvkak) == 1)
			{
				gfqwm.cbgge((T0)p0);
			}
		}
	}

	private sealed class bbyhu<T0>
	{
		public List<exkzi> axjoh;

		public dvxgu<T0[]> vhoku;

		public int icnbw;

		public void dmhri(exkzi p0)
		{
			if (Interlocked.Decrement(ref icnbw) == 0 || 1 == 0)
			{
				rlisj(vhoku, axjoh);
			}
		}
	}

	private sealed class kahgi<T0>
	{
		public exkzi aulcj;

		public Func<T0> loqnq;

		public AsyncCallback cpzlv;

		public T0 lggwx(object p0)
		{
			return loqnq();
		}

		public void dvoha(exkzi p0)
		{
			cpzlv(aulcj);
		}
	}

	private sealed class brted<T0, T1> where T0 : T1
	{
		public dvxgu<T1> nrsso;

		public njvzu<T0> icnbm;

		public void junul(exkzi p0)
		{
			switch (p0.awssf)
			{
			case gmpgj.ejvun:
				nrsso.gxdnj();
				break;
			case gmpgj.pcduu:
				nrsso.cbgge((T1)(object)icnbm.islme);
				break;
			case gmpgj.uznaq:
				nrsso.dassc(icnbm.mnscz.InnerException);
				break;
			default:
				throw new InvalidOperationException("Unexpected task status.");
			}
		}
	}

	private sealed class cvwls<T0>
	{
		public dvxgu<T0> jnmox;

		public Func<IAsyncResult, T0> orntm;

		public void bqqyy(IAsyncResult p0)
		{
			try
			{
				T0 p1 = orntm(p0);
				jnmox.cbgge(p1);
			}
			catch (Exception p2)
			{
				jnmox.dassc(p2);
			}
		}
	}

	public const int ccvol = 0;

	public const int zezof = 1;

	private static int qvoqf;

	private static int kyfnc;

	private static readonly exkzi bcdzh;

	private static readonly njvzu<int> cfxve;

	private static readonly njvzu<bool> vcake;

	private static readonly njvzu<bool> qooyk;

	private static readonly Action<object> rcnyu;

	private static readonly Action<exkzi, object> iojfd;

	private static readonly TimerCallback kaupm;

	private static bool weiwi;

	private static Action<object> lgsua;

	private static Action<exkzi, object> htzvx;

	private static TimerCallback tqtfn;

	public static bool kigng
	{
		get
		{
			return weiwi;
		}
		set
		{
			weiwi = value;
		}
	}

	public static exkzi iccat => bcdzh;

	public static bool gftxs
	{
		get
		{
			if (kigng && 0 == 0)
			{
				return Interlocked.CompareExchange(ref qvoqf, -1, -1) == 1;
			}
			return false;
		}
	}

	public static int bpvyo => kyfnc;

	public static njvzu<int> retzk => cfxve;

	public static njvzu<bool> cgfch => vcake;

	public static njvzu<bool> mrpkx => qooyk;

	public static exkzi fvxvk => iccat;

	static rxpjc()
	{
		qvoqf = 0;
		kyfnc = 0;
		bcdzh = caxut<object>(null);
		cfxve = caxut(0);
		vcake = caxut(p0: true);
		qooyk = caxut(p0: false);
		if (lgsua == null || 1 == 0)
		{
			lgsua = ltixb;
		}
		rcnyu = lgsua;
		if (htzvx == null || 1 == 0)
		{
			htzvx = uaqwa;
		}
		iojfd = htzvx;
		if (tqtfn == null || 1 == 0)
		{
			tqtfn = cuuwh;
		}
		kaupm = tqtfn;
		kigng = true;
		tjzre();
	}

	public static exkzi oxwba(Action p0)
	{
		exkzi exkzi2 = new exkzi(p0);
		exkzi2.dfewl();
		return exkzi2;
	}

	public static exkzi chuft(Action<object> p0, object p1)
	{
		exkzi exkzi2 = new exkzi(p0, p1);
		exkzi2.dfewl();
		return exkzi2;
	}

	public static njvzu<TResult> jgkuq<TResult>(Func<TResult> p0)
	{
		njvzu<TResult> njvzu2 = new njvzu<TResult>(p0);
		njvzu2.dfewl();
		return njvzu2;
	}

	public static njvzu<TResult> bbjth<TResult>(Func<object, TResult> p0, object p1)
	{
		njvzu<TResult> njvzu2 = new njvzu<TResult>(p0, p1);
		njvzu2.dfewl();
		return njvzu2;
	}

	public static exkzi kujgo(Exception p0)
	{
		return ibfoj<object>(p0);
	}

	public static njvzu<TResult> ibfoj<TResult>(Exception p0)
	{
		dvxgu<TResult> dvxgu2 = new dvxgu<TResult>();
		dvxgu2.dassc(p0);
		return dvxgu2.dioyl;
	}

	public static njvzu<TResult> caxut<TResult>(TResult p0)
	{
		dvxgu<TResult> dvxgu2 = new dvxgu<TResult>();
		dvxgu2.cbgge(p0);
		return dvxgu2.dioyl;
	}

	public static exkzi vaukn(int p0)
	{
		Action action = null;
		Action<Exception> action2 = null;
		ifijz ifijz = new ifijz();
		ifijz.kkxiq = new dvxgu<object>();
		if (p0 == 0 || 1 == 0)
		{
			ifijz.kkxiq.cbgge(null);
		}
		else if (p0 > 0)
		{
			if (action == null || 1 == 0)
			{
				action = ifijz.dslsw;
			}
			Action p1 = action;
			if (action2 == null || 1 == 0)
			{
				action2 = ifijz.sfkbu;
			}
			dahxy.nqapv(p1, action2, p0);
		}
		else if (p0 != -1)
		{
			throw hifyx.nztrs("milliseconds", p0, "Invalid value.");
		}
		return ifijz.kkxiq.dioyl;
	}

	public static exkzi aexnr(int p0, ddmlv p1)
	{
		if (p0 == 0 || 1 == 0)
		{
			return iccat;
		}
		dvxgu<object> dvxgu2 = new dvxgu<object>();
		p1.kjwdi(rcnyu, dvxgu2);
		if (p0 > 0)
		{
			IDisposable p2 = jhgqc.ghlqp(kaupm, dvxgu2, p0);
			return dvxgu2.dioyl.wdogv(iojfd, p2);
		}
		if (p0 != -1)
		{
			throw hifyx.nztrs("milliseconds", p0, "Invalid value.");
		}
		return dvxgu2.dioyl;
	}

	public static exkzi jhljn(IEnumerable<exkzi> p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("tasks");
		}
		return qwatt<object>(p0);
	}

	public static njvzu<TResult[]> gnmui<TResult>(IEnumerable<njvzu<TResult>> p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("tasks");
		}
		return qwatt<TResult>(p0.Cast<exkzi>());
	}

	public static exkzi ybfqj(params exkzi[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("tasks");
		}
		return qwatt<object>(p0);
	}

	public static njvzu<TResult[]> xyurq<TResult>(params njvzu<TResult>[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("tasks");
		}
		return qwatt<TResult>(p0);
	}

	public static njvzu<exkzi> jkgpi(IEnumerable<exkzi> p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("tasks");
		}
		return ijurx(p0);
	}

	public static njvzu<njvzu<TResult>> hubul<TResult>(IEnumerable<njvzu<TResult>> p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("tasks");
		}
		return ijurx(p0);
	}

	public static njvzu<exkzi> veygc(params exkzi[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("tasks");
		}
		return ijurx(p0);
	}

	public static njvzu<njvzu<TResult>> afuut<TResult>(params njvzu<TResult>[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("tasks");
		}
		return ijurx(p0);
	}

	internal static njvzu<TTaskResult> ijurx<TTaskResult>(IEnumerable<TTaskResult> p0) where TTaskResult : exkzi
	{
		Action<exkzi> action = null;
		wmyre<TTaskResult> wmyre = new wmyre<TTaskResult>();
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("tasks");
		}
		wmyre.gfqwm = new dvxgu<TTaskResult>();
		List<TTaskResult> list = new List<TTaskResult>();
		int num = 0;
		IEnumerator<TTaskResult> enumerator = p0.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				TTaskResult current = enumerator.Current;
				if (current == null || 1 == 0)
				{
					throw new ArgumentException("Null task was specified.");
				}
				if (current.IsCompleted && 0 == 0)
				{
					wmyre.gfqwm.cbgge(current);
					return wmyre.gfqwm.dioyl;
				}
				list.Add(current);
				num++;
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		if (num == 0 || 1 == 0)
		{
			throw new ArgumentException("No tasks specified.", "tasks");
		}
		wmyre.cvkak = 0;
		using (List<TTaskResult>.Enumerator enumerator2 = list.GetEnumerator())
		{
			while (enumerator2.MoveNext() ? true : false)
			{
				TTaskResult current2 = enumerator2.Current;
				if (action == null || 1 == 0)
				{
					action = wmyre.xliim;
				}
				current2.kvzxl(action);
			}
		}
		return wmyre.gfqwm.dioyl;
	}

	internal static njvzu<TResult[]> qwatt<TResult>(IEnumerable<exkzi> p0)
	{
		Action<exkzi> action = null;
		bbyhu<TResult> bbyhu = new bbyhu<TResult>();
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("tasks");
		}
		List<exkzi> list = new List<exkzi>();
		bbyhu.axjoh = new List<exkzi>();
		IEnumerator<exkzi> enumerator = p0.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				exkzi current = enumerator.Current;
				if (current == null || 1 == 0)
				{
					throw new ArgumentException("Null task was specified.");
				}
				if (!current.IsCompleted || 1 == 0)
				{
					list.Add(current);
				}
				bbyhu.axjoh.Add(current);
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		if (bbyhu.axjoh.Count == 0 || 1 == 0)
		{
			throw new ArgumentException("No tasks specified.", "tasks");
		}
		bbyhu.vhoku = new dvxgu<TResult[]>();
		bbyhu.icnbw = list.Count;
		if (bbyhu.icnbw == 0 || 1 == 0)
		{
			rlisj(bbyhu.vhoku, p0);
		}
		else
		{
			using List<exkzi>.Enumerator enumerator2 = list.GetEnumerator();
			while (enumerator2.MoveNext() ? true : false)
			{
				exkzi current2 = enumerator2.Current;
				if (action == null || 1 == 0)
				{
					action = bbyhu.dmhri;
				}
				current2.kvzxl(action);
			}
		}
		return bbyhu.vhoku.dioyl;
	}

	private static void rlisj<TResult>(dvxgu<TResult[]> p0, IEnumerable<exkzi> p1)
	{
		Func<exkzi, Exception> func = null;
		IEnumerable<exkzi> source = p1.Where(smmtx<TResult>);
		if (source.Any() && 0 == 0)
		{
			if (func == null || 1 == 0)
			{
				func = ncfuj<TResult>;
			}
			p0.omxrj(source.Select(func));
		}
		else if (p1.Any(qbgdm<TResult>) && 0 == 0)
		{
			p0.gxdnj();
		}
		else
		{
			p0.cbgge(p1.Select(jfhjb<TResult>).ToArray());
		}
	}

	private static TResult vxyxs<TResult>(exkzi p0)
	{
		if (p0 is njvzu<TResult> njvzu2 && 0 == 0)
		{
			return njvzu2.islme;
		}
		return default(TResult);
	}

	public static IAsyncResult tzeev<TResult>(Func<TResult> p0, AsyncCallback p1, object p2)
	{
		Action<exkzi> action = null;
		kahgi<TResult> kahgi = new kahgi<TResult>();
		kahgi.loqnq = p0;
		kahgi.cpzlv = p1;
		if (kahgi.loqnq == null || 1 == 0)
		{
			throw new ArgumentNullException("action");
		}
		kahgi.aulcj = bbjth(kahgi.lggwx, p2);
		if (kahgi.cpzlv != null && 0 == 0)
		{
			exkzi aulcj = kahgi.aulcj;
			if (action == null || 1 == 0)
			{
				action = kahgi.dvoha;
			}
			aulcj.kvzxl(action);
		}
		return kahgi.aulcj;
	}

	public static TResult wzgzd<TResult>(IAsyncResult p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("asyncResult");
		}
		if (!(p0 is njvzu<TResult> p1) || 1 == 0)
		{
			throw new ArgumentException(brgjd.edcru("The IAsyncResult object supplied to End{0} was not returned from the corresponding Begin{0} method on this class.", ""));
		}
		return p1.ymczw();
	}

	public static njvzu<TResult> etalz<TInnerResult, TResult>(this njvzu<TInnerResult> p0) where TInnerResult : TResult
	{
		brted<TInnerResult, TResult> brted = new brted<TInnerResult, TResult>();
		brted.icnbm = p0;
		brted.nrsso = new dvxgu<TResult>();
		brted.icnbm.kvzxl(brted.junul);
		return brted.nrsso.dioyl;
	}

	public static njvzu<TR> pimhv<TR>(Action<AsyncCallback> p0, Func<IAsyncResult, TR> p1)
	{
		AsyncCallback asyncCallback = null;
		cvwls<TR> cvwls = new cvwls<TR>();
		cvwls.orntm = p1;
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("beginFunc");
		}
		if (cvwls.orntm == null || 1 == 0)
		{
			throw new ArgumentNullException("endFunc");
		}
		cvwls.jnmox = new dvxgu<TR>();
		try
		{
			if (asyncCallback == null || 1 == 0)
			{
				asyncCallback = cvwls.bqqyy;
			}
			p0(asyncCallback);
		}
		catch (Exception p2)
		{
			cvwls.jnmox.dassc(p2);
		}
		return cvwls.jnmox.dioyl;
	}

	public static exkzi lnolo(ddmlv p0)
	{
		return yiget<object>(p0);
	}

	public static njvzu<T> yiget<T>(ddmlv p0)
	{
		if (!p0.bhxda || 1 == 0)
		{
			throw new ArgumentOutOfRangeException("ct", "Cancellation has not been requested.");
		}
		dvxgu<T> dvxgu2 = new dvxgu<T>();
		dvxgu2.gxdnj();
		return dvxgu2.dioyl;
	}

	public static bool ukhui()
	{
		return false;
	}

	public static void tjzre()
	{
	}

	public static void ehrzb()
	{
	}

	private static void ltixb(object p0)
	{
		((dvxgu<object>)p0).kcuac();
	}

	private static void uaqwa(exkzi p0, object p1)
	{
		((IDisposable)p1).Dispose();
	}

	private static void cuuwh(object p0)
	{
		((dvxgu<object>)p0).lxwus(null);
	}

	private static bool smmtx<TResult>(exkzi p0)
	{
		return p0.ijeei;
	}

	private static Exception ncfuj<TResult>(exkzi p0)
	{
		return p0.mnscz.InnerException;
	}

	private static bool qbgdm<TResult>(exkzi p0)
	{
		return p0.lctag;
	}

	private static TResult jfhjb<TResult>(exkzi p0)
	{
		return vxyxs<TResult>(p0);
	}
}
