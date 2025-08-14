using System;
using System.Security.Cryptography;

namespace Org.Mentalis.Security.Cryptography;

public sealed class RijndaelCryptoServiceProvider : Rijndael
{
	private bool m_Disposed;

	private RijndaelManaged m_Managed;

	private int m_Provider;

	public override int BlockSize
	{
		get
		{
			return m_Managed.BlockSize;
		}
		set
		{
			m_Managed.BlockSize = value;
		}
	}

	public override int FeedbackSize
	{
		get
		{
			return m_Managed.FeedbackSize;
		}
		set
		{
			m_Managed.FeedbackSize = value;
		}
	}

	public override byte[] IV
	{
		get
		{
			return m_Managed.IV;
		}
		set
		{
			m_Managed.IV = value;
		}
	}

	public override byte[] Key
	{
		get
		{
			return m_Managed.Key;
		}
		set
		{
			m_Managed.Key = value;
		}
	}

	public override int KeySize
	{
		get
		{
			return m_Managed.KeySize;
		}
		set
		{
			m_Managed.KeySize = value;
		}
	}

	public override KeySizes[] LegalBlockSizes => m_Managed.LegalBlockSizes;

	public override KeySizes[] LegalKeySizes => m_Managed.LegalKeySizes;

	public override CipherMode Mode
	{
		get
		{
			return m_Managed.Mode;
		}
		set
		{
			m_Managed.Mode = value;
		}
	}

	public override PaddingMode Padding
	{
		get
		{
			return m_Managed.Padding;
		}
		set
		{
			m_Managed.Padding = value;
		}
	}

	private CryptoAlgorithm KeyType => GetKeyType(KeySize);

	public RijndaelCryptoServiceProvider()
	{
		try
		{
			m_Provider = CAPIProvider.Handle;
			if (CAPIProvider.HandleProviderType != 24)
			{
				m_Provider = 0;
			}
		}
		catch
		{
			m_Provider = 0;
		}
		m_Managed = new RijndaelManaged();
	}

	~RijndaelCryptoServiceProvider()
	{
		Dispose(disposing: true);
	}

	protected override void Dispose(bool disposing)
	{
		if (m_Managed != null)
		{
			m_Managed.Clear();
			m_Managed = null;
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

	public override void GenerateIV()
	{
		m_Managed.GenerateIV();
	}

	public override void GenerateKey()
	{
		m_Managed.GenerateKey();
	}

	public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
	{
		if (m_Disposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (rgbKey == null || rgbIV == null)
		{
			throw new ArgumentNullException();
		}
		if (Mode == CipherMode.CTS || Mode == CipherMode.OFB || Mode == CipherMode.CFB)
		{
			throw new CryptographicException(Mode.ToString() + " is not supported by this implementation.");
		}
		try
		{
			if (CanUseUnmanaged(rgbKey.Length * 8, rgbIV.Length * 8, Padding))
			{
				return new RijndaelUnmanagedTransform(GetKeyType(rgbKey.Length * 8), CryptoMethod.Decrypt, rgbKey, rgbIV, Mode, FeedbackSize, Padding);
			}
		}
		catch
		{
		}
		return m_Managed.CreateDecryptor(rgbKey, rgbIV);
	}

	public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
	{
		if (m_Disposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (rgbKey == null || rgbIV == null)
		{
			throw new ArgumentNullException();
		}
		if (Mode == CipherMode.CTS || Mode == CipherMode.OFB || Mode == CipherMode.CFB)
		{
			throw new CryptographicException(Mode.ToString() + " is not supported by this implementation.");
		}
		try
		{
			if (CanUseUnmanaged(rgbKey.Length * 8, rgbIV.Length * 8, Padding))
			{
				return new RijndaelUnmanagedTransform(GetKeyType(rgbKey.Length * 8), CryptoMethod.Encrypt, rgbKey, rgbIV, Mode, FeedbackSize, Padding);
			}
		}
		catch
		{
		}
		return m_Managed.CreateEncryptor(rgbKey, rgbIV);
	}

	private CryptoAlgorithm GetKeyType(int size)
	{
		return size switch
		{
			128 => CryptoAlgorithm.Rijndael128, 
			192 => CryptoAlgorithm.Rijndael192, 
			256 => CryptoAlgorithm.Rijndael256, 
			_ => throw new CryptographicException("Invalid keysize!"), 
		};
	}

	private bool CanUseUnmanaged(int keySize, int blockSize, PaddingMode padding)
	{
		if (m_Provider != 0 && blockSize == 128 && (padding == PaddingMode.PKCS7 || padding == PaddingMode.None))
		{
			if (keySize != 128 && keySize != 192)
			{
				return keySize == 256;
			}
			return true;
		}
		return false;
	}

	private bool CanUseUnmanaged()
	{
		return CanUseUnmanaged(KeySize, BlockSize, Padding);
	}
}
