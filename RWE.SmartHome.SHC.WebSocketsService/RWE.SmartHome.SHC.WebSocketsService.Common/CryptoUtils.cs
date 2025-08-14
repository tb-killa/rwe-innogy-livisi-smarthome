using System.Security.Cryptography;

namespace RWE.SmartHome.SHC.WebSocketsService.Common;

public static class CryptoUtils
{
	public static byte[] GetRandomBytes(int length)
	{
		byte[] array = new byte[length];
		RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
		rNGCryptoServiceProvider.GetBytes(array);
		return array;
	}

	public static byte[] ComputeSha1Hash(byte[] input)
	{
		byte[] array = null;
		using HashAlgorithm hashAlgorithm = SHA1.Create();
		hashAlgorithm.Initialize();
		return hashAlgorithm.ComputeHash(input);
	}
}
