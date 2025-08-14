using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using onrkn;

namespace Rebex.IO;

public class FileSystemItemCollection : ICollection<FileSystemItem>, IEnumerable<FileSystemItem>, IEnumerable
{
	private class rurpr : IComparer<FileSystemItem>
	{
		private readonly IComparer eevxf;

		public rurpr(IComparer comparer)
		{
			eevxf = comparer;
		}

		public int Compare(FileSystemItem x, FileSystemItem y)
		{
			if (eevxf == null || 1 == 0)
			{
				return string.Compare(x.Path, y.Path, StringComparison.Ordinal);
			}
			return eevxf.Compare(x, y);
		}
	}

	internal static readonly char[] ngcvv = new char[11]
	{
		'\0', '\b', '\u0010', '\u0011', '\u0012', '\u0014', '\u0015', '\u0016', '\u0017', '\u0018',
		'\u0019'
	};

	[NonSerialized]
	private Dictionary<string, FileSystemItem> nrhep;

	private readonly List<FileSystemItem> lmgyk;

	private bool agruk;

	public virtual object SyncRoot => lmgyk;

	public int Count => lmgyk.Count;

	public FileSystemItem this[int index] => lmgyk[index];

	public FileSystemItem this[string path]
	{
		get
		{
			if (nrhep.TryGetValue(path, out var value) && 0 == 0)
			{
				return value;
			}
			return null;
		}
	}

	private bool lzfji => false;

	public bool UsePath
	{
		get
		{
			return agruk;
		}
		set
		{
			agruk = value;
		}
	}

	public FileSystemItemCollection()
	{
		nrhep = new Dictionary<string, FileSystemItem>();
		lmgyk = new List<FileSystemItem>();
	}

	protected virtual void CheckItem(FileSystemItem item)
	{
	}

	private IEnumerator qwfnx()
	{
		return lmgyk.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in qwfnx
		return this.qwfnx();
	}

	public IEnumerator<FileSystemItem> GetEnumerator()
	{
		return lmgyk.GetEnumerator();
	}

	protected void AddRange(IEnumerable<FileSystemItem> collection)
	{
		if (collection == null || 1 == 0)
		{
			throw new ArgumentNullException("collection");
		}
		IEnumerator<FileSystemItem> enumerator = collection.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				FileSystemItem current = enumerator.Current;
				Add(current);
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

	public virtual void Add(FileSystemItem item)
	{
		if (item == null || 1 == 0)
		{
			throw new ArgumentNullException("item");
		}
		CheckItem(item);
		if (nrhep.ContainsKey(item.Path) && 0 == 0)
		{
			throw new ArgumentException(brgjd.edcru("Collection already contains item '{0}'.", item.Path));
		}
		lmgyk.Add(item);
		nrhep.Add(item.Path, item);
	}

	internal virtual void rswev(int p0, FileSystemItem p1)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("item");
		}
		if (p0 < 0 || p0 > lmgyk.Count)
		{
			throw hifyx.nztrs("index", p0, "Index is out of the bounds of an array.");
		}
		CheckItem(p1);
		if (nrhep.ContainsKey(p1.Path) && 0 == 0)
		{
			throw new ArgumentException(brgjd.edcru("Collection already contains item '{0}'.", p1.Path));
		}
		lmgyk.Insert(p0, p1);
		nrhep.Add(p1.Path, p1);
	}

	public virtual bool Remove(FileSystemItem item)
	{
		if (item == null || 1 == 0)
		{
			throw new ArgumentNullException("item");
		}
		if (lmgyk.Remove(item) && 0 == 0)
		{
			nrhep.Remove(item.Path);
			return true;
		}
		return false;
	}

	public bool Remove(string path)
	{
		if (path == null || 1 == 0)
		{
			throw new ArgumentNullException("path", "Path cannot be null.");
		}
		if (!nrhep.TryGetValue(path, out var value) || 1 == 0)
		{
			return false;
		}
		return Remove(value);
	}

	public void RemoveAt(int index)
	{
		if (index < 0 || index >= lmgyk.Count)
		{
			throw hifyx.nztrs("index", index, "Argument is out of range of valid values.");
		}
		Remove(lmgyk[index]);
	}

	public void Clear()
	{
		lmgyk.Clear();
		nrhep.Clear();
	}

	public void CopyTo(FileSystemItem[] array, int index)
	{
		lmgyk.CopyTo(array, index);
	}

	public bool Contains(FileSystemItem item)
	{
		if (item == null || 1 == 0)
		{
			throw new ArgumentNullException("item");
		}
		return lmgyk.Contains(item);
	}

	public virtual bool Contains(string path)
	{
		return nrhep.ContainsKey(path);
	}

	public long GetTotalSize()
	{
		long num = 0L;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0009;
		}
		goto IL_0033;
		IL_0009:
		FileSystemItem fileSystemItem = lmgyk[num2];
		if (fileSystemItem.IsFile && 0 == 0)
		{
			num += fileSystemItem.Length;
		}
		num2++;
		goto IL_0033;
		IL_0033:
		if (num2 < lmgyk.Count)
		{
			goto IL_0009;
		}
		return num;
	}

	public string[] GetFiles(Regex regExp)
	{
		if (regExp == null || 1 == 0)
		{
			throw new ArgumentNullException("regExp");
		}
		ArrayList arrayList = new ArrayList();
		int num = 0;
		if (num != 0)
		{
			goto IL_0024;
		}
		goto IL_0072;
		IL_0024:
		FileSystemItem fileSystemItem = lmgyk[num];
		string text = ((UsePath ? true : false) ? fileSystemItem.Path : fileSystemItem.Name);
		if (regExp.Match(text).Success && 0 == 0)
		{
			arrayList.Add(text);
		}
		num++;
		goto IL_0072;
		IL_0072:
		if (num < lmgyk.Count)
		{
			goto IL_0024;
		}
		return (string[])arrayList.ToArray(typeof(string));
	}

	public string[] GetFiles(string searchPattern, bool caseSensitive)
	{
		if (searchPattern == null || 1 == 0)
		{
			throw new ArgumentNullException("searchPattern");
		}
		if (searchPattern.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("String cannot have zero length.", "searchPattern");
		}
		if (searchPattern.IndexOfAny(ngcvv) >= 0)
		{
			throw new ArgumentException("Illegal characters in mask.", "searchPattern");
		}
		Regex regExp = dahxy.khmpt(searchPattern, caseSensitive);
		return GetFiles(regExp);
	}

	public string[] GetFiles(string searchPattern)
	{
		return GetFiles(searchPattern, caseSensitive: false);
	}

	public void Sort()
	{
		Sort(null);
	}

	public void Sort(IComparer comparer)
	{
		rurpr comparer2 = new rurpr(comparer);
		lmgyk.Sort(comparer2);
	}

	public void Sort(int index, int count, IComparer comparer)
	{
		rurpr comparer2 = new rurpr(comparer);
		lmgyk.Sort(index, count, comparer2);
	}
}
