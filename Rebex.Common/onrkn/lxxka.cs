using System.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class lxxka : lnabj
{
	private wyjqw muzxr;

	private aipxl lhhuf;

	public wyjqw rrcbj => muzxr;

	public lxxka()
	{
	}

	public lxxka(ContentInfo contentInfo, bool detached)
	{
		if (contentInfo == null || 1 == 0)
		{
			muzxr = new wyjqw("1.2.840.113549.1.7.1");
			return;
		}
		muzxr = new wyjqw(contentInfo.ContentType);
		if (!detached || 1 == 0)
		{
			lhhuf = contentInfo.jphgq();
		}
	}

	public aipxl lywza()
	{
		return lhhuf;
	}

	public void taeog(aipxl p0)
	{
		lhhuf = p0;
	}

	public lxxka inqqo()
	{
		lxxka lxxka2 = new lxxka();
		lxxka2.muzxr = muzxr;
		if (lhhuf != null && 0 == 0)
		{
			lxxka2.lhhuf = lhhuf.mcyhd();
		}
		return lxxka2;
	}

	private void rvdct(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in rvdct
		this.rvdct(p0, p1, p2);
	}

	private lnabj tstau(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			muzxr = new wyjqw();
			return muzxr;
		case 65536:
			lhhuf = new aipxl();
			return new rporh(lhhuf, 0);
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in tstau
		return this.tstau(p0, p1, p2);
	}

	private void sjvok(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in sjvok
		this.sjvok(p0, p1, p2);
	}

	private void qrojr()
	{
		if (muzxr == null || 1 == 0)
		{
			throw new CryptographicException("Content info does not contain a type.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in qrojr
		this.qrojr();
	}

	private void wotyk(fxakl p0)
	{
		if (lhhuf == null || 1 == 0)
		{
			p0.suudj(muzxr);
		}
		else
		{
			p0.suudj(muzxr, new rporh(lhhuf, 0));
		}
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in wotyk
		this.wotyk(p0);
	}
}
