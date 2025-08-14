using Rebex.Security.Cryptography;

namespace onrkn;

internal class wusby : lnabj
{
	private readonly wyjqw tzzef;

	private readonly nnzwd hcabu;

	public ObjectIdentifier sdyid => tzzef.scakm;

	public byte[] fajfk => hcabu.lktyp;

	public wusby()
	{
		tzzef = new wyjqw();
		hcabu = new nnzwd();
	}

	public wusby(ObjectIdentifier oid, lnabj value)
	{
		tzzef = new wyjqw(oid);
		hcabu = new nnzwd(fxakl.kncuz(value));
	}

	private void wcapc(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in wcapc
		this.wcapc(p0, p1, p2);
	}

	private lnabj dozzg(rmkkr p0, bool p1, int p2)
	{
		return p2 switch
		{
			0 => tzzef, 
			1 => hcabu, 
			_ => null, 
		};
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in dozzg
		return this.dozzg(p0, p1, p2);
	}

	private void xhsew(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xhsew
		this.xhsew(p0, p1, p2);
	}

	private void kwuly()
	{
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in kwuly
		this.kwuly();
	}

	private void fgala(fxakl p0)
	{
		p0.suudj(tzzef, hcabu);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in fgala
		this.fgala(p0);
	}
}
