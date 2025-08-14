using System;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class hsbey : lnabj
{
	private readonly rwknq ibone;

	public bool rkajo => ynadv == ezpxd.zyamk;

	public ezpxd ynadv => (ezpxd)ibone.tshfh;

	public ucqpd lawha => ibone.iiebl as ucqpd;

	private hsbey(lnabj inner, int tag, rmkkr type)
	{
		ibone = new rwknq(inner, tag, type);
	}

	internal static hsbey xjkwj(rmkkr p0, bool p1, int p2)
	{
		return p2 switch
		{
			65536 => new hsbey(new mdvaz(), 0, rmkkr.nqrrp), 
			65537 => new hsbey(new ucqpd(), 1, rmkkr.osptv), 
			65538 => new hsbey(new mdvaz(), 2, rmkkr.nqrrp), 
			_ => null, 
		};
	}

	public static hsbey evpcl()
	{
		return new hsbey(new mdvaz(), 0, rmkkr.nqrrp);
	}

	public static hsbey oomwf(RevocationReason p0, DateTime p1)
	{
		return new hsbey(new ucqpd(p0, p1), 1, rmkkr.osptv);
	}

	public static hsbey wmuxb()
	{
		return new hsbey(new mdvaz(), 2, rmkkr.nqrrp);
	}

	private void zxqno(rmkkr p0, bool p1, int p2)
	{
		ibone.zkxnk(p0, p1, p2);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in zxqno
		this.zxqno(p0, p1, p2);
	}

	private lnabj twpqf(rmkkr p0, bool p1, int p2)
	{
		return ibone.qaqes(p0, p1, p2);
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in twpqf
		return this.twpqf(p0, p1, p2);
	}

	private void krprb()
	{
		ibone.somzq();
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in krprb
		this.krprb();
	}

	private void npbmp(byte[] p0, int p1, int p2)
	{
		ibone.lnxah(p0, p1, p2);
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in npbmp
		this.npbmp(p0, p1, p2);
	}

	private void lrmxl(fxakl p0)
	{
		p0.kfyej(ibone);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in lrmxl
		this.lrmxl(p0);
	}
}
