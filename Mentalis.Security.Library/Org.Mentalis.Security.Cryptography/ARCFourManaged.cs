using System;
using System.Security.Cryptography;

namespace Org.Mentalis.Security.Cryptography;

public sealed class ARCFourManaged : RC4
{
	private bool m_IsDisposed;

	public ARCFourManaged()
	{
		m_IsDisposed = false;
	}

	public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
	{
		if (m_IsDisposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (rgbKey == null)
		{
			throw new ArgumentNullException("Key is a null reference.");
		}
		if (rgbKey.Length == 0 || rgbKey.Length > 256)
		{
			throw new CryptographicException("Invalid Key.");
		}
		if (rgbIV != null && rgbIV.Length > 1)
		{
			throw new CryptographicException("Invalid Initialization Vector.");
		}
		return new ARCFourManagedTransform(rgbKey);
	}

	public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
	{
		return CreateDecryptor(rgbKey, rgbIV);
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing: true);
		m_IsDisposed = true;
	}
}
