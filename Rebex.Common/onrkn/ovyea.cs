using System;

namespace onrkn;

internal class ovyea : jcgcz
{
	public int? akyte
	{
		get
		{
			if (base.rtrhq == null || false || base.rtrhq.Length != 1)
			{
				return null;
			}
			return base.rtrhq[0];
		}
	}

	internal ovyea()
	{
	}

	public ovyea(int value)
		: this(new byte[1] { (byte)value }, clone: false)
	{
		if (value <= 255)
		{
			return;
		}
		throw hifyx.nztrs("value", value, "Value must be in range of 0-255.");
	}

	public ovyea(byte[] data)
		: this(data, clone: true)
	{
	}

	public ovyea(byte[] data, bool clone)
		: base(rmkkr.wbdro, (clone ? true : false) ? lhthq(data) : data)
	{
	}

	private static byte[] lhthq(byte[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("data");
		}
		return (byte[])p0.Clone();
	}

	public override void zkxnk(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.wbdro, p0, p1);
		base.zkxnk(p0, p1, p2);
	}

	public static ovyea ncquy(byte[] p0)
	{
		ovyea ovyea2 = new ovyea();
		hfnnn.oalpn(ovyea2, p0, 0, p0.Length);
		return ovyea2;
	}
}
