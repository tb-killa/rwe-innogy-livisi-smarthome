using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace onrkn;

internal class sroer<T0, T1> : IDictionary<T0, T1>, ICollection<KeyValuePair<T0, T1>>, IEnumerable<KeyValuePair<T0, T1>>, IEnumerable
{
	private sealed class ovyjb
	{
		public T1 rirxw;

		public T1 rdaxe(T0 p0)
		{
			return rirxw;
		}
	}

	private sealed class mlgfc : IEnumerator<KeyValuePair<T0, T1>>, IEnumerator, IDisposable
	{
		private KeyValuePair<T0, T1> hegrr;

		private int ekkph;

		public sroer<T0, T1> pefmn;

		public KeyValuePair<T0, T1>[] cbyua;

		public KeyValuePair<T0, T1> gtbpa;

		public Dictionary<T0, T1> rtiqa;

		public KeyValuePair<T0, T1>[] sxuol;

		public int vdxvr;

		private KeyValuePair<T0, T1> xkqrr => hegrr;

		private object uznwa => hegrr;

		private bool wiycc()
		{
			try
			{
				int num = ekkph;
				if (num != 0)
				{
					if (num != 3)
					{
						goto IL_00e1;
					}
					ekkph = 2;
					vdxvr++;
				}
				else
				{
					ekkph = -1;
					Monitor.Enter(rtiqa = pefmn.mrgcg);
					try
					{
						cbyua = pefmn.mrgcg.ToArray();
					}
					finally
					{
						Monitor.Exit(rtiqa);
					}
					ekkph = 2;
					sxuol = cbyua;
					vdxvr = 0;
				}
				if (vdxvr < sxuol.Length)
				{
					gtbpa = sxuol[vdxvr];
					hegrr = gtbpa;
					ekkph = 3;
					return true;
				}
				nmynu();
				goto IL_00e1;
				IL_00e1:
				return false;
			}
			catch
			{
				//try-fault
				lmenr();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in wiycc
			return this.wiycc();
		}

		private void ymvjl()
		{
			throw new NotSupportedException();
		}

		void IEnumerator.Reset()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ymvjl
			this.ymvjl();
		}

		private void lmenr()
		{
			switch (ekkph)
			{
			case 2:
			case 3:
				nmynu();
				break;
			}
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in lmenr
			this.lmenr();
		}

		public mlgfc(int _003C_003E1__state)
		{
			ekkph = _003C_003E1__state;
		}

		private void nmynu()
		{
			ekkph = -1;
		}
	}

	private sealed class btvqw
	{
		public T1 cfjzs;

		public T1 wmrfw(T0 p0)
		{
			return cfjzs;
		}
	}

	private readonly Dictionary<T0, T1> mrgcg;

	public T1 this[T0 key]
	{
		get
		{
			lock (mrgcg)
			{
				return mrgcg[key];
			}
		}
		set
		{
			lock (mrgcg)
			{
				mrgcg[key] = value;
			}
		}
	}

	public int Count
	{
		get
		{
			lock (mrgcg)
			{
				return mrgcg.Count;
			}
		}
	}

	public bool eovng => Count == 0;

	public ICollection<T0> Keys
	{
		get
		{
			lock (mrgcg)
			{
				return mrgcg.Keys.ToArray();
			}
		}
	}

	public ICollection<T1> Values
	{
		get
		{
			lock (mrgcg)
			{
				return mrgcg.Values.ToArray();
			}
		}
	}

	private bool engpg => false;

	public sroer()
		: this((IEqualityComparer<T0>)null)
	{
	}

	public sroer(IEqualityComparer<T0> comparer)
	{
		object equalityComparer = comparer;
		if (equalityComparer == null || 1 == 0)
		{
			equalityComparer = EqualityComparer<T0>.Default;
		}
		mrgcg = new Dictionary<T0, T1>((IEqualityComparer<T0>)equalityComparer);
	}

	public sroer(IEnumerable<KeyValuePair<T0, T1>> collection)
	{
		mrgcg = new Dictionary<T0, T1>();
		IEnumerator<KeyValuePair<T0, T1>> enumerator = collection.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				KeyValuePair<T0, T1> current = enumerator.Current;
				((ICollection<KeyValuePair<T0, T1>>)mrgcg).Add(current);
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

	public sroer(IEnumerable<KeyValuePair<T0, T1>> collection, IEqualityComparer<T0> comparer)
	{
		mrgcg = new Dictionary<T0, T1>(comparer);
		IEnumerator<KeyValuePair<T0, T1>> enumerator = collection.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				KeyValuePair<T0, T1> current = enumerator.Current;
				((ICollection<KeyValuePair<T0, T1>>)mrgcg).Add(current);
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

	public T1 cjpxe(T0 p0, Func<T0, T1> p1, Func<T0, T1, T1> p2)
	{
		if (p2 == null || 1 == 0)
		{
			throw new ArgumentNullException("updateValueFunc");
		}
		lock (mrgcg)
		{
			T1 val = ((mrgcg.TryGetValue(p0, out val) ? true : false) ? p2(p0, val) : p1(p0));
			mrgcg[p0] = val;
			return val;
		}
	}

	public T1 yjvlm(T0 p0, T1 p1, Func<T0, T1, T1> p2)
	{
		ovyjb ovyjb = new ovyjb();
		ovyjb.rirxw = p1;
		return cjpxe(p0, ovyjb.rdaxe, p2);
	}

	public void Clear()
	{
		lock (mrgcg)
		{
			mrgcg.Clear();
		}
	}

	public bool ContainsKey(T0 key)
	{
		lock (mrgcg)
		{
			return mrgcg.ContainsKey(key);
		}
	}

	public IEnumerator<KeyValuePair<T0, T1>> GetEnumerator()
	{
		mlgfc mlgfc = new mlgfc(0);
		mlgfc.pefmn = this;
		return mlgfc;
	}

	public T1 rcwvk(T0 p0, Func<T0, T1> p1)
	{
		T1 value;
		lock (mrgcg)
		{
			if (!mrgcg.TryGetValue(p0, out value) || 1 == 0)
			{
				value = p1(p0);
				mrgcg.Add(p0, value);
			}
		}
		return value;
	}

	public T1 wsggz(T0 p0, T1 p1)
	{
		btvqw btvqw = new btvqw();
		btvqw.cfjzs = p1;
		return rcwvk(p0, btvqw.wmrfw);
	}

	public KeyValuePair<T0, T1>[] yttku()
	{
		lock (mrgcg)
		{
			return mrgcg.ToArray();
		}
	}

	public bool opwgf(T0 p0, T1 p1)
	{
		lock (mrgcg)
		{
			if (mrgcg.ContainsKey(p0) && 0 == 0)
			{
				return false;
			}
			mrgcg.Add(p0, p1);
			return true;
		}
	}

	public bool TryGetValue(T0 key, out T1 value)
	{
		lock (mrgcg)
		{
			return mrgcg.TryGetValue(key, out value);
		}
	}

	public bool xdcrq(T0 p0, out T1 p1)
	{
		lock (mrgcg)
		{
			if (mrgcg.TryGetValue(p0, out p1) && 0 == 0)
			{
				return mrgcg.Remove(p0);
			}
			p1 = default(T1);
			return false;
		}
	}

	public bool swwjc(T0 p0, T1 p1, T1 p2)
	{
		lock (mrgcg)
		{
			if (!mrgcg.TryGetValue(p0, out var value) || 1 == 0)
			{
				return false;
			}
			IEqualityComparer<T1> equalityComparer = EqualityComparer<T1>.Default;
			if (!equalityComparer.Equals(value, p2) || 1 == 0)
			{
				return false;
			}
			mrgcg[p0] = p1;
			return true;
		}
	}

	private void zvjpd(KeyValuePair<T0, T1> p0)
	{
		lock (mrgcg)
		{
			((ICollection<KeyValuePair<T0, T1>>)mrgcg).Add(p0);
		}
	}

	void ICollection<KeyValuePair<T0, T1>>.Add(KeyValuePair<T0, T1> p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in zvjpd
		this.zvjpd(p0);
	}

	private void ivnxk(T0 p0, T1 p1)
	{
		lock (mrgcg)
		{
			mrgcg.Add(p0, p1);
		}
	}

	void IDictionary<T0, T1>.Add(T0 p0, T1 p1)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ivnxk
		this.ivnxk(p0, p1);
	}

	private bool qpfsk(KeyValuePair<T0, T1> p0)
	{
		lock (mrgcg)
		{
			return ((ICollection<KeyValuePair<T0, T1>>)mrgcg).Contains(p0);
		}
	}

	bool ICollection<KeyValuePair<T0, T1>>.Contains(KeyValuePair<T0, T1> p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in qpfsk
		return this.qpfsk(p0);
	}

	private void eghsl(KeyValuePair<T0, T1>[] p0, int p1)
	{
		lock (mrgcg)
		{
			((ICollection<KeyValuePair<T0, T1>>)mrgcg).CopyTo(p0, p1);
		}
	}

	void ICollection<KeyValuePair<T0, T1>>.CopyTo(KeyValuePair<T0, T1>[] p0, int p1)
	{
		//ILSpy generated this explicit interface implementation from .override directive in eghsl
		this.eghsl(p0, p1);
	}

	private IEnumerator tckyr()
	{
		return GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in tckyr
		return this.tckyr();
	}

	private bool vpnmt(KeyValuePair<T0, T1> p0)
	{
		lock (mrgcg)
		{
			return ((ICollection<KeyValuePair<T0, T1>>)mrgcg).Remove(p0);
		}
	}

	bool ICollection<KeyValuePair<T0, T1>>.Remove(KeyValuePair<T0, T1> p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in vpnmt
		return this.vpnmt(p0);
	}

	private bool iumcb(T0 p0)
	{
		lock (mrgcg)
		{
			return mrgcg.Remove(p0);
		}
	}

	bool IDictionary<T0, T1>.Remove(T0 p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in iumcb
		return this.iumcb(p0);
	}
}
