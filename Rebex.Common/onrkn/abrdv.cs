using System;

namespace onrkn;

internal class abrdv
{
	private static readonly byte[] rzfsw = new byte[24]
	{
		79, 114, 112, 104, 101, 97, 110, 66, 101, 104,
		111, 108, 100, 101, 114, 83, 99, 114, 121, 68,
		111, 117, 98, 116
	};

	public int evfhw => 184;

	public byte[] jrgah(byte[] p0, byte[] p1, int p2)
	{
		return vgflu(p0, p1, p2, rzfsw);
	}

	internal byte[] vgflu(byte[] p0, byte[] p1, int p2, byte[] p3)
	{
		byte[] array = new byte[p3.Length];
		Array.Copy(p3, array, p3.Length);
		fficc fficc2 = fficc.todjk(p0, p1, p2);
		int num = 0;
		if (num != 0)
		{
			goto IL_0025;
		}
		goto IL_0042;
		IL_0025:
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_002a;
		}
		goto IL_0038;
		IL_002a:
		fficc2.zpcqe(array, num2, array, num2);
		num2 += 8;
		goto IL_0038;
		IL_0038:
		if (num2 < array.Length)
		{
			goto IL_002a;
		}
		num++;
		goto IL_0042;
		IL_0042:
		if (num < 64)
		{
			goto IL_0025;
		}
		return array;
	}
}
