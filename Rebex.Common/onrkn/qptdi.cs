using System.Collections;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class qptdi : lnabj
{
	private CertificateCollection htdyl = new CertificateCollection();

	private CertificateRevocationListCollection bwmym = new CertificateRevocationListCollection();

	public CertificateCollection blnog => htdyl;

	public CertificateRevocationListCollection urrfw => bwmym;

	public bool kchbe
	{
		get
		{
			if (htdyl.Count > 0)
			{
				return false;
			}
			if (bwmym != null && 0 == 0)
			{
				return bwmym.Count == 0;
			}
			return true;
		}
	}

	private void jqpvt(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in jqpvt
		this.jqpvt(p0, p1, p2);
	}

	private lnabj pdnrw(rmkkr p0, bool p1, int p2)
	{
		return p2 switch
		{
			65536 => new rwknq(htdyl, 0, rmkkr.wguaf), 
			65537 => new rwknq(bwmym, 1, rmkkr.wguaf), 
			_ => null, 
		};
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in pdnrw
		return this.pdnrw(p0, p1, p2);
	}

	private void hjyrt(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in hjyrt
		this.hjyrt(p0, p1, p2);
	}

	private void camto()
	{
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in camto
		this.camto();
	}

	private void xzxso(fxakl p0)
	{
		ArrayList arrayList = new ArrayList();
		if (htdyl != null && 0 == 0 && htdyl.Count > 0)
		{
			arrayList.Add(new rwknq(htdyl, 0, rmkkr.wguaf));
		}
		if (bwmym != null && 0 == 0 && bwmym.Count > 0)
		{
			arrayList.Add(new rwknq(bwmym, 1, rmkkr.wguaf));
		}
		p0.aiflg(rmkkr.osptv, arrayList);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xzxso
		this.xzxso(p0);
	}
}
