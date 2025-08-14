using System;
using System.Collections;

namespace Org.Mentalis.Security.Certificates;

public class CertificateStoreCollection : CertificateStore
{
	private ArrayList m_Stores;

	public CertificateStoreCollection(CertificateStore[] stores)
		: base(SspiProvider.CertOpenStore(new IntPtr(11), 0, 0, 0, null), duplicate: false)
	{
		if (stores == null)
		{
			throw new ArgumentNullException();
		}
		for (int i = 0; i < stores.Length; i++)
		{
			if (stores[i].ToString() == ToString())
			{
				throw new ArgumentException("A certificate store collection cannot hold other certificate store collections.");
			}
		}
		m_Stores = new ArrayList();
		m_Stores.AddRange(stores);
	}

	public CertificateStoreCollection(CertificateStoreCollection collection)
		: base(SspiProvider.CertOpenStore(new IntPtr(11), 0, 0, 0, null), duplicate: false)
	{
		if (collection == null)
		{
			throw new ArgumentNullException();
		}
		m_Stores = new ArrayList(collection.m_Stores);
	}

	public void AddStore(CertificateStore store)
	{
		if (store == null)
		{
			throw new ArgumentNullException();
		}
		if (store.ToString() == ToString())
		{
			throw new ArgumentException("A certificate store collection cannot hold other certificate store collections.");
		}
		m_Stores.Add(store);
	}

	public void RemoveStore(CertificateStore store)
	{
		if (store == null)
		{
			throw new ArgumentNullException();
		}
		m_Stores.Remove(store);
	}
}
