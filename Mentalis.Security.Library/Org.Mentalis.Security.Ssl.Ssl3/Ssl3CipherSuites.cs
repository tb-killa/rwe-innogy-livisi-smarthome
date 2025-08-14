using System;
using System.Security.Cryptography;
using Org.Mentalis.Security.Ssl.Shared;

namespace Org.Mentalis.Security.Ssl.Ssl3;

internal sealed class Ssl3CipherSuites
{
	private Ssl3CipherSuites()
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
		Ssl3DeriveBytes ssl3DeriveBytes = new Ssl3DeriveBytes(master, clientrnd, serverrnd, clientServer: false);
		byte[] bytes = ssl3DeriveBytes.GetBytes(definition.HashSize);
		byte[] bytes2 = ssl3DeriveBytes.GetBytes(definition.HashSize);
		byte[] array = ssl3DeriveBytes.GetBytes(definition.BulkKeySize);
		byte[] array2 = ssl3DeriveBytes.GetBytes(definition.BulkKeySize);
		byte[] array3 = ssl3DeriveBytes.GetBytes(definition.BulkIVSize);
		byte[] array4 = ssl3DeriveBytes.GetBytes(definition.BulkIVSize);
		ssl3DeriveBytes.Dispose();
		if (definition.Exportable)
		{
			MD5 mD = new MD5CryptoServiceProvider();
			mD.TransformBlock(array, 0, array.Length, array, 0);
			mD.TransformBlock(clientrnd, 0, clientrnd.Length, clientrnd, 0);
			mD.TransformFinalBlock(serverrnd, 0, serverrnd.Length);
			array = new byte[definition.BulkExpandedSize];
			Array.Copy(mD.Hash, 0, array, 0, array.Length);
			mD.Initialize();
			mD.TransformBlock(array2, 0, array2.Length, array2, 0);
			mD.TransformBlock(serverrnd, 0, serverrnd.Length, serverrnd, 0);
			mD.TransformFinalBlock(clientrnd, 0, clientrnd.Length);
			array2 = new byte[definition.BulkExpandedSize];
			Array.Copy(mD.Hash, 0, array2, 0, array2.Length);
			mD.Initialize();
			mD.TransformBlock(clientrnd, 0, clientrnd.Length, clientrnd, 0);
			mD.TransformFinalBlock(serverrnd, 0, serverrnd.Length);
			array3 = new byte[definition.BulkIVSize];
			Array.Copy(mD.Hash, 0, array3, 0, array3.Length);
			mD.Initialize();
			mD.TransformBlock(serverrnd, 0, serverrnd.Length, serverrnd, 0);
			mD.TransformFinalBlock(clientrnd, 0, clientrnd.Length);
			array4 = new byte[definition.BulkIVSize];
			Array.Copy(mD.Hash, 0, array4, 0, array4.Length);
			mD.Clear();
		}
		if (entity == ConnectionEnd.Client)
		{
			cipherSuite.Encryptor = symmetricAlgorithm.CreateEncryptor(array, array3);
			cipherSuite.Decryptor = symmetricAlgorithm.CreateDecryptor(array2, array4);
			cipherSuite.LocalHasher = new Ssl3RecordMAC(definition.HashAlgorithmType, bytes);
			cipherSuite.RemoteHasher = new Ssl3RecordMAC(definition.HashAlgorithmType, bytes2);
		}
		else
		{
			cipherSuite.Encryptor = symmetricAlgorithm.CreateEncryptor(array2, array4);
			cipherSuite.Decryptor = symmetricAlgorithm.CreateDecryptor(array, array3);
			cipherSuite.LocalHasher = new Ssl3RecordMAC(definition.HashAlgorithmType, bytes2);
			cipherSuite.RemoteHasher = new Ssl3RecordMAC(definition.HashAlgorithmType, bytes);
		}
		Array.Clear(bytes, 0, bytes.Length);
		Array.Clear(bytes2, 0, bytes2.Length);
		Array.Clear(array, 0, array.Length);
		Array.Clear(array2, 0, array2.Length);
		Array.Clear(array3, 0, array3.Length);
		Array.Clear(array4, 0, array4.Length);
		return cipherSuite;
	}

	public static byte[] GenerateMasterSecret(byte[] premaster, byte[] clientRandom, byte[] serverRandom)
	{
		Ssl3DeriveBytes ssl3DeriveBytes = new Ssl3DeriveBytes(premaster, clientRandom, serverRandom, clientServer: true);
		byte[] bytes = ssl3DeriveBytes.GetBytes(48);
		ssl3DeriveBytes.Dispose();
		return bytes;
	}
}
