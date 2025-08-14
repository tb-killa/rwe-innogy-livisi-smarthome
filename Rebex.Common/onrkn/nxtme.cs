using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace onrkn;

internal struct nxtme<T0> : IEquatable<nxtme<T0>>, IEnumerable<T0>, IEnumerable, aviir<T0>
{
	public struct rrjio : IEnumerator<T0>, IDisposable, IEnumerator
	{
		private nxtme<T0> ebvrh;

		private int qaikh;

		public T0 Current => ebvrh[qaikh];

		private object phkqr => Current;

		public rrjio(nxtme<T0> arrayView)
		{
			this = default(rrjio);
			ebvrh = arrayView;
			qaikh = -1;
		}

		public void Dispose()
		{
			qaikh = -1;
		}

		public bool MoveNext()
		{
			return ++qaikh < ebvrh.tvoem;
		}

		public void Reset()
		{
			throw new NotImplementedException();
		}
	}

	private sealed class fmjbw : IEnumerable<nxtme<T0>>, IEnumerable, IEnumerator<nxtme<T0>>, IEnumerator, IDisposable
	{
		private nxtme<T0> cjnbl;

		private int ygplg;

		private int polvt;

		public nxtme<T0> gbkqw;

		public nxtme<T0> hipni;

		public int qhdot;

		public int ntiur;

		public int agaqx;

		public int jxvuv;

		public nxtme<T0> uvokn;

		private nxtme<T0> goual => cjnbl;

		private object ygsen => cjnbl;

		private IEnumerator<nxtme<T0>> ttold()
		{
			fmjbw fmjbw;
			if (Thread.CurrentThread.ManagedThreadId == polvt && ygplg == -2)
			{
				ygplg = 0;
				fmjbw = this;
			}
			else
			{
				fmjbw = new fmjbw(0);
				fmjbw.gbkqw = gbkqw;
			}
			fmjbw.gbkqw = hipni;
			return fmjbw;
		}

		IEnumerator<nxtme<T0>> IEnumerable<nxtme<T0>>.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ttold
			return this.ttold();
		}

		private IEnumerator sgmxz()
		{
			return ttold();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in sgmxz
			return this.sgmxz();
		}

		private bool qvzvn()
		{
			switch (ygplg)
			{
			case 0:
				ygplg = -1;
				if ((gbkqw.hvbtp ? true : false) || (gbkqw.bopab ? true : false))
				{
					break;
				}
				gbkqw.rxzgs(0, out qhdot, out ntiur);
				agaqx = gbkqw.tvoem;
				jxvuv = qhdot;
				goto IL_0163;
			case 1:
				{
					ygplg = -1;
					agaqx -= uvokn.tvoem;
					jxvuv++;
					goto IL_0163;
				}
				IL_0163:
				if (jxvuv < gbkqw.vvaet.Length && agaqx > 0)
				{
					uvokn = gbkqw.vvaet[jxvuv];
					if (jxvuv == qhdot)
					{
						int p = Math.Min(uvokn.tvoem - ntiur, agaqx);
						uvokn = uvokn.jlxhy(ntiur, p);
					}
					else if (uvokn.tvoem > agaqx)
					{
						uvokn = uvokn.jlxhy(0, agaqx);
					}
					cjnbl = uvokn;
					ygplg = 1;
					return true;
				}
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in qvzvn
			return this.qvzvn();
		}

		private void wyeny()
		{
			throw new NotSupportedException();
		}

		void IEnumerator.Reset()
		{
			//ILSpy generated this explicit interface implementation from .override directive in wyeny
			this.wyeny();
		}

		private void logax()
		{
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in logax
			this.logax();
		}

		public fmjbw(int _003C_003E1__state)
		{
			ygplg = _003C_003E1__state;
			polvt = Thread.CurrentThread.ManagedThreadId;
		}
	}

	private const int cuuvu = -1;

	private const string yzwdh = "Cannot cast chained (non-contiguous) ArrayView to ArraySegment. Consider using Coalesce method before casting.";

	private const string cqcbs = "Contiguous buffer is not available. Consider using Coalesce method.";

	private const string pqkem = "No matching item found in the collection.";

	private const string snihi = "No items matched the predicate";

	private const string tpnkc = "Sequence contains more than one matching element.";

	private const string zttay = "No last item, sequence was empty.";

	private static readonly nxtme<T0> qlthn;

	private static Func<T0, T0> yaguq;

	private nxtme<T0>[] vvaet;

	private T0[] sclyb;

	private int kirqq;

	private int dvmdk;

	private static Func<T0, T0> xqkpk;

	public static nxtme<T0> gihlo => qlthn;

	public T0[] lthjd
	{
		get
		{
			if (wzdji && 0 == 0)
			{
				throw new InvalidOperationException("Contiguous buffer is not available. Consider using Coalesce method.");
			}
			return sclyb;
		}
	}

	public int frlfs => kirqq;

	public int tvoem => dvmdk;

	public int hvvsm => dvmdk;

	public bool wzdji => vvaet != null;

	public bool bopab => !wzdji;

	public IEnumerable<nxtme<T0>> xnxkj
	{
		get
		{
			fmjbw fmjbw = new fmjbw(-2);
			fmjbw.hipni = this;
			return fmjbw;
		}
	}

	public nxtme<T0> ldelr => this;

	public bool hvbtp => tvoem == 0;

	public T0 this[int index]
	{
		get
		{
			if ((!hvbtp || 1 == 0) && bopab && 0 == 0)
			{
				return sclyb[frlfs + index];
			}
			return nikmo(index);
		}
		set
		{
			if ((!hvbtp || 1 == 0) && bopab && 0 == 0)
			{
				sclyb[frlfs + index] = value;
			}
			else
			{
				wethy(index, value);
			}
		}
	}

	static nxtme()
	{
		qlthn = default(nxtme<T0>);
		yaguq = null;
	}

	public nxtme(T0[] array)
		: this(array, 0, array.Length)
	{
	}

	public nxtme(T0[] array, int offset)
		: this(array, offset, array.Length - offset)
	{
	}

	public nxtme(T0[] array, int offset, int count)
	{
		this = default(nxtme<T0>);
		if (array == null || 1 == 0)
		{
			throw new ArgumentNullException("array");
		}
		if (offset < 0 || offset > array.Length)
		{
			throw new ArgumentException("offset");
		}
		if (count < 0 || offset + count > array.Length)
		{
			throw new ArgumentException("count");
		}
		sclyb = array;
		kirqq = offset;
		dvmdk = count;
		vvaet = null;
	}

	public nxtme(params nxtme<T0>[] subViews)
		: this(0, null, subViews)
	{
	}

	private nxtme(int offset, int? count, params nxtme<T0>[] subViews)
	{
		this = default(nxtme<T0>);
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (subViews == null || false || subViews.Length == 0 || 1 == 0)
		{
			this = gihlo;
			return;
		}
		if (subViews.Length == 1 && (offset == 0 || 1 == 0))
		{
			int? num = count;
			int num2 = subViews[0].tvoem;
			if (num.GetValueOrDefault() == num2 && num.HasValue && 0 == 0)
			{
				this = subViews[0];
				return;
			}
		}
		int? num3 = count;
		if ((num3.GetValueOrDefault() == 0 || 1 == 0) && num3.HasValue && 0 == 0)
		{
			this = gihlo;
			return;
		}
		int num4 = -offset;
		int num5 = 0;
		if (num5 != 0)
		{
			goto IL_00c9;
		}
		goto IL_00dc;
		IL_00c9:
		num4 += subViews[num5].tvoem;
		num5++;
		goto IL_00dc;
		IL_00dc:
		if (num5 >= subViews.Length)
		{
			if (num4 == 0 || 1 == 0)
			{
				this = gihlo;
				return;
			}
			int? num6 = count;
			dvmdk = ((num6.HasValue ? true : false) ? num6.GetValueOrDefault() : num4);
			if (dvmdk < 0 || dvmdk > num4)
			{
				throw new ArgumentException("count");
			}
			kirqq = offset;
			vvaet = subViews;
			if (vwedq(out var p, out var p2) && 0 == 0)
			{
				this = new nxtme<T0>(p, p2.Value, dvmdk);
			}
			return;
		}
		goto IL_00c9;
	}

	public nxtme<T0> rwlqr()
	{
		if (!wzdji || 1 == 0)
		{
			return this;
		}
		return ooasp();
	}

	public nxtme<T0> pyfmm(nxtme<T0> p0)
	{
		if (p0.hvbtp && 0 == 0)
		{
			return this;
		}
		if (!hvbtp || 1 == 0)
		{
			return new nxtme<T0>(p0, this);
		}
		return p0;
	}

	public nxtme<T0> omtfq(nxtme<T0> p0)
	{
		if (p0.hvbtp && 0 == 0)
		{
			return this;
		}
		if (!hvbtp || 1 == 0)
		{
			return new nxtme<T0>(this, p0);
		}
		return p0;
	}

	public nxtme<T0> xjycg(int p0)
	{
		if (p0 < 0 || p0 > tvoem)
		{
			throw new ArgumentException("offset");
		}
		return jlxhy(p0, tvoem - p0);
	}

	public nxtme<T0> jlxhy(int p0, int p1)
	{
		if (p0 < 0 || p0 > tvoem)
		{
			throw new ArgumentException("offset");
		}
		if (p1 < 0 || p0 + p1 > tvoem)
		{
			throw new ArgumentException("count");
		}
		if ((p0 == 0 || 1 == 0) && p1 == dvmdk)
		{
			return this;
		}
		if (!wzdji || 1 == 0)
		{
			return new nxtme<T0>(lthjd, p0 + frlfs, p1);
		}
		if (p0 == dvmdk)
		{
			return gihlo;
		}
		rxzgs(p0, out var p2, out var p3);
		nxtme<T0> nxtme2 = vvaet[p2];
		int num = nxtme2.tvoem;
		if (num - p3 < p1)
		{
			return new nxtme<T0>(p0 + frlfs, p1, vvaet);
		}
		return nxtme2.jlxhy(p3, p1);
	}

	public static implicit operator nxtme<T0>(ArraySegment<T0> arraySegment)
	{
		return new nxtme<T0>(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
	}

	public static implicit operator ArraySegment<T0>(nxtme<T0> arrayView)
	{
		if (arrayView.wzdji && 0 == 0)
		{
			throw new InvalidOperationException("Cannot cast chained (non-contiguous) ArrayView to ArraySegment. Consider using Coalesce method before casting.");
		}
		T0[] array = arrayView.lthjd;
		if (array == null || 1 == 0)
		{
			array = new T0[0];
		}
		return new ArraySegment<T0>(array, arrayView.frlfs, arrayView.tvoem);
	}

	public static implicit operator nxtme<T0>(T0[] sourceArray)
	{
		if (sourceArray == null || 1 == 0)
		{
			return gihlo;
		}
		return new nxtme<T0>(sourceArray);
	}

	public static explicit operator T0[](nxtme<T0> arrayView)
	{
		return arrayView.ooasp();
	}

	public T0[] ooasp()
	{
		if (hvbtp && 0 == 0)
		{
			return new T0[0];
		}
		T0[] array = new T0[tvoem];
		if (wzdji && 0 == 0)
		{
			nxtme<T0> p = array.liutv();
			IEnumerator<nxtme<T0>> enumerator = xnxkj.GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					nxtme<T0> current = enumerator.Current;
					current.rjwrl(p);
					p = p.xjycg(current.tvoem);
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
		else
		{
			Array.Copy(sclyb, kirqq, array, 0, dvmdk);
		}
		return array;
	}

	public T0[] trkhv()
	{
		if (hvbtp && 0 == 0)
		{
			return new T0[0];
		}
		if ((wzdji ? true : false) || tvoem != lthjd.Length)
		{
			return ooasp();
		}
		return lthjd;
	}

	public void rjwrl(nxtme<T0> p0)
	{
		if (p0.tvoem < tvoem)
		{
			throw new ArgumentException("other");
		}
		if (hvbtp && 0 == 0)
		{
			return;
		}
		int num;
		if ((wzdji ? true : false) || p0.wzdji)
		{
			num = 0;
			if (num != 0)
			{
				goto IL_0054;
			}
			goto IL_006d;
		}
		Array.Copy(sclyb, kirqq, p0.sclyb, p0.frlfs, dvmdk);
		return;
		IL_0054:
		p0[num] = this[num];
		num++;
		goto IL_006d;
		IL_006d:
		if (num >= tvoem)
		{
			return;
		}
		goto IL_0054;
	}

	public bool Equals(nxtme<T0> other)
	{
		if (object.Equals(sclyb, other.sclyb) && 0 == 0 && object.Equals(vvaet, other.vvaet) && 0 == 0 && kirqq == other.kirqq)
		{
			return dvmdk == other.dvmdk;
		}
		return false;
	}

	public rrjio twvtt()
	{
		return new rrjio(this);
	}

	private IEnumerator<T0> kizek()
	{
		return new rrjio(this);
	}

	IEnumerator<T0> IEnumerable<T0>.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in kizek
		return this.kizek();
	}

	public override int GetHashCode()
	{
		int num = ((vvaet != null) ? vvaet.GetHashCode() : 0);
		num = (num * 397) ^ ((sclyb != null) ? sclyb.GetHashCode() : 0);
		num = (num * 397) ^ kirqq;
		return (num * 397) ^ dvmdk;
	}

	public override string ToString()
	{
		if (wzdji && 0 == 0)
		{
			return brgjd.edcru("ArrayView({0}[], {1}, {2}, 'Chained')", typeof(T0).Name, frlfs, tvoem);
		}
		return brgjd.edcru("ArrayView({0}[{1}], {2}, {3}, 'Contiguous')", typeof(T0).Name, (lthjd != null && 0 == 0) ? lthjd.Length : 0, frlfs, tvoem);
	}

	public override bool Equals(object obj)
	{
		if (object.ReferenceEquals(null, obj) && 0 == 0)
		{
			return false;
		}
		if (obj is nxtme<T0> && 0 == 0)
		{
			return Equals((nxtme<T0>)obj);
		}
		return false;
	}

	private IEnumerator sdgzn()
	{
		return twvtt();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in sdgzn
		return this.sdgzn();
	}

	public static bool operator ==(nxtme<T0> left, nxtme<T0> right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(nxtme<T0> left, nxtme<T0> right)
	{
		return !left.Equals(right);
	}

	public bool rpxvr()
	{
		return !hvbtp;
	}

	public bool tlskz(Func<T0, bool> p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("predicate");
		}
		if (hvbtp && 0 == 0)
		{
			return false;
		}
		using (rrjio rrjio = twvtt())
		{
			while (rrjio.MoveNext() ? true : false)
			{
				T0 current = rrjio.Current;
				if (p0(current) && 0 == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public T0 gpysi()
	{
		if (hvbtp && 0 == 0)
		{
			return default(T0);
		}
		return this[0];
	}

	public T0 srvzv()
	{
		if (hvbtp && 0 == 0)
		{
			throw new InvalidOperationException("No matching item found in the collection.");
		}
		return this[0];
	}

	public T0 xoiju(Func<T0, bool> p0)
	{
		return hlbps(p0, p1: true);
	}

	public T0 osfbb(Func<T0, bool> p0)
	{
		return hlbps(p0, p1: false);
	}

	public T0 gvtiy()
	{
		if (hvbtp && 0 == 0)
		{
			throw new InvalidOperationException("No items matched the predicate");
		}
		if (tvoem > 1)
		{
			throw new InvalidOperationException("Sequence contains more than one matching element.");
		}
		return this[0];
	}

	public T0 iimyq(Func<T0, bool> p0)
	{
		return rfrkp(p0, p1: true);
	}

	public T0 wajhl()
	{
		if (hvbtp && 0 == 0)
		{
			return default(T0);
		}
		if (tvoem > 1)
		{
			throw new InvalidOperationException("Sequence contains more than one matching element.");
		}
		return this[0];
	}

	public T0 zsvvz(Func<T0, bool> p0)
	{
		return rfrkp(p0, p1: false);
	}

	public T0 gkmcu()
	{
		if (hvbtp && 0 == 0)
		{
			throw new InvalidOperationException("No last item, sequence was empty.");
		}
		return this[tvoem - 1];
	}

	public T0 qrpzj()
	{
		if (hvbtp && 0 == 0)
		{
			return default(T0);
		}
		return this[tvoem - 1];
	}

	public T0 pnjro(Func<T0, bool> p0)
	{
		return qsuqo(p0, p1: true);
	}

	public T0 qhgdv(Func<T0, bool> p0)
	{
		return qsuqo(p0, p1: false);
	}

	public bool arduu(T0 p0)
	{
		return jwlsq(p0, EqualityComparer<T0>.Default);
	}

	public bool jwlsq(T0 p0, EqualityComparer<T0> p1)
	{
		if (hvbtp && 0 == 0)
		{
			return false;
		}
		EqualityComparer<T0> equalityComparer = p1;
		if (equalityComparer == null || 1 == 0)
		{
			equalityComparer = EqualityComparer<T0>.Default;
		}
		p1 = equalityComparer;
		using (rrjio rrjio = twvtt())
		{
			while (rrjio.MoveNext() ? true : false)
			{
				T0 current = rrjio.Current;
				if (p1.Equals(current, p0) && 0 == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool tnnzx(Func<T0, bool> p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("predicate");
		}
		if (hvbtp && 0 == 0)
		{
			return true;
		}
		using (rrjio rrjio = twvtt())
		{
			while (rrjio.MoveNext() ? true : false)
			{
				T0 current = rrjio.Current;
				if (!p0(current) || 1 == 0)
				{
					return false;
				}
			}
		}
		return true;
	}

	public TReturn gwwqn<TReturn>(TReturn p0, Func<TReturn, T0, TReturn> p1)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("aggregator");
		}
		if (hvbtp && 0 == 0)
		{
			return p0;
		}
		TReturn val = p0;
		using rrjio rrjio = twvtt();
		while (rrjio.MoveNext() ? true : false)
		{
			T0 current = rrjio.Current;
			val = p1(val, current);
		}
		return val;
	}

	public TReturn shhcn<TAccumulate, TReturn>(TAccumulate p0, Func<TAccumulate, T0, TAccumulate> p1, Func<TAccumulate, TReturn> p2)
	{
		if (p2 == null || 1 == 0)
		{
			throw new ArgumentNullException("resultSelector");
		}
		return p2(gwwqn(p0, p1));
	}

	private T0 qsuqo(Func<T0, bool> p0, bool p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("predicate");
		}
		if (hvbtp && 0 == 0)
		{
			return rkdce(p1);
		}
		for (int num = tvoem - 1; num >= 0; num--)
		{
			T0 val = this[num];
			if (p0(val) && 0 == 0)
			{
				return val;
			}
		}
		return rkdce(p1);
	}

	public Dictionary<TKey, T0> qowbk<TKey>(Func<T0, TKey> p0)
	{
		return zbsgh(p0, mhatf(), EqualityComparer<TKey>.Default);
	}

	public Dictionary<TKey, TElement> cllfc<TKey, TElement>(Func<T0, TKey> p0, Func<T0, TElement> p1)
	{
		return zbsgh(p0, p1, EqualityComparer<TKey>.Default);
	}

	public Dictionary<TKey, T0> illgx<TKey>(Func<T0, TKey> p0, IEqualityComparer<TKey> p1)
	{
		return zbsgh(p0, mhatf(), p1);
	}

	public Dictionary<TKey, TElement> zbsgh<TKey, TElement>(Func<T0, TKey> p0, Func<T0, TElement> p1, IEqualityComparer<TKey> p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("keySelector");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("elementSelector");
		}
		object equalityComparer = p2;
		if (equalityComparer == null || 1 == 0)
		{
			equalityComparer = EqualityComparer<TKey>.Default;
		}
		p2 = (IEqualityComparer<TKey>)equalityComparer;
		Dictionary<TKey, TElement> dictionary = ((hvbtp ? true : false) ? new Dictionary<TKey, TElement>(p2) : new Dictionary<TKey, TElement>(tvoem, p2));
		if (hvbtp && 0 == 0)
		{
			return dictionary;
		}
		using rrjio rrjio = twvtt();
		while (rrjio.MoveNext() ? true : false)
		{
			T0 current = rrjio.Current;
			TKey key = p0(current);
			TElement value = p1(current);
			dictionary.Add(key, value);
		}
		return dictionary;
	}

	public nxtme<T0> vmobs(T0 p0)
	{
		return new nxtme<T0>(this, new T0[1] { p0 });
	}

	public nxtme<T0> hbher(T0 p0)
	{
		return new nxtme<T0>(new T0[1] { p0 }, this);
	}

	public nxtme<T0> aasof(nxtme<T0> p0)
	{
		return omtfq(p0);
	}

	public void fofnq(Action<T0> p0)
	{
		if (hvbtp && 0 == 0)
		{
			return;
		}
		using rrjio rrjio = twvtt();
		while (rrjio.MoveNext() ? true : false)
		{
			T0 current = rrjio.Current;
			p0(current);
		}
	}

	public int eqehe(Func<T0, int> p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("selector");
		}
		int num = 0;
		for (int i = kirqq; i < dvmdk; i++)
		{
			num = checked(num + p0(this[i]));
		}
		return num;
	}

	public long ejmkl(Func<T0, long> p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("selector");
		}
		long num = 0L;
		for (int i = kirqq; i < dvmdk; i++)
		{
			num = checked(num + p0(this[i]));
		}
		return num;
	}

	private T0 hlbps(Func<T0, bool> p0, bool p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("predicate");
		}
		if (hvbtp && 0 == 0)
		{
			return rkdce(p1);
		}
		using (rrjio rrjio = twvtt())
		{
			while (rrjio.MoveNext() ? true : false)
			{
				T0 current = rrjio.Current;
				if (p0(current) && 0 == 0)
				{
					return current;
				}
			}
		}
		return rkdce(p1);
	}

	private T0 rfrkp(Func<T0, bool> p0, bool p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("predicate");
		}
		if (hvbtp && 0 == 0)
		{
			return rkdce(p1);
		}
		T0 result = default(T0);
		bool flag = false;
		using (rrjio rrjio = twvtt())
		{
			while (rrjio.MoveNext() ? true : false)
			{
				T0 current = rrjio.Current;
				if (p0(current) && 0 == 0)
				{
					if (flag && 0 == 0)
					{
						throw new InvalidOperationException("Sequence contains more than one matching element.");
					}
					flag = true;
					result = current;
				}
			}
		}
		if ((!flag || 1 == 0) && p1 && 0 == 0)
		{
			throw new InvalidOperationException("No items matched the predicate");
		}
		return result;
	}

	private T0 rkdce(bool p0)
	{
		if (p0 && 0 == 0)
		{
			throw new InvalidOperationException("No matching item found in the collection.");
		}
		return default(T0);
	}

	private static Func<T0, T0> mhatf()
	{
		Func<T0, T0> func = yaguq;
		if (func == null || 1 == 0)
		{
			if (xqkpk == null || 1 == 0)
			{
				xqkpk = ynzmi;
			}
			func = (yaguq = xqkpk);
		}
		return func;
	}

	private void pcnfa(int p0)
	{
		if (p0 >= 0 && p0 < tvoem)
		{
			return;
		}
		throw new IndexOutOfRangeException("offset");
	}

	private void rxzgs(int p0, out int p1, out int p2)
	{
		int num = frlfs + p0;
		if (p0 == tvoem)
		{
			p1 = vvaet.Length - 1;
			p2 = vvaet[p1].tvoem - 1;
			return;
		}
		int num2 = 0;
		p1 = -1;
		nxtme<T0> nxtme2;
		do
		{
			p1++;
			nxtme2 = vvaet[p1];
			num2 += nxtme2.tvoem;
		}
		while (num >= num2);
		int num3 = nxtme2.tvoem;
		int num4 = num2 - num3;
		p2 = num - num4;
	}

	private T0 nikmo(int p0)
	{
		pcnfa(p0);
		rxzgs(p0, out var p1, out var p2);
		return vvaet[p1][p2];
	}

	private void wethy(int p0, T0 p1)
	{
		pcnfa(p0);
		rxzgs(p0, out var p2, out var p3);
		vvaet[p2][p3] = p1;
	}

	private bool vwedq(out T0[] p0, out int? p1)
	{
		if (bopab && 0 == 0)
		{
			p0 = lthjd;
			p1 = frlfs;
			return true;
		}
		p0 = null;
		p1 = null;
		rxzgs(0, out var p2, out var p3);
		rxzgs(dvmdk - 1, out var p4, out var p5);
		int? num = (p1 = p3);
		T0[] array = null;
		for (int i = p2; i <= p4; i++)
		{
			nxtme<T0> nxtme2 = vvaet[i];
			if (i == p2)
			{
				nxtme2 = nxtme2.xjycg(p3);
			}
			if (i == p4)
			{
				nxtme2 = ((p2 == p4) ? nxtme2.jlxhy(0, p5 - p3 + 1) : nxtme2.jlxhy(0, p5 + 1));
			}
			if (!nxtme2.hvbtp || 1 == 0)
			{
				if (!nxtme2.vwedq(out var p6, out var _) || false || (array != null && 0 == 0 && !object.ReferenceEquals(array, p6)) || (i != p2 && (nxtme2.frlfs != num || 1 == 0)))
				{
					p0 = null;
					p1 = null;
					return false;
				}
				array = p6;
				p0 = p6;
				int? num2 = num;
				int num3 = nxtme2.tvoem;
				num = ((num2.HasValue ? true : false) ? new int?(num2.GetValueOrDefault() + num3) : ((int?)null));
			}
		}
		return true;
	}

	private static T0 ynzmi(T0 p0)
	{
		return p0;
	}
}
