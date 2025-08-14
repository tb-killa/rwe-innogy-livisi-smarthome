using System.Net;
using Rebex;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class ygomx : lnabj
{
	private ukmqt uohso;

	private lnabj dfbse;

	internal lnabj wntxx => dfbse;

	public ukmqt jcvng => uohso;

	public ygomx()
	{
	}

	public ygomx(ObjectIdentifier type, lnabj value)
	{
		uohso = ukmqt.oduln;
		dfbse = new ffhjw(type, value);
	}

	public ygomx(string name, ukmqt kind)
	{
		uohso = kind;
		dfbse = new vesyi(rmkkr.dzwiy, EncodingTools.ASCII, name);
	}

	public ygomx(IPAddress address)
	{
		uohso = ukmqt.kwrqu;
		dfbse = new rwolq(address.GetAddressBytes());
	}

	public void zkxnk(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.cxxlq, p0, p1);
		uohso = (ukmqt)(p2 - 65536);
		switch (uohso)
		{
		case ukmqt.oduln:
			dfbse = new ffhjw();
			p0 = rmkkr.osptv;
			break;
		case ukmqt.yzwxj:
		case ukmqt.zhlgm:
		case ukmqt.fcaeo:
			dfbse = new vesyi();
			p0 = rmkkr.dzwiy;
			break;
		case ukmqt.isxiv:
			dfbse = new ukjdk();
			p0 = rmkkr.osptv;
			break;
		case ukmqt.kwrqu:
			dfbse = new rwolq();
			p0 = rmkkr.zkxoz;
			break;
		default:
			dfbse = new nnzwd();
			p0 = rmkkr.motgn;
			break;
		}
		if (p0 != rmkkr.motgn)
		{
			dfbse.zkxnk(p0, p1, p2);
		}
	}

	public lnabj qaqes(rmkkr p0, bool p1, int p2)
	{
		return dfbse.qaqes(p0, p1, p2);
	}

	public void lnxah(byte[] p0, int p1, int p2)
	{
		dfbse.lnxah(p0, p1, p2);
	}

	public void somzq()
	{
		dfbse.somzq();
	}

	private void pmjkf(fxakl p0)
	{
		p0.uuhqt((int)uohso, dfbse);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in pmjkf
		this.pmjkf(p0);
	}
}
