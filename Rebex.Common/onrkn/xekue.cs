using System;

namespace onrkn;

internal class xekue
{
	public static nxtme<byte> vyusq(nxtme<byte> p0, nxtme<byte> p1, nxtme<byte> p2)
	{
		if (p0.hvbtp && 0 == 0 && p1.hvbtp && 0 == 0 && p2.hvbtp && 0 == 0)
		{
			return p2;
		}
		if (p0.tvoem != p1.tvoem || p0.tvoem != p2.tvoem)
		{
			throw new ArgumentException("All views should have same length.");
		}
		jlfbq.cfvhy(p0.trkhv(), 0, p1.trkhv(), 0, p2.trkhv(), 0, p0.tvoem);
		return p2;
	}
}
