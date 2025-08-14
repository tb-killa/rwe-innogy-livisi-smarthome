using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography;

public abstract class CryptographicCollection : lnabj, ICollection, IEnumerable
{
	private readonly rmkkr bchrz;

	private readonly IList pijfc;

	public int Count => pijfc.Count;

	public bool IsSynchronized => pijfc.IsSynchronized;

	public object SyncRoot => pijfc.SyncRoot;

	internal IList zctor => pijfc;

	internal CryptographicCollection(rmkkr type, IList items)
	{
		bchrz = type;
		pijfc = items;
	}

	private void bcnyh(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(bchrz, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in bcnyh
		this.bcnyh(p0, p1, p2);
	}

	private lnabj mhlwe(rmkkr p0, bool p1, int p2)
	{
		throw new NotSupportedException("The collection does not implement an ASN.1 parser.");
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in mhlwe
		return this.mhlwe(p0, p1, p2);
	}

	private void sxzoc(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in sxzoc
		this.sxzoc(p0, p1, p2);
	}

	private void cmepi()
	{
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in cmepi
		this.cmepi();
	}

	public IEnumerator GetEnumerator()
	{
		return pijfc.GetEnumerator();
	}

	private void qaama(fxakl p0)
	{
		p0.aiflg(bchrz, pijfc);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in qaama
		this.qaama(p0);
	}

	public void CopyTo(Array array, int index)
	{
		pijfc.CopyTo(array, index);
	}
}
public abstract class CryptographicCollection<T> : CryptographicCollection, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	private readonly List<T> zgzvh;

	private bool lzocw;

	internal IList<T> lquvo => zgzvh;

	public bool IsReadOnly => lzocw;

	public T this[int index]
	{
		get
		{
			return zgzvh[index];
		}
		set
		{
			tqdqu(index, value);
		}
	}

	internal CryptographicCollection(rmkkr type)
		: base(type, new List<T>())
	{
		zgzvh = (List<T>)base.zctor;
	}

	internal void hksnh()
	{
		lzocw = true;
	}

	internal void vgeou()
	{
		if (lzocw && 0 == 0)
		{
			throw new CryptographicException("Collection is read-only.");
		}
	}

	internal virtual void soaex(T p0, string p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException(p1);
		}
	}

	public void Add(T item)
	{
		ttbls(item);
	}

	internal virtual void ttbls(T p0)
	{
		soaex(p0, "item");
		vgeou();
		zgzvh.Add(p0);
	}

	public void Clear()
	{
		bxkbx();
	}

	internal virtual void bxkbx()
	{
		vgeou();
		zgzvh.Clear();
	}

	public bool Contains(T item)
	{
		return zgzvh.Contains(item);
	}

	public int IndexOf(T item)
	{
		return zgzvh.IndexOf(item);
	}

	public void CopyTo(T[] array, int index)
	{
		zgzvh.CopyTo(array, index);
	}

	public bool Remove(T item)
	{
		return rgnjk(item);
	}

	internal virtual bool rgnjk(T p0)
	{
		vgeou();
		return zgzvh.Remove(p0);
	}

	public void RemoveAt(int index)
	{
		prujo(index);
	}

	internal virtual void prujo(int p0)
	{
		vgeou();
		zgzvh.RemoveAt(p0);
	}

	public void Insert(int index, T item)
	{
		gwubw(index, item);
	}

	internal virtual void gwubw(int p0, T p1)
	{
		soaex(p1, "item");
		vgeou();
		zgzvh.Insert(p0, p1);
	}

	internal virtual void tqdqu(int p0, T p1)
	{
		soaex(p1, "value");
		vgeou();
		zgzvh[p0] = p1;
	}

	public new IEnumerator<T> GetEnumerator()
	{
		return ((IEnumerable<T>)zgzvh).GetEnumerator();
	}

	private IEnumerator<T> mywjj()
	{
		return GetEnumerator();
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in mywjj
		return this.mywjj();
	}
}
