using System.Collections.Generic;
using System.Security.Cryptography;

namespace onrkn;

internal class avxco : lnabj
{
	private const int etlbj = 0;

	private zjcch gryze;

	private ygomx jfnuy;

	private dyemd otphp;

	private mcwjl swusr;

	public dyemd ritpy => otphp;

	public mcwjl ooxiu
	{
		get
		{
			if (swusr != null && 0 == 0)
			{
				return swusr;
			}
			return swusr = mcwjl.mfjid();
		}
	}

	internal avxco()
	{
	}

	public avxco(ygomx requestorName, ajezg[] certIds, params zyked[] extensions)
	{
		jfnuy = requestorName;
		otphp = new dyemd();
		int num = 0;
		if (num != 0)
		{
			goto IL_001c;
		}
		goto IL_0035;
		IL_001c:
		ajezg certId = certIds[num];
		otphp.Add(new tqhjd(certId));
		num++;
		goto IL_0035;
		IL_0035:
		if (num >= certIds.Length)
		{
			if (extensions != null && 0 == 0 && extensions.Length > 0)
			{
				swusr = mcwjl.mfjid(extensions);
			}
			return;
		}
		goto IL_001c;
	}

	private void rfcqw(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in rfcqw
		this.rfcqw(p0, p1, p2);
	}

	private lnabj fsnzc(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 65536:
			gryze = new zjcch();
			return new rporh(gryze, 0);
		case 65537:
			jfnuy = new ygomx();
			return new rporh(jfnuy, 1);
		case 0:
			otphp = new dyemd();
			return otphp;
		case 65538:
			swusr = new mcwjl();
			return new rporh(swusr, 2);
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in fsnzc
		return this.fsnzc(p0, p1, p2);
	}

	private void krchy()
	{
		if (otphp == null || 1 == 0)
		{
			throw new CryptographicException("RequestList not found in OcspRequest.");
		}
		if (swusr != null && 0 == 0)
		{
			swusr.hksnh();
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in krchy
		this.krchy();
	}

	private void dhwpr(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in dhwpr
		this.dhwpr(p0, p1, p2);
	}

	private void wdkdj(fxakl p0)
	{
		List<lnabj> list = new List<lnabj>();
		if (gryze != null && 0 == 0 && gryze.kybig() != 0 && 0 == 0)
		{
			list.Add(new rporh(gryze, 0));
		}
		if (jfnuy != null && 0 == 0)
		{
			list.Add(new rporh(jfnuy, 1));
		}
		list.Add(otphp);
		if (swusr != null && 0 == 0 && swusr.Count > 0)
		{
			list.Add(new rporh(swusr, 2));
		}
		p0.qjrka(list);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in wdkdj
		this.wdkdj(p0);
	}
}
