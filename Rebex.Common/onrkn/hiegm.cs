using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Rebex.Security.Certificates;

namespace onrkn;

internal class hiegm : lnabj
{
	private const int sbhbp = 0;

	private zjcch hjurf;

	private rporh zwvxm;

	private gfiwx tjjrc;

	private itxgi cobmg;

	private mcwjl noyap;

	public itxgi kndop => cobmg;

	public mcwjl atlhx
	{
		get
		{
			if (noyap != null && 0 == 0)
			{
				return noyap;
			}
			return noyap = mcwjl.mfjid();
		}
	}

	internal hiegm()
	{
	}

	public hiegm(DistinguishedName responder, DateTime producedAt, itxgi responses, params zyked[] extensions)
	{
		zwvxm = new rporh(new ukjdk(responder), 1);
		tjjrc = new gfiwx(producedAt, rmkkr.nwijl);
		cobmg = responses;
		if (extensions != null && 0 == 0 && extensions.Length > 0)
		{
			noyap = mcwjl.mfjid(extensions);
		}
	}

	public void zkxnk(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	public lnabj qaqes(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 65536:
			hjurf = new zjcch();
			return new rporh(hjurf, 0);
		case 0:
			tjjrc = new gfiwx();
			return tjjrc;
		case 1:
			cobmg = new itxgi();
			return cobmg;
		case 65537:
			if (zwvxm == null || 1 == 0)
			{
				return zwvxm = new rporh(btngx.ttzrc(p0, p1, p2), 1);
			}
			noyap = new mcwjl();
			return new rporh(noyap, 1);
		case 65538:
			return zwvxm = new rporh(btngx.ttzrc(p0, p1, p2), 2);
		default:
			return null;
		}
	}

	public void somzq()
	{
		if (zwvxm == null || 1 == 0)
		{
			throw new CryptographicException("Responder not found in BasicOcspResponse.");
		}
		if (tjjrc == null || 1 == 0)
		{
			throw new CryptographicException("ProducedAt not found in BasicOcspResponse.");
		}
		if (cobmg == null || 1 == 0)
		{
			throw new CryptographicException("Responses not found in BasicOcspResponse.");
		}
		if (noyap != null && 0 == 0)
		{
			noyap.hksnh();
		}
	}

	public void lnxah(byte[] p0, int p1, int p2)
	{
	}

	public void vlfdh(fxakl p0)
	{
		List<lnabj> list = new List<lnabj>();
		list.Add(zwvxm);
		list.Add(tjjrc);
		list.Add(cobmg);
		List<lnabj> list2 = list;
		if (hjurf != null && 0 == 0 && hjurf.kybig() != 0 && 0 == 0)
		{
			list2.Insert(0, new rporh(hjurf, 0));
		}
		if (noyap != null && 0 == 0 && noyap.Count > 0)
		{
			list2.Add(new rporh(noyap, 1));
		}
		p0.qjrka(list2);
	}
}
