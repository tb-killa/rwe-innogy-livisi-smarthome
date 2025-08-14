using System;
using System.Security.Cryptography;
using Mentalis.Security.Library.Security;

namespace Org.Mentalis.Security.Cryptography;

public sealed class HMAC : KeyedHashAlgorithm0
{
	private HashAlgorithm m_HashAlgorithm;

	private bool m_IsDisposed;

	private bool m_IsHashing;

	private byte[] m_KeyBuffer;

	private byte[] m_Padded;

	public override int HashSize => m_HashAlgorithm.HashSize;

	public HMAC(HashAlgorithm hash)
		: this(hash, null)
	{
	}

	public HMAC(HashAlgorithm hash, byte[] rgbKey)
	{
		if (hash == null)
		{
			throw new ArgumentNullException();
		}
		if (rgbKey == null)
		{
			rgbKey = new byte[hash.HashSize / 8];
			new RNGCryptoServiceProvider().GetBytes(rgbKey);
		}
		m_HashAlgorithm = hash;
		base.Key = (byte[])rgbKey.Clone();
		m_IsDisposed = false;
		m_KeyBuffer = new byte[64];
		m_Padded = new byte[64];
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
		Array.Clear(m_KeyBuffer, 0, m_KeyBuffer.Length);
	}

	protected override void HashCore(byte[] rgb, int ib, int cb)
	{
		if (m_IsDisposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (!m_IsHashing)
		{
			byte[] array = ((base.Key.Length <= 64) ? base.Key : m_HashAlgorithm.ComputeHash(base.Key));
			Array.Copy(array, 0, m_KeyBuffer, 0, array.Length);
			for (int i = 0; i < 64; i++)
			{
				m_Padded[i] = (byte)(m_KeyBuffer[i] ^ 0x36);
			}
			m_HashAlgorithm.TransformBlock(m_Padded, 0, m_Padded.Length, m_Padded, 0);
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
		for (int i = 0; i < 64; i++)
		{
			m_Padded[i] = (byte)(m_KeyBuffer[i] ^ 0x5C);
		}
		m_HashAlgorithm.Initialize();
		m_HashAlgorithm.TransformBlock(m_Padded, 0, m_Padded.Length, m_Padded, 0);
		m_HashAlgorithm.TransformFinalBlock(hash, 0, hash.Length);
		hash = m_HashAlgorithm.Hash;
		Array.Clear(m_KeyBuffer, 0, m_KeyBuffer.Length);
		m_IsHashing = false;
		return hash;
	}

	protected override void Dispose(bool disposing)
	{
		m_IsDisposed = true;
		base.Dispose(disposing: true);
		m_HashAlgorithm.Clear();
		try
		{
			GC.SuppressFinalize(this);
		}
		catch
		{
		}
	}

	~HMAC()
	{
		m_HashAlgorithm.Clear();
	}
}
