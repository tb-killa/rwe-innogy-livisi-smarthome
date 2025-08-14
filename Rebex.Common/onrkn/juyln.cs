using System;
using System.Collections;
using System.Collections.Generic;

namespace onrkn;

internal class juyln<T0> : rrckb<T0>, IEnumerable<T0>, ICollection, IEnumerable
{
	private readonly Queue<T0> kfenr;

	public int Count
	{
		get
		{
			lock (kfenr)
			{
				return kfenr.Count;
			}
		}
	}

	public bool nladl => Count == 0;

	private bool huvxq => false;

	private object hdtzy
	{
		get
		{
			throw new NotSupportedException("Synchronization root is not supported on concurrent collections.");
		}
	}

	public juyln()
	{
		kfenr = new Queue<T0>();
	}

	public juyln(IEnumerable<T0> collection)
	{
		kfenr = new Queue<T0>(collection);
	}

	public void gnauf(T0[] p0, int p1)
	{
		lock (kfenr)
		{
			kfenr.CopyTo(p0, p1);
		}
	}

	public void bzigb(T0 p0)
	{
		lock (kfenr)
		{
			kfenr.Enqueue(p0);
		}
	}

	public IEnumerator<T0> GetEnumerator()
	{
		return ((IEnumerable<T0>)ocxkr()).GetEnumerator();
	}

	public T0[] ocxkr()
	{
		lock (kfenr)
		{
			return kfenr.ToArray();
		}
	}

	public bool vuroo(out T0 p0)
	{
		lock (kfenr)
		{
			if (kfenr.Count == 0 || 1 == 0)
			{
				p0 = default(T0);
				return false;
			}
			p0 = kfenr.Dequeue();
			return true;
		}
	}

	public bool heykj(out T0 p0)
	{
		lock (kfenr)
		{
			if (kfenr.Count == 0 || 1 == 0)
			{
				p0 = default(T0);
				return false;
			}
			p0 = kfenr.Peek();
			return true;
		}
	}

	private IEnumerator lluug()
	{
		return GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in lluug
		return this.lluug();
	}

	private void gzfjo(Array p0, int p1)
	{
		lock (kfenr)
		{
			((ICollection)kfenr).CopyTo(p0, p1);
		}
	}

	void ICollection.CopyTo(Array p0, int p1)
	{
		//ILSpy generated this explicit interface implementation from .override directive in gzfjo
		this.gzfjo(p0, p1);
	}

	private bool ximmh(T0 p0)
	{
		bzigb(p0);
		return true;
	}

	bool rrckb<T0>.pbpvr(T0 p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ximmh
		return this.ximmh(p0);
	}

	private bool bhvfy(out T0 p0)
	{
		return vuroo(out p0);
	}

	bool rrckb<T0>.ztnzd(out T0 p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in bhvfy
		return this.bhvfy(out p0);
	}
}
