using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography;

public class ArcTwoManaged : RC2
{
	public override CipherMode Mode
	{
		get
		{
			return ModeValue;
		}
		set
		{
			switch (value)
			{
			default:
				throw new CryptographicException(brgjd.edcru("Mode {0} not supported.", value));
			case CipherMode.CBC:
			case CipherMode.ECB:
				ModeValue = value;
				break;
			}
		}
	}

	public override PaddingMode Padding
	{
		get
		{
			return PaddingValue;
		}
		set
		{
			switch (value)
			{
			default:
				throw new CryptographicException(brgjd.edcru("Padding mode {0} not supported.", value));
			case PaddingMode.None:
			case PaddingMode.PKCS7:
			case PaddingMode.Zeros:
				PaddingValue = value;
				break;
			}
		}
	}

	public ArcTwoManaged()
		: this(skipFipsCheck: false)
	{
	}

	internal ArcTwoManaged(bool skipFipsCheck)
	{
		if ((!skipFipsCheck || 1 == 0) && CryptoHelper.UseFipsAlgorithmsOnly && 0 == 0)
		{
			throw new CryptographicException("Algorithm not supported in FIPS-compliant mode.");
		}
	}

	public override void GenerateIV()
	{
		IVValue = CryptoHelper.GetRandomBytes(BlockSizeValue >> 3);
	}

	public override void GenerateKey()
	{
		KeyValue = CryptoHelper.GetRandomBytes(KeySizeValue >> 3);
	}

	public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
	{
		CryptoHelper.hzwkb(ref rgbKey, ref rgbIV, KeySizeValue, BlockSizeValue, ModeValue, LegalKeySizesValue);
		if (EffectiveKeySizeValue > KeySizeValue)
		{
			throw new CryptographicException("Invalid effective key size.");
		}
		if (EffectiveKeySizeValue == 0 || 1 == 0)
		{
			EffectiveKeySizeValue = KeySizeValue;
		}
		return new gpvmk(new pucov(rgbKey, EffectiveKeySizeValue), rgbIV, ModeValue, PaddingValue, encrypt: true);
	}

	public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
	{
		CryptoHelper.hzwkb(ref rgbKey, ref rgbIV, KeySizeValue, BlockSizeValue, ModeValue, LegalKeySizesValue);
		if (EffectiveKeySizeValue > KeySizeValue)
		{
			throw new CryptographicException("Invalid effective key size.");
		}
		if (EffectiveKeySizeValue == 0 || 1 == 0)
		{
			EffectiveKeySizeValue = KeySizeValue;
		}
		return new gpvmk(new pucov(rgbKey, EffectiveKeySizeValue), rgbIV, ModeValue, PaddingValue, encrypt: false);
	}
}
