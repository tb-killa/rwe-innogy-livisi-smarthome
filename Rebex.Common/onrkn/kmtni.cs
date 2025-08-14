using System;
using System.Collections;
using System.Security.Cryptography;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class kmtni : lnabj
{
	private zjcch dltzq;

	private AlgorithmIdentifier zmjgn;

	private ukjdk mwilj;

	private gfiwx lzlxh;

	private gfiwx ldymp;

	private RevokedCertificateCollection iprqt = new RevokedCertificateCollection();

	private CertificateExtensionCollection bkvgm = new CertificateExtensionCollection();

	private int vdxht;

	public AlgorithmIdentifier zszue => zmjgn;

	public DistinguishedName rrbpq => mwilj.efqft;

	public DateTime kuide => lzlxh.fzcfd();

	public DateTime pevmz
	{
		get
		{
			if (ldymp == null || 1 == 0)
			{
				return lzlxh.fzcfd().AddDays(1.0);
			}
			return ldymp.fzcfd();
		}
	}

	public RevokedCertificateCollection imcen => iprqt;

	public CertificateExtensionCollection abicy => bkvgm;

	internal kmtni(Certificate authority, AlgorithmIdentifier signatureAlgorithm, DateTime thisUpdate, DateTime nextUpdate)
	{
		dltzq = new zjcch(1);
		zmjgn = signatureAlgorithm;
		mwilj = new ukjdk(authority.GetSubject());
		lzlxh = new gfiwx(thisUpdate);
		ldymp = new gfiwx(nextUpdate);
	}

	internal kmtni()
	{
	}

	private void jwvfu(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
		vdxht = 0;
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in jwvfu
		this.jwvfu(p0, p1, p2);
	}

	private lnabj nsufx(rmkkr p0, bool p1, int p2)
	{
		if (p2 >= 65536)
		{
			if (p2 == 65536)
			{
				return new rporh(bkvgm, 0);
			}
		}
		switch (p2 - vdxht)
		{
		case 0:
			if ((vdxht == 0 || 1 == 0) && p0 == rmkkr.sklxf)
			{
				dltzq = new zjcch();
				vdxht = 1;
				return dltzq;
			}
			zmjgn = new AlgorithmIdentifier();
			return zmjgn;
		case 1:
			mwilj = new ukjdk();
			return mwilj;
		case 2:
			lzlxh = new gfiwx();
			return lzlxh;
		case 3:
			if ((p0 == rmkkr.keeoc || p0 == rmkkr.nwijl) && (ldymp == null || 1 == 0))
			{
				ldymp = new gfiwx();
				vdxht++;
				return ldymp;
			}
			if (p0 == rmkkr.osptv)
			{
				iprqt = new RevokedCertificateCollection();
				return iprqt;
			}
			return null;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in nsufx
		return this.nsufx(p0, p1, p2);
	}

	private void jpyha(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in jpyha
		this.jpyha(p0, p1, p2);
	}

	private void jioon()
	{
		if (zmjgn == null || 1 == 0)
		{
			throw new CryptographicException("Signature not found in certificate.");
		}
		if (mwilj == null || 1 == 0)
		{
			throw new CryptographicException("Issuer algorithm not found in certificate.");
		}
		if (lzlxh == null || 1 == 0)
		{
			throw new CryptographicException("Update time not found in certificate.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in jioon
		this.jioon();
	}

	private void lcvcf(fxakl p0)
	{
		ArrayList arrayList = new ArrayList();
		if (dltzq != null && 0 == 0)
		{
			arrayList.Add(dltzq);
		}
		arrayList.Add(zmjgn);
		arrayList.Add(mwilj);
		arrayList.Add(lzlxh);
		if (ldymp != null && 0 == 0)
		{
			arrayList.Add(ldymp);
		}
		if (iprqt.Count > 0)
		{
			arrayList.Add(iprqt);
		}
		if (bkvgm.Count > 0)
		{
			arrayList.Add(new rporh(bkvgm, 0));
		}
		p0.aiflg(rmkkr.osptv, arrayList);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in lcvcf
		this.lcvcf(p0);
	}
}
