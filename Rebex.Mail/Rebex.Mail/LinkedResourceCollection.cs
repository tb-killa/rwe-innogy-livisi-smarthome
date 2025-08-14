using System;
using System.Collections;
using System.Collections.Generic;
using onrkn;

namespace Rebex.Mail;

public class LinkedResourceCollection : ICollection, IEnumerable<LinkedResource>, IEnumerable
{
	private readonly ArrayList kyclp;

	private bool sqlay;

	internal bool tvgwf
	{
		get
		{
			return sqlay;
		}
		set
		{
			sqlay = value;
			IEnumerator enumerator = kyclp.GetEnumerator();
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

	internal ArrayList pusbf => kyclp;

	public int Count => kyclp.Count;

	public LinkedResource this[int index]
	{
		get
		{
			return (LinkedResource)kyclp[index];
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			if (sqlay && 0 == 0)
			{
				throw new MailException("Cannot change read-only message.");
			}
			kyclp[index] = value;
		}
	}

	public bool IsSynchronized => false;

	public object SyncRoot => kyclp.SyncRoot;

	public LinkedResourceCollection()
	{
		kyclp = new ArrayList();
	}

	internal LinkedResourceCollection euifs()
	{
		LinkedResourceCollection linkedResourceCollection = new LinkedResourceCollection();
		linkedResourceCollection.sqlay = sqlay;
		IEnumerator enumerator = kyclp.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				AttachmentBase attachmentBase = (AttachmentBase)enumerator.Current;
				linkedResourceCollection.kyclp.Add(attachmentBase.vpeyb());
			}
			return linkedResourceCollection;
		}
		finally
		{
			if (enumerator is IDisposable disposable && 0 == 0)
			{
				disposable.Dispose();
			}
		}
	}

	public void RemoveAt(int index)
	{
		if (sqlay && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		kyclp.RemoveAt(index);
	}

	public int Add(LinkedResource value)
	{
		if (value == null || 1 == 0)
		{
			throw new ArgumentNullException("value");
		}
		if (sqlay && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		return kyclp.Add(value);
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
				if (!(current is LinkedResource) || 1 == 0)
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
		if (sqlay && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		kyclp.AddRange(collection);
	}

	private IEnumerator nxxxp()
	{
		return kyclp.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in nxxxp
		return this.nxxxp();
	}

	public IEnumerator<LinkedResource> GetEnumerator()
	{
		return this.eaqmu<LinkedResource>().GetEnumerator();
	}

	private void jjhnx(Array p0, int p1)
	{
		kyclp.CopyTo(p0, p1);
	}

	void ICollection.CopyTo(Array p0, int p1)
	{
		//ILSpy generated this explicit interface implementation from .override directive in jjhnx
		this.jjhnx(p0, p1);
	}

	public void CopyTo(LinkedResource[] array, int index)
	{
		((ICollection)this).CopyTo((Array)array, index);
	}
}
