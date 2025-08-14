using System.Security.Cryptography;

namespace Rebex.Security.Cryptography;

public abstract class ArcFour : SymmetricAlgorithm
{
	public override int BlockSize
	{
		get
		{
			return 8;
		}
		set
		{
			if (value != 8 && value != 0 && 0 == 0)
			{
				throw new CryptographicException("ArcFour is not a block cipher.");
			}
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
			if (value != null && 0 == 0 && value.Length > 1)
			{
				throw new CryptographicException("ArcFour does not use IV.");
			}
		}
	}

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
				throw new CryptographicException("Unsupported cipher mode.");
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
				throw new CryptographicException("ArcFour is not a block cipher.");
			}
		}
	}

	protected ArcFour()
	{
		KeySizeValue = 256;
		BlockSizeValue = 8;
		LegalBlockSizesValue = new KeySizes[1]
		{
			new KeySizes(8, 8, 0)
		};
		LegalKeySizesValue = new KeySizes[1]
		{
			new KeySizes(8, 2048, 8)
		};
	}

	public override void GenerateIV()
	{
	}

	public override void GenerateKey()
	{
		KeyValue = CryptoHelper.GetRandomBytes(KeySizeValue >> 3);
	}

	public new static ArcFour Create()
	{
		return new ArcFourManaged();
	}
}
