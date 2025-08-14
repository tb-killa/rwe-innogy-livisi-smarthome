using System;
using System.Security.Cryptography;
using Org.Mentalis.Security.Cryptography;
using Org.Mentalis.Security.Ssl.Shared;

namespace Org.Mentalis.Security.Ssl.Tls1;

internal sealed class Tls1CipherSuites
{
	private Tls1CipherSuites()
	{
	}

	public static CipherSuite InitializeCipherSuite(byte[] master, byte[] clientrnd, byte[] serverrnd, CipherDefinition definition, ConnectionEnd entity)
	{
		CipherSuite cipherSuite = new CipherSuite();
		SymmetricAlgorithm symmetricAlgorithm = (SymmetricAlgorithm)Activator.CreateInstance(definition.BulkCipherAlgorithm);
		if (definition.BulkIVSize > 0)
		{
			symmetricAlgorithm.Mode = CipherMode.CBC;
		}
		symmetricAlgorithm.Padding = PaddingMode.None;
		symmetricAlgorithm.BlockSize = definition.BulkIVSize * 8;
		byte[] array = new byte[64];
		Array.Copy(serverrnd, 0, array, 0, 32);
		Array.Copy(clientrnd, 0, array, 32, 32);
		PseudoRandomDeriveBytes pseudoRandomDeriveBytes = new PseudoRandomDeriveBytes(master, "key expansion", array);
		byte[] bytes = pseudoRandomDeriveBytes.GetBytes(definition.HashSize);
		byte[] bytes2 = pseudoRandomDeriveBytes.GetBytes(definition.HashSize);
		byte[] bytes3 = pseudoRandomDeriveBytes.GetBytes(definition.BulkKeySize);
		byte[] bytes4 = pseudoRandomDeriveBytes.GetBytes(definition.BulkKeySize);
		byte[] bytes5 = pseudoRandomDeriveBytes.GetBytes(definition.BulkIVSize);
		byte[] bytes6 = pseudoRandomDeriveBytes.GetBytes(definition.BulkIVSize);
		pseudoRandomDeriveBytes.Dispose();
		if (definition.Exportable)
		{
			Array.Copy(clientrnd, 0, array, 0, 32);
			Array.Copy(serverrnd, 0, array, 32, 32);
			pseudoRandomDeriveBytes = new PseudoRandomDeriveBytes(bytes3, "client write key", array);
			bytes3 = pseudoRandomDeriveBytes.GetBytes(definition.BulkExpandedSize);
			pseudoRandomDeriveBytes.Dispose();
			pseudoRandomDeriveBytes = new PseudoRandomDeriveBytes(bytes4, "server write key", array);
			bytes4 = pseudoRandomDeriveBytes.GetBytes(definition.BulkExpandedSize);
			pseudoRandomDeriveBytes.Dispose();
			pseudoRandomDeriveBytes = new PseudoRandomDeriveBytes(new byte[0], "IV block", array);
			bytes5 = pseudoRandomDeriveBytes.GetBytes(definition.BulkIVSize);
			bytes6 = pseudoRandomDeriveBytes.GetBytes(definition.BulkIVSize);
			pseudoRandomDeriveBytes.Dispose();
		}
		if (entity == ConnectionEnd.Client)
		{
			cipherSuite.Encryptor = symmetricAlgorithm.CreateEncryptor(bytes3, bytes5);
			cipherSuite.Decryptor = symmetricAlgorithm.CreateDecryptor(bytes4, bytes6);
			cipherSuite.LocalHasher = new Org.Mentalis.Security.Cryptography.HMAC((HashAlgorithm)Activator.CreateInstance(definition.HashAlgorithm), bytes);
			cipherSuite.RemoteHasher = new Org.Mentalis.Security.Cryptography.HMAC((HashAlgorithm)Activator.CreateInstance(definition.HashAlgorithm), bytes2);
		}
		else
		{
			cipherSuite.Encryptor = symmetricAlgorithm.CreateEncryptor(bytes4, bytes6);
			cipherSuite.Decryptor = symmetricAlgorithm.CreateDecryptor(bytes3, bytes5);
			cipherSuite.LocalHasher = new Org.Mentalis.Security.Cryptography.HMAC((HashAlgorithm)Activator.CreateInstance(definition.HashAlgorithm), bytes2);
			cipherSuite.RemoteHasher = new Org.Mentalis.Security.Cryptography.HMAC((HashAlgorithm)Activator.CreateInstance(definition.HashAlgorithm), bytes);
		}
		Array.Clear(bytes, 0, bytes.Length);
		Array.Clear(bytes2, 0, bytes2.Length);
		Array.Clear(bytes3, 0, bytes3.Length);
		Array.Clear(bytes4, 0, bytes4.Length);
		Array.Clear(bytes5, 0, bytes5.Length);
		Array.Clear(bytes6, 0, bytes6.Length);
		Array.Clear(array, 0, array.Length);
		return cipherSuite;
	}

	public static byte[] GenerateMasterSecret(byte[] premaster, byte[] clientRandom, byte[] serverRandom)
	{
		byte[] array = new byte[64];
		Array.Copy(clientRandom, 0, array, 0, 32);
		Array.Copy(serverRandom, 0, array, 32, 32);
		PseudoRandomDeriveBytes pseudoRandomDeriveBytes = new PseudoRandomDeriveBytes(premaster, "master secret", array);
		array = pseudoRandomDeriveBytes.GetBytes(48);
		pseudoRandomDeriveBytes.Dispose();
		return array;
	}
}
