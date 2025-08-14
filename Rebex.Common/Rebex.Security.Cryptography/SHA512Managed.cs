namespace Rebex.Security.Cryptography;

public class SHA512Managed : SHA2Managed
{
	public SHA512Managed()
		: base(HashingAlgorithmId.SHA512)
	{
	}

	public new static SHA512Managed Create()
	{
		return new SHA512Managed();
	}
}
