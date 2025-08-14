using System;
using System.Collections;

namespace RWE.SmartHome.SHC.WebSocketsService.Collections;

public class NameValueCollection : ICollection, IEnumerable
{
	protected class NameValueEnumerator : INameValueEnumerator, IEnumerator
	{
		protected IEnumerator hashEnumerator;

		object IEnumerator.Current => Current;

		public NameValuePair Current
		{
			get
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)hashEnumerator.Current;
				return new NameValuePair((string)dictionaryEntry.Key, (string)dictionaryEntry.Value);
			}
		}

		public NameValuePair Entry => Current;

		public string Name => Current.Name;

		public string Value => Current.Value;

		public NameValueEnumerator(Hashtable hashTable)
		{
			hashEnumerator = hashTable.GetEnumerator();
		}

		public void Reset()
		{
			hashEnumerator.Reset();
		}

		public bool MoveNext()
		{
			return hashEnumerator.MoveNext();
		}
	}

	protected Hashtable hashTable;

	public string this[string name]
	{
		get
		{
			if (!hashTable.Contains("name"))
			{
				return null;
			}
			return (string)hashTable[name];
		}
		set
		{
			hashTable[name] = value;
		}
	}

	public bool IsSynchronized => false;

	public object SyncRoot => hashTable.SyncRoot;

	public int Count => hashTable.Count;

	public NameValueCollection()
	{
		hashTable = new Hashtable();
	}

	public void Add(string name, string value)
	{
		hashTable[name] = value;
	}

	public void Clear()
	{
		hashTable.Clear();
	}

	public bool Contains(string name)
	{
		return hashTable.Contains(name);
	}

	public void Remove(string name)
	{
		hashTable.Remove(name);
	}

	public void CopyTo(NameValuePair[] array, int arrayIndex)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		if (array == null)
		{
			throw new InvalidCastException("array must be of type NameValuePair[]");
		}
		if (arrayIndex < 0 || array.Length - arrayIndex < hashTable.Keys.Count)
		{
			throw new ArgumentOutOfRangeException("arrayIndex");
		}
		foreach (object key in hashTable.Keys)
		{
			ref NameValuePair reference = ref array[arrayIndex++];
			reference = new NameValuePair((string)key, (string)hashTable[key]);
		}
	}

	void ICollection.CopyTo(Array array, int arrayIndex)
	{
		CopyTo((NameValuePair[])array, arrayIndex);
	}

	public INameValueEnumerator GetEnumerator()
	{
		return new NameValueEnumerator(hashTable);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
