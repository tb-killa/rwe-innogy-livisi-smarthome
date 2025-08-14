namespace onrkn;

internal class dyaoh : lnabj
{
	private zaraq rbywk;

	private qlyth wieye;

	private qlyth jcndn;

	private htykq vdrpi;

	private qlyth dwofg;

	private qlyth fuepa;

	public zaraq bqemz => rbywk;

	public bool zrcpk
	{
		get
		{
			if (wieye == null || 1 == 0)
			{
				return false;
			}
			return wieye.ogtep();
		}
	}

	public bool gokvn
	{
		get
		{
			if (jcndn == null || 1 == 0)
			{
				return false;
			}
			return jcndn.ogtep();
		}
	}

	public bool hvycy
	{
		get
		{
			if (dwofg == null || 1 == 0)
			{
				return false;
			}
			return dwofg.ogtep();
		}
	}

	public bool lnobp
	{
		get
		{
			if (fuepa == null || 1 == 0)
			{
				return false;
			}
			return fuepa.ogtep();
		}
	}

	public hqjml cfkdr
	{
		get
		{
			if (vdrpi == null || 1 == 0)
			{
				return (hqjml)0;
			}
			return (hqjml)vdrpi.xmojg();
		}
	}

	private void qyhov(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in qyhov
		this.qyhov(p0, p1, p2);
	}

	private lnabj necbq(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 65536:
			rbywk = new zaraq();
			return new rporh(rbywk, 0);
		case 65537:
			wieye = new qlyth();
			return new dyhlg(wieye, 1);
		case 65538:
			jcndn = new qlyth();
			return new dyhlg(jcndn, 2);
		case 65539:
			vdrpi = new htykq();
			return new rporh(vdrpi, 3);
		case 65540:
			dwofg = new qlyth();
			return new dyhlg(dwofg, 4);
		case 65541:
			fuepa = new qlyth();
			return new dyhlg(fuepa, 5);
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in necbq
		return this.necbq(p0, p1, p2);
	}

	private void wzinn()
	{
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in wzinn
		this.wzinn();
	}

	private void abegw(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in abegw
		this.abegw(p0, p1, p2);
	}

	private void ebvut(fxakl p0)
	{
		if (rbywk != null && 0 == 0)
		{
			p0.xadip(0, rbywk);
		}
		if (wieye != null && 0 == 0)
		{
			p0.xadip(1, wieye);
		}
		if (jcndn != null && 0 == 0)
		{
			p0.xadip(2, jcndn);
		}
		if (vdrpi != null && 0 == 0)
		{
			p0.xadip(3, vdrpi);
		}
		if (dwofg != null && 0 == 0)
		{
			p0.xadip(4, dwofg);
		}
		if (fuepa != null && 0 == 0)
		{
			p0.xadip(5, fuepa);
		}
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ebvut
		this.ebvut(p0);
	}
}
