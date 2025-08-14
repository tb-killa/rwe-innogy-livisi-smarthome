using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography;

public class MD5Managed : MD5
{
	private readonly vclqf ebqvw;

	internal IHashTransform uhvte()
	{
		return ebqvw;
	}

	public MD5Managed()
		: this(skipFipsCheck: false)
	{
	}

	internal MD5Managed(bool skipFipsCheck)
	{
		if ((!skipFipsCheck || 1 == 0) && CryptoHelper.UseFipsAlgorithmsOnly && 0 == 0)
		{
			throw new CryptographicException("Algorithm not supported in FIPS-compliant mode.");
		}
		HashSizeValue = 128;
		ebqvw = new mwoks();
	}

	public override void Initialize()
	{
		ebqvw.Reset();
	}

	protected override void HashCore(byte[] buffer, int offset, int count)
	{
		ebqvw.Process(buffer, offset, count);
	}

	protected override byte[] HashFinal()
	{
		return HashValue = ebqvw.GetHash();
	}
}
