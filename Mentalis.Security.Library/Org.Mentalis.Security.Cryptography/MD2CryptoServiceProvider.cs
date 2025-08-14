using System;
using System.Security.Cryptography;

namespace Org.Mentalis.Security.Cryptography;

public sealed class MD2CryptoServiceProvider : MD2
{
	private bool m_Disposed;

	private int m_Hash;

	private int m_Provider;

	public MD2CryptoServiceProvider()
	{
		m_Provider = CAPIProvider.Handle;
		Initialize();
		m_Disposed = false;
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
		SspiProvider.CryptCreateHash(m_Provider, 32769, 0, 0, out m_Hash);
	}

	protected override void HashCore(byte[] array, int ibStart, int cbSize)
	{
		if (m_Disposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		byte[] array2 = new byte[cbSize];
		Array.Copy(array, ibStart, array2, 0, cbSize);
		if (SspiProvider.CryptHashData(m_Hash, array2, array2.Length, 0) == 0)
		{
			throw new CryptographicException("The data could not be hashed.");
		}
	}

	protected override byte[] HashFinal()
	{
		if (m_Disposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		byte[] array = new byte[16];
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

	~MD2CryptoServiceProvider()
	{
		Clear();
	}
}
