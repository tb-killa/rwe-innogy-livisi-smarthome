using System.IO;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math.EC;

namespace Org.BouncyCastle.Security;

public sealed class PublicKeyFactory
{
	private PublicKeyFactory()
	{
	}

	public static AsymmetricKeyParameter CreateKey(byte[] keyInfoData)
	{
		return CreateKey(SubjectPublicKeyInfo.GetInstance(Asn1Object.FromByteArray(keyInfoData)));
	}

	public static AsymmetricKeyParameter CreateKey(Stream inStr)
	{
		return CreateKey(SubjectPublicKeyInfo.GetInstance(Asn1Object.FromStream(inStr)));
	}

	public static AsymmetricKeyParameter CreateKey(SubjectPublicKeyInfo keyInfo)
	{
		AlgorithmIdentifier algorithmID = keyInfo.AlgorithmID;
		DerObjectIdentifier algorithm = algorithmID.Algorithm;
		if (algorithm.Equals(X9ObjectIdentifiers.IdECPublicKey))
		{
			X962Parameters x962Parameters = new X962Parameters(algorithmID.Parameters.ToAsn1Object());
			X9ECParameters x9ECParameters = ((!x962Parameters.IsNamedCurve) ? new X9ECParameters((Asn1Sequence)x962Parameters.Parameters) : ECKeyPairGenerator.FindECCurveByOid((DerObjectIdentifier)x962Parameters.Parameters));
			Asn1OctetString s = new DerOctetString(keyInfo.PublicKeyData.GetBytes());
			X9ECPoint x9ECPoint = new X9ECPoint(x9ECParameters.Curve, s);
			ECPoint point = x9ECPoint.Point;
			if (x962Parameters.IsNamedCurve)
			{
				return new ECPublicKeyParameters("EC", point, (DerObjectIdentifier)x962Parameters.Parameters);
			}
			ECDomainParameters parameters = new ECDomainParameters(x9ECParameters.Curve, x9ECParameters.G, x9ECParameters.N, x9ECParameters.H, x9ECParameters.GetSeed());
			return new ECPublicKeyParameters(point, parameters);
		}
		throw new SecurityUtilityException("algorithm identifier in key not recognised: " + algorithm);
	}
}
