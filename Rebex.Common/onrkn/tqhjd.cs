using System.Security.Cryptography;

namespace onrkn;

internal class tqhjd : lnabj
{
	private ajezg zdnmq;

	private mcwjl sqchw;

	public ajezg ymogv => zdnmq;

	public mcwjl ynzah
	{
		get
		{
			if (sqchw != null && 0 == 0)
			{
				return sqchw;
			}
			return sqchw = mcwjl.mfjid();
		}
	}

	internal tqhjd()
	{
	}

	public tqhjd(ajezg certId)
	{
		zdnmq = certId;
	}

	private void empjj(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in empjj
		this.empjj(p0, p1, p2);
	}

	private lnabj nahne(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			zdnmq = new ajezg();
			return zdnmq;
		case 65536:
			sqchw = new mcwjl();
			return new rporh(sqchw, 0);
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in nahne
		return this.nahne(p0, p1, p2);
	}

	private void yuape()
	{
		if (zdnmq == null || 1 == 0)
		{
			throw new CryptographicException("CertID not found in OcspRequest.");
		}
		if (sqchw != null && 0 == 0)
		{
			sqchw.hksnh();
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in yuape
		this.yuape();
	}

	private void phphn(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in phphn
		this.phphn(p0, p1, p2);
	}

	private void fyzkq(fxakl p0)
	{
		if (sqchw == null || false || sqchw.Count == 0 || 1 == 0)
		{
			p0.suudj(zdnmq);
		}
		else
		{
			p0.suudj(zdnmq, new rporh(sqchw, 0));
		}
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in fyzkq
		this.fyzkq(p0);
	}
}
