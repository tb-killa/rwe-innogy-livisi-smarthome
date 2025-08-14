using Rebex.Security.Certificates;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class cjvew : lnabj
{
	private rmkkr[] juplh = new rmkkr[4];

	private int syepu;

	public static bool mqmde(byte[] p0, int p1, out PrivateKeyInfo.hhmob p2, out KeyAlgorithm p3, out string p4)
	{
		cjvew cjvew2 = new cjvew();
		hfnnn.oalpn(cjvew2, p0, 0, p1);
		p4 = null;
		p3 = KeyAlgorithm.Unsupported;
		p2 = PrivateKeyInfo.hhmob.uemtk;
		if (cjvew2.juplh[0] != rmkkr.sklxf)
		{
			if (cjvew2.syepu == 2)
			{
				if (cjvew2.juplh[1] == rmkkr.zkxoz)
				{
					p2 = PrivateKeyInfo.hhmob.vtdze;
					p3 = KeyAlgorithm.Unsupported;
					return true;
				}
				if (cjvew2.juplh[1] == rmkkr.ysphu)
				{
					p2 = PrivateKeyInfo.hhmob.uemtk;
					p3 = KeyAlgorithm.Unsupported;
					return true;
				}
			}
		}
		else if (cjvew2.syepu >= 3)
		{
			if (cjvew2.juplh[1] == rmkkr.sklxf)
			{
				if (cjvew2.juplh[2] != rmkkr.sklxf)
				{
					return false;
				}
				p4 = "";
				p2 = PrivateKeyInfo.hhmob.dpoma;
				if (cjvew2.syepu == 6)
				{
					p3 = KeyAlgorithm.DSA;
					return true;
				}
				if (cjvew2.syepu >= 9)
				{
					p3 = KeyAlgorithm.RSA;
					return true;
				}
			}
			else if (cjvew2.juplh[2] == rmkkr.zkxoz)
			{
				p2 = PrivateKeyInfo.hhmob.ajugp;
				p3 = KeyAlgorithm.Unsupported;
				return true;
			}
		}
		return false;
	}

	private void luaqz(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in luaqz
		this.luaqz(p0, p1, p2);
	}

	private lnabj jrveu(rmkkr p0, bool p1, int p2)
	{
		syepu = p2 + 1;
		if (p2 < 4)
		{
			juplh[p2] = p0;
		}
		return null;
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in jrveu
		return this.jrveu(p0, p1, p2);
	}

	private void iopge(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in iopge
		this.iopge(p0, p1, p2);
	}

	private void xhped()
	{
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in xhped
		this.xhped();
	}

	public void vlfdh(fxakl p0)
	{
	}
}
