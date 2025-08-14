using System;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class zjcch : jcgcz
{
	public zjcch()
	{
	}

	public zjcch(int value)
		: base(rmkkr.sklxf, ubvim(value))
	{
	}

	public zjcch(byte[] data)
		: base(rmkkr.sklxf, eixdf(data))
	{
	}

	public zjcch(byte[] data, bool allowNegative)
		: this(((allowNegative ? true : false) ? bdjih.alcom(data) : bdjih.foxoi(data)).kskce(p0: true))
	{
	}

	private static byte[] eixdf(byte[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("data");
		}
		return (byte[])p0.Clone();
	}

	private static byte[] ubvim(int p0)
	{
		return ((bdjih)p0).kskce(p0: true);
	}

	public override void zkxnk(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.sklxf, p0, p1);
		base.zkxnk(p0, p1, p2);
	}

	public int kybig()
	{
		return bdjih.alcom(base.rtrhq).pcwqf();
	}

	public static zjcch yowmh(byte[] p0)
	{
		zjcch zjcch2 = new zjcch();
		hfnnn.oalpn(zjcch2, p0, 0, p0.Length);
		return zjcch2;
	}

	public static byte[] euzxs(byte[] p0)
	{
		return jlfbq.cnbay(p0);
	}

	public static byte[] bxisb(byte[] p0, int p1, int p2)
	{
		return jlfbq.elzlr(p0, p1, p2, CryptoHelper.zdhth);
	}

	public static bool wduyr(byte[] p0, byte[] p1)
	{
		return jlfbq.oreja(p0, p1);
	}
}
