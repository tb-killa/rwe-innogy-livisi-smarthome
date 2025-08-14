using Rebex.Security.Cryptography;

namespace onrkn;

internal class ffhjw : lnabj
{
	private readonly wyjqw biftr;

	private readonly nnzwd zjabo;

	public ObjectIdentifier bxqwr => biftr.scakm;

	public byte[] iuugy
	{
		get
		{
			if (zjabo == null || 1 == 0)
			{
				return null;
			}
			return zjabo.lktyp;
		}
	}

	public ffhjw()
	{
		biftr = new wyjqw();
		zjabo = new nnzwd();
	}

	public ffhjw(ObjectIdentifier type, lnabj value)
	{
		biftr = new wyjqw(type);
		zjabo = new nnzwd(fxakl.kncuz(value));
	}

	private void mqymp(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in mqymp
		this.mqymp(p0, p1, p2);
	}

	private lnabj xmvyi(rmkkr p0, bool p1, int p2)
	{
		return p2 switch
		{
			0 => biftr, 
			65536 => new rporh(zjabo, 0), 
			_ => null, 
		};
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xmvyi
		return this.xmvyi(p0, p1, p2);
	}

	private void olgcv(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in olgcv
		this.olgcv(p0, p1, p2);
	}

	private void jkzfw()
	{
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in jkzfw
		this.jkzfw();
	}

	private void eumha(fxakl p0)
	{
		p0.suudj(biftr, new rporh(zjabo, 0));
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in eumha
		this.eumha(p0);
	}
}
