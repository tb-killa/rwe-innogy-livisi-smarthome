using System.Security.Cryptography;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class rhegb : lnabj
{
	private readonly wyjqw fnxzi;

	private lnabj sxbyk;

	private readonly bool vwxsa;

	private readonly ObjectIdentifier yvznc;

	private bool muvpv;

	public ObjectIdentifier nsfih => fnxzi.scakm;

	internal rhegb(bool detectOnly)
	{
		vwxsa = detectOnly;
		fnxzi = new wyjqw();
	}

	internal rhegb(lnabj content, ObjectIdentifier expectedContentType)
	{
		fnxzi = new wyjqw(expectedContentType);
		yvznc = expectedContentType;
		sxbyk = content;
	}

	protected virtual lnabj qqsit()
	{
		return null;
	}

	private void htnpo(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in htnpo
		this.htnpo(p0, p1, p2);
	}

	private lnabj lglqc(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			return fnxzi;
		default:
			return null;
		case 65536:
			muvpv = true;
			if (yvznc == null || 1 == 0)
			{
				if (vwxsa && 0 == 0)
				{
					return null;
				}
				sxbyk = qqsit();
				if (sxbyk == null || 1 == 0)
				{
					throw new CryptographicException("Unsupported content type '" + fnxzi.scakm.Value + "'.");
				}
			}
			else if (yvznc.Value != fnxzi.scakm.Value && 0 == 0)
			{
				throw new CryptographicException("Unexpected content type.");
			}
			return new rporh(sxbyk, 0);
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in lglqc
		return this.lglqc(p0, p1, p2);
	}

	private void spvby(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in spvby
		this.spvby(p0, p1, p2);
	}

	private void otkov()
	{
		if (!muvpv || 1 == 0)
		{
			throw new CryptographicException("Content was not found.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in otkov
		this.otkov();
	}

	private void slkhb(fxakl p0)
	{
		p0.suudj(fnxzi, new rporh(sxbyk, 0));
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in slkhb
		this.slkhb(p0);
	}
}
