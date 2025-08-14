using System;
using System.Security.Cryptography;

namespace onrkn;

internal class knzkr : lnabj
{
	private gfiwx ystxx;

	private gfiwx nipca;

	public DateTime xjxmd => ystxx.fzcfd();

	public DateTime jgyif => nipca.fzcfd();

	internal knzkr()
	{
	}

	internal knzkr(DateTime effectiveDate, DateTime expirationDate)
	{
		ystxx = new gfiwx(effectiveDate);
		nipca = new gfiwx(expirationDate);
	}

	private void edzgk(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in edzgk
		this.edzgk(p0, p1, p2);
	}

	private lnabj pdpmq(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			ystxx = new gfiwx();
			return ystxx;
		case 1:
			nipca = new gfiwx();
			return nipca;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in pdpmq
		return this.pdpmq(p0, p1, p2);
	}

	private void poaps(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in poaps
		this.poaps(p0, p1, p2);
	}

	private void qsvyu()
	{
		if (ystxx == null || 1 == 0)
		{
			throw new CryptographicException("Effective date not found in certificate.");
		}
		if (nipca == null || 1 == 0)
		{
			throw new CryptographicException("Expiration date not found in certificate.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in qsvyu
		this.qsvyu();
	}

	private void bhlrz(fxakl p0)
	{
		p0.suudj(ystxx, nipca);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in bhlrz
		this.bhlrz(p0);
	}
}
