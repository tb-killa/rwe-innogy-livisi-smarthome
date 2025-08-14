using System.Security.Cryptography;

namespace onrkn;

internal class rcqtb : lnabj
{
	private readonly zjcch vpnmp = new zjcch();

	private readonly zjcch upxai = new zjcch();

	private bool svexg;

	public rcqtb()
	{
	}

	public rcqtb(RSAParameters rp)
	{
		vpnmp = new zjcch(rp.Modulus, allowNegative: false);
		upxai = new zjcch(rp.Exponent, allowNegative: false);
	}

	public RSAParameters hqyyf()
	{
		return new RSAParameters
		{
			Modulus = zjcch.euzxs(vpnmp.rtrhq),
			Exponent = zjcch.euzxs(upxai.rtrhq)
		};
	}

	private void wdkzu(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in wdkzu
		this.wdkzu(p0, p1, p2);
	}

	private lnabj qoaks(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			return vpnmp;
		case 1:
			svexg = true;
			return upxai;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in qoaks
		return this.qoaks(p0, p1, p2);
	}

	private void sojxg(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in sojxg
		this.sojxg(p0, p1, p2);
	}

	private void xdwai()
	{
		if (!svexg || 1 == 0)
		{
			throw new CryptographicException("Invalid RsaPublicKey.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in xdwai
		this.xdwai();
	}

	public void vlfdh(fxakl p0)
	{
		p0.suudj(vpnmp, upxai);
	}
}
