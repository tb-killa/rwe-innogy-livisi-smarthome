using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class tsnbe : lnabj
{
	private readonly zjcch gixoc;

	private rwolq pubdf;

	private wyjqw butex;

	private htykq bqlly;

	public wyjqw ckbjk => butex;

	public rwolq rpaiz => pubdf;

	public htykq puxmw => bqlly;

	public tsnbe()
	{
		gixoc = new zjcch();
		pubdf = new rwolq();
	}

	public tsnbe(byte[] privateKey, byte[] publicKey, ObjectIdentifier curve)
	{
		if (privateKey == null || 1 == 0)
		{
			throw new ArgumentNullException("privateKey");
		}
		gixoc = new zjcch(1);
		if (curve != null && 0 == 0)
		{
			privateKey = xgvuu(privateKey, curve.Value);
			butex = new wyjqw(curve);
		}
		if (publicKey != null && 0 == 0)
		{
			bqlly = new htykq(publicKey, 0);
		}
		pubdf = new rwolq(privateKey);
	}

	private static byte[] xgvuu(byte[] p0, string p1)
	{
		int p2 = lpcge.yzaxk(p1);
		try
		{
			p0 = jlfbq.xphpx(p0, p2);
			return p0;
		}
		catch (ArgumentException inner)
		{
			throw new CryptographicException("Unable to parse raw private key.", inner);
		}
	}

	internal static void guhqz(byte[] p0, out byte[] p1, out byte[] p2)
	{
		tsnbe tsnbe2 = new tsnbe();
		hfnnn.qnzgo(tsnbe2, p0);
		if (tsnbe2.puxmw == null || 1 == 0)
		{
			throw new CryptographicException("Missing EC public key.");
		}
		p1 = tsnbe2.rpaiz.rtrhq;
		p2 = tsnbe2.puxmw.lssxa;
	}

	internal static tsnbe kusmi(bgosr p0)
	{
		string text = bpkgq.mjwcm(p0.iztaf);
		if (text == null || 1 == 0)
		{
			throw new CryptographicException("Unsupported curve.");
		}
		if (p0.gwjuq == null || 1 == 0)
		{
			throw new CryptographicException("Missing EC private key.");
		}
		byte[] publicKey = lpcge.spbnp(p0, text);
		return new tsnbe(p0.gwjuq, publicKey, text);
	}

	private void zbsrs(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in zbsrs
		this.zbsrs(p0, p1, p2);
	}

	private lnabj dtjvx(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			return gixoc;
		case 1:
			return pubdf;
		case 65536:
			butex = new wyjqw();
			return new rporh(butex, 0);
		case 65537:
			bqlly = new htykq();
			return new rporh(bqlly, 1);
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in dtjvx
		return this.dtjvx(p0, p1, p2);
	}

	private void ahotf(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ahotf
		this.ahotf(p0, p1, p2);
	}

	private void membl()
	{
		if (butex != null && 0 == 0)
		{
			byte[] array = xgvuu(pubdf.rtrhq, butex.scakm.Value);
			if (array.Length != pubdf.qstkb)
			{
				pubdf = new rwolq(array);
			}
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in membl
		this.membl();
	}

	public void vlfdh(fxakl p0)
	{
		List<lnabj> list = new List<lnabj>();
		list.Add(gixoc);
		list.Add(pubdf);
		if (butex != null && 0 == 0)
		{
			list.Add(new rporh(butex, 0));
		}
		if (bqlly != null && 0 == 0)
		{
			list.Add(new rporh(bqlly, 1));
		}
		p0.suudj(list.ToArray());
	}

	public static tsnbe mpano(byte[] p0, string p1)
	{
		tsnbe tsnbe2 = new tsnbe();
		try
		{
			hfnnn.qnzgo(tsnbe2, p0);
		}
		catch (CryptographicException inner)
		{
			throw new CryptographicException("Unable to parse the key.", inner);
		}
		htykq htykq2 = tsnbe2.puxmw;
		if (htykq2 == null || 1 == 0)
		{
			throw new CryptographicException("Missing private key.");
		}
		rwolq rwolq2 = tsnbe2.rpaiz;
		if (rwolq2 == null || 1 == 0)
		{
			throw new CryptographicException("Missing public key.");
		}
		if (p1 != null && 0 == 0 && (tsnbe2.ckbjk == null || 1 == 0))
		{
			string text = bpkgq.mjwcm(p1);
			if (text != null && 0 == 0)
			{
				tsnbe2 = new tsnbe(rwolq2.rtrhq, htykq2.lssxa, text);
			}
		}
		return tsnbe2;
	}

	public static byte[] hvphu(byte[] p0, string p1)
	{
		tsnbe p2 = mpano(p0, p1);
		return fxakl.kncuz(p2);
	}
}
