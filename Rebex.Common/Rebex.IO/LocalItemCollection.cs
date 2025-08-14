using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using onrkn;

namespace Rebex.IO;

public class LocalItemCollection : FileSystemItemCollection, ICollection<LocalItem>, IEnumerable<LocalItem>, IEnumerable
{
	[NonSerialized]
	private Dictionary<string, LocalItem> ptbck;

	private bool gkgkb => false;

	public new LocalItem this[int index] => (LocalItem)base[index];

	public new LocalItem this[string path]
	{
		get
		{
			if (ptbck.TryGetValue(path, out var value) && 0 == 0)
			{
				return value;
			}
			return (LocalItem)base[path];
		}
	}

	public LocalItemCollection()
	{
		ptbck = new Dictionary<string, LocalItem>();
	}

	protected override void CheckItem(FileSystemItem item)
	{
		if (item is LocalItem && 0 == 0)
		{
			return;
		}
		throw new InvalidOperationException("Only instances of LocalItem can be added to this collection.");
	}

	public void Add(LocalItem item)
	{
		Add((FileSystemItem)item);
	}

	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void Add(FileSystemItem item)
	{
		if (item == null || 1 == 0)
		{
			throw new ArgumentNullException("item");
		}
		CheckItem(item);
		LocalItem localItem = (LocalItem)item;
		if (ptbck.ContainsKey(localItem.FullPath) && 0 == 0)
		{
			throw new ArgumentException(brgjd.edcru("Collection already contains item '{0}'.", localItem.FullPath));
		}
		base.Add(item);
		ptbck.Add(localItem.FullPath, localItem);
	}

	public void AddRange(IEnumerable<LocalItem> collection)
	{
		if (collection == null || 1 == 0)
		{
			throw new ArgumentNullException("collection");
		}
		AddRange(collection.Cast<FileSystemItem>());
	}

	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal override void rswev(int p0, FileSystemItem p1)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("item");
		}
		CheckItem(p1);
		LocalItem localItem = (LocalItem)p1;
		if (ptbck.ContainsKey(localItem.FullPath) && 0 == 0)
		{
			throw new ArgumentException(brgjd.edcru("Collection already contains item '{0}'.", localItem.FullPath));
		}
		base.rswev(p0, p1);
		ptbck.Add(localItem.FullPath, localItem);
	}

	public bool Contains(LocalItem item)
	{
		return base.Contains((FileSystemItem)item);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	public new bool Contains(FileSystemItem item)
	{
		return base.Contains(item);
	}

	public override bool Contains(string path)
	{
		if (ptbck.ContainsKey(path) && 0 == 0)
		{
			return true;
		}
		return base.Contains(path);
	}

	public void CopyTo(LocalItem[] array, int arrayIndex)
	{
		CopyTo((FileSystemItem[])array, arrayIndex);
	}

	public bool Remove(LocalItem item)
	{
		return Remove((FileSystemItem)item);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	public override bool Remove(FileSystemItem item)
	{
		if (base.Remove(item) && 0 == 0)
		{
			ptbck.Remove(((LocalItem)item).FullPath);
			return true;
		}
		return false;
	}

	public new IEnumerator<LocalItem> GetEnumerator()
	{
		return this.eaqmu<LocalItem>().GetEnumerator();
	}
}
