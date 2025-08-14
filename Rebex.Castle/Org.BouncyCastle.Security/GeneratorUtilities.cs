using System.Collections;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Security;

public sealed class GeneratorUtilities
{
	private static readonly IDictionary kgAlgorithms;

	private static readonly IDictionary kpgAlgorithms;

	private static readonly IDictionary defaultKeySizes;

	private GeneratorUtilities()
	{
	}

	static GeneratorUtilities()
	{
		kgAlgorithms = Platform.CreateHashtable();
		kpgAlgorithms = Platform.CreateHashtable();
		defaultKeySizes = Platform.CreateHashtable();
		AddKpgAlgorithm("EC", X9ObjectIdentifiers.DHSinglePassStdDHSha1KdfScheme);
		AddKpgAlgorithm("ECDH", "ECIES");
	}

	private static void AddDefaultKeySizeEntries(int size, params string[] algorithms)
	{
		foreach (string key in algorithms)
		{
			defaultKeySizes.Add(key, size);
		}
	}

	private static void AddKgAlgorithm(string canonicalName, params object[] aliases)
	{
		kgAlgorithms[canonicalName] = canonicalName;
		foreach (object obj in aliases)
		{
			kgAlgorithms[obj.ToString()] = canonicalName;
		}
	}

	private static void AddKpgAlgorithm(string canonicalName, params object[] aliases)
	{
		kpgAlgorithms[canonicalName] = canonicalName;
		foreach (object obj in aliases)
		{
			kpgAlgorithms[obj.ToString()] = canonicalName;
		}
	}

	internal static string GetCanonicalKeyGeneratorAlgorithm(string algorithm)
	{
		return (string)kgAlgorithms[Platform.ToUpperInvariant(algorithm)];
	}

	internal static string GetCanonicalKeyPairGeneratorAlgorithm(string algorithm)
	{
		return (string)kpgAlgorithms[Platform.ToUpperInvariant(algorithm)];
	}

	public static IAsymmetricCipherKeyPairGenerator GetKeyPairGenerator(DerObjectIdentifier oid)
	{
		return GetKeyPairGenerator(oid.Id);
	}

	public static IAsymmetricCipherKeyPairGenerator GetKeyPairGenerator(string algorithm)
	{
		string canonicalKeyPairGeneratorAlgorithm = GetCanonicalKeyPairGeneratorAlgorithm(algorithm);
		if (canonicalKeyPairGeneratorAlgorithm == null)
		{
			throw new SecurityUtilityException("KeyPairGenerator " + algorithm + " not recognised.");
		}
		if (Platform.StartsWith(canonicalKeyPairGeneratorAlgorithm, "EC"))
		{
			return new ECKeyPairGenerator(canonicalKeyPairGeneratorAlgorithm);
		}
		throw new SecurityUtilityException("KeyPairGenerator " + algorithm + " (" + canonicalKeyPairGeneratorAlgorithm + ") not supported.");
	}

	internal static int GetDefaultKeySize(DerObjectIdentifier oid)
	{
		return GetDefaultKeySize(oid.Id);
	}

	internal static int GetDefaultKeySize(string algorithm)
	{
		string canonicalKeyGeneratorAlgorithm = GetCanonicalKeyGeneratorAlgorithm(algorithm);
		if (canonicalKeyGeneratorAlgorithm == null)
		{
			throw new SecurityUtilityException("KeyGenerator " + algorithm + " not recognised.");
		}
		int num = FindDefaultKeySize(canonicalKeyGeneratorAlgorithm);
		if (num == -1)
		{
			throw new SecurityUtilityException("KeyGenerator " + algorithm + " (" + canonicalKeyGeneratorAlgorithm + ") not supported.");
		}
		return num;
	}

	private static int FindDefaultKeySize(string canonicalName)
	{
		if (!defaultKeySizes.Contains(canonicalName))
		{
			return -1;
		}
		return (int)defaultKeySizes[canonicalName];
	}
}
