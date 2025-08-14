using System;

namespace onrkn;

internal class rwolq : jcgcz
{
	internal rwolq()
	{
	}

	public rwolq(byte[] data)
		: this(data, clone: true)
	{
	}

	public rwolq(byte[] data, int offset, int count)
		: this(hnvrr(data, offset, count), clone: true)
	{
	}

	public rwolq(byte[] data, bool clone)
		: base(rmkkr.zkxoz, (clone ? true : false) ? hnvrr(data, 0, data.Length) : data)
	{
	}

	private static byte[] hnvrr(byte[] p0, int p1, int p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("data");
		}
		byte[] array = new byte[p2];
		Array.Copy(p0, p1, array, 0, p2);
		return array;
	}

	public override void zkxnk(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.zkxoz, p0, p1);
		base.zkxnk(p0, p1, p2);
	}

	public static rwolq tvjgt(byte[] p0)
	{
		rwolq rwolq2 = new rwolq();
		hfnnn.oalpn(rwolq2, p0, 0, p0.Length);
		return rwolq2;
	}
}
