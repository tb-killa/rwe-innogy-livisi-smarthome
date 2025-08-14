using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class yyzlj : lnabj
{
	private sxlwf jvlnq;

	private rwolq wixml;

	private isamm pdddf;

	private lnabj qmudm;

	public SubjectIdentifier haacf()
	{
		if (jvlnq != null && 0 == 0)
		{
			return new SubjectIdentifier(jvlnq);
		}
		if (wixml != null && 0 == 0)
		{
			return new SubjectIdentifier(wixml);
		}
		if (pdddf != null && 0 == 0)
		{
			return new SubjectIdentifier(pdddf);
		}
		return null;
	}

	private void flknc(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.cxxlq, p0, p1);
		switch (p2)
		{
		case 0:
			qmudm = (jvlnq = new sxlwf());
			break;
		case 65536:
			wixml = new rwolq();
			qmudm = new rwknq(wixml, 0, rmkkr.zkxoz);
			break;
		case 65537:
			pdddf = new isamm();
			qmudm = new rwknq(pdddf, 1, rmkkr.osptv);
			break;
		default:
			qmudm = rillp.yeukt;
			break;
		}
		qmudm.zkxnk(p0, p1, p2);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in flknc
		this.flknc(p0, p1, p2);
	}

	private lnabj ybyrz(rmkkr p0, bool p1, int p2)
	{
		if (qmudm != null && 0 == 0)
		{
			return qmudm.qaqes(p0, p1, p2);
		}
		return null;
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ybyrz
		return this.ybyrz(p0, p1, p2);
	}

	private void kxqvi(byte[] p0, int p1, int p2)
	{
		if (qmudm != null && 0 == 0)
		{
			qmudm.lnxah(p0, p1, p2);
		}
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in kxqvi
		this.kxqvi(p0, p1, p2);
	}

	private void cmxrr()
	{
		if (qmudm != null && 0 == 0)
		{
			qmudm.somzq();
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in cmxrr
		this.cmxrr();
	}

	private void vkbqq(fxakl p0)
	{
		qmudm.vlfdh(p0);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in vkbqq
		this.vkbqq(p0);
	}
}
