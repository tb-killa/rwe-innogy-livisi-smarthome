using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace onrkn;

internal static class kzvbz
{
	private sealed class zenpa<T0, T1, T2> : IEnumerable<T2>, IEnumerable, IEnumerator<T2>, IEnumerator, IDisposable
	{
		private T2 zbibq;

		private int geuoi;

		private int eudma;

		public IEnumerable<T0> yuzkt;

		public IEnumerable<T0> xtgfl;

		public IEnumerable<T1> bqkyk;

		public IEnumerable<T1> sccml;

		public Func<T0, T1, T2> xaujj;

		public Func<T0, T1, T2> kshfs;

		public IEnumerator<T0> iuufg;

		public IEnumerator<T1> eeabz;

		private T2 ziham => zbibq;

		private object mbtve => zbibq;

		private IEnumerator<T2> pdoug()
		{
			zenpa<T0, T1, T2> zenpa;
			if (Thread.CurrentThread.ManagedThreadId == eudma && geuoi == -2)
			{
				geuoi = 0;
				zenpa = this;
			}
			else
			{
				zenpa = new zenpa<T0, T1, T2>(0);
			}
			zenpa.yuzkt = xtgfl;
			zenpa.bqkyk = sccml;
			zenpa.xaujj = kshfs;
			return zenpa;
		}

		IEnumerator<T2> IEnumerable<T2>.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in pdoug
			return this.pdoug();
		}

		private IEnumerator vipgv()
		{
			return pdoug();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in vipgv
			return this.vipgv();
		}

		private bool awxbd()
		{
			try
			{
				int num = geuoi;
				if (num != 0)
				{
					if (num != 3)
					{
						goto IL_011c;
					}
					geuoi = 2;
				}
				else
				{
					geuoi = -1;
					if (yuzkt == null || 1 == 0)
					{
						throw new ArgumentNullException("first");
					}
					if (bqkyk == null || 1 == 0)
					{
						throw new ArgumentNullException("second");
					}
					if (xaujj == null || 1 == 0)
					{
						throw new ArgumentNullException("resultSelector");
					}
					iuufg = yuzkt.GetEnumerator();
					geuoi = 1;
					eeabz = bqkyk.GetEnumerator();
					geuoi = 2;
				}
				if (iuufg.MoveNext() && 0 == 0 && (eeabz.MoveNext() ? true : false))
				{
					zbibq = xaujj(iuufg.Current, eeabz.Current);
					geuoi = 3;
					return true;
				}
				jyygd();
				nxyah();
				goto IL_011c;
				IL_011c:
				return false;
			}
			catch
			{
				//try-fault
				zjosm();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in awxbd
			return this.awxbd();
		}

		private void vdvvr()
		{
			throw new NotSupportedException();
		}

		void IEnumerator.Reset()
		{
			//ILSpy generated this explicit interface implementation from .override directive in vdvvr
			this.vdvvr();
		}

		private void zjosm()
		{
			switch (geuoi)
			{
			case 1:
			case 2:
			case 3:
				try
				{
					switch (geuoi)
					{
					case 2:
					case 3:
						try
						{
							break;
						}
						finally
						{
							jyygd();
						}
					}
					break;
				}
				finally
				{
					nxyah();
				}
			}
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in zjosm
			this.zjosm();
		}

		public zenpa(int _003C_003E1__state)
		{
			geuoi = _003C_003E1__state;
			eudma = Thread.CurrentThread.ManagedThreadId;
		}

		private void nxyah()
		{
			geuoi = -1;
			if (iuufg != null && 0 == 0)
			{
				iuufg.Dispose();
			}
		}

		private void jyygd()
		{
			geuoi = 1;
			if (eeabz != null && 0 == 0)
			{
				eeabz.Dispose();
			}
		}
	}

	public static IEnumerable<TResult> ecpzs<TFirst, TSecond, TResult>(this IEnumerable<TFirst> p0, IEnumerable<TSecond> p1, Func<TFirst, TSecond, TResult> p2)
	{
		zenpa<TFirst, TSecond, TResult> zenpa = new zenpa<TFirst, TSecond, TResult>(-2);
		zenpa.xtgfl = p0;
		zenpa.sccml = p1;
		zenpa.kshfs = p2;
		return zenpa;
	}
}
