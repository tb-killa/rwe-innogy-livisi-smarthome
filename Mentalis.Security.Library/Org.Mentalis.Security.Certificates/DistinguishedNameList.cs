using System;
using System.Collections;

namespace Org.Mentalis.Security.Certificates;

public class DistinguishedNameList : IEnumerable, ICloneable
{
	private readonly ArrayList m_List;

	public bool IsFixedSize => m_List.IsFixedSize;

	public bool IsReadOnly => m_List.IsReadOnly;

	public DistinguishedName this[int index]
	{
		get
		{
			return (DistinguishedName)m_List[index];
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			m_List[index] = value;
		}
	}

	public int Count => m_List.Count;

	public bool IsSynchronized => m_List.IsSynchronized;

	public object SyncRoot => m_List.SyncRoot;

	public DistinguishedNameList()
	{
		m_List = new ArrayList();
	}

	internal DistinguishedNameList(ArrayList state)
	{
		if (state == null)
		{
			throw new ArgumentNullException();
		}
		m_List = (ArrayList)state.Clone();
	}

	public object Clone()
	{
		return new DistinguishedNameList(m_List);
	}

	public IEnumerator GetEnumerator()
	{
		return m_List.GetEnumerator();
	}

	public int Add(DistinguishedName value)
	{
		if (value == null)
		{
			throw new ArgumentNullException();
		}
		return m_List.Add(value);
	}

	public void Clear()
	{
		m_List.Clear();
	}

	public bool Contains(DistinguishedName value)
	{
		if (value == null)
		{
			throw new ArgumentNullException();
		}
		return m_List.Contains(value);
	}

	public int IndexOf(DistinguishedName value)
	{
		if (value == null)
		{
			throw new ArgumentNullException();
		}
		return m_List.IndexOf(value);
	}

	public void Insert(int index, DistinguishedName value)
	{
		if (value == null)
		{
			throw new ArgumentNullException();
		}
		m_List.Insert(index, value);
	}

	public void Remove(DistinguishedName value)
	{
		m_List.Remove(value);
	}

	public void RemoveAt(int index)
	{
		m_List.RemoveAt(index);
	}

	public void CopyTo(Array array, int index)
	{
		m_List.CopyTo(array, index);
	}
}
