using System;
using System.IO;
using System.Security.Cryptography;

namespace onrkn;

internal class tmygz : lnabj
{
	private nnzwd hmqiz;

	private avxco eheos;

	private wzdny pwkfe;

	public bool fbkho => pwkfe != null;

	public dyemd ypajc => eheos.ritpy;

	public mcwjl jduoj => eheos.ooxiu;

	public tmygz()
	{
	}

	public tmygz(ajezg certId, params zyked[] extensions)
		: this(new ajezg[1] { certId }, extensions)
	{
	}

	public tmygz(ajezg[] certIds, params zyked[] extensions)
	{
		if (certIds == null || false || certIds.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Collection cannot be empty.", "certIds");
		}
		eheos = new avxco(null, certIds, extensions);
		hmqiz = new nnzwd(fxakl.kncuz(eheos));
	}

	private void ywrmq(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ywrmq
		this.ywrmq(p0, p1, p2);
	}

	private lnabj lfech(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			hmqiz = new nnzwd();
			return hmqiz;
		case 65536:
			pwkfe = new wzdny();
			return new rporh(pwkfe, 0);
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in lfech
		return this.lfech(p0, p1, p2);
	}

	private void dzdvc()
	{
		if (hmqiz == null || 1 == 0)
		{
			throw new CryptographicException("TBSRequest not found in OcspRequest.");
		}
		eheos = new avxco();
		hfnnn.qnzgo(eheos, hmqiz.lktyp);
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in dzdvc
		this.dzdvc();
	}

	private void gzjgs(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in gzjgs
		this.gzjgs(p0, p1, p2);
	}

	private void avfnq(fxakl p0)
	{
		if (pwkfe == null || 1 == 0)
		{
			p0.suudj(hmqiz);
		}
		else
		{
			p0.suudj(hmqiz, new rporh(pwkfe, 0));
		}
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in avfnq
		this.avfnq(p0);
	}

	public static tmygz vukxt(Stream p0)
	{
		tmygz tmygz2 = new tmygz();
		hfnnn hfnnn2 = new hfnnn(tmygz2);
		try
		{
			p0.alskc(hfnnn2);
			return tmygz2;
		}
		finally
		{
			if (hfnnn2 != null && 0 == 0)
			{
				((IDisposable)hfnnn2).Dispose();
			}
		}
	}
}
