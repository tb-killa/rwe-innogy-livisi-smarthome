using System.Security.Cryptography;

namespace onrkn;

internal class dyhlg : rporh, lnabj
{
	private readonly lnabj drebx;

	private bool rocqd;

	public dyhlg(lnabj inner, int tag)
		: base(inner, tag)
	{
		drebx = inner;
	}

	private void kswye(rmkkr p0, bool p1, int p2)
	{
		rocqd = p1;
		hfnnn.xmjay(rmkkr.cxxlq, p0, p2: true);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in kswye
		this.kswye(p0, p1, p2);
	}

	private lnabj euumd(rmkkr p0, bool p1, int p2)
	{
		if (!rocqd || false || !p1 || 1 == 0)
		{
			throw new CryptographicException("Unable to parse explicit node.");
		}
		return qaqes(p0, p1, p2);
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in euumd
		return this.euumd(p0, p1, p2);
	}

	private void xyxmp(byte[] p0, int p1, int p2)
	{
		if (rocqd && 0 == 0)
		{
			throw new CryptographicException("Unable to parse explicit node.");
		}
		drebx.lnxah(p0, p1, p2);
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xyxmp
		this.xyxmp(p0, p1, p2);
	}
}
