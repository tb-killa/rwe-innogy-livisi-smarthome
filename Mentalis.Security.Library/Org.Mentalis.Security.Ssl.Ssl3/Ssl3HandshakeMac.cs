using System;
using System.Security.Cryptography;
using Mentalis.Security.Library.Security;

namespace Org.Mentalis.Security.Ssl.Ssl3;

internal sealed class Ssl3HandshakeMac : KeyedHashAlgorithm0
{
	private HashAlgorithm m_HashAlgorithm;

	private bool m_IsDisposed;

	private int m_PadSize;

	public override int HashSize => m_HashAlgorithm.HashSize;

	public Ssl3HandshakeMac(HashType hash)
		: this(hash, null)
	{
	}

	public Ssl3HandshakeMac(HashType hashType, byte[] rgbKey)
	{
		if (rgbKey == null)
		{
			throw new ArgumentNullException();
		}
		if (hashType == HashType.MD5)
		{
			m_HashAlgorithm = new MD5CryptoServiceProvider();
			m_PadSize = 48;
		}
		else
		{
			m_HashAlgorithm = new SHA1CryptoServiceProvider();
			m_PadSize = 40;
		}
		KeyValue = (byte[])rgbKey.Clone();
		m_IsDisposed = false;
		Initialize();
	}

	public Ssl3HandshakeMac(HashType hashType, HashAlgorithm hash, byte[] rgbKey)
	{
		if (rgbKey == null)
		{
			throw new ArgumentNullException();
		}
		if (hashType == HashType.MD5)
		{
			m_PadSize = 48;
		}
		else
		{
			m_PadSize = 40;
		}
		m_HashAlgorithm = hash;
		KeyValue = (byte[])rgbKey.Clone();
		m_IsDisposed = false;
	}

	public override void Initialize()
	{
		if (m_IsDisposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		m_HashAlgorithm.Initialize();
		State = 0;
	}

	protected override void HashCore(byte[] rgb, int ib, int cb)
	{
		if (m_IsDisposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		m_HashAlgorithm.TransformBlock(rgb, ib, cb, rgb, ib);
	}

	protected override byte[] HashFinal()
	{
		if (m_IsDisposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		m_HashAlgorithm.TransformBlock(KeyValue, 0, KeyValue.Length, KeyValue, 0);
		byte[] array = new byte[m_PadSize];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = 54;
		}
		m_HashAlgorithm.TransformFinalBlock(array, 0, array.Length);
		byte[] hash = m_HashAlgorithm.Hash;
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = 92;
		}
		m_HashAlgorithm.Initialize();
		m_HashAlgorithm.TransformBlock(base.Key, 0, base.Key.Length, base.Key, 0);
		m_HashAlgorithm.TransformBlock(array, 0, array.Length, array, 0);
		m_HashAlgorithm.TransformFinalBlock(hash, 0, hash.Length);
		return m_HashAlgorithm.Hash;
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		m_IsDisposed = true;
		m_HashAlgorithm.Clear();
		try
		{
			GC.SuppressFinalize(this);
		}
		catch
		{
		}
	}

	~Ssl3HandshakeMac()
	{
		m_HashAlgorithm.Clear();
	}
}
