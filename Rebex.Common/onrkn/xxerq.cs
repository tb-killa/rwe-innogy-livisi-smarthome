namespace onrkn;

internal class xxerq : lnabj
{
	private qlyth aqqmp;

	private zjcch uaqfq;

	public bool xhaoc
	{
		get
		{
			if (aqqmp == null || 1 == 0)
			{
				return false;
			}
			return aqqmp.ogtep();
		}
	}

	public int fuext
	{
		get
		{
			if (uaqfq == null || 1 == 0)
			{
				return int.MaxValue;
			}
			return uaqfq.kybig();
		}
	}

	private void gbujs(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in gbujs
		this.gbujs(p0, p1, p2);
	}

	private lnabj yhejo(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			aqqmp = new qlyth();
			return aqqmp;
		case 1:
			uaqfq = new zjcch();
			return uaqfq;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in yhejo
		return this.yhejo(p0, p1, p2);
	}

	private void ixprd()
	{
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ixprd
		this.ixprd();
	}

	private void qimpj(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in qimpj
		this.qimpj(p0, p1, p2);
	}

	private void chfup(fxakl p0)
	{
		p0.suudj(aqqmp, uaqfq);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in chfup
		this.chfup(p0);
	}
}
