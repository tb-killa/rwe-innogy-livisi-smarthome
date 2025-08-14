using System.Security.Cryptography;
using Rebex.Security.Cryptography;

namespace onrkn;

internal static class nailt
{
	public static byte[] zeuix(eatps p0, byte[] p1, SignatureFormat p2)
	{
		if (p2 != SignatureFormat.Pkcs)
		{
			return p1;
		}
		switch (p0.bptsq)
		{
		case AsymmetricKeyAlgorithmId.DSA:
			p1 = lmjia.xfocq(p1, 40);
			break;
		case AsymmetricKeyAlgorithmId.ECDsa:
		{
			int keySize = p0.KeySize;
			int p3 = (keySize + 7) / 8 * 2;
			p1 = lmjia.xfocq(p1, p3);
			break;
		}
		default:
			throw new CryptographicException("Specified signature format is not supported for this algorithm.");
		case AsymmetricKeyAlgorithmId.RSA:
		case AsymmetricKeyAlgorithmId.EdDsa:
			break;
		}
		return p1;
	}

	public static byte[] izcrh(eatps p0, byte[] p1, SignatureFormat p2)
	{
		if (p2 != SignatureFormat.Pkcs)
		{
			return p1;
		}
		switch (p0.bptsq)
		{
		case AsymmetricKeyAlgorithmId.DSA:
			p1 = lmjia.bhrbh(p1, 40);
			break;
		case AsymmetricKeyAlgorithmId.ECDsa:
		{
			int p3 = (p0.KeySize + 7) / 8 * 2;
			p1 = lmjia.bhrbh(p1, p3);
			break;
		}
		default:
			throw new CryptographicException("Specified signature format is not supported for this algorithm.");
		case AsymmetricKeyAlgorithmId.RSA:
		case AsymmetricKeyAlgorithmId.EdDsa:
			break;
		}
		return p1;
	}
}
