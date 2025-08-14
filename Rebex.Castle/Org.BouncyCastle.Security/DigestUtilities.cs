using System;
using System.Collections;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Security;

public sealed class DigestUtilities
{
	private enum DigestAlgorithm
	{
		GOST3411,
		KECCAK_224,
		KECCAK_256,
		KECCAK_288,
		KECCAK_384,
		KECCAK_512,
		MD2,
		MD4,
		MD5,
		RIPEMD128,
		RIPEMD160,
		RIPEMD256,
		RIPEMD320,
		SHA_1,
		SHA_224,
		SHA_256,
		SHA_384,
		SHA_512,
		SHA_512_224,
		SHA_512_256,
		SHA3_224,
		SHA3_256,
		SHA3_384,
		SHA3_512,
		SHAKE128,
		SHAKE256,
		TIGER,
		WHIRLPOOL
	}

	private static readonly IDictionary algorithms;

	private static readonly IDictionary oids;

	public static ICollection Algorithms => oids.Keys;

	private DigestUtilities()
	{
	}

	static DigestUtilities()
	{
		algorithms = Platform.CreateHashtable();
		oids = Platform.CreateHashtable();
		((DigestAlgorithm)(object)Enums.GetArbitraryValue(typeof(DigestAlgorithm))).ToString();
		algorithms["SHA224"] = "SHA-224";
		algorithms[NistObjectIdentifiers.IdSha224.Id] = "SHA-224";
		algorithms["SHA256"] = "SHA-256";
		algorithms[NistObjectIdentifiers.IdSha256.Id] = "SHA-256";
		algorithms["SHA384"] = "SHA-384";
		algorithms[NistObjectIdentifiers.IdSha384.Id] = "SHA-384";
		algorithms["SHA512"] = "SHA-512";
		algorithms[NistObjectIdentifiers.IdSha512.Id] = "SHA-512";
		algorithms["SHA512/224"] = "SHA-512/224";
		algorithms[NistObjectIdentifiers.IdSha512_224.Id] = "SHA-512/224";
		algorithms["SHA512/256"] = "SHA-512/256";
		algorithms[NistObjectIdentifiers.IdSha512_256.Id] = "SHA-512/256";
		oids["SHA-224"] = NistObjectIdentifiers.IdSha224;
		oids["SHA-256"] = NistObjectIdentifiers.IdSha256;
		oids["SHA-384"] = NistObjectIdentifiers.IdSha384;
		oids["SHA-512"] = NistObjectIdentifiers.IdSha512;
		oids["SHA-512/224"] = NistObjectIdentifiers.IdSha512_224;
		oids["SHA-512/256"] = NistObjectIdentifiers.IdSha512_256;
	}

	public static DerObjectIdentifier GetObjectIdentifier(string mechanism)
	{
		if (mechanism == null)
		{
			throw new ArgumentNullException("mechanism");
		}
		mechanism = Platform.ToUpperInvariant(mechanism);
		string text = (string)algorithms[mechanism];
		if (text != null)
		{
			mechanism = text;
		}
		return (DerObjectIdentifier)oids[mechanism];
	}

	public static IDigest GetDigest(DerObjectIdentifier id)
	{
		return GetDigest(id.Id);
	}

	public static IDigest GetDigest(string algorithm)
	{
		string text = Platform.ToUpperInvariant(algorithm);
		string text2 = (string)algorithms[text];
		if (text2 == null)
		{
			text2 = text;
		}
		try
		{
			switch ((DigestAlgorithm)(object)Enums.GetEnumValue(typeof(DigestAlgorithm), text2))
			{
			case DigestAlgorithm.SHA_256:
				return new Sha256Digest();
			case DigestAlgorithm.SHA_384:
				return new Sha384Digest();
			case DigestAlgorithm.SHA_512:
				return new Sha512Digest();
			}
		}
		catch (ArgumentException)
		{
		}
		throw new SecurityUtilityException("Digest " + text2 + " not recognised.");
	}

	public static string GetAlgorithmName(DerObjectIdentifier oid)
	{
		return (string)algorithms[oid.Id];
	}

	public static byte[] CalculateDigest(string algorithm, byte[] input)
	{
		IDigest digest = GetDigest(algorithm);
		digest.BlockUpdate(input, 0, input.Length);
		return DoFinal(digest);
	}

	public static byte[] DoFinal(IDigest digest)
	{
		byte[] array = new byte[digest.GetDigestSize()];
		digest.DoFinal(array, 0);
		return array;
	}

	public static byte[] DoFinal(IDigest digest, byte[] input)
	{
		digest.BlockUpdate(input, 0, input.Length);
		return DoFinal(digest);
	}
}
