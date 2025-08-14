using System.Security.Cryptography;
using Rebex.Security.Certificates;

namespace onrkn;

internal class sxlwf : lnabj
{
	private ukjdk byewt;

	private zjcch mpuog;

	public DistinguishedName mmlwe => byewt.efqft;

	public byte[] btmfq => mpuog.rtrhq;

	public sxlwf()
	{
	}

	public sxlwf(DistinguishedName dn, byte[] serialNumber)
	{
		byewt = new ukjdk(dn);
		mpuog = new zjcch(serialNumber);
	}

	private void uyqpm(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in uyqpm
		this.uyqpm(p0, p1, p2);
	}

	private lnabj lsdqu(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			byewt = new ukjdk();
			return byewt;
		case 1:
			mpuog = new zjcch();
			return mpuog;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in lsdqu
		return this.lsdqu(p0, p1, p2);
	}

	private void ivofu(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ivofu
		this.ivofu(p0, p1, p2);
	}

	private void irutt()
	{
		if (byewt == null || 1 == 0)
		{
			throw new CryptographicException("Subject identifier does not contain an issuer.");
		}
		if (mpuog == null || 1 == 0)
		{
			throw new CryptographicException("Subject identifier does not contain a serial number.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in irutt
		this.irutt();
	}

	private void qejqu(fxakl p0)
	{
		p0.aiflg(rmkkr.osptv, new lnabj[2] { byewt, mpuog });
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in qejqu
		this.qejqu(p0);
	}
}
