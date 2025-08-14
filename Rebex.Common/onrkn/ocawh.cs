using System.Security.Cryptography;

namespace onrkn;

internal class ocawh : lnabj
{
	private readonly zjcch ncqoq = new zjcch();

	private readonly zjcch rhxbn = new zjcch();

	private readonly zjcch lkadg = new zjcch();

	private bool gjibd;

	internal ocawh()
	{
	}

	internal ocawh(zjcch p, zjcch q, zjcch g)
	{
		ncqoq = p;
		rhxbn = q;
		lkadg = g;
	}

	public DSAParameters mabcx()
	{
		return new DSAParameters
		{
			P = zjcch.euzxs(ncqoq.rtrhq),
			Q = zjcch.euzxs(rhxbn.rtrhq),
			G = zjcch.euzxs(lkadg.rtrhq)
		};
	}

	private void xxkbu(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xxkbu
		this.xxkbu(p0, p1, p2);
	}

	private lnabj zuzpa(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			return ncqoq;
		case 1:
			return rhxbn;
		case 2:
			gjibd = true;
			return lkadg;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in zuzpa
		return this.zuzpa(p0, p1, p2);
	}

	private void akove(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in akove
		this.akove(p0, p1, p2);
	}

	private void qrtvd()
	{
		if (!gjibd || 1 == 0)
		{
			throw new CryptographicException("Invalid DssParms.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in qrtvd
		this.qrtvd();
	}

	public void vlfdh(fxakl p0)
	{
		p0.suudj(ncqoq, rhxbn, lkadg);
	}

	internal static DSAParameters rgahm(byte[] p0)
	{
		ocawh ocawh2 = new ocawh();
		hfnnn.qnzgo(ocawh2, p0);
		DSAParameters result = ocawh2.mabcx();
		bdjih bdjih2 = bdjih.foxoi(result.P);
		bdjih bdjih3 = bdjih.foxoi(result.Q);
		bdjih bdjih4 = (bdjih2 - 1) / bdjih3;
		result.J = bdjih4.kskce(p0: false);
		return result;
	}
}
