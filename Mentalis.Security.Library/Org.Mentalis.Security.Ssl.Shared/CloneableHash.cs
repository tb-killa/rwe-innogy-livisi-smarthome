using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Org.Mentalis.Security.Cryptography;

namespace Org.Mentalis.Security.Ssl.Shared;

internal sealed class CloneableHash : HashAlgorithm, ICloneable
{
	private bool m_Disposed;

	private int m_Hash;

	private int m_Provider;

	private int m_Size;

	private HashType m_Type;

	public CloneableHash(HashType type)
	{
		m_Type = type;
		m_Provider = CAPIProvider.Handle;
		Initialize();
		m_Disposed = false;
	}

	public CloneableHash(int hash, HashType type, int size)
	{
		m_Provider = CAPIProvider.Handle;
		m_Type = type;
		m_Size = size;
		m_Disposed = false;
		if (SspiProvider.CryptDuplicateHash(hash, IntPtr.Zero, 0, out m_Hash) == 0)
		{
			throw new CryptographicException("Couldn't duplicate hash.");
		}
	}

	public object Clone()
	{
		return new CloneableHash(m_Hash, m_Type, m_Size);
	}

	public override void Initialize()
	{
		if (m_Disposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (m_Hash != 0)
		{
			SspiProvider.CryptDestroyHash(m_Hash);
		}
		int algid = 32772;
		m_Size = 20;
		if (m_Type == HashType.MD5)
		{
			algid = 32771;
			m_Size = 16;
		}
		SspiProvider.CryptCreateHash(m_Provider, algid, 0, 0, out m_Hash);
	}

	protected override void HashCore(byte[] array, int ibStart, int cbSize)
	{
		if (ibStart > 0)
		{
			GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			try
			{
				IntPtr intPtr = gCHandle.AddrOfPinnedObject();
				if (SspiProvider.CryptHashData(m_Hash, new IntPtr(intPtr.ToInt64() + ibStart), cbSize, 0) == 0)
				{
					throw new CryptographicException("The data could not be hashed.");
				}
				return;
			}
			finally
			{
				gCHandle.Free();
			}
		}
		if (SspiProvider.CryptHashData(m_Hash, array, cbSize, 0) == 0)
		{
			throw new CryptographicException("The data could not be hashed.");
		}
	}

	protected override byte[] HashFinal()
	{
		byte[] array = new byte[m_Size];
		int pdwDataLen = array.Length;
		if (SspiProvider.CryptGetHashParam(m_Hash, 2, array, ref pdwDataLen, 0) == 0)
		{
			throw new CryptographicException("The hash value could not be read.");
		}
		return array;
	}

	protected override void Dispose(bool disposing)
	{
		if (!m_Disposed)
		{
			if (m_Hash != 0)
			{
				SspiProvider.CryptDestroyHash(m_Hash);
				m_Hash = 0;
			}
			try
			{
				GC.SuppressFinalize(this);
			}
			catch
			{
			}
			m_Disposed = true;
		}
	}

	~CloneableHash()
	{
		Clear();
	}
}
