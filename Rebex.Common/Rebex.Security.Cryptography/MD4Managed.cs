using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography;

public class MD4Managed : HashAlgorithm
{
	private readonly vclqf hwotn;

	internal IHashTransform hkaul()
	{
		return hwotn;
	}

	public MD4Managed()
		: this(skipFipsCheck: false)
	{
	}

	internal MD4Managed(bool skipFipsCheck)
	{
		if ((!skipFipsCheck || 1 == 0) && CryptoHelper.UseFipsAlgorithmsOnly && 0 == 0)
		{
			throw new CryptographicException("Algorithm not supported in FIPS-compliant mode.");
		}
		HashSizeValue = 128;
		hwotn = new mjsvv();
	}

	public override void Initialize()
	{
		hwotn.Reset();
	}

	protected override void HashCore(byte[] buffer, int offset, int count)
	{
		hwotn.Process(buffer, offset, count);
	}

	protected override byte[] HashFinal()
	{
		return HashValue = hwotn.GetHash();
	}
}
