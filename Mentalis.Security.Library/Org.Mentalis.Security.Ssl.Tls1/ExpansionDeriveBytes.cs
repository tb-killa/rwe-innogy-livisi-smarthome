using System;
using System.Security.Cryptography;
using System.Text;
using Org.Mentalis.Security.Cryptography;

namespace Org.Mentalis.Security.Ssl.Tls1;

internal class ExpansionDeriveBytes : DeriveBytes0, IDisposable
{
	private byte[] m_Ai;

	private bool m_Disposed;

	private int m_HashSize;

	private Org.Mentalis.Security.Cryptography.HMAC m_HMAC;

	private byte[] m_NextBytes;

	private byte[] m_Seed;

	public ExpansionDeriveBytes(HashAlgorithm hash, byte[] secret, string seed)
	{
		if (seed == null)
		{
			throw new ArgumentNullException();
		}
		Initialize(hash, secret, Encoding.ASCII.GetBytes(seed));
	}

	public ExpansionDeriveBytes(HashAlgorithm hash, byte[] secret, byte[] seed)
	{
		Initialize(hash, secret, seed);
	}

	public void Dispose()
	{
		if (!m_Disposed)
		{
			m_Disposed = true;
			m_HMAC.Clear();
			Array.Clear(m_Seed, 0, m_Seed.Length);
			Array.Clear(m_Ai, 0, m_Ai.Length);
			Array.Clear(m_NextBytes, 0, m_NextBytes.Length);
		}
	}

	protected void Initialize(HashAlgorithm hash, byte[] secret, byte[] seed)
	{
		if (seed == null || secret == null || hash == null)
		{
			throw new ArgumentNullException();
		}
		m_Disposed = false;
		m_HMAC = new Org.Mentalis.Security.Cryptography.HMAC(hash, secret);
		m_Seed = seed;
		m_HashSize = m_HMAC.HashSize / 8;
		Reset();
	}

	protected byte[] GetNextBytes()
	{
		m_HMAC.TransformBlock(m_Ai, 0, m_HashSize, m_Ai, 0);
		m_HMAC.TransformFinalBlock(m_Seed, 0, m_Seed.Length);
		byte[] hash = m_HMAC.Hash;
		m_HMAC.Initialize();
		m_Ai = m_HMAC.ComputeHash(m_Ai);
		return hash;
	}

	public override byte[] GetBytes(int cb)
	{
		if (m_Disposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (cb < 0)
		{
			throw new ArgumentException();
		}
		byte[] array = new byte[cb];
		int num = 0;
		while (num < array.Length)
		{
			if (num + m_NextBytes.Length >= cb)
			{
				Array.Copy(m_NextBytes, 0, array, num, cb - num);
				byte[] array2 = new byte[m_NextBytes.Length - (cb - num)];
				Array.Copy(m_NextBytes, m_NextBytes.Length - array2.Length, array2, 0, array2.Length);
				m_NextBytes = array2;
				num = array.Length;
			}
			else
			{
				Array.Copy(m_NextBytes, 0, array, num, m_NextBytes.Length);
				num += m_NextBytes.Length;
				m_NextBytes = GetNextBytes();
			}
		}
		return array;
	}

	public override void Reset()
	{
		if (m_Disposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		m_Ai = m_HMAC.ComputeHash(m_Seed);
		m_NextBytes = GetNextBytes();
	}

	~ExpansionDeriveBytes()
	{
		Dispose();
	}
}
