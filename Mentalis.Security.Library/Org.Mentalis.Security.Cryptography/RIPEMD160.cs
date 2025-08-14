using System.Security.Cryptography;

namespace Org.Mentalis.Security.Cryptography;

public abstract class RIPEMD160 : HashAlgorithm
{
	protected RIPEMD160()
	{
		HashSizeValue = 160;
	}

	public new static RIPEMD160 Create()
	{
		return Create("RIPEMD160");
	}

	public new static RIPEMD160 Create(string hashName)
	{
		try
		{
			if (hashName.ToUpper() == "RIPEMD160" || hashName.ToUpper() == "RIPEMD" || hashName.ToLower() == "org.mentalis.security.cryptography.ripemd160")
			{
				return new RIPEMD160Managed();
			}
		}
		catch
		{
		}
		return null;
	}
}
