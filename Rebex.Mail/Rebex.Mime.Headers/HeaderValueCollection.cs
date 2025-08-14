using System;
using System.Collections;
using System.IO;
using onrkn;

namespace Rebex.Mime.Headers;

public abstract class HeaderValueCollection : ICollection, IEnumerable, IHeader, qmqrp
{
	[NonSerialized]
	private ArrayList qbrhq;

	[NonSerialized]
	private afmdb jicij;

	[NonSerialized]
	private long jglue;

	[NonSerialized]
	private bool rguxr;

	internal abstract Type sxlev { get; }

	private bool bfhfm
	{
		get
		{
			return rguxr;
		}
		set
		{
			rguxr = value;
		}
	}

	private long vpbwm => jglue;

	public int Count
	{
		get
		{
			if (jicij != null && 0 == 0)
			{
				return jicij.srbcw;
			}
			return qbrhq.Count;
		}
	}

	internal object this[int index]
	{
		get
		{
			if (jicij != null && 0 == 0)
			{
				return jicij[index];
			}
			return qbrhq[index];
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			if (!sxlev.IsInstanceOfType(value) || 1 == 0)
			{
				throw new ArgumentException("Invalid argument type.", "value");
			}
			if (rguxr && 0 == 0)
			{
				throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
			}
			if (jicij != null && 0 == 0)
			{
				jicij[index] = value;
			}
			else
			{
				qbrhq[index] = value;
			}
			jglue++;
		}
	}

	internal bool kxiib => jicij != null;

	public bool IsSynchronized => false;

	public object SyncRoot => qbrhq.SyncRoot;

	internal afmdb ownoy()
	{
		return jicij;
	}

	internal HeaderValueCollection()
	{
		qbrhq = new ArrayList();
	}

	public void Clear()
	{
		if (rguxr && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		if (jicij != null && 0 == 0)
		{
			jicij.rkqew();
		}
		else
		{
			qbrhq.Clear();
		}
		jglue++;
	}

	public void RemoveAt(int index)
	{
		if (rguxr && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		if (jicij != null && 0 == 0)
		{
			jicij.ijhui(index);
		}
		else
		{
			qbrhq.RemoveAt(index);
		}
		jglue++;
	}

	internal int kkdtb(object p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("value");
		}
		if (!sxlev.IsInstanceOfType(p0) || 1 == 0)
		{
			throw new ArgumentException("Invalid argument type.", "value");
		}
		if (rguxr && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		int result = ((jicij == null) ? qbrhq.Add(p0) : jicij.wecpv(p0));
		jglue++;
		return result;
	}

	public virtual void AddRange(ICollection collection)
	{
		if (collection == null || 1 == 0)
		{
			throw new ArgumentNullException("collection");
		}
		IEnumerator enumerator = collection.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				object current = enumerator.Current;
				if (current == null || 1 == 0)
				{
					throw new ArgumentException("Null item is present in the collection.", "collection");
				}
				if (!sxlev.IsInstanceOfType(current) || 1 == 0)
				{
					throw new ArgumentException("Invalid item type in the collection.", "collection");
				}
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable && 0 == 0)
			{
				disposable.Dispose();
			}
		}
		if (rguxr && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		jglue++;
		if (jicij != null && 0 == 0)
		{
			ArrayList arrayList = new ArrayList();
			IEnumerator enumerator2 = collection.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext() ? true : false)
				{
					object current2 = enumerator2.Current;
					arrayList.Add(current2);
				}
			}
			finally
			{
				if (enumerator2 is IDisposable disposable2 && 0 == 0)
				{
					disposable2.Dispose();
				}
			}
			IEnumerator enumerator3 = arrayList.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext() ? true : false)
				{
					object current3 = enumerator3.Current;
					kkdtb(current3);
				}
				return;
			}
			finally
			{
				if (enumerator3 is IDisposable disposable3 && 0 == 0)
				{
					disposable3.Dispose();
				}
			}
		}
		qbrhq.AddRange(collection);
	}

	internal void wrrck()
	{
		jglue = 0L;
	}

	internal void kgrvh(afmdb p0)
	{
		jicij = p0;
	}

	public virtual IEnumerator GetEnumerator()
	{
		return aqogv();
	}

	internal IEnumerator aqogv()
	{
		if (jicij != null && 0 == 0)
		{
			return jicij.nxtqg();
		}
		return qbrhq.GetEnumerator();
	}

	public abstract void Encode(TextWriter writer);

	public void CopyTo(Array array, int index)
	{
		if (array == null || 1 == 0)
		{
			throw new ArgumentNullException("array");
		}
		int count;
		int num;
		if (jicij != null)
		{
			count = Count;
			num = 0;
			if (num != 0)
			{
				goto IL_0032;
			}
			goto IL_004c;
		}
		qbrhq.CopyTo(array, index);
		return;
		IL_0032:
		array.SetValue(this[num], index + num);
		num++;
		goto IL_004c;
		IL_004c:
		if (num >= count)
		{
			return;
		}
		goto IL_0032;
	}

	public IHeader Clone()
	{
		HeaderValueCollection headerValueCollection = (HeaderValueCollection)Activator.CreateInstance(GetType());
		int count = Count;
		int num = 0;
		if (num != 0)
		{
			goto IL_001e;
		}
		goto IL_003a;
		IL_001e:
		headerValueCollection.kkdtb(((IHeader)this[num]).Clone());
		num++;
		goto IL_003a;
		IL_003a:
		if (num >= count)
		{
			headerValueCollection.jglue = jglue;
			return headerValueCollection;
		}
		goto IL_001e;
	}
}
