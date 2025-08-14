using System.Security.Cryptography;

namespace onrkn;

internal class milge
{
	public const string xinpr = "Invalid rsa parameters.";

	public static void khiul(ref RSAParameters p0)
	{
		if (p0.Exponent == null || false || p0.Modulus == null)
		{
			throw new CryptographicException("Invalid rsa parameters.");
		}
	}
}
