namespace onrkn;

internal class ohkxj : lnabj
{
	private rwolq zcidt;

	public byte[] gvwnt
	{
		get
		{
			if (zcidt == null || 1 == 0)
			{
				return null;
			}
			return zcidt.rtrhq;
		}
	}

	public ohkxj(byte[] ski)
	{
		zcidt = new rwolq(ski);
	}

	internal ohkxj()
	{
	}

	private void ibzum(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ibzum
		this.ibzum(p0, p1, p2);
	}

	private lnabj irbcz(rmkkr p0, bool p1, int p2)
	{
		if (p2 == 65536)
		{
			zcidt = new rwolq();
			return new rwknq(zcidt, 0, rmkkr.zkxoz);
		}
		return null;
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in irbcz
		return this.irbcz(p0, p1, p2);
	}

	private void ffdsk(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ffdsk
		this.ffdsk(p0, p1, p2);
	}

	private void aykfw()
	{
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in aykfw
		this.aykfw();
	}

	private void ghuwo(fxakl p0)
	{
		p0.suudj(new rwknq(zcidt, 0, rmkkr.zkxoz));
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ghuwo
		this.ghuwo(p0);
	}
}
