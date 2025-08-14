using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace onrkn;

internal class wlztg<T0> : IDisposable
{
	private class ilopv
	{
		private T0 iicxh;

		private Exception staei;

		public T0 ozklb
		{
			get
			{
				if (xbenq != null && 0 == 0)
				{
					throw xbenq;
				}
				return iicxh;
			}
			private set
			{
				iicxh = value;
			}
		}

		private Exception xbenq
		{
			get
			{
				return staei;
			}
			set
			{
				staei = value;
			}
		}

		public static implicit operator ilopv(T0 value)
		{
			ilopv ilopv = new ilopv();
			ilopv.ozklb = value;
			return ilopv;
		}

		public static implicit operator ilopv(Exception exception)
		{
			ilopv ilopv = new ilopv();
			ilopv.xbenq = exception;
			return ilopv;
		}
	}

	private sealed class uxrlp
	{
		public Func<T0> qstfr;

		public ilopv ibbrf(Thread p0)
		{
			try
			{
				return qstfr();
			}
			catch (Exception ex)
			{
				return ex;
			}
		}
	}

	private readonly Func<Thread, ilopv> zhejr;

	private readonly sroer<Thread, ilopv> qqiyl;

	private bool mepll;

	private static Func<Thread, ilopv> qdrqr;

	private static Func<ilopv, T0> afati;

	public T0 exziz
	{
		get
		{
			plzss();
			ilopv ilopv = qqiyl.rcwvk(Thread.CurrentThread, zhejr);
			return ilopv.ozklb;
		}
		set
		{
			plzss();
			qqiyl[Thread.CurrentThread] = value;
		}
	}

	public IList<T0> fozqx
	{
		get
		{
			plzss();
			ICollection<ilopv> values = qqiyl.Values;
			if (afati == null || 1 == 0)
			{
				afati = wpeyp;
			}
			return values.Select(afati).ToList();
		}
	}

	public bool mnnoy
	{
		get
		{
			plzss();
			return qqiyl.ContainsKey(Thread.CurrentThread);
		}
	}

	public wlztg()
	{
		if (qdrqr == null || 1 == 0)
		{
			qdrqr = vzhqw;
		}
		zhejr = qdrqr;
		qqiyl = new sroer<Thread, ilopv>();
	}

	public wlztg(Func<T0> valueFactory)
	{
		Func<Thread, ilopv> func = null;
		uxrlp uxrlp = new uxrlp
		{
			qstfr = valueFactory
		};
		base._002Ector();
		if (uxrlp.qstfr == null || 1 == 0)
		{
			throw new ArgumentNullException("valueFactory");
		}
		if (func == null || 1 == 0)
		{
			func = uxrlp.ibbrf;
		}
		zhejr = func;
		qqiyl = new sroer<Thread, ilopv>();
	}

	public void Dispose()
	{
		if (!mepll)
		{
			yvhjq(p0: true);
			mepll = true;
			GC.SuppressFinalize(this);
		}
	}

	public void ditqa()
	{
		plzss();
		qqiyl.xdcrq(Thread.CurrentThread, out var _);
	}

	public void jozpc()
	{
		plzss();
		xfscu();
	}

	protected virtual void yvhjq(bool p0)
	{
		if (p0 ? true : false)
		{
			xfscu();
		}
	}

	private void plzss()
	{
		if (mepll && 0 == 0)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
	}

	private void xfscu()
	{
		qqiyl.Clear();
	}

	~wlztg()
	{
		yvhjq(p0: false);
	}

	private static ilopv vzhqw(Thread p0)
	{
		return default(T0);
	}

	private static T0 wpeyp(ilopv p0)
	{
		return p0.ozklb;
	}
}
