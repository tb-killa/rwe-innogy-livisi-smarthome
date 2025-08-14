using System;
using System.Security.Cryptography;
using System.Text;

namespace Org.Mentalis.Security.Ssl.Tls1;

internal class PseudoRandomDeriveBytes : DeriveBytes0, IDisposable
{
	private bool m_Disposed;

	private ExpansionDeriveBytes m_MD5;

	private ExpansionDeriveBytes m_SHA1;

	public PseudoRandomDeriveBytes(byte[] secret, string label, byte[] seed)
	{
		if (label == null)
		{
			throw new ArgumentNullException();
		}
		Initialize(secret, Encoding.ASCII.GetBytes(label), seed);
	}

	public PseudoRandomDeriveBytes(byte[] secret, byte[] label, byte[] seed)
	{
		if (label == null)
		{
			throw new ArgumentNullException();
		}
		Initialize(secret, label, seed);
	}

	public void Dispose()
	{
		if (!m_Disposed)
		{
			m_Disposed = true;
			m_MD5.Dispose();
			m_SHA1.Dispose();
		}
	}

	protected void Initialize(byte[] secret, byte[] label, byte[] seed)
	{
		if (secret == null || seed == null)
		{
			throw new ArgumentNullException();
		}
		m_Disposed = false;
		byte[] array = new byte[label.Length + seed.Length];
		Array.Copy(label, 0, array, 0, label.Length);
		Array.Copy(seed, 0, array, label.Length, seed.Length);
		int num = ((secret.Length % 2 != 0) ? (secret.Length / 2 + 1) : (secret.Length / 2));
		byte[] array2 = new byte[num];
		byte[] array3 = new byte[num];
		Array.Copy(secret, 0, array2, 0, num);
		Array.Copy(secret, secret.Length - num, array3, 0, num);
		m_MD5 = new ExpansionDeriveBytes(new MD5CryptoServiceProvider(), array2, array);
		m_SHA1 = new ExpansionDeriveBytes(new SHA1CryptoServiceProvider(), array3, array);
	}

	public override byte[] GetBytes(int cb)
	{
		if (m_Disposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		byte[] bytes = m_MD5.GetBytes(cb);
		byte[] bytes2 = m_SHA1.GetBytes(cb);
		byte[] array = new byte[cb];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (byte)(bytes[i] ^ bytes2[i]);
		}
		return array;
	}

	public override void Reset()
	{
		if (m_Disposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		m_MD5.Reset();
		m_SHA1.Reset();
	}

	~PseudoRandomDeriveBytes()
	{
		Dispose();
	}
}
