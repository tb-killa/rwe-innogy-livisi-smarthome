using System;
using Rebex.Security.Certificates;

namespace onrkn;

internal class zaraq : lnabj
{
	private azfwz jcfvd;

	private nnzwd rendp;

	private lnabj ksiug;

	public string zlwug
	{
		get
		{
			if (jcfvd == null || 1 == 0)
			{
				return null;
			}
			string[] array = jcfvd.hhoay(ukmqt.fcaeo);
			if (array.Length == 0 || 1 == 0)
			{
				return null;
			}
			return array[0];
		}
		set
		{
			rendp = null;
			jcfvd = new azfwz();
			jcfvd.gfsfs(value, ukmqt.fcaeo);
		}
	}

	internal DistinguishedName wxetk
	{
		get
		{
			if (jcfvd == null || 1 == 0)
			{
				return null;
			}
			DistinguishedName[] array = jcfvd.xngqi();
			if (array.Length == 0 || 1 == 0)
			{
				return null;
			}
			return array[0];
		}
	}

	public void rntji(string p0)
	{
		rendp = null;
		if (jcfvd == null || 1 == 0)
		{
			jcfvd = new azfwz();
		}
		jcfvd.gfsfs(p0, ukmqt.fcaeo);
	}

	public string[] yxays()
	{
		if (jcfvd != null && 0 == 0)
		{
			return jcfvd.hhoay(ukmqt.fcaeo);
		}
		return null;
	}

	private void hnlva(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.cxxlq, p0, p1);
		switch (p2)
		{
		case 65536:
			ksiug = (jcfvd = new azfwz());
			p0 = rmkkr.osptv;
			break;
		case 65537:
			ksiug = (rendp = new nnzwd());
			p0 = rmkkr.wguaf;
			break;
		default:
			throw new Exception();
		}
		ksiug.zkxnk(p0, p1, p2);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in hnlva
		this.hnlva(p0, p1, p2);
	}

	private lnabj xunqp(rmkkr p0, bool p1, int p2)
	{
		return ksiug.qaqes(p0, p1, p2);
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xunqp
		return this.xunqp(p0, p1, p2);
	}

	private void nzanr(byte[] p0, int p1, int p2)
	{
		ksiug.lnxah(p0, p1, p2);
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in nzanr
		this.nzanr(p0, p1, p2);
	}

	private void lxpzv()
	{
		ksiug.somzq();
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in lxpzv
		this.lxpzv();
	}

	private void lvuyq(fxakl p0)
	{
		if (jcfvd != null && 0 == 0)
		{
			p0.uuhqt(0, jcfvd);
		}
		else
		{
			p0.uuhqt(1, rendp);
		}
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in lvuyq
		this.lvuyq(p0);
	}
}
