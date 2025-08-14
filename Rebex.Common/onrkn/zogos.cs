using System;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

[rbjhl("windows")]
internal class zogos : imfrk
{
	private zogos()
	{
	}

	public static imfrk ojezw(string p0)
	{
		if (!bpkgq.guhie(p0, out var p1, out var p2) || 1 == 0)
		{
			return null;
		}
		if (p1 != AsymmetricKeyAlgorithmId.RSA && 0 == 0)
		{
			return null;
		}
		if ((((p2 & 0x3F) != 0) ? true : false) || p2 < 512 || p2 > 4096)
		{
			return null;
		}
		return new zogos();
	}

	public eatps poerm()
	{
		throw new NotImplementedException();
	}

	public eatps xaunu(PrivateKeyInfo p0)
	{
		throw new NotImplementedException();
	}

	public eatps neqkn(PublicKeyInfo p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("publicKey");
		}
		if (p0.KeyAlgorithm.edeag() != AsymmetricKeyAlgorithmId.RSA && 0 == 0)
		{
			throw new CryptographicException("Unsupported key algorithm.");
		}
		return xgwba.vxfno(p0);
	}
}
