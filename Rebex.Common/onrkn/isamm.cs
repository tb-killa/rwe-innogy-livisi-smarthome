using System.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class isamm : lnabj
{
	private AlgorithmIdentifier atksd;

	private nnzwd ncrsl;

	public AlgorithmIdentifier xrkhc => atksd;

	public byte[] boazh => ncrsl.lktyp;

	private void tdxix(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in tdxix
		this.tdxix(p0, p1, p2);
	}

	private lnabj lycvp(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			atksd = new AlgorithmIdentifier();
			return atksd;
		case 1:
			ncrsl = new nnzwd();
			return ncrsl;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in lycvp
		return this.lycvp(p0, p1, p2);
	}

	private void arsxc(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in arsxc
		this.arsxc(p0, p1, p2);
	}

	private void dkerc()
	{
		if (atksd == null || 1 == 0)
		{
			throw new CryptographicException("Originator identifier does not contain an algorithm.");
		}
		if (ncrsl == null || 1 == 0)
		{
			throw new CryptographicException("Originator identifier does not contain a public key.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in dkerc
		this.dkerc();
	}

	private void wmzfl(fxakl p0)
	{
		p0.aiflg(rmkkr.osptv, new lnabj[2] { atksd, ncrsl });
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in wmzfl
		this.wmzfl(p0);
	}
}
