using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography;

public abstract class Blowfish : SymmetricAlgorithm
{
	protected Blowfish()
	{
		KeySizeValue = 256;
		BlockSizeValue = 64;
		LegalKeySizesValue = fficc.jrsdd();
		LegalBlockSizesValue = fficc.ilnej();
	}

	public new static Blowfish Create()
	{
		return new BlowfishManaged();
	}
}
