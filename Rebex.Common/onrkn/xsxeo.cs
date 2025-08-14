using System.Security.Cryptography;

namespace onrkn;

internal class xsxeo : lnabj
{
	public const string dvphe = "1.3.6.1.5.5.7.1.1";

	public const string iyxbd = "1.3.6.1.5.5.7.48.1";

	public const string nmokc = "1.3.6.1.5.5.7.48.2";

	private wyjqw xlvbm;

	private ygomx dsfpg;

	public rvoup uxfmu
	{
		get
		{
			string value;
			if ((value = xlvbm.scakm.Value) != null && 0 == 0)
			{
				if (value == "1.3.6.1.5.5.7.48.1")
				{
					return rvoup.nyjao;
				}
				if (value == "1.3.6.1.5.5.7.48.2")
				{
					return rvoup.jtwrs;
				}
			}
			return rvoup.tllkg;
		}
	}

	public string mpfci
	{
		get
		{
			if (!(dsfpg.wntxx is vesyi vesyi2) || 1 == 0)
			{
				return null;
			}
			return vesyi2.dcokg;
		}
	}

	internal xsxeo()
	{
	}

	public xsxeo(rvoup method, string url)
	{
		xlvbm = new wyjqw(ivyoy(method));
		dsfpg = new ygomx(url, ukmqt.fcaeo);
	}

	public static xsxeo dodij(string p0)
	{
		return new xsxeo(rvoup.nyjao, p0);
	}

	public static xsxeo kjcwf(string p0)
	{
		return new xsxeo(rvoup.jtwrs, p0);
	}

	private static string ivyoy(rvoup p0)
	{
		return p0 switch
		{
			rvoup.nyjao => "1.3.6.1.5.5.7.48.1", 
			rvoup.jtwrs => "1.3.6.1.5.5.7.48.2", 
			_ => throw hifyx.nztrs("method", p0, "Argument is out of range of valid values."), 
		};
	}

	private void tjuaf(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in tjuaf
		this.tjuaf(p0, p1, p2);
	}

	private lnabj qlsdm(rmkkr p0, bool p1, int p2)
	{
		if (p2 == 0)
		{
			return xlvbm = new wyjqw();
		}
		if (p2 < 65536 || dsfpg != null)
		{
			return null;
		}
		return dsfpg = new ygomx();
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in qlsdm
		return this.qlsdm(p0, p1, p2);
	}

	private void tvixz(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in tvixz
		this.tvixz(p0, p1, p2);
	}

	private void grpkv()
	{
		if (xlvbm == null || 1 == 0)
		{
			throw new CryptographicException("AccessMethod not found in AccessDescription.");
		}
		if (dsfpg == null || 1 == 0)
		{
			throw new CryptographicException("AccessLocation not found in AccessDescription.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in grpkv
		this.grpkv();
	}

	private void xhzfw(fxakl p0)
	{
		p0.suudj(xlvbm, dsfpg);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xhzfw
		this.xhzfw(p0);
	}
}
