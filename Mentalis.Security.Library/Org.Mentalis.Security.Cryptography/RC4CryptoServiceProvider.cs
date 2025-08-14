using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Org.Mentalis.Security.Cryptography;

public sealed class RC4CryptoServiceProvider : RC4
{
	private bool m_Disposed;

	private ARCFourManaged m_Managed;

	private int m_MaxLen;

	private int m_MinLen;

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

	public RC4CryptoServiceProvider()
	{
		m_Provider = CAPIProvider.Handle;
		if (m_Provider != 0)
		{
			int dwFlags = 1;
			bool flag = false;
			IntPtr intPtr = Marshal.AllocHGlobal(100);
			do
			{
				int pdwDataLen = 100;
				if (SspiProvider.CryptGetProvParam(m_Provider, 22, intPtr, ref pdwDataLen, dwFlags) == 0)
				{
					break;
				}
				dwFlags = 0;
				PROV_ENUMALGS_EX pROV_ENUMALGS_EX = (PROV_ENUMALGS_EX)Marshal.PtrToStructure(intPtr, typeof(PROV_ENUMALGS_EX));
				if (pROV_ENUMALGS_EX.aiAlgid == 26625)
				{
					flag = true;
					m_MinLen = pROV_ENUMALGS_EX.dwMinLen;
					m_MaxLen = pROV_ENUMALGS_EX.dwMaxLen;
				}
			}
			while (!flag);
			Marshal.FreeHGlobal(intPtr);
		}
		m_Managed = new ARCFourManaged();
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
		try
		{
			if (CanUseUnmanaged(rgbKey.Length * 8))
			{
				return new RC4UnmanagedTransform(rgbKey);
			}
		}
		catch
		{
		}
		return m_Managed.CreateDecryptor(rgbKey, rgbIV);
	}

	public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
	{
		return CreateDecryptor(rgbKey, rgbIV);
	}

	private bool CanUseUnmanaged(int keySize)
	{
		if (m_Provider != 0 && keySize >= m_MinLen)
		{
			return keySize <= m_MaxLen;
		}
		return false;
	}

	private new void Dispose()
	{
		if (!m_Disposed)
		{
			m_Disposed = true;
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
		}
	}

	~RC4CryptoServiceProvider()
	{
		Dispose();
	}
}
