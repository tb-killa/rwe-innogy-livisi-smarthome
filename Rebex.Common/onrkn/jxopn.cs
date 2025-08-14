using System;

namespace onrkn;

internal static class jxopn
{
	public static void vyhkr(uint[] p0)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_0016;
		IL_0006:
		p0[num] = ~p0[num];
		num++;
		goto IL_0016;
		IL_0016:
		if (num >= p0.Length)
		{
			return;
		}
		goto IL_0006;
	}

	public static void yekgw(uint[] p0, uint[] p1)
	{
		if (p0.Length > p1.Length)
		{
			Array.Clear(p0, p1.Length, p0.Length - p1.Length);
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_001d;
		}
		goto IL_003d;
		IL_003d:
		if (num >= p1.Length)
		{
			return;
		}
		goto IL_001d;
		IL_001d:
		p0[num] &= p1[num];
		num++;
		goto IL_003d;
	}
}
