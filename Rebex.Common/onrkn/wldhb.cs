using System;
using System.Security.Cryptography;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal static class wldhb
{
	[rbjhl("windows")]
	private static jayrg lhyrj(Certificate p0, bool p1, bool p2, out int p3)
	{
		p3 = 0;
		jayrg jayrg2 = null;
		CspParameters cspParameters = hflqg.xbcyx(p0, p1: false);
		if (cspParameters != null && 0 == 0)
		{
			switch (cspParameters.ProviderType)
			{
			case 0:
				if (p2 && 0 == 0)
				{
					throw new CryptographicException("CNG keys are not supported on this platform.");
				}
				return null;
			case 1:
			case 2:
			case 12:
			case 13:
				jayrg2 = xgwba.jwzup(cspParameters, p0.KeyAlgorithm, p1, p2, out p3);
				break;
			}
		}
		if (jayrg2 != null && 0 == 0)
		{
			return jayrg2;
		}
		return hflqg.ahfzl(p0, p1, p2, out p3);
	}

	public static AsymmetricKeyAlgorithm beraz(Certificate p0, bool p1, bool p2)
	{
		bool p3;
		return uktlc(p0, p1, p2, out p3);
	}

	public static AsymmetricKeyAlgorithm uktlc(Certificate p0, bool p1, bool p2, out bool p3)
	{
		p3 = false;
		jayrg jayrg2;
		if (pothu.aicde && 0 == 0)
		{
			jayrg2 = lhyrj(p0, p1, p2, out var p4);
			p3 = p4 == -2146893790;
		}
		else
		{
			jayrg2 = null;
		}
		if (jayrg2 == null || 1 == 0)
		{
			if (p2 && 0 == 0)
			{
				throw new CryptographicException("Unable to acquire private key.");
			}
			return null;
		}
		eatps publicKeyAlgorithm = wevnt(p0);
		fvjkt asymmetric = new fvjkt(publicKeyAlgorithm, jayrg2);
		return new AsymmetricKeyAlgorithm(asymmetric, publicOnly: false, ownsAlgorithm: true);
	}

	public static AsymmetricKeyAlgorithm hidpx(Certificate p0)
	{
		eatps eatps2 = wevnt(p0);
		if (eatps2.bptsq != AsymmetricKeyAlgorithmId.EdDsa)
		{
			eatps2 = new fvjkt(eatps2, null);
		}
		return new AsymmetricKeyAlgorithm(eatps2, publicOnly: true, ownsAlgorithm: true);
	}

	private static eatps wevnt(Certificate p0)
	{
		PublicKeyInfo publicKeyInfo = p0.GetPublicKeyInfo();
		string p1 = publicKeyInfo.dtrjf();
		imfrk imfrk2 = rmnyn.bknre(p1);
		return imfrk2.neqkn(publicKeyInfo);
	}

	[rbjhl("windows")]
	public static void zafys(Certificate p0, PrivateKeyInfo p1, bool p2)
	{
		string p3 = Guid.NewGuid().ToString();
		switch (p0.KeyAlgorithm)
		{
		case KeyAlgorithm.RSA:
		case KeyAlgorithm.DSA:
		{
			CspParameters cspParameters = fxnlx.laldo(p1, p3, p2);
			if (cspParameters == null || 1 == 0)
			{
				throw new CryptographicException("Unable to bind private key.");
			}
			hflqg.jqcpo(p0, cspParameters);
			break;
		}
		default:
			throw new CryptographicException("Permanent bind is not supported for this key on this platform.");
		}
	}

	[rbjhl("windows")]
	public static byte[] fxwel(Certificate p0, CspParameters p1, string p2)
	{
		Certificate certificate = new Certificate(p0.GetRawCertData());
		try
		{
			hflqg.jqcpo(certificate, p1);
			CertificateStore certificateStore = new CertificateStore();
			try
			{
				certificateStore.Add(certificate);
				return certificateStore.liifj(p2);
			}
			finally
			{
				if (certificateStore != null && 0 == 0)
				{
					((IDisposable)certificateStore).Dispose();
				}
			}
		}
		finally
		{
			if (certificate != null && 0 == 0)
			{
				((IDisposable)certificate).Dispose();
			}
		}
	}

	[rbjhl("windows")]
	public static byte[] jzwyy(Certificate p0, PrivateKeyInfo p1, string p2)
	{
		CspParameters cspParameters = null;
		string p3 = p0.pjvyh();
		try
		{
			switch (p0.KeyAlgorithm)
			{
			case KeyAlgorithm.RSA:
			case KeyAlgorithm.DSA:
				cspParameters = fxnlx.laldo(p1, p3, CryptoHelper.mdumc);
				if (cspParameters == null || 1 == 0)
				{
					throw new CryptographicException("Unable to save PFX/P12.");
				}
				return fxwel(p0, cspParameters, p2);
			default:
				throw new CryptographicException(string.Concat("PFX/P12 saving is not supported for ", p0.KeyAlgorithm, " keys."));
			}
		}
		finally
		{
			if (cspParameters != null && 0 == 0)
			{
				hflqg.pkpvn(cspParameters, p1: true);
			}
		}
	}
}
