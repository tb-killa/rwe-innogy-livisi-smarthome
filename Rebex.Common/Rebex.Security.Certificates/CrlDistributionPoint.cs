using onrkn;

namespace Rebex.Security.Certificates;

public class CrlDistributionPoint : lnabj
{
	private zaraq atmzk;

	private nnzwd xjkjp;

	private nnzwd xvwqp;

	public string Url
	{
		get
		{
			if (atmzk == null || 1 == 0)
			{
				return null;
			}
			return atmzk.zlwug;
		}
	}

	internal DistinguishedName wbnox
	{
		get
		{
			if (atmzk == null || 1 == 0)
			{
				return null;
			}
			return atmzk.wxetk;
		}
	}

	internal string[] ujxot()
	{
		if (atmzk != null && 0 == 0)
		{
			return atmzk.yxays();
		}
		return null;
	}

	internal CrlDistributionPoint()
	{
	}

	public CrlDistributionPoint(string url)
		: this(url, (string[])null)
	{
	}

	internal CrlDistributionPoint(string url, params string[] additionalUrls)
	{
		atmzk = new zaraq();
		atmzk.zlwug = url;
		if (additionalUrls == null || 1 == 0)
		{
			return;
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_002d;
		}
		goto IL_004a;
		IL_002d:
		string p = additionalUrls[num];
		atmzk.rntji(p);
		num++;
		goto IL_004a;
		IL_004a:
		if (num >= additionalUrls.Length)
		{
			return;
		}
		goto IL_002d;
	}

	private void bvgsx(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in bvgsx
		this.bvgsx(p0, p1, p2);
	}

	private lnabj adiuh(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 65536:
			atmzk = new zaraq();
			return new rporh(atmzk, 0);
		case 65537:
			xjkjp = new nnzwd();
			return new rporh(xjkjp, 1);
		case 65538:
			xvwqp = new nnzwd();
			return new rporh(xvwqp, 2);
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in adiuh
		return this.adiuh(p0, p1, p2);
	}

	private void mhhtj(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in mhhtj
		this.mhhtj(p0, p1, p2);
	}

	private void ybwrv()
	{
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ybwrv
		this.ybwrv();
	}

	private void lrqkq(fxakl p0)
	{
		p0.suudj(new rporh(atmzk, 0));
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in lrqkq
		this.lrqkq(p0);
	}
}
