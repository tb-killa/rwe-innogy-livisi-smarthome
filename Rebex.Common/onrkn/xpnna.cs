using System.Security.Cryptography;

namespace onrkn;

internal class xpnna : lnabj
{
	private wyjqw ndllv;

	private rwolq nkwyh;

	private cgxdn pswxy;

	public cgxdn qehke => pswxy;

	internal xpnna()
	{
	}

	public xpnna(cgxdn response)
	{
		ndllv = new wyjqw(cgxdn.pfvpw);
		pswxy = response;
		nkwyh = new rwolq(fxakl.kncuz(response));
	}

	public void zkxnk(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	public lnabj qaqes(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			ndllv = new wyjqw();
			return ndllv;
		case 1:
			nkwyh = new rwolq();
			return nkwyh;
		default:
			return null;
		}
	}

	public void somzq()
	{
		if (ndllv == null || 1 == 0)
		{
			throw new CryptographicException("ResponseType not found in OcspResponse.");
		}
		if (nkwyh == null || 1 == 0)
		{
			throw new CryptographicException("Response data not found in OcspResponse.");
		}
		if (ndllv.scakm.Value != cgxdn.pfvpw.Value && 0 == 0)
		{
			throw new CryptographicException(brgjd.edcru("Encountered unsupported ResponseType in OcspResponse ({0}).", ndllv.scakm));
		}
		pswxy = new cgxdn();
		hfnnn.qnzgo(pswxy, nkwyh.rtrhq);
	}

	public void lnxah(byte[] p0, int p1, int p2)
	{
	}

	public void vlfdh(fxakl p0)
	{
		p0.suudj(ndllv, nkwyh);
	}
}
