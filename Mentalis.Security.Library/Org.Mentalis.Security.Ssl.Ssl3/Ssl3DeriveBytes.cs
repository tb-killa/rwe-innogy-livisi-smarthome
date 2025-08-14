using System;
using System.Security.Cryptography;
using Org.Mentalis.Security.Ssl.Tls1;

namespace Org.Mentalis.Security.Ssl.Ssl3;

internal class Ssl3DeriveBytes : DeriveBytes0, IDisposable
{
	private bool m_Disposed;

	private int m_Iteration;

	private MD5 m_MD5;

	private byte[] m_NextBytes;

	private byte[] m_Random;

	private byte[] m_Secret;

	private SHA1 m_SHA1;

	public Ssl3DeriveBytes(byte[] secret, byte[] clientRandom, byte[] serverRandom, bool clientServer)
	{
		if (secret == null || clientRandom == null || serverRandom == null)
		{
			throw new ArgumentNullException();
		}
		if (clientRandom.Length != 32 || serverRandom.Length != 32)
		{
			throw new ArgumentException();
		}
		m_Disposed = false;
		m_Secret = (byte[])secret.Clone();
		m_Random = new byte[64];
		if (clientServer)
		{
			Array.Copy(clientRandom, 0, m_Random, 0, 32);
			Array.Copy(serverRandom, 0, m_Random, 32, 32);
		}
		else
		{
			Array.Copy(serverRandom, 0, m_Random, 0, 32);
			Array.Copy(clientRandom, 0, m_Random, 32, 32);
		}
		m_MD5 = new MD5CryptoServiceProvider();
		m_SHA1 = new SHA1CryptoServiceProvider();
		Reset();
	}

	public void Dispose()
	{
		if (!m_Disposed)
		{
			m_Disposed = true;
			m_MD5.Clear();
			m_SHA1.Clear();
			Array.Clear(m_Secret, 0, m_Secret.Length);
			Array.Clear(m_NextBytes, 0, m_NextBytes.Length);
			Array.Clear(m_Random, 0, m_Random.Length);
		}
	}

	protected byte[] GetNextBytes()
	{
		if (m_Iteration > 26)
		{
			throw new CryptographicException("The SSL3 pseudo random function can only output 416 bytes.");
		}
		byte[] array = new byte[m_Iteration];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (byte)(64 + m_Iteration);
		}
		m_SHA1.TransformBlock(array, 0, array.Length, array, 0);
		m_SHA1.TransformBlock(m_Secret, 0, m_Secret.Length, m_Secret, 0);
		m_SHA1.TransformFinalBlock(m_Random, 0, m_Random.Length);
		m_MD5.TransformBlock(m_Secret, 0, m_Secret.Length, m_Secret, 0);
		m_MD5.TransformFinalBlock(m_SHA1.Hash, 0, 20);
		array = m_MD5.Hash;
		m_SHA1.Initialize();
		m_MD5.Initialize();
		m_Iteration++;
		return array;
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
		m_Iteration = 1;
		m_NextBytes = GetNextBytes();
	}

	~Ssl3DeriveBytes()
	{
		Dispose();
	}
}
