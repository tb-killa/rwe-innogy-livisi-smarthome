using System;
using System.Security.Cryptography;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class cgxdn : lnabj
{
	public static readonly ObjectIdentifier pfvpw = "1.3.6.1.5.5.7.48.1.1";

	private nnzwd ihdtx;

	private hiegm xqbid;

	private AlgorithmIdentifier scrac;

	private htykq azzvf;

	private CertificateCollection gegif;

	public hiegm jgdyi => xqbid;

	public AlgorithmIdentifier pcvsi => scrac;

	public CertificateCollection waxgs => gegif;

	internal cgxdn()
	{
	}

	public cgxdn(hwwit signer, DistinguishedName responder, DateTime producedAt, itxgi responses, params zyked[] extensions)
	{
		xqbid = new hiegm(responder, producedAt, responses, extensions);
		byte[] array = fxakl.kncuz(xqbid);
		ihdtx = new nnzwd(array);
		scrac = signer.kjail;
		azzvf = new htykq(signer.noegy(array), 0);
	}

	public bool mjaxu(Certificate p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("issuer");
		}
		SignatureParameters signatureParameters = new SignatureParameters();
		signatureParameters.HashAlgorithm = scrac.vvmoi(p0: true);
		signatureParameters.Format = SignatureFormat.Pkcs;
		return p0.VerifyMessage(ihdtx.lktyp, azzvf.lssxa, signatureParameters);
	}

	public void zkxnk(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	public lnabj qaqes(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			ihdtx = new nnzwd();
			return ihdtx;
		case 1:
			scrac = new AlgorithmIdentifier();
			return scrac;
		case 2:
			azzvf = new htykq();
			return azzvf;
		case 65536:
			gegif = new CertificateCollection(isSet: false);
			return new rporh(gegif, 0);
		default:
			return null;
		}
	}

	public void somzq()
	{
		if (ihdtx == null || 1 == 0)
		{
			throw new CryptographicException("ResponseData not found in BasicOcspResponse.");
		}
		if (scrac == null || 1 == 0)
		{
			throw new CryptographicException("SignatureAlgorithm not found in BasicOcspResponse.");
		}
		if (azzvf == null || 1 == 0)
		{
			throw new CryptographicException("Signature not found in BasicOcspResponse.");
		}
		xqbid = new hiegm();
		hfnnn.qnzgo(xqbid, ihdtx.lktyp);
		if (gegif != null && 0 == 0)
		{
			gegif.hksnh();
		}
	}

	public void lnxah(byte[] p0, int p1, int p2)
	{
	}

	public void vlfdh(fxakl p0)
	{
		if (gegif == null || false || gegif.Count == 0 || 1 == 0)
		{
			p0.suudj(ihdtx, scrac, azzvf);
		}
		else
		{
			p0.suudj(ihdtx, scrac, azzvf, new rporh(gegif, 0));
		}
	}
}
