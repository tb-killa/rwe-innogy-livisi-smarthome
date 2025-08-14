using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography;

public abstract class Twofish : SymmetricAlgorithm
{
	protected Twofish()
	{
		KeySizeValue = 192;
		BlockSizeValue = 128;
		LegalKeySizesValue = shxuy.pmfgl();
		LegalBlockSizesValue = shxuy.kkcam();
	}

	public new static Twofish Create()
	{
		return new TwofishManaged();
	}
}
