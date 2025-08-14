using System;
using System.Security.Cryptography;
using Mentalis.Security.Library.Security;

namespace Org.Mentalis.Security.Ssl.Ssl3;

internal sealed class Ssl3RecordMAC : KeyedHashAlgorithm0
{
	private HashAlgorithm m_HashAlgorithm;

	private bool m_IsDisposed;

	private bool m_IsHashing;

	private int m_PadSize;

	public override int HashSize => m_HashAlgorithm.HashSize;

	public Ssl3RecordMAC(HashType hash)
		: this(hash, null)
	{
	}

	public Ssl3RecordMAC(HashType hash, byte[] rgbKey)
	{
		if (rgbKey == null)
		{
			throw new ArgumentNullException();
		}
		if (hash == HashType.MD5)
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

	public override void Initialize()
	{
		if (m_IsDisposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		m_HashAlgorithm.Initialize();
		m_IsHashing = false;
		State = 0;
	}

	protected override void HashCore(byte[] rgb, int ib, int cb)
	{
		if (m_IsDisposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (!m_IsHashing)
		{
			m_HashAlgorithm.TransformBlock(KeyValue, 0, KeyValue.Length, KeyValue, 0);
			byte[] array = new byte[m_PadSize];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 54;
			}
			m_HashAlgorithm.TransformBlock(array, 0, array.Length, array, 0);
			m_IsHashing = true;
		}
		m_HashAlgorithm.TransformBlock(rgb, ib, cb, rgb, ib);
	}

	protected override byte[] HashFinal()
	{
		if (m_IsDisposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		m_HashAlgorithm.TransformFinalBlock(new byte[0], 0, 0);
		byte[] hash = m_HashAlgorithm.Hash;
		byte[] array = new byte[m_PadSize];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = 92;
		}
		m_HashAlgorithm.Initialize();
		m_HashAlgorithm.TransformBlock(KeyValue, 0, KeyValue.Length, KeyValue, 0);
		m_HashAlgorithm.TransformBlock(array, 0, array.Length, array, 0);
		m_HashAlgorithm.TransformFinalBlock(hash, 0, hash.Length);
		m_IsHashing = false;
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

	~Ssl3RecordMAC()
	{
		m_HashAlgorithm.Clear();
	}
}
