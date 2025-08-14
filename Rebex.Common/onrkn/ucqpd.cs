using System;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class ucqpd : lnabj
{
	private gfiwx ctyzg;

	private RevocationReason tyrmz;

	private ovyea xixbv;

	public DateTime dmgnf => ctyzg.fzcfd();

	public RevocationReason kxygz => tyrmz;

	internal ucqpd()
	{
	}

	public ucqpd(RevocationReason revocationReason, DateTime revocationTime)
	{
		ctyzg = new gfiwx(revocationTime, rmkkr.nwijl);
		tyrmz = revocationReason;
		if (revocationReason != RevocationReason.Unspecified && 0 == 0)
		{
			xixbv = new ovyea((int)revocationReason);
		}
	}

	private void posej(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in posej
		this.posej(p0, p1, p2);
	}

	private lnabj mpner(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			ctyzg = new gfiwx();
			return ctyzg;
		case 65536:
			xixbv = new ovyea();
			return new rporh(xixbv, 0);
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in mpner
		return this.mpner(p0, p1, p2);
	}

	private void trkwp()
	{
		if (xixbv != null && 0 == 0)
		{
			tyrmz = (RevocationReason)(xixbv.akyte ?? 0);
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in trkwp
		this.trkwp();
	}

	private void camyv(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in camyv
		this.camyv(p0, p1, p2);
	}

	private void wsrsy(fxakl p0)
	{
		if (xixbv == null || 1 == 0)
		{
			p0.suudj(ctyzg);
		}
		else
		{
			p0.suudj(ctyzg, new rporh(xixbv, 0));
		}
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in wsrsy
		this.wsrsy(p0);
	}
}
