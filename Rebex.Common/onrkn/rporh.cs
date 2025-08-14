using System;
using System.Security.Cryptography;

namespace onrkn;

internal class rporh : lnabj
{
	private readonly lnabj dhtbw;

	private readonly int ybxpg;

	private bool hnhwd;

	public rporh(lnabj inner, int tag)
	{
		if (inner == null || 1 == 0)
		{
			throw new ArgumentNullException("inner");
		}
		dhtbw = inner;
		ybxpg = tag;
	}

	public void zkxnk(rmkkr p0, bool p1, int p2)
	{
		if (!p1 || 1 == 0)
		{
			throw new CryptographicException("Primitive explicit node encountered.");
		}
		hfnnn.xmjay(rmkkr.cxxlq, p0, p1);
	}

	public lnabj qaqes(rmkkr p0, bool p1, int p2)
	{
		if (hnhwd && 0 == 0)
		{
			throw new CryptographicException("Explicit node with more than one underlying node encountered.");
		}
		lnabj result = dhtbw;
		hnhwd = true;
		return result;
	}

	public void lnxah(byte[] p0, int p1, int p2)
	{
	}

	public void somzq()
	{
	}

	private void khxtk(fxakl p0)
	{
		p0.xadip(ybxpg, dhtbw);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in khxtk
		this.khxtk(p0);
	}
}
