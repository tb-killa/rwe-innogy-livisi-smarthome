using System;
using System.Security.Cryptography;

namespace onrkn;

internal class lmjia : lnabj
{
	private zjcch pnbjt;

	private zjcch sufqi;

	public byte[] xzlsp()
	{
		return jlfbq.cnbay(pnbjt.rtrhq);
	}

	public byte[] cgiaw()
	{
		return jlfbq.cnbay(sufqi.rtrhq);
	}

	public lmjia()
	{
	}

	public lmjia(byte[] signature)
	{
		if (signature == null || 1 == 0)
		{
			throw new ArgumentNullException("signature");
		}
		int num = signature.Length;
		if (num == 0 || false || (num & 1) == 1)
		{
			throw new ArgumentException("Invalid signature.", "signature");
		}
		num /= 2;
		byte[] array = new byte[num];
		byte[] array2 = new byte[num];
		Array.Copy(signature, 0, array, 0, num);
		Array.Copy(signature, num, array2, 0, num);
		pnbjt = new zjcch(jlfbq.twxvm(array));
		sufqi = new zjcch(jlfbq.twxvm(array2));
	}

	public lmjia(byte[] r, byte[] s)
	{
		if (r == null || 1 == 0)
		{
			throw new ArgumentNullException("r");
		}
		if (s == null || 1 == 0)
		{
			throw new ArgumentNullException("r");
		}
		pnbjt = new zjcch(jlfbq.twxvm(r));
		sufqi = new zjcch(jlfbq.twxvm(s));
	}

	public byte[] czcrz(int p0)
	{
		if (p0 == 0 || false || (p0 & 1) == 1)
		{
			throw new ArgumentException("Invalid signature.", "signature");
		}
		byte[] array = new byte[p0];
		p0 /= 2;
		byte[] sourceArray = zjcch.bxisb(pnbjt.rtrhq, p0, p0);
		byte[] sourceArray2 = zjcch.bxisb(sufqi.rtrhq, p0, p0);
		Array.Copy(sourceArray, 0, array, 0, p0);
		Array.Copy(sourceArray2, 0, array, p0, p0);
		return array;
	}

	private void rvrsy(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in rvrsy
		this.rvrsy(p0, p1, p2);
	}

	private lnabj xmbhk(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			pnbjt = new zjcch();
			return pnbjt;
		case 1:
			sufqi = new zjcch();
			return sufqi;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xmbhk
		return this.xmbhk(p0, p1, p2);
	}

	private void lscxr(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in lscxr
		this.lscxr(p0, p1, p2);
	}

	private void dcxvf()
	{
		if (pnbjt == null || 1 == 0)
		{
			throw new CryptographicException("R value not found in DSS signature.");
		}
		if (sufqi == null || 1 == 0)
		{
			throw new CryptographicException("S value not found in DSS signature.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in dcxvf
		this.dcxvf();
	}

	private void nbsxw(fxakl p0)
	{
		p0.suudj(pnbjt, sufqi);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in nbsxw
		this.nbsxw(p0);
	}

	public static byte[] bhrbh(byte[] p0, int p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("signature");
		}
		lmjia lmjia2 = new lmjia();
		hfnnn.qnzgo(lmjia2, p0);
		return lmjia2.czcrz(p1);
	}

	public static byte[] xfocq(byte[] p0, int p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("signature");
		}
		if (p0.Length != p1)
		{
			throw new ArgumentException("Invalid signature.", "signature");
		}
		lmjia p2 = new lmjia(p0);
		return fxakl.kncuz(p2);
	}
}
