using System;

namespace onrkn;

internal class rwknq : lnabj
{
	private lnabj ywnys;

	private readonly int ghyze;

	private readonly rmkkr wqnga;

	internal lnabj iiebl => ywnys;

	public int tshfh => ghyze;

	public rwknq(lnabj inner, int tag, rmkkr type)
	{
		if (inner == null || 1 == 0)
		{
			throw new ArgumentNullException("inner");
		}
		ywnys = inner;
		ghyze = tag;
		wqnga = type;
	}

	public void zkxnk(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.cxxlq, p0, p1);
		ywnys.zkxnk(wqnga, p1, p2);
	}

	public lnabj qaqes(rmkkr p0, bool p1, int p2)
	{
		return ywnys.qaqes(p0, p1, p2);
	}

	public void lnxah(byte[] p0, int p1, int p2)
	{
		ywnys.lnxah(p0, p1, p2);
	}

	public void somzq()
	{
		ywnys.somzq();
	}

	private void solox(fxakl p0)
	{
		p0.uuhqt(ghyze, ywnys);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in solox
		this.solox(p0);
	}
}
