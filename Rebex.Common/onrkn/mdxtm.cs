using System.Security.Cryptography;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class mdxtm : fxnlx
{
	private readonly AsymmetricKeyAlgorithmId licyy;

	private readonly string dgexg;

	public override AsymmetricKeyAlgorithmId bptsq => licyy;

	public override string janem => dgexg + eyanf.KeySize;

	public mdxtm(RSA rsa, bool ownsAlgorithm)
		: base(rsa, ownsAlgorithm)
	{
		licyy = AsymmetricKeyAlgorithmId.RSA;
		dgexg = "rsa";
	}

	public mdxtm(DSA dsa, bool ownsAlgorithm)
		: base(dsa, ownsAlgorithm)
	{
		licyy = AsymmetricKeyAlgorithmId.DSA;
		dgexg = "dsa";
	}

	public static mdxtm zpyak(AsymmetricAlgorithm p0, bool p1)
	{
		if (p0 is RSA && 0 == 0)
		{
			return new mdxtm((RSA)p0, p1);
		}
		if (p0 is DSA && 0 == 0)
		{
			return new mdxtm((DSA)p0, p1);
		}
		return null;
	}

	public override bool knvjq(jyamo p0)
	{
		if (eyanf is RSA && 0 == 0)
		{
			switch (p0.vmeor)
			{
			case xdgzn.ctbmq:
				return true;
			case xdgzn.bntzq:
			{
				if (!dahxy.kxxtc || 1 == 0)
				{
					return false;
				}
				if (!jlfbq.uqqcs(p0.blonh) || 1 == 0)
				{
					return false;
				}
				if (p0.fbcyx != p0.bablj)
				{
					return false;
				}
				HashingAlgorithmId fbcyx = p0.fbcyx;
				if (fbcyx == HashingAlgorithmId.SHA1)
				{
					return eyanf is RSACryptoServiceProvider;
				}
				return false;
			}
			default:
				return false;
			}
		}
		return base.knvjq(p0);
	}

	public override byte[] sfbms(byte[] p0, jyamo p1)
	{
		if (eyanf is RSA rSA && 0 == 0 && rSA is RSACryptoServiceProvider rSACryptoServiceProvider && 0 == 0)
		{
			mksxa(p1, out var p2);
			return rSACryptoServiceProvider.Encrypt(p0, p2);
		}
		return base.sfbms(p0, p1);
	}

	public override byte[] lhhds(byte[] p0, jyamo p1)
	{
		if (eyanf is RSA rSA && 0 == 0 && rSA is RSACryptoServiceProvider rSACryptoServiceProvider && 0 == 0)
		{
			if (rSACryptoServiceProvider.PublicOnly && 0 == 0)
			{
				throw new CryptographicException("Cannot decrypt without private key.");
			}
			mksxa(p1, out var p2);
			return rSACryptoServiceProvider.Decrypt(p0, p2);
		}
		return base.lhhds(p0, p1);
	}

	private static void mksxa(jyamo p0, out bool p1)
	{
		switch (p0.vmeor)
		{
		case xdgzn.ctbmq:
			p1 = false;
			break;
		case xdgzn.bntzq:
			if (!dahxy.kxxtc || 1 == 0)
			{
				throw new CryptographicException("RSA/OAEP is not supported on this platform.");
			}
			if (p0.fbcyx != HashingAlgorithmId.SHA1)
			{
				throw new CryptographicException(string.Concat("RSA/OAEP with ", p0.fbcyx, " is not supported for this key."));
			}
			if (p0.bablj != p0.fbcyx)
			{
				throw new CryptographicException("RSA/OAEP with mismatched hash algorithms is not supported for this key.");
			}
			if (!jlfbq.uqqcs(p0.blonh) || 1 == 0)
			{
				throw new CryptographicException("RSA/OAEP with input parameter is not supported for this key.");
			}
			p1 = true;
			break;
		default:
			throw new CryptographicException("Padding scheme is not supported for this algorithm.");
		}
	}

	public override bool vmedb(mrxvh p0)
	{
		if (eyanf is RSA && 0 == 0)
		{
			if (!kzayp(p0.faqqk) || 1 == 0)
			{
				return false;
			}
			return p0.hqtwc == goies.lfkki;
		}
		if (eyanf is DSA && 0 == 0)
		{
			if (p0.hqtwc == goies.gbwxv)
			{
				return p0.faqqk == SignatureHashAlgorithm.SHA1;
			}
			return false;
		}
		return base.vmedb(p0);
	}

	public override byte[] rypyi(byte[] p0, mrxvh p1)
	{
		if (eyanf is RSA p2 && 0 == 0)
		{
			if (p1.hqtwc != goies.lfkki)
			{
				throw new CryptographicException("Specified padding scheme is not supported for this algorithm.");
			}
			if (!kzayp(p1.faqqk) || 1 == 0)
			{
				throw new CryptographicException("Specified hash algorithm is not supported for this algorithm.");
			}
			SignatureHashAlgorithm faqqk = p1.faqqk;
			return CryptoHelper.vjjer(p2, p0, faqqk.ToString());
		}
		if (eyanf is DSA dSA && 0 == 0)
		{
			if (p1.faqqk != SignatureHashAlgorithm.SHA1)
			{
				throw new CryptographicException("Only SHA-1 is supported for this algorithm.");
			}
			if (p1.hqtwc != goies.gbwxv)
			{
				throw new CryptographicException("Padding scheme is not supported for this algorithm.");
			}
			return dSA.CreateSignature(p0);
		}
		return base.rypyi(p0, p1);
	}

	public override bool cbzmp(byte[] p0, byte[] p1, mrxvh p2)
	{
		if (eyanf is RSA p3 && 0 == 0)
		{
			if (p2.hqtwc != goies.lfkki)
			{
				throw new CryptographicException("Padding scheme is not supported for this algorithm.");
			}
			if (!kzayp(p2.faqqk) || 1 == 0)
			{
				throw new CryptographicException("Specified hash algorithm is not supported for this algorithm.");
			}
			SignatureHashAlgorithm faqqk = p2.faqqk;
			return CryptoHelper.ehtve(p3, p0, faqqk.ToString(), p1);
		}
		if (eyanf is DSA dSA && 0 == 0)
		{
			if (p2.faqqk != SignatureHashAlgorithm.SHA1)
			{
				throw new CryptographicException("Only SHA-1 is supported for this algorithm.");
			}
			if (p2.hqtwc != goies.gbwxv)
			{
				throw new CryptographicException("Padding scheme is not supported for this algorithm.");
			}
			return dSA.VerifySignature(p0, p1);
		}
		return base.cbzmp(p0, p1, p2);
	}

	private static bool kzayp(SignatureHashAlgorithm p0)
	{
		switch (p0)
		{
		case SignatureHashAlgorithm.MD5:
		case SignatureHashAlgorithm.SHA1:
		case SignatureHashAlgorithm.SHA256:
		case SignatureHashAlgorithm.SHA384:
		case SignatureHashAlgorithm.SHA512:
			return true;
		case SignatureHashAlgorithm.MD5SHA1:
			return true;
		default:
			return false;
		}
	}

	public override PublicKeyInfo kptoi()
	{
		if (eyanf is RSA rSA && 0 == 0)
		{
			return new PublicKeyInfo(rSA.ExportParameters(includePrivateParameters: false));
		}
		if (eyanf is DSA dSA && 0 == 0)
		{
			return new PublicKeyInfo(dSA.ExportParameters(includePrivateParameters: false));
		}
		return base.kptoi();
	}

	public override PrivateKeyInfo jbbgs(bool p0)
	{
		if (eyanf is RSA rSA && 0 == 0)
		{
			if (rSA is RSACryptoServiceProvider rSACryptoServiceProvider && 0 == 0)
			{
				azcgf(rSACryptoServiceProvider.PublicOnly, rSACryptoServiceProvider, p0);
			}
			return new PrivateKeyInfo(rSA.ExportParameters(includePrivateParameters: true));
		}
		if (eyanf is DSA dSA && 0 == 0)
		{
			if (dSA is DSACryptoServiceProvider dSACryptoServiceProvider && 0 == 0)
			{
				azcgf(dSACryptoServiceProvider.PublicOnly, dSACryptoServiceProvider, p0);
			}
			return new PrivateKeyInfo(dSA.ExportParameters(includePrivateParameters: true));
		}
		return base.jbbgs(p0);
	}

	[rbjhl("windows")]
	public override CspParameters iqqfj()
	{
		if (eyanf is RSACryptoServiceProvider rSACryptoServiceProvider && 0 == 0)
		{
			if (!rSACryptoServiceProvider.PersistKeyInCsp || 1 == 0)
			{
				return null;
			}
			return fxnlx.izxdl(rSACryptoServiceProvider.CspKeyContainerInfo);
		}
		if (eyanf is DSACryptoServiceProvider dSACryptoServiceProvider && 0 == 0)
		{
			if (!dSACryptoServiceProvider.PersistKeyInCsp || 1 == 0)
			{
				return null;
			}
			return fxnlx.izxdl(dSACryptoServiceProvider.CspKeyContainerInfo);
		}
		return base.iqqfj();
	}

	internal static void azcgf(bool p0, ICspAsymmetricAlgorithm p1, bool p2)
	{
		if (dahxy.xzevd)
		{
			return;
		}
		if (p0 && 0 == 0)
		{
			throw new CryptographicException("Unable to acquire private key.");
		}
		CspKeyContainerInfo cspKeyContainerInfo = p1.CspKeyContainerInfo;
		if (cspKeyContainerInfo == null)
		{
			return;
		}
		bool flag;
		if (dahxy.uttbp && 0 == 0)
		{
			flag = cspKeyContainerInfo.Protected;
		}
		else
		{
			try
			{
				flag = cspKeyContainerInfo.Protected;
			}
			catch (CryptographicException)
			{
				flag = false;
			}
		}
		if (!p2 || false || !flag)
		{
			return;
		}
		throw new CryptographicException("CSP needs to display UI to operate.");
	}
}
