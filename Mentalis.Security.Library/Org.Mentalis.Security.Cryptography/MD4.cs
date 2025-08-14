using System.Security.Cryptography;

namespace Org.Mentalis.Security.Cryptography;

public abstract class MD4 : HashAlgorithm
{
	public MD4()
	{
		HashSizeValue = 128;
	}

	public new static MD4 Create()
	{
		return Create("MD4");
	}

	public new static MD4 Create(string hashName)
	{
		try
		{
			if (hashName.ToUpper() == "MD4" || hashName.ToLower() == "org.mentalis.security.cryptography.md4cryptoserviceprovider")
			{
				return new MD4CryptoServiceProvider();
			}
		}
		catch
		{
		}
		return null;
	}
}
