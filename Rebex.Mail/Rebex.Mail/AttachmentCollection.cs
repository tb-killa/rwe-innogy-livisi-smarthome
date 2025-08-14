using System;
using System.Collections;
using System.Collections.Generic;
using onrkn;

namespace Rebex.Mail;

public class AttachmentCollection : ICollection, IEnumerable<Attachment>, IEnumerable
{
	private readonly ArrayList uerhj;

	[NonSerialized]
	private MailMessage xzejl;

	private bool xwxjj;

	internal bool pubjq
	{
		get
		{
			return xwxjj;
		}
		set
		{
			xwxjj = value;
			IEnumerator enumerator = uerhj.GetEnumerator();
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

	internal MailMessage mhqha
	{
		get
		{
			return xzejl;
		}
		set
		{
			xzejl = value;
		}
	}

	internal ArrayList cmbhn => uerhj;

	public int Count => uerhj.Count;

	public Attachment this[int index]
	{
		get
		{
			return (Attachment)uerhj[index];
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			if (xwxjj && 0 == 0)
			{
				throw new MailException("Cannot change read-only message.");
			}
			imksv(value);
			uerhj[index] = value;
		}
	}

	public bool IsSynchronized => false;

	public object SyncRoot => uerhj.SyncRoot;

	public AttachmentCollection()
	{
		uerhj = new ArrayList();
	}

	internal AttachmentCollection zwpay()
	{
		AttachmentCollection attachmentCollection = new AttachmentCollection();
		attachmentCollection.xwxjj = xwxjj;
		IEnumerator enumerator = uerhj.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				AttachmentBase attachmentBase = (AttachmentBase)enumerator.Current;
				attachmentCollection.uerhj.Add(attachmentBase.vpeyb());
			}
			return attachmentCollection;
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
		if (xwxjj && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		uerhj.RemoveAt(index);
	}

	public int Add(Attachment value)
	{
		if (value == null || 1 == 0)
		{
			throw new ArgumentNullException("value");
		}
		if (xwxjj && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		imksv(value);
		return uerhj.Add(value);
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
				if (!(current is Attachment) || 1 == 0)
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
		if (xwxjj && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		IEnumerator enumerator2 = collection.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext() ? true : false)
			{
				Attachment p = (Attachment)enumerator2.Current;
				imksv(p);
			}
		}
		finally
		{
			if (enumerator2 is IDisposable disposable2 && 0 == 0)
			{
				disposable2.Dispose();
			}
		}
		uerhj.AddRange(collection);
	}

	private IEnumerator yxroy()
	{
		return uerhj.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in yxroy
		return this.yxroy();
	}

	public IEnumerator<Attachment> GetEnumerator()
	{
		return this.eaqmu<Attachment>().GetEnumerator();
	}

	private void fgkjv(Array p0, int p1)
	{
		uerhj.CopyTo(p0, p1);
	}

	void ICollection.CopyTo(Array p0, int p1)
	{
		//ILSpy generated this explicit interface implementation from .override directive in fgkjv
		this.fgkjv(p0, p1);
	}

	public void CopyTo(Attachment[] array, int index)
	{
		((ICollection)this).CopyTo((Array)array, index);
	}

	private void imksv(Attachment p0)
	{
		if (p0.pptit != null && 0 == 0)
		{
			uskdy(p0.pptit, xzejl);
		}
	}

	private static void uskdy(MailMessage p0, MailMessage p1)
	{
		if (p0 == null || false || p1 == null)
		{
			return;
		}
		if (p0 == p1)
		{
			throw new MailException("Adding this embedded message to the list would lead to circular dependency.");
		}
		IEnumerator<Attachment> enumerator = p0.Attachments.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				Attachment current = enumerator.Current;
				if (current.pptit != null && 0 == 0)
				{
					uskdy(current.pptit, p1);
				}
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
}
