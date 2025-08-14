using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Org.Mentalis.Security.Certificates;
using Org.Mentalis.Security.Ssl.Ssl3;

namespace Org.Mentalis.Security.Ssl.Shared;

internal sealed class MD5SHA1CryptoServiceProvider : HashAlgorithm
{
	private byte[] m_MasterKey;

	private HashAlgorithm m_MD5;

	private SecureProtocol m_Protocol;

	private HashAlgorithm m_SHA1;

	public SecureProtocol Protocol
	{
		get
		{
			return m_Protocol;
		}
		set
		{
			m_Protocol = value;
		}
	}

	public byte[] MasterKey
	{
		get
		{
			return m_MasterKey;
		}
		set
		{
			m_MasterKey = (byte[])value.Clone();
		}
	}

	public MD5SHA1CryptoServiceProvider()
	{
		HashSizeValue = 36;
		m_MD5 = new MD5CryptoServiceProvider();
		m_SHA1 = new SHA1CryptoServiceProvider();
	}

	protected override void Dispose(bool disposing)
	{
		m_MD5.Clear();
		m_SHA1.Clear();
		if (m_MasterKey != null)
		{
			Array.Clear(m_MasterKey, 0, m_MasterKey.Length);
		}
		try
		{
			GC.SuppressFinalize(this);
		}
		catch
		{
		}
	}

	public override void Initialize()
	{
		m_MD5.Initialize();
		m_SHA1.Initialize();
	}

	protected override void HashCore(byte[] array, int ibStart, int cbSize)
	{
		m_MD5.TransformBlock(array, ibStart, cbSize, array, ibStart);
		m_SHA1.TransformBlock(array, ibStart, cbSize, array, ibStart);
	}

	protected override byte[] HashFinal()
	{
		if (m_Protocol == SecureProtocol.Ssl3)
		{
			m_MD5 = new Ssl3HandshakeMac(HashType.MD5, m_MD5, m_MasterKey);
			m_SHA1 = new Ssl3HandshakeMac(HashType.SHA1, m_SHA1, m_MasterKey);
		}
		byte[] array = new byte[36];
		m_MD5.TransformFinalBlock(array, 0, 0);
		m_SHA1.TransformFinalBlock(array, 0, 0);
		Array.Copy(m_MD5.Hash, 0, array, 0, 16);
		Array.Copy(m_SHA1.Hash, 0, array, 16, 20);
		return array;
	}

	public bool VerifySignature(Certificate cert, byte[] signature)
	{
		return VerifySignature(cert, signature, Hash);
	}

	public bool VerifySignature(Certificate cert, byte[] signature, byte[] hash)
	{
		int phProv = 0;
		int phHash = 0;
		int phKey = 0;
		try
		{
			if (SspiProvider.CryptAcquireContext(ref phProv, IntPtr.Zero, null, 1, 0) == 0 && Marshal.GetLastWin32Error() == -2146893802)
			{
				SspiProvider.CryptAcquireContext(ref phProv, IntPtr.Zero, null, 1, 8);
			}
			if (phProv == 0)
			{
				throw new CryptographicException("Unable to acquire a cryptographic context.");
			}
			if (SspiProvider.CryptCreateHash(phProv, 32776, 0, 0, out phHash) == 0)
			{
				throw new CryptographicException("Unable to create the SHA-MD5 hash.");
			}
			if (SspiProvider.CryptSetHashParam(phHash, 2, hash, 0) == 0)
			{
				throw new CryptographicException("Unable to set the value of the SHA-MD5 hash.");
			}
			CertificateInfo certificateInfo = cert.GetCertificateInfo();
			CERT_PUBLIC_KEY_INFO pInfo = new CERT_PUBLIC_KEY_INFO(certificateInfo);
			if (SspiProvider.CryptImportPublicKeyInfo(phProv, 65537, ref pInfo, out phKey) == 0)
			{
				throw new CryptographicException("Unable to get a handle to the public key of the specified certificate.");
			}
			byte[] array = new byte[signature.Length];
			Array.Copy(signature, 0, array, 0, signature.Length);
			Array.Reverse(array);
			return SspiProvider.CryptVerifySignature(phHash, array, array.Length, phKey, IntPtr.Zero, 0) != 0;
		}
		finally
		{
			if (phKey != 0)
			{
				SspiProvider.CryptDestroyKey(phKey);
			}
			if (phHash != 0)
			{
				SspiProvider.CryptDestroyHash(phHash);
			}
			if (phProv != 0)
			{
				SspiProvider.CryptReleaseContext(phProv, 0);
			}
		}
	}

	public byte[] CreateSignature(Certificate cert)
	{
		return CreateSignature(cert, Hash);
	}

	public byte[] CreateSignature(Certificate cert, byte[] hash)
	{
		int num = 0;
		int pfCallerFreeProv = 0;
		int phCryptProv = 0;
		int pdwKeySpec = 0;
		int phHash = 0;
		int pdwSigLen = 0;
		try
		{
			num = 64;
			if (SspiProvider.CryptAcquireCertificatePrivateKey(cert.Handle, num, IntPtr.Zero, ref phCryptProv, ref pdwKeySpec, ref pfCallerFreeProv) == 0)
			{
				throw new SslException(AlertDescription.InternalError, "Could not acquire private key.");
			}
			if (SspiProvider.CryptCreateHash(phCryptProv, 32776, 0, 0, out phHash) == 0)
			{
				throw new CryptographicException("Unable to create the SHA-MD5 hash.");
			}
			if (SspiProvider.CryptSetHashParam(phHash, 2, hash, 0) == 0)
			{
				throw new CryptographicException("Unable to set the value of the SHA-MD5 hash.");
			}
			SspiProvider.CryptSignHash(phHash, pdwKeySpec, IntPtr.Zero, 0, null, ref pdwSigLen);
			if (pdwSigLen == 0)
			{
				throw new CryptographicException("Unable to sign the data.");
			}
			byte[] array = new byte[pdwSigLen];
			if (SspiProvider.CryptSignHash(phHash, pdwKeySpec, IntPtr.Zero, 0, array, ref pdwSigLen) == 0)
			{
				throw new CryptographicException("Unable to sign the data.");
			}
			Array.Reverse(array);
			return array;
		}
		finally
		{
			if (phHash != 0)
			{
				SspiProvider.CryptDestroyHash(phHash);
			}
			if (pfCallerFreeProv != 0 && phCryptProv != 0)
			{
				SspiProvider.CryptReleaseContext(phCryptProv, 0);
			}
		}
	}

	~MD5SHA1CryptoServiceProvider()
	{
		Clear();
	}
}
