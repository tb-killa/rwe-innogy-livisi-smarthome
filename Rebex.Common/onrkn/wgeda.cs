namespace onrkn;

internal static class wgeda
{
	public const int yllkw = -1;

	private static readonly int[] pqlcz;

	static wgeda()
	{
		pqlcz = new int[256]
		{
			0, 0, 1, 1, 2, 2, 2, 2, 3, 3,
			3, 3, 3, 3, 3, 3, 4, 4, 4, 4,
			4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
			4, 4, 5, 5, 5, 5, 5, 5, 5, 5,
			5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
			5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
			5, 5, 5, 5, 6, 6, 6, 6, 6, 6,
			6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
			6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
			6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
			6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
			6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
			6, 6, 6, 6, 6, 6, 6, 6, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7
		};
	}

	public static int njbpp(uint p0)
	{
		if (p0 >= 65536)
		{
			if (p0 >= 16777216)
			{
				return pqlcz[p0 >> 24] + 24;
			}
			return pqlcz[p0 >> 16] + 16;
		}
		if (p0 >= 256)
		{
			return pqlcz[p0 >> 8] + 8;
		}
		return pqlcz[p0];
	}

	public static int sokja(ulong p0)
	{
		if (p0 >= 4294967296L)
		{
			return njbpp((uint)(p0 >> 32)) + 32;
		}
		return njbpp((uint)p0);
	}

	public static int ybzqm(ulong[] p0)
	{
		return ppvgm(p0, p0.Length - 1);
	}

	public static int pmhhm(ulong[] p0)
	{
		return gvamm(p0, p0.Length - 1);
	}

	public static int gvamm(ulong[] p0, int p1)
	{
		for (int num = p1; num >= 0; num--)
		{
			if (p0[num] != 0)
			{
				int num2 = sokja(p0[num]);
				if (p0[num] != (ulong)(1L << num2 % 64))
				{
					return num * 64 + num2 + 1;
				}
				for (int num3 = num - 1; num3 >= 0; num3--)
				{
					if (p0[num3] != 0)
					{
						return num * 64 + num2 + 1;
					}
				}
				return num * 64 + num2;
			}
		}
		return -1;
	}

	public static int ppvgm(ulong[] p0, int p1)
	{
		for (int num = p1; num >= 0; num--)
		{
			if (p0[num] != 0)
			{
				return sokja(p0[num]) + num * 64;
			}
		}
		return -1;
	}
}
