using System;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Rebex.Security.Cryptography;

public class EllipticCurveDiffieHellman : EllipticCurveAlgorithm
{
	public EllipticCurveDiffieHellman(string oid, string curveName)
		: base(oid, curveName)
	{
	}

	public byte[] GetSharedSecret(byte[] otherPublicKey)
	{
		if (otherPublicKey == null)
		{
			throw new ArgumentNullException("otherPublicKey");
		}
		EnsurePrivate();
		EllipticCurveAlgorithm ellipticCurveAlgorithm = CreateBase();
		ellipticCurveAlgorithm.FromPublicKey(otherPublicKey);
		IBasicAgreement basicAgreement = AgreementUtilities.GetBasicAgreement("ECDH");
		basicAgreement.Init(base.PrivateKey);
		BigInteger bigInteger = basicAgreement.CalculateAgreement(ellipticCurveAlgorithm.PublicKey);
		return bigInteger.ToByteArray();
	}
}
