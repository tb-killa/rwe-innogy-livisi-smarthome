namespace Rebex.Security.Cryptography;

public class SHA384Managed : SHA2Managed
{
	public SHA384Managed()
		: base(HashingAlgorithmId.SHA384)
	{
	}

	public new static SHA384Managed Create()
	{
		return new SHA384Managed();
	}
}
