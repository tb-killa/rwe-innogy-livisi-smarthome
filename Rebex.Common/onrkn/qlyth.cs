namespace onrkn;

internal class qlyth : jcgcz
{
	public qlyth()
	{
	}

	public qlyth(bool value)
		: base(rmkkr.pubvj, ezbbj(value))
	{
	}

	private static byte[] ezbbj(bool p0)
	{
		return new byte[1] { (byte)((p0 ? true : false) ? 255 : 0) };
	}

	public override void zkxnk(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.pubvj, p0, p1);
		base.zkxnk(p0, p1, p2);
	}

	public bool ogtep()
	{
		byte[] array = base.rtrhq;
		if (array == null || false || array.Length != 1)
		{
			return false;
		}
		return array[0] == byte.MaxValue;
	}

	public static qlyth rkmhq(byte[] p0)
	{
		qlyth qlyth2 = new qlyth();
		hfnnn.oalpn(qlyth2, p0, 0, p0.Length);
		return qlyth2;
	}
}
