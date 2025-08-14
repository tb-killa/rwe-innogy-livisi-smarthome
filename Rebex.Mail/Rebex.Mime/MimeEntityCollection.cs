using System;
using System.Collections;
using System.Collections.Generic;
using onrkn;

namespace Rebex.Mime;

public class MimeEntityCollection : IEnumerable<MimeEntity>, IEnumerable
{
	private readonly ArrayList ixjmg;

	private readonly MimeEntity yuywq;

	private bool zrvif;

	internal bool vfvpt
	{
		set
		{
			zrvif = value;
			IEnumerator enumerator = ixjmg.GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					MimeEntity mimeEntity = (MimeEntity)enumerator.Current;
					mimeEntity.ReadOnly = value;
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

	public MimeEntity this[int index]
	{
		get
		{
			return (MimeEntity)ixjmg[index];
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			if (zrvif && 0 == 0)
			{
				throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
			}
			MimeEntity mimeEntity = this[index];
			if (value != mimeEntity)
			{
				xikwj(value);
				this[index].dtfjp(null);
				value.dtfjp(yuywq);
				ixjmg[index] = value;
			}
		}
	}

	public object SyncRoot => ixjmg.SyncRoot;

	public int Count => ixjmg.Count;

	internal MimeEntityCollection(MimeEntity owner)
	{
		if (owner == null || 1 == 0)
		{
			throw new ArgumentNullException("owner");
		}
		yuywq = owner;
		ixjmg = new ArrayList();
	}

	internal MimeEntity[] wuetq()
	{
		return (MimeEntity[])ixjmg.ToArray(typeof(MimeEntity));
	}

	private void lhpsp()
	{
		if (!yuywq.IsMultipart || 1 == 0)
		{
			throw new MimeException(brgjd.edcru("Cannot add a subpart to an entity of type '{0}'.", yuywq.ContentType), MimeExceptionStatus.OperationError);
		}
	}

	public void Insert(int index, MimeEntity entity)
	{
		if (entity == null || 1 == 0)
		{
			throw new ArgumentNullException("entity");
		}
		if (zrvif && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		lhpsp();
		xikwj(entity);
		ixjmg.Insert(index, entity);
		entity.dtfjp(yuywq);
	}

	public int Add(MimeEntity entity)
	{
		if (entity == null || 1 == 0)
		{
			throw new ArgumentNullException("entity");
		}
		if (zrvif && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		lhpsp();
		xikwj(entity);
		int result = ixjmg.Add(entity);
		entity.dtfjp(yuywq);
		return result;
	}

	public void RemoveAt(int index)
	{
		if (zrvif && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		MimeEntity mimeEntity = this[index];
		ixjmg.RemoveAt(index);
		mimeEntity.dtfjp(null);
	}

	public void Remove(MimeEntity entity)
	{
		if (entity == null || 1 == 0)
		{
			throw new ArgumentNullException("entity");
		}
		if (zrvif && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		int num = ixjmg.IndexOf(entity);
		if (num >= 0)
		{
			RemoveAt(num);
		}
	}

	public void Clear()
	{
		if (zrvif && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0021;
		}
		goto IL_004b;
		IL_004b:
		if (num < ixjmg.Count)
		{
			goto IL_0021;
		}
		ixjmg.Clear();
		return;
		IL_0021:
		MimeEntity mimeEntity = (MimeEntity)ixjmg[num];
		mimeEntity.dtfjp(null);
		ixjmg[num] = null;
		num++;
		goto IL_004b;
	}

	public IEnumerator GetEnumerator()
	{
		return ixjmg.GetEnumerator();
	}

	private IEnumerator<MimeEntity> gyhtl()
	{
		return this.eaqmu<MimeEntity>().GetEnumerator();
	}

	IEnumerator<MimeEntity> IEnumerable<MimeEntity>.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in gyhtl
		return this.gyhtl();
	}

	private void xikwj(MimeEntity p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("entity");
		}
		if (p0.Parent == yuywq)
		{
			throw new MimeException("The entity is already a child of this parent.", MimeExceptionStatus.OperationError);
		}
		if (p0.Parent != null && 0 == 0)
		{
			throw new MimeException("The entity is already a child of another parent.", MimeExceptionStatus.OperationError);
		}
		MimeEntity parent = yuywq;
		do
		{
			if (parent == p0)
			{
				throw new MimeException("Adding this entity to the list would lead to circular dependency.", MimeExceptionStatus.OperationError);
			}
			parent = parent.Parent;
		}
		while (parent != null);
	}
}
