using System.Security.Cryptography;

namespace Org.Mentalis.Security.Cryptography;

public abstract class MD2 : HashAlgorithm
{
	public MD2()
	{
		HashSizeValue = 128;
	}

	public new static MD2 Create()
	{
		return Create("MD2");
	}

	public new static MD2 Create(string hashName)
	{
		try
		{
			if (hashName.ToUpper() == "MD2" || hashName.ToLower() == "org.mentalis.security.cryptography.md2cryptoserviceprovider")
			{
				return new MD2CryptoServiceProvider();
			}
		}
		catch
		{
		}
		return null;
	}
}
