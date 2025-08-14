using System;
using Rebex.IO;

namespace onrkn;

internal static class omcud
{
	public static void vnkle(string p0, TraversalMode p1, string p2)
	{
		mjuri(p0, p1, dahxy.ujgsn, p2);
	}

	public static void spadz(string p0, TraversalMode p1, char[] p2, string p3)
	{
		mjuri(p0, p1, p2, p3);
	}

	private static void mjuri(string p0, TraversalMode p1, char[] p2, string p3)
	{
		if (string.IsNullOrEmpty(p0) && 0 == 0)
		{
			return;
		}
		int num = p0.IndexOfAny(dahxy.fmacz);
		if (num >= 0)
		{
			int num2 = brgjd.pkosy(p0, p2);
			if (num < num2)
			{
				throw new ArgumentException("Illegal use of wildcards in path.", p3);
			}
		}
		if (p1 != TraversalMode.MatchFilesDeep && p1 != TraversalMode.MatchFilesShallow)
		{
			return;
		}
		bool flag = false;
		int num3 = p0.Length - 1;
		if (axqcb(p2, p0[num3]) && 0 == 0)
		{
			flag = true;
			if (flag)
			{
				goto IL_0104;
			}
		}
		if (p0[num3] == '.')
		{
			num3--;
			switch (p0.Length)
			{
			case 1:
				flag = true;
				if (flag)
				{
					break;
				}
				goto case 2;
			case 2:
				if (p0[num3] != '.' && !axqcb(p2, p0[num3]))
				{
					break;
				}
				flag = true;
				if (flag)
				{
					break;
				}
				goto default;
			default:
				if (axqcb(p2, p0[num3]) && 0 == 0)
				{
					flag = true;
					if (flag)
					{
						break;
					}
				}
				if (p0[num3] == '.')
				{
					flag = axqcb(p2, p0[num3 - 1]);
				}
				break;
			}
		}
		goto IL_0104;
		IL_0104:
		if (flag && 0 == 0)
		{
			throw new ArgumentException("Ambiguous usage of path and mode.", p3);
		}
	}

	private static bool axqcb<T>(T[] p0, T p1)
	{
		return Array.IndexOf(p0, p1) >= 0;
	}
}
