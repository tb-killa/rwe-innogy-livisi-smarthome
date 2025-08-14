using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class nisgb : lnabj
{
	private nnzwd dkreo;

	private AlgorithmIdentifier hbzam;

	private htykq pnzcc;

	private cobxv mnwwt;

	public AlgorithmIdentifier qwiov => hbzam;

	public DistinguishedName zofhh => mnwwt.mhoze;

	public DistinguishedName phamo => mnwwt.msxpx;

	public AlgorithmIdentifier jmggg => mnwwt.wflxw.KeyAlgorithm;

	public DateTime rnidf => mnwwt.hvjmd.xjxmd;

	public DateTime ocdia => mnwwt.hvjmd.jgyif;

	public CertificateExtensionCollection fdmty => mnwwt.bmefi;

	internal nisgb(hwwit signer, DistinguishedName authority, byte[] serialNumber, DistinguishedName subject, PublicKeyInfo publicKey, CertificateExtensionCollection extensions, DateTime effectiveDate, DateTime expirationDate)
	{
		hbzam = signer.kjail;
		mnwwt = new cobxv(authority, serialNumber, subject, publicKey, hbzam, new knzkr(effectiveDate, expirationDate));
		IEnumerator<CertificateExtension> enumerator = extensions.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				CertificateExtension current = enumerator.Current;
				mnwwt.bmefi.Add(current);
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		byte[] array = bjdjh();
		byte[] data = signer.noegy(array);
		dkreo = new nnzwd(array);
		pnzcc = new htykq(data, 0);
	}

	internal byte[] bjdjh()
	{
		MemoryStream memoryStream = new MemoryStream();
		try
		{
			fxakl fxakl2 = new fxakl(memoryStream);
			fxakl2.afwyb();
			fxakl2.kfyej(mnwwt);
			fxakl2.xljze();
			fxakl2.imfsc();
			return memoryStream.ToArray();
		}
		finally
		{
			if (memoryStream != null && 0 == 0)
			{
				((IDisposable)memoryStream).Dispose();
			}
		}
	}

	public nisgb(byte[] data)
	{
		hfnnn.qnzgo(this, data);
	}

	public byte[] dfjyj()
	{
		return mnwwt.wmddh.aqhfc();
	}

	public byte[] lwwfi()
	{
		return mnwwt.wflxw.ToBytes();
	}

	public byte[] ohsbd()
	{
		HashingAlgorithmId algorithm = hbzam.vvmoi(p0: true);
		return HashingAlgorithm.ComputeHash(algorithm, dkreo.lktyp);
	}

	public bool qpdba(Certificate p0)
	{
		SignatureParameters signatureParameters = new SignatureParameters();
		signatureParameters.HashAlgorithm = hbzam.vvmoi(p0: true);
		signatureParameters.Format = SignatureFormat.Pkcs;
		return p0.VerifyMessage(dkreo.lktyp, pnzcc.lssxa, signatureParameters);
	}

	public byte[] uypld()
	{
		return pnzcc.lssxa.aqhfc();
	}

	public int cdybo()
	{
		return mnwwt.tiilm();
	}

	private void kiaek(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in kiaek
		this.kiaek(p0, p1, p2);
	}

	private lnabj kflzt(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			dkreo = new nnzwd();
			return dkreo;
		case 1:
			hbzam = new AlgorithmIdentifier();
			return hbzam;
		case 2:
			pnzcc = new htykq();
			return pnzcc;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in kflzt
		return this.kflzt(p0, p1, p2);
	}

	private void ggeji(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ggeji
		this.ggeji(p0, p1, p2);
	}

	private void nifkl()
	{
		if (dkreo == null || 1 == 0)
		{
			throw new CryptographicException("TBSCertificate not found in certificate.");
		}
		if (hbzam == null || 1 == 0)
		{
			throw new CryptographicException("Signature algorithm not found in certificate.");
		}
		if (pnzcc == null || 1 == 0)
		{
			throw new CryptographicException("Signature not found in certificate.");
		}
		mnwwt = new cobxv();
		hfnnn.qnzgo(mnwwt, dkreo.lktyp);
		byte[] p = fxakl.kncuz(hbzam);
		byte[] p2 = fxakl.kncuz(mnwwt.kqlmy);
		if (!zjcch.wduyr(p, p2) || 1 == 0)
		{
			throw new CryptographicException("Signature algorithms do not match.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in nifkl
		this.nifkl();
	}

	private void xlakd(fxakl p0)
	{
		p0.suudj(dkreo, hbzam, pnzcc);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xlakd
		this.xlakd(p0);
	}
}
