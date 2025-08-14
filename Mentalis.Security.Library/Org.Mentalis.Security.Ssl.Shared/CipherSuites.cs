using System.IO;
using System.Security.Cryptography;
using Org.Mentalis.Security.Cryptography;
using Org.Mentalis.Security.Ssl.Ssl3;
using Org.Mentalis.Security.Ssl.Tls1;

namespace Org.Mentalis.Security.Ssl.Shared;

internal sealed class CipherSuites
{
	private static CipherDefinition[] Definitions = new CipherDefinition[9]
	{
		new CipherDefinition(SslAlgorithms.RSA_RC4_128_MD5, typeof(ARCFourManaged), 16, 0, 16, typeof(MD5CryptoServiceProvider), HashType.MD5, 16, exportable: false),
		new CipherDefinition(SslAlgorithms.RSA_RC4_128_SHA, typeof(ARCFourManaged), 16, 0, 16, typeof(SHA1CryptoServiceProvider), HashType.SHA1, 20, exportable: false),
		new CipherDefinition(SslAlgorithms.RSA_RC4_40_MD5, typeof(ARCFourManaged), 5, 0, 16, typeof(MD5CryptoServiceProvider), HashType.MD5, 16, exportable: true),
		new CipherDefinition(SslAlgorithms.RSA_RC2_40_MD5, typeof(RC2CryptoServiceProvider), 5, 8, 16, typeof(MD5CryptoServiceProvider), HashType.MD5, 16, exportable: true),
		new CipherDefinition(SslAlgorithms.RSA_DES_56_SHA, typeof(DESCryptoServiceProvider), 8, 8, 8, typeof(SHA1CryptoServiceProvider), HashType.SHA1, 20, exportable: false),
		new CipherDefinition(SslAlgorithms.RSA_3DES_168_SHA, typeof(TripleDESCryptoServiceProvider), 24, 8, 24, typeof(SHA1CryptoServiceProvider), HashType.SHA1, 20, exportable: false),
		new CipherDefinition(SslAlgorithms.RSA_DES_40_SHA, typeof(DESCryptoServiceProvider), 5, 8, 8, typeof(SHA1CryptoServiceProvider), HashType.SHA1, 20, exportable: true),
		new CipherDefinition(SslAlgorithms.RSA_AES_128_SHA, typeof(RijndaelManaged), 16, 16, 16, typeof(SHA1CryptoServiceProvider), HashType.SHA1, 20, exportable: false),
		new CipherDefinition(SslAlgorithms.RSA_AES_256_SHA, typeof(RijndaelManaged), 32, 16, 32, typeof(SHA1CryptoServiceProvider), HashType.SHA1, 20, exportable: false)
	};

	private CipherSuites()
	{
	}

	public static SslAlgorithms GetCipherAlgorithmType(byte[] buffer, int offset)
	{
		if (buffer.Length < offset + 2)
		{
			throw new SslException(AlertDescription.InternalError, "Buffer overflow in GetCipherAlgorithm.");
		}
		byte b = buffer[offset];
		byte b2 = buffer[offset + 1];
		if (b == 0 && b2 == 0)
		{
			return SslAlgorithms.NONE;
		}
		if (b == 0 && b2 == 5)
		{
			return SslAlgorithms.RSA_RC4_128_SHA;
		}
		if (b == 0 && b2 == 4)
		{
			return SslAlgorithms.RSA_RC4_128_MD5;
		}
		if (b == 0 && b2 == 3)
		{
			return SslAlgorithms.RSA_RC4_40_MD5;
		}
		if (b == 0 && b2 == 6)
		{
			return SslAlgorithms.RSA_RC2_40_MD5;
		}
		if (b == 0 && b2 == 9)
		{
			return SslAlgorithms.RSA_DES_56_SHA;
		}
		if (b == 0 && b2 == 10)
		{
			return SslAlgorithms.RSA_3DES_168_SHA;
		}
		if (b == 0 && b2 == 8)
		{
			return SslAlgorithms.RSA_DES_40_SHA;
		}
		if (b == 0 && b2 == 47)
		{
			return SslAlgorithms.RSA_AES_128_SHA;
		}
		if (b == 0 && b2 == 53)
		{
			return SslAlgorithms.RSA_AES_256_SHA;
		}
		return SslAlgorithms.NONE;
	}

	public static byte[] GetCipherAlgorithmBytes(SslAlgorithms algorithm)
	{
		MemoryStream memoryStream = new MemoryStream();
		if ((algorithm & SslAlgorithms.RSA_AES_256_SHA) != SslAlgorithms.NONE)
		{
			memoryStream.Write(new byte[2] { 0, 53 }, 0, 2);
		}
		if ((algorithm & SslAlgorithms.RSA_AES_128_SHA) != SslAlgorithms.NONE)
		{
			memoryStream.Write(new byte[2] { 0, 47 }, 0, 2);
		}
		if ((algorithm & SslAlgorithms.RSA_RC4_128_SHA) != SslAlgorithms.NONE)
		{
			memoryStream.Write(new byte[2] { 0, 5 }, 0, 2);
		}
		if ((algorithm & SslAlgorithms.RSA_RC4_128_MD5) != SslAlgorithms.NONE)
		{
			memoryStream.Write(new byte[2] { 0, 4 }, 0, 2);
		}
		if ((algorithm & SslAlgorithms.RSA_3DES_168_SHA) != SslAlgorithms.NONE)
		{
			memoryStream.Write(new byte[2] { 0, 10 }, 0, 2);
		}
		if ((algorithm & SslAlgorithms.RSA_DES_56_SHA) != SslAlgorithms.NONE)
		{
			memoryStream.Write(new byte[2] { 0, 9 }, 0, 2);
		}
		if ((algorithm & SslAlgorithms.RSA_RC4_40_MD5) != SslAlgorithms.NONE)
		{
			memoryStream.Write(new byte[2] { 0, 3 }, 0, 2);
		}
		if ((algorithm & SslAlgorithms.RSA_RC2_40_MD5) != SslAlgorithms.NONE)
		{
			memoryStream.Write(new byte[2] { 0, 6 }, 0, 2);
		}
		if ((algorithm & SslAlgorithms.RSA_DES_40_SHA) != SslAlgorithms.NONE)
		{
			memoryStream.Write(new byte[2] { 0, 8 }, 0, 2);
		}
		return memoryStream.ToArray();
	}

	public static SslAlgorithms GetCipherSuiteAlgorithm(byte[] algorithms, SslAlgorithms allowed)
	{
		for (int i = 0; i < algorithms.Length; i += 2)
		{
			SslAlgorithms cipherAlgorithmType = GetCipherAlgorithmType(algorithms, i);
			if ((cipherAlgorithmType & allowed) != SslAlgorithms.NONE)
			{
				return cipherAlgorithmType;
			}
		}
		throw new SslException(AlertDescription.HandshakeFailure, "No encryption scheme matches the available schemes.");
	}

	public static CipherSuite GetCipherSuite(SecureProtocol protocol, byte[] master, byte[] clientrnd, byte[] serverrnd, SslAlgorithms scheme, ConnectionEnd entity)
	{
		for (int i = 0; i < Definitions.Length; i++)
		{
			if (Definitions[i].Scheme == scheme)
			{
				switch (protocol)
				{
				case SecureProtocol.Tls1:
					return Tls1CipherSuites.InitializeCipherSuite(master, clientrnd, serverrnd, Definitions[i], entity);
				case SecureProtocol.Ssl3:
					return Ssl3CipherSuites.InitializeCipherSuite(master, clientrnd, serverrnd, Definitions[i], entity);
				}
			}
		}
		throw new SslException(AlertDescription.IllegalParameter, "The cipher suite is unknown.");
	}

	public static CipherDefinition GetCipherDefinition(SslAlgorithms scheme)
	{
		for (int i = 0; i < Definitions.Length; i++)
		{
			if (Definitions[i].Scheme == scheme)
			{
				return Definitions[i];
			}
		}
		throw new SslException(AlertDescription.IllegalParameter, "The cipher suite is unknown.");
	}
}
