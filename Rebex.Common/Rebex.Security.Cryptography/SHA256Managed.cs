namespace Rebex.Security.Cryptography;

public class SHA256Managed : SHA2Managed
{
	public SHA256Managed()
		: base(HashingAlgorithmId.SHA256)
	{
	}

	public new static SHA256Managed Create()
	{
		return new SHA256Managed();
	}
}
