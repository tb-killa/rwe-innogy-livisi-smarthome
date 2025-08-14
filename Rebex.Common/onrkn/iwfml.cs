using System.Collections;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class iwfml : lnabj
{
	private class tgexn : lnabj
	{
		private readonly rwolq nffav = new rwolq();

		private gfiwx lbugb;

		private nnzwd owzhl;

		public SubjectIdentifier bafqd()
		{
			return new SubjectIdentifier(nffav);
		}

		private void ebofl(rmkkr p0, bool p1, int p2)
		{
			hfnnn.xmjay(rmkkr.osptv, p0, p1);
		}

		void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
		{
			//ILSpy generated this explicit interface implementation from .override directive in ebofl
			this.ebofl(p0, p1, p2);
		}

		private lnabj wxywc(rmkkr p0, bool p1, int p2)
		{
			switch (p2)
			{
			case 0:
				return nffav;
			case 1:
				switch (p0)
				{
				case rmkkr.nwijl:
					lbugb = new gfiwx();
					return lbugb;
				case rmkkr.osptv:
					owzhl = new nnzwd();
					return owzhl;
				default:
					return null;
				}
			case 2:
				if (lbugb != null && 0 == 0 && p0 == rmkkr.osptv)
				{
					owzhl = new nnzwd();
					return owzhl;
				}
				return null;
			default:
				return null;
			}
		}

		lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
		{
			//ILSpy generated this explicit interface implementation from .override directive in wxywc
			return this.wxywc(p0, p1, p2);
		}

		private void ycufm(byte[] p0, int p1, int p2)
		{
		}

		void lnabj.lnxah(byte[] p0, int p1, int p2)
		{
			//ILSpy generated this explicit interface implementation from .override directive in ycufm
			this.ycufm(p0, p1, p2);
		}

		private void vyelj()
		{
		}

		void lnabj.somzq()
		{
			//ILSpy generated this explicit interface implementation from .override directive in vyelj
			this.vyelj();
		}

		private void toazp(fxakl p0)
		{
			ArrayList arrayList = new ArrayList();
			arrayList.Add(nffav);
			if (lbugb != null && 0 == 0)
			{
				arrayList.Add(lbugb);
			}
			if (owzhl != null && 0 == 0)
			{
				arrayList.Add(owzhl);
			}
			p0.aiflg(rmkkr.osptv, arrayList);
		}

		void lnabj.vlfdh(fxakl p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in toazp
			this.toazp(p0);
		}
	}

	private SubjectIdentifier auojx;

	private tgexn tfhwi;

	private rwolq jmlnl;

	public SubjectIdentifier simzu => auojx;

	public byte[] lkfuo => jmlnl.rtrhq;

	private void rhcyo(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in rhcyo
		this.rhcyo(p0, p1, p2);
	}

	private lnabj wbuqe(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			if (tfhwi == null || 1 == 0)
			{
				auojx = new SubjectIdentifier(SubjectIdentifierType.IssuerAndSerialNumber);
				return auojx.wdiep();
			}
			jmlnl = new rwolq();
			return jmlnl;
		case 65536:
			tfhwi = new tgexn();
			return new rwknq(tfhwi, 0, rmkkr.osptv);
		case 1:
			if (tfhwi == null || 1 == 0)
			{
				jmlnl = new rwolq();
				return jmlnl;
			}
			return null;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in wbuqe
		return this.wbuqe(p0, p1, p2);
	}

	private void faeko(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in faeko
		this.faeko(p0, p1, p2);
	}

	private void pagyb()
	{
		if (tfhwi != null && 0 == 0)
		{
			auojx = tfhwi.bafqd();
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in pagyb
		this.pagyb();
	}

	private void gtuku(fxakl p0)
	{
		if (tfhwi == null || 1 == 0)
		{
			p0.suudj(auojx.wdiep(), jmlnl);
		}
		else
		{
			p0.suudj(new rwknq(tfhwi, 0, rmkkr.osptv), jmlnl);
		}
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in gtuku
		this.gtuku(p0);
	}
}
