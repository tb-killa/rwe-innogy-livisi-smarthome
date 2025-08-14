using System.Security.Cryptography;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class mrxvh : fivsd
{
	private SignatureHashAlgorithm hxfne;

	private SignatureHashAlgorithm bbsoi;

	private goies ayzkk;

	private int yvkgn;

	public SignatureHashAlgorithm faqqk
	{
		get
		{
			return hxfne;
		}
		private set
		{
			hxfne = value;
		}
	}

	public SignatureHashAlgorithm wqjdn
	{
		get
		{
			return bbsoi;
		}
		private set
		{
			bbsoi = value;
		}
	}

	private bool srciv
	{
		get
		{
			if (hqtwc == goies.mrskp)
			{
				return faqqk != wqjdn;
			}
			return false;
		}
	}

	public bool izbcg
	{
		get
		{
			if (!srciv || 1 == 0)
			{
				return faqqk == SignatureHashAlgorithm.SHA224;
			}
			return true;
		}
	}

	public goies hqtwc
	{
		get
		{
			return ayzkk;
		}
		private set
		{
			ayzkk = value;
		}
	}

	public int xvcnk
	{
		get
		{
			return yvkgn;
		}
		private set
		{
			yvkgn = value;
		}
	}

	public mrxvh(SignatureHashAlgorithm hashAlgorithm, SignatureHashAlgorithm maskGenHashAlgorithm, goies paddingScheme, int saltLength)
	{
		faqqk = hashAlgorithm;
		wqjdn = maskGenHashAlgorithm;
		hqtwc = paddingScheme;
		xvcnk = saltLength;
		nvsmt();
	}

	public static mrxvh vtcca(SignatureHashAlgorithm p0, AsymmetricKeyAlgorithmId p1)
	{
		goies goies2;
		if (p1 == AsymmetricKeyAlgorithmId.RSA)
		{
			goies2 = goies.lfkki;
			if (goies2 != 0)
			{
				goto IL_0013;
			}
		}
		goies2 = goies.gbwxv;
		goto IL_0013;
		IL_0013:
		return new mrxvh(p0, SignatureHashAlgorithm.Unsupported, goies2, 0);
	}

	private void nvsmt()
	{
		if (izbcg && 0 == 0 && CryptoHelper.UseFipsAlgorithmsOnly && 0 == 0)
		{
			string text = ((srciv ? true : false) ? "mismatched hash algorithms" : "the specified hash algorithm");
			throw new CryptographicException("RSA/PSS with " + text + " is not supported in FIPS-only environments.");
		}
		if (hqtwc != goies.lfkki || faqqk != SignatureHashAlgorithm.MD5SHA1)
		{
			HashingAlgorithm.jwiqd(bpkgq.wrqur(faqqk), p1: false);
			if (hqtwc == goies.mrskp)
			{
				HashingAlgorithm.jwiqd(bpkgq.wrqur(wqjdn), p1: false);
			}
		}
	}
}
