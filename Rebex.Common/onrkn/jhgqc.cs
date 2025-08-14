using System;
using System.Collections.Generic;
using System.Threading;

namespace onrkn;

internal class jhgqc : IDisposable
{
	private class owjsf : IComparable<owjsf>, IDisposable
	{
		private readonly jhgqc fbijh;

		private TimerCallback rubxd;

		private object mowoy;

		private int glssc;

		public TimerCallback ptrbl
		{
			get
			{
				return rubxd;
			}
			private set
			{
				rubxd = value;
			}
		}

		public object ifviw
		{
			get
			{
				return mowoy;
			}
			private set
			{
				mowoy = value;
			}
		}

		public int ocgvs
		{
			get
			{
				return glssc;
			}
			private set
			{
				glssc = value;
			}
		}

		public owjsf(jhgqc owner, TimerCallback action, object state, int time)
		{
			fbijh = owner;
			ptrbl = action;
			ifviw = state;
			ocgvs = time;
		}

		public int CompareTo(owjsf other)
		{
			return ocgvs.CompareTo(other.ocgvs);
		}

		public void Dispose()
		{
			fbijh.kdqeu(this);
		}
	}

	private sealed class pjggf
	{
		public owjsf arxns;

		public void pfkoc(object p0)
		{
			arxns.ptrbl(arxns.ifviw);
		}
	}

	private const int ukukq = 3;

	private static readonly int ygbnn = -1;

	private static readonly rwfyw<jhgqc> srmcu;

	private int? drvhf;

	private readonly object msdar = new object();

	private readonly List<owjsf> ceceo = new List<owjsf>();

	private Timer euhuo;

	private static Func<jhgqc> ygxbj;

	public static jhgqc hwatx => srmcu.avlfd;

	private jhgqc()
	{
		euhuo = new Timer(shftt, null, ygbnn, ygbnn);
	}

	public static IDisposable ghlqp(TimerCallback p0, object p1, double p2)
	{
		if (p2 >= 2147483647.0)
		{
			throw new ArgumentOutOfRangeException("delay", "Maximum delay is " + 2147483646 + "ms");
		}
		if (p2 < 0.0)
		{
			throw new ArgumentOutOfRangeException("delay", "Delay must be a non-negative number");
		}
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("action");
		}
		return srmcu.avlfd.mvlje(p0, p1, (int)p2);
	}

	public static IDisposable oadmz(TimerCallback p0, object p1, TimeSpan p2)
	{
		return ghlqp(p0, p1, p2.TotalMilliseconds);
	}

	public void Dispose()
	{
		Timer timer = Interlocked.Exchange(ref euhuo, null);
		if (timer != null && 0 == 0)
		{
			timer.Dispose();
		}
	}

	private IDisposable mvlje(TimerCallback p0, object p1, int p2)
	{
		int time = alfvx(Environment.TickCount, p2);
		owjsf owjsf = new owjsf(this, p0, p1, time);
		lock (msdar)
		{
			ceceo.Add(owjsf);
		}
		hiqih();
		return owjsf;
	}

	private void bgqwb()
	{
		lock (msdar)
		{
			if (drvhf.HasValue && 0 == 0)
			{
				drvhf = null;
				euhuo.Change(ygbnn, ygbnn);
			}
		}
	}

	private owjsf ewrer(out int p0)
	{
		p0 = int.MaxValue;
		lock (msdar)
		{
			if (ceceo.Count == 0 || 1 == 0)
			{
				return null;
			}
			int tickCount = Environment.TickCount;
			owjsf result = null;
			using (List<owjsf>.Enumerator enumerator = ceceo.GetEnumerator())
			{
				while (enumerator.MoveNext() ? true : false)
				{
					owjsf current = enumerator.Current;
					int num = yhdxn(current.ocgvs, tickCount);
					if (num < p0)
					{
						result = current;
						p0 = num;
					}
				}
			}
			p0 = ((p0 >= 3) ? p0 : 0);
			return result;
		}
	}

	private void shftt(object p0)
	{
		lock (msdar)
		{
			drvhf = null;
			owjsf owjsf;
			int p1;
			while ((owjsf = ewrer(out p1)) != null && 0 == 0 && yhdxn(owjsf.ocgvs, Environment.TickCount) < 3)
			{
				pjggf pjggf = new pjggf();
				pjggf.arxns = owjsf;
				bvilq.eiuho(pjggf.pfkoc);
				ceceo.Remove(owjsf);
			}
		}
		hiqih();
	}

	private void hiqih()
	{
		lock (msdar)
		{
			int p;
			owjsf owjsf = ewrer(out p);
			if (owjsf == null || 1 == 0)
			{
				bgqwb();
			}
			else
			{
				dusac(p);
			}
		}
	}

	private void kdqeu(owjsf p0)
	{
		lock (msdar)
		{
			if (ceceo.Remove(p0) && 0 == 0)
			{
				hiqih();
			}
		}
	}

	private void dusac(int p0)
	{
		lock (msdar)
		{
			int num = alfvx(Environment.TickCount, p0);
			if (num != drvhf && 0 == 0)
			{
				drvhf = num;
				euhuo.Change(p0, ygbnn);
			}
		}
	}

	private int yhdxn(int p0, int p1)
	{
		return p0 - p1;
	}

	private int alfvx(int p0, int p1)
	{
		return p0 + p1;
	}

	static jhgqc()
	{
		if (ygxbj == null || 1 == 0)
		{
			ygxbj = vyyxd;
		}
		srmcu = new rwfyw<jhgqc>(ygxbj);
	}

	private static jhgqc vyyxd()
	{
		return new jhgqc();
	}
}
