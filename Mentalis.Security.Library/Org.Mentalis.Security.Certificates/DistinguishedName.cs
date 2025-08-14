using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Org.Mentalis.Security.Certificates;

public class DistinguishedName
{
	private readonly ArrayList m_List;

	public NameAttribute this[int index]
	{
		get
		{
			return (NameAttribute)m_List[index];
		}
		set
		{
			m_List[index] = value;
		}
	}

	public int Count => m_List.Count;

	public DistinguishedName()
	{
		m_List = new ArrayList();
	}

	internal DistinguishedName(CertificateNameInfo cni)
		: this()
	{
		Initialize(cni);
	}

	internal DistinguishedName(IntPtr input, int length)
		: this()
	{
		int pcbStructInfo = 0;
		SspiProvider.CryptDecodeObject(65537, new IntPtr(20), input, length, 0, IntPtr.Zero, ref pcbStructInfo);
		if (pcbStructInfo <= 0)
		{
			throw new CertificateException("Unable to decode the name of the certificate.");
		}
		IntPtr intPtr = Marshal.AllocHGlobal(pcbStructInfo);
		if (SspiProvider.CryptDecodeObject(65537, new IntPtr(20), input, length, 0, intPtr, ref pcbStructInfo) == 0)
		{
			throw new CertificateException("Unable to decode the name of the certificate.");
		}
		try
		{
			CertificateNameInfo cni = (CertificateNameInfo)Marshal.PtrToStructure(intPtr, typeof(CertificateNameInfo));
			Initialize(cni);
		}
		catch (CertificateException ex)
		{
			throw ex;
		}
		catch (Exception inner)
		{
			throw new CertificateException("Could not get the certificate distinguished name.", inner);
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}
	}

	private void Initialize(CertificateNameInfo cni)
	{
		if (cni.cRDN <= 0)
		{
			throw new CertificateException("Certificate does not have a subject relative distinguished name.");
		}
		for (int i = 0; i < cni.cRDN; i++)
		{
			RelativeDistinguishedName relativeDistinguishedName = (RelativeDistinguishedName)Marshal.PtrToStructure(new IntPtr(cni.rgRDN.ToInt64() + i * Marshal.SizeOf(typeof(RelativeDistinguishedName))), typeof(RelativeDistinguishedName));
			for (int j = 0; j < relativeDistinguishedName.cRDNAttr; j++)
			{
				RdnAttribute rdnAttribute = (RdnAttribute)Marshal.PtrToStructure(new IntPtr(relativeDistinguishedName.rgRDNAttr.ToInt64() + j * Marshal.SizeOf(typeof(RdnAttribute))), typeof(RdnAttribute));
				m_List.Add(new NameAttribute(CeMarshal.PtrToStringAnsi(rdnAttribute.pszObjId), Marshal.PtrToStringUni(rdnAttribute.pbData)));
			}
		}
	}

	public int Add(NameAttribute attribute)
	{
		return m_List.Add(attribute);
	}

	public void Clear()
	{
		m_List.Clear();
	}

	public bool Contains(NameAttribute value)
	{
		return m_List.Contains(value);
	}

	public int IndexOf(NameAttribute value)
	{
		return m_List.IndexOf(value);
	}

	public int IndexOf(string oid)
	{
		for (int i = 0; i < m_List.Count; i++)
		{
			if (((NameAttribute)m_List[i]).ObjectID == oid)
			{
				return i;
			}
		}
		return -1;
	}

	public void Insert(int index, NameAttribute value)
	{
		m_List.Insert(index, value);
	}

	public void Remove(NameAttribute value)
	{
		m_List.Remove(value);
	}

	public void RemoveAt(int index)
	{
		m_List.RemoveAt(index);
	}
}
