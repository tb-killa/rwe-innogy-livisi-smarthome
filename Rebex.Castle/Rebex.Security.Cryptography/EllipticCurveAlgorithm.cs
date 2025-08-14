using System;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.EC;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;

namespace Rebex.Security.Cryptography;

public class EllipticCurveAlgorithm
{
	public const string EcDsaSha2Nistp256 = "ecdsa-sha2-nistp256";

	public const string EcDsaSha2Nistp384 = "ecdsa-sha2-nistp384";

	public const string EcDsaSha2Nistp521 = "ecdsa-sha2-nistp521";

	public const string EcdhSha2BrainpoolP256R1 = "ecdh-sha2-brainpoolp256r1";

	public const string EcdhSha2BrainpoolP384R1 = "ecdh-sha2-brainpoolp384r1";

	public const string EcdhSha2BrainpoolP512R1 = "ecdh-sha2-brainpoolp512r1";

	public const string EcdhSha2Nistp256 = "ecdh-sha2-nistp256";

	public const string EcdhSha2Nistp384 = "ecdh-sha2-nistp384";

	public const string EcdhSha2Nistp521 = "ecdh-sha2-nistp521";

	private readonly ECKeyParametersExt _info;

	private int _003CBitLength_003Ek__BackingField;

	private string _003CCurveName_003Ek__BackingField;

	private ECPrivateKeyParameters _003CPrivateKey_003Ek__BackingField;

	private ECPublicKeyParameters _003CPublicKey_003Ek__BackingField;

	private string _003CSignatureAlgorithm_003Ek__BackingField;

	internal int BitLength
	{
		get
		{
			return _003CBitLength_003Ek__BackingField;
		}
		private set
		{
			_003CBitLength_003Ek__BackingField = value;
		}
	}

	public string CurveName
	{
		get
		{
			return _003CCurveName_003Ek__BackingField;
		}
		private set
		{
			_003CCurveName_003Ek__BackingField = value;
		}
	}

	public virtual string Name => CurveName;

	internal ECPrivateKeyParameters PrivateKey
	{
		get
		{
			return _003CPrivateKey_003Ek__BackingField;
		}
		private set
		{
			_003CPrivateKey_003Ek__BackingField = value;
		}
	}

	internal ECPublicKeyParameters PublicKey
	{
		get
		{
			return _003CPublicKey_003Ek__BackingField;
		}
		private set
		{
			_003CPublicKey_003Ek__BackingField = value;
		}
	}

	internal string SignatureAlgorithm
	{
		get
		{
			return _003CSignatureAlgorithm_003Ek__BackingField;
		}
		private set
		{
			_003CSignatureAlgorithm_003Ek__BackingField = value;
		}
	}

	protected EllipticCurveAlgorithm(string oid, string curveName)
	{
		if (curveName == null)
		{
			throw new ArgumentNullException("curveName");
		}
		if (oid == null)
		{
			throw new ArgumentNullException("oid");
		}
		CurveName = curveName;
		X9ECParameters byName = CustomNamedCurves.GetByName(oid);
		if (byName != null)
		{
			_info = new ECKeyParametersExt(byName);
		}
		else
		{
			DerObjectIdentifier curve;
			try
			{
				curve = new DerObjectIdentifier(oid);
			}
			catch (FormatException)
			{
				throw new InvalidOperationException("Unknown curve: '" + oid + "'.");
			}
			_info = new ECKeyParametersExt(curve);
		}
		BitLength = (string.Equals(oid, "curve25519", StringComparison.OrdinalIgnoreCase) ? 256 : _info.Parameters.N.BitLength);
		if (BitLength <= 256)
		{
			SignatureAlgorithm = "SHA-256withECDSA";
		}
		else if (BitLength <= 384)
		{
			SignatureAlgorithm = "SHA-384withECDSA";
		}
		else
		{
			SignatureAlgorithm = "SHA-512withECDSA";
		}
	}

	private EllipticCurveAlgorithm(EllipticCurveAlgorithm obj)
	{
		_info = obj._info;
		BitLength = obj.BitLength;
		SignatureAlgorithm = obj.SignatureAlgorithm;
	}

	public static EllipticCurveAlgorithm Create(string algName)
	{
		return algName.ToLower() switch
		{
			"ecdsa-sha2-nistp256" => new EllipticCurveDsa("1.2.840.10045.3.1.7", "nistp256"), 
			"ecdsa-sha2-nistp384" => new EllipticCurveDsa("1.3.132.0.34", "nistp384"), 
			"ecdsa-sha2-nistp521" => new EllipticCurveDsa("1.3.132.0.35", "nistp521"), 
			"ecdh-sha2-brainpoolp256r1" => new EllipticCurveDiffieHellman("1.3.36.3.3.2.8.1.1.7", "brainpoolp256r1"), 
			"ecdh-sha2-brainpoolp384r1" => new EllipticCurveDiffieHellman("1.3.36.3.3.2.8.1.1.11", "brainpoolp384r1"), 
			"ecdh-sha2-brainpoolp512r1" => new EllipticCurveDiffieHellman("1.3.36.3.3.2.8.1.1.13", "brainpoolp512r1"), 
			"ecdh-sha2-nistp256" => new EllipticCurveDiffieHellman("1.2.840.10045.3.1.7", "nistp256"), 
			"ecdh-sha2-nistp384" => new EllipticCurveDiffieHellman("1.3.132.0.34", "nistp384"), 
			"ecdh-sha2-nistp521" => new EllipticCurveDiffieHellman("1.3.132.0.35", "nistp521"), 
			_ => null, 
		};
	}

	internal EllipticCurveAlgorithm CreateBase()
	{
		return new EllipticCurveAlgorithm(this);
	}

	internal void EnsurePublic()
	{
		if (PublicKey == null)
		{
			IAsymmetricCipherKeyPairGenerator keyPairGenerator = GeneratorUtilities.GetKeyPairGenerator("ECDH");
			keyPairGenerator.Init(new ECKeyGenerationParameters(_info.Parameters, new SecureRandom()));
			AsymmetricCipherKeyPair asymmetricCipherKeyPair = keyPairGenerator.GenerateKeyPair();
			PublicKey = (ECPublicKeyParameters)asymmetricCipherKeyPair.Public;
			PrivateKey = (ECPrivateKeyParameters)asymmetricCipherKeyPair.Private;
		}
	}

	internal void EnsurePrivate()
	{
		EnsurePublic();
		if (PrivateKey == null)
		{
			throw new InvalidOperationException("Private key not available.");
		}
	}

	public void FromPublicKey(byte[] publicKey)
	{
		if (publicKey == null)
		{
			throw new ArgumentNullException("publicKey");
		}
		if (publicKey.Length == 0)
		{
			throw new InvalidOperationException("Invalid EC key.");
		}
		if (publicKey[0] != 4)
		{
			throw new InvalidOperationException("EC point compression not supported.");
		}
		if ((publicKey.Length & 1) != 1)
		{
			throw new InvalidOperationException("Unsupported EC key.");
		}
		int num = publicKey.Length / 2;
		if ((BitLength + 7) / 8 != num)
		{
			throw new InvalidOperationException("Unexpected EC key bit length.");
		}
		byte[] array = new byte[num];
		byte[] array2 = new byte[num];
		Array.Copy(publicKey, 1, array, 0, num);
		Array.Copy(publicKey, 1 + num, array2, 0, num);
		ECCurve curve = _info.Parameters.Curve;
		ECFieldElement x = curve.FromBigInteger(new BigInteger(1, array));
		ECFieldElement y = curve.FromBigInteger(new BigInteger(1, array2));
		ECPoint q = new FpPoint(curve, x, y);
		PublicKey = new ECPublicKeyParameters(_info.AlgorithmName, q, _info.Parameters);
		PrivateKey = null;
	}

	public void FromPrivateKey(byte[] privateKey)
	{
		if (privateKey == null)
		{
			throw new ArgumentNullException("privateKey");
		}
		if (privateKey.Length == 0)
		{
			throw new InvalidOperationException("Invalid EC key.");
		}
		if (!(Asn1Object.FromByteArray(privateKey) is Asn1Sequence obj))
		{
			throw new InvalidOperationException("Unsupported EC key.");
		}
		ECPrivateKeyStructure instance = ECPrivateKeyStructure.GetInstance(obj);
		BigInteger key = instance.GetKey();
		FromPublicKey(instance.GetPublicKey().GetBytes());
		PrivateKey = new ECPrivateKeyParameters(_info.AlgorithmName, key, _info.Parameters);
	}

	public byte[] GetPrivateKey()
	{
		EnsurePrivate();
		DerBitString publicKey = new DerBitString(PublicKey.Q.GetEncoded());
		ECPrivateKeyStructure eCPrivateKeyStructure = new ECPrivateKeyStructure(BitLength, PrivateKey.D, publicKey, PublicKey.PublicKeyParamSet);
		return eCPrivateKeyStructure.GetDerEncoded();
	}

	public byte[] GetPublicKey()
	{
		EnsurePublic();
		return PublicKey.Q.GetEncoded();
	}
}
