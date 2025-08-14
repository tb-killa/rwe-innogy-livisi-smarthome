using System;
using System.Collections;
using System.Collections.Generic;

namespace onrkn;

internal class bbyvy<T0, T1> : IEnumerable<KeyValuePair<T0, T1>>, IEnumerable
{
	private class cgkmq
	{
		private long kdohm;

		private T1 zdmsf;

		public T1 jdupr
		{
			get
			{
				return zdmsf;
			}
			set
			{
				zdmsf = value;
			}
		}

		public cgkmq(T1 value, gixyn clock)
		{
			jdupr = value;
			wzwqv(clock);
		}

		public bool rgvjb(int p0, gixyn p1)
		{
			return kdohm + p0 < p1.csevu;
		}

		public void wzwqv(gixyn p0)
		{
			kdohm = p0.csevu;
		}
	}

	private class gixyn
	{
		private long tjbud;

		private int syesk;

		public long csevu
		{
			get
			{
				int num = syesk;
				syesk = Environment.TickCount;
				tjbud += (uint)(syesk - num);
				return tjbud;
			}
		}

		public gixyn()
		{
			ozueu();
		}

		public void ozueu()
		{
			tjbud = 0L;
			syesk = Environment.TickCount;
		}
	}

	private sealed class vdnkk : IEnumerator<KeyValuePair<T0, T1>>, IEnumerator, IDisposable
	{
		private KeyValuePair<T0, T1> aarmq;

		private int ftchm;

		public bbyvy<T0, T1> bkwlr;

		public KeyValuePair<T0, cgkmq> zouhh;

		public Dictionary<T0, cgkmq>.Enumerator rvbas;

		private KeyValuePair<T0, T1> imqbe => aarmq;

		private object excha => aarmq;

		private bool ybcwa()
		{
			try
			{
				switch (ftchm)
				{
				case 0:
					ftchm = -1;
					rvbas = bkwlr.sgetj.GetEnumerator();
					ftchm = 1;
					goto IL_0097;
				case 2:
					{
						ftchm = 1;
						goto IL_0097;
					}
					IL_0097:
					if (rvbas.MoveNext() ? true : false)
					{
						zouhh = rvbas.Current;
						KeyValuePair<T0, cgkmq> keyValuePair = zouhh;
						T0 key = keyValuePair.Key;
						KeyValuePair<T0, cgkmq> keyValuePair2 = zouhh;
						aarmq = new KeyValuePair<T0, T1>(key, keyValuePair2.Value.jdupr);
						ftchm = 2;
						return true;
					}
					vsdfx();
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				ltmeh();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ybcwa
			return this.ybcwa();
		}

		private void idexm()
		{
			throw new NotSupportedException();
		}

		void IEnumerator.Reset()
		{
			//ILSpy generated this explicit interface implementation from .override directive in idexm
			this.idexm();
		}

		private void ltmeh()
		{
			switch (ftchm)
			{
			case 1:
			case 2:
				try
				{
					break;
				}
				finally
				{
					vsdfx();
				}
			}
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ltmeh
			this.ltmeh();
		}

		public vdnkk(int _003C_003E1__state)
		{
			ftchm = _003C_003E1__state;
		}

		private void vsdfx()
		{
			ftchm = -1;
			((IDisposable)rvbas/*cast due to .constrained prefix*/).Dispose();
		}
	}

	private readonly object jghqk = new object();

	private readonly int snzxh;

	private readonly gixyn kicpj;

	private Dictionary<T0, cgkmq> sgetj;

	private Dictionary<T0, cgkmq> gfydl;

	private long fozuh;

	public int xqtls => snzxh;

	public bbyvy(int timeout)
	{
		snzxh = timeout;
		kicpj = new gixyn();
		sgetj = new Dictionary<T0, cgkmq>();
		gfydl = new Dictionary<T0, cgkmq>();
		fozuh = ftcji();
	}

	private long ftcji()
	{
		return kicpj.csevu / xqtls;
	}

	public T1 wzwsv(T0 p0, T1 p1)
	{
		lock (jghqk)
		{
			zfyvf();
			cgkmq cgkmq = pzeio(p0, p1: false);
			if (cgkmq != null && 0 == 0)
			{
				cgkmq.jdupr = p1;
			}
			else
			{
				sgetj[p0] = new cgkmq(p1, kicpj);
			}
			return p1;
		}
	}

	public T1 twxbv(T0 p0)
	{
		lock (jghqk)
		{
			zfyvf();
			cgkmq cgkmq = pzeio(p0, p1: true);
			if (cgkmq != null && 0 == 0)
			{
				return cgkmq.jdupr;
			}
			return default(T1);
		}
	}

	public bool kvsbw(T0 p0, out T1 p1)
	{
		lock (jghqk)
		{
			zfyvf();
			cgkmq cgkmq = pzeio(p0, p1: true);
			if (cgkmq != null && 0 == 0)
			{
				p1 = cgkmq.jdupr;
				return true;
			}
			p1 = default(T1);
			return false;
		}
	}

	public void xpgwc()
	{
		lock (jghqk)
		{
			gfydl.Clear();
			sgetj.Clear();
		}
	}

	private void zfyvf()
	{
		long num = ftcji();
		if (num != fozuh)
		{
			gfydl.Clear();
			if (num == fozuh + 1)
			{
				dahxy.ynzte(ref gfydl, ref sgetj);
			}
			else
			{
				sgetj.Clear();
			}
			fozuh = num;
		}
	}

	private cgkmq pzeio(T0 p0, bool p1)
	{
		cgkmq cgkmq = cphqq(sgetj, p0, p2: false);
		if (cgkmq != null && 0 == 0)
		{
			return cgkmq;
		}
		cgkmq = cphqq(gfydl, p0, p1);
		if (cgkmq != null && 0 == 0)
		{
			gfydl.Remove(p0);
			sgetj[p0] = cgkmq;
		}
		return cgkmq;
	}

	private cgkmq cphqq(Dictionary<T0, cgkmq> p0, T0 p1, bool p2)
	{
		if (p0.TryGetValue(p1, out var value) && 0 == 0)
		{
			if (p2 && 0 == 0 && value.rgvjb(xqtls, kicpj) && 0 == 0)
			{
				p0.Remove(p1);
				return null;
			}
			value.wzwqv(kicpj);
		}
		return value;
	}

	public IEnumerator<KeyValuePair<T0, T1>> GetEnumerator()
	{
		vdnkk vdnkk = new vdnkk(0);
		vdnkk.bkwlr = this;
		return vdnkk;
	}

	private IEnumerator ajqrs()
	{
		return GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ajqrs
		return this.ajqrs();
	}
}
