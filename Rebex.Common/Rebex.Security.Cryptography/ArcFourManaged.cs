using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography;

public class ArcFourManaged : ArcFour
{
	public ArcFourManaged()
		: this(skipFipsCheck: false)
	{
	}

	internal ArcFourManaged(bool skipFipsCheck)
	{
		if ((!skipFipsCheck || 1 == 0) && CryptoHelper.UseFipsAlgorithmsOnly && 0 == 0)
		{
			throw new CryptographicException("Algorithm not supported in FIPS-compliant mode.");
		}
	}

	public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
	{
		return new lzaxk(rgbKey);
	}

	public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
	{
		return CreateDecryptor(rgbKey, rgbIV);
	}
}
