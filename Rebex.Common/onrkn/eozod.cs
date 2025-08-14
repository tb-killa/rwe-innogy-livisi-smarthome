using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace onrkn;

internal class eozod : lnabj
{
	private ajezg xlawo;

	private hsbey tsriv;

	private gfiwx wuyya;

	private gfiwx rdsym;

	private mcwjl bjztz;

	public ajezg ewoaz => xlawo;

	public hsbey ikelp => tsriv;

	public DateTime cgyfx => wuyya.fzcfd();

	public DateTime? egrdf
	{
		get
		{
			if (rdsym != null && 0 == 0)
			{
				return rdsym.fzcfd();
			}
			return null;
		}
	}

	public mcwjl klguf
	{
		get
		{
			if (bjztz != null && 0 == 0)
			{
				return bjztz;
			}
			return bjztz = mcwjl.mfjid();
		}
	}

	internal eozod()
	{
	}

	public eozod(ajezg certId, hsbey status, DateTime thisUpdate, DateTime? nextUpdate, params zyked[] extensions)
	{
		xlawo = certId;
		tsriv = status;
		wuyya = new gfiwx(thisUpdate, rmkkr.nwijl);
		if (nextUpdate.HasValue && 0 == 0)
		{
			rdsym = new gfiwx(nextUpdate.Value, rmkkr.nwijl);
		}
		if (extensions != null && 0 == 0 && extensions.Length > 0)
		{
			bjztz = mcwjl.mfjid(extensions);
		}
	}

	private void fqhao(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in fqhao
		this.fqhao(p0, p1, p2);
	}

	private lnabj dhlpy(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			xlawo = new ajezg();
			return xlawo;
		case 1:
			wuyya = new gfiwx();
			return wuyya;
		case 65536:
			if (wuyya == null || 1 == 0)
			{
				tsriv = hsbey.xjkwj(p0, p1, p2);
				return tsriv;
			}
			rdsym = new gfiwx();
			return new rporh(rdsym, 0);
		case 65537:
			if (wuyya == null || 1 == 0)
			{
				tsriv = hsbey.xjkwj(p0, p1, p2);
				return tsriv;
			}
			bjztz = new mcwjl();
			return new rporh(bjztz, 1);
		case 65538:
			tsriv = hsbey.xjkwj(p0, p1, p2);
			return tsriv;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in dhlpy
		return this.dhlpy(p0, p1, p2);
	}

	private void kwkwj()
	{
		if (xlawo == null || 1 == 0)
		{
			throw new CryptographicException("CertID not found in BasicOcspResponse.");
		}
		if (tsriv == null || 1 == 0)
		{
			throw new CryptographicException("CertStatus not found in BasicOcspResponse.");
		}
		if (wuyya == null || 1 == 0)
		{
			throw new CryptographicException("ThisUpdate not found in BasicOcspResponse.");
		}
		if (bjztz != null && 0 == 0)
		{
			bjztz.hksnh();
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in kwkwj
		this.kwkwj();
	}

	private void tfryf(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in tfryf
		this.tfryf(p0, p1, p2);
	}

	private void flyqo(fxakl p0)
	{
		List<lnabj> list = new List<lnabj>();
		list.Add(xlawo);
		list.Add(tsriv);
		list.Add(wuyya);
		List<lnabj> list2 = list;
		if (rdsym != null && 0 == 0)
		{
			list2.Add(new rporh(rdsym, 0));
		}
		if (bjztz != null && 0 == 0 && bjztz.Count > 0)
		{
			list2.Add(new rporh(bjztz, 1));
		}
		p0.qjrka(list2);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in flyqo
		this.flyqo(p0);
	}
}
