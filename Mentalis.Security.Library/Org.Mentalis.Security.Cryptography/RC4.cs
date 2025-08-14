using System.Security.Cryptography;

namespace Org.Mentalis.Security.Cryptography;

public abstract class RC4 : SymmetricAlgorithm
{
	private RNGCryptoServiceProvider m_RNG;

	public override int BlockSize
	{
		get
		{
			return 8;
		}
		set
		{
			if (value != 8 && value != 0)
			{
				throw new CryptographicException("RC4 is a stream cipher, not a block cipher.");
			}
		}
	}

	public override int FeedbackSize
	{
		get
		{
			throw new CryptographicException("RC4 doesn't use the feedback size property.");
		}
		set
		{
			throw new CryptographicException("RC4 doesn't use the feedback size property.");
		}
	}

	public override byte[] IV
	{
		get
		{
			return new byte[1];
		}
		set
		{
			if (value != null && value.Length > 1)
			{
				throw new CryptographicException("RC4 doesn't use an Initialization Vector.");
			}
		}
	}

	public override KeySizes[] LegalBlockSizes => new KeySizes[1]
	{
		new KeySizes(8, 8, 0)
	};

	public override KeySizes[] LegalKeySizes => new KeySizes[1]
	{
		new KeySizes(8, 2048, 8)
	};

	public override CipherMode Mode
	{
		get
		{
			return CipherMode.OFB;
		}
		set
		{
			if (value != CipherMode.OFB)
			{
				throw new CryptographicException("RC4 only supports OFB.");
			}
		}
	}

	public override PaddingMode Padding
	{
		get
		{
			return PaddingMode.None;
		}
		set
		{
			if (value != PaddingMode.None)
			{
				throw new CryptographicException("RC4 is a stream cipher, not a block cipher.");
			}
		}
	}

	public RC4()
	{
		KeySizeValue = 128;
	}

	public override void GenerateIV()
	{
	}

	public override void GenerateKey()
	{
		byte[] array = new byte[KeySize / 8];
		GetRNGCSP().GetBytes(array);
		Key = array;
	}

	public new static RC4 Create()
	{
		return Create("ARCFOUR");
	}

	public new static RC4 Create(string AlgName)
	{
		try
		{
			if (AlgName.ToUpper() == "RC4" || AlgName.ToLower() == "org.mentalis.security.cryptography.rc4cryptoserviceprovider")
			{
				return new RC4CryptoServiceProvider();
			}
			if (AlgName.ToUpper() == "ARCFOUR" || AlgName.ToLower() == "org.mentalis.security.cryptography.arcfourmanaged")
			{
				return new ARCFourManaged();
			}
		}
		catch
		{
		}
		return null;
	}

	protected RNGCryptoServiceProvider GetRNGCSP()
	{
		if (m_RNG == null)
		{
			m_RNG = new RNGCryptoServiceProvider();
		}
		return m_RNG;
	}
}
