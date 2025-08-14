using System;
using System.Collections;
using System.Collections.Generic;
using onrkn;

namespace Rebex.Mail;

public class AlternateViewCollection : ICollection, IEnumerable<AlternateView>, IEnumerable
{
	private readonly ArrayList kzzgp;

	private bool mnshp;

	internal bool oomcm
	{
		get
		{
			return mnshp;
		}
		set
		{
			mnshp = value;
			IEnumerator enumerator = kzzgp.GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					AttachmentBase attachmentBase = (AttachmentBase)enumerator.Current;
					attachmentBase.tidto(value);
				}
			}
			finally
			{
				if (enumerator is IDisposable disposable && 0 == 0)
				{
					disposable.Dispose();
				}
			}
		}
	}

	internal ArrayList wzxkg => kzzgp;

	public int Count => kzzgp.Count;

	public AlternateView this[int index]
	{
		get
		{
			return (AlternateView)kzzgp[index];
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			if (mnshp && 0 == 0)
			{
				throw new MailException("Cannot change read-only message.");
			}
			kzzgp[index] = value;
		}
	}

	public bool IsSynchronized => false;

	public object SyncRoot => kzzgp.SyncRoot;

	public AlternateViewCollection()
	{
		kzzgp = new ArrayList();
	}

	internal AlternateViewCollection momyw()
	{
		AlternateViewCollection alternateViewCollection = new AlternateViewCollection();
		alternateViewCollection.mnshp = mnshp;
		IEnumerator enumerator = kzzgp.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				AttachmentBase attachmentBase = (AttachmentBase)enumerator.Current;
				alternateViewCollection.kzzgp.Add(attachmentBase.vpeyb());
			}
			return alternateViewCollection;
		}
		finally
		{
			if (enumerator is IDisposable disposable && 0 == 0)
			{
				disposable.Dispose();
			}
		}
	}

	public void Insert(int index, AlternateView value)
	{
		if (mnshp && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		kzzgp.Insert(index, value);
	}

	public void RemoveAt(int index)
	{
		if (mnshp && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		kzzgp.RemoveAt(index);
	}

	public int Add(AlternateView value)
	{
		if (value == null || 1 == 0)
		{
			throw new ArgumentNullException("value");
		}
		if (mnshp && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		return kzzgp.Add(value);
	}

	public void AddRange(ICollection collection)
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
				if (!(current is AlternateView) || 1 == 0)
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
		if (mnshp && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		kzzgp.AddRange(collection);
	}

	private IEnumerator knxlx()
	{
		return kzzgp.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in knxlx
		return this.knxlx();
	}

	public IEnumerator<AlternateView> GetEnumerator()
	{
		return this.eaqmu<AlternateView>().GetEnumerator();
	}

	private void znnhw(Array p0, int p1)
	{
		kzzgp.CopyTo(p0, p1);
	}

	void ICollection.CopyTo(Array p0, int p1)
	{
		//ILSpy generated this explicit interface implementation from .override directive in znnhw
		this.znnhw(p0, p1);
	}

	public void CopyTo(AlternateView[] array, int index)
	{
		((ICollection)this).CopyTo((Array)array, index);
	}
}
