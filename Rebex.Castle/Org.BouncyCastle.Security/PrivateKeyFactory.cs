using System.IO;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Security;

public sealed class PrivateKeyFactory
{
	private PrivateKeyFactory()
	{
	}

	public static AsymmetricKeyParameter CreateKey(byte[] privateKeyInfoData)
	{
		return CreateKey(PrivateKeyInfo.GetInstance(Asn1Object.FromByteArray(privateKeyInfoData)));
	}

	public static AsymmetricKeyParameter CreateKey(Stream inStr)
	{
		return CreateKey(PrivateKeyInfo.GetInstance(Asn1Object.FromStream(inStr)));
	}

	public static AsymmetricKeyParameter CreateKey(PrivateKeyInfo keyInfo)
	{
		AlgorithmIdentifier privateKeyAlgorithm = keyInfo.PrivateKeyAlgorithm;
		DerObjectIdentifier algorithm = privateKeyAlgorithm.Algorithm;
		if (algorithm.Equals(X9ObjectIdentifiers.IdECPublicKey))
		{
			X962Parameters x962Parameters = new X962Parameters(privateKeyAlgorithm.Parameters.ToAsn1Object());
			X9ECParameters x9ECParameters = ((!x962Parameters.IsNamedCurve) ? new X9ECParameters((Asn1Sequence)x962Parameters.Parameters) : ECKeyPairGenerator.FindECCurveByOid((DerObjectIdentifier)x962Parameters.Parameters));
			ECPrivateKeyStructure instance = ECPrivateKeyStructure.GetInstance(keyInfo.ParsePrivateKey());
			BigInteger key = instance.GetKey();
			if (x962Parameters.IsNamedCurve)
			{
				return new ECPrivateKeyParameters("EC", key, (DerObjectIdentifier)x962Parameters.Parameters);
			}
			ECDomainParameters parameters = new ECDomainParameters(x9ECParameters.Curve, x9ECParameters.G, x9ECParameters.N, x9ECParameters.H, x9ECParameters.GetSeed());
			return new ECPrivateKeyParameters(key, parameters);
		}
		throw new SecurityUtilityException("algorithm identifier in key not recognised");
	}
}
