using System;
using System.Collections.Generic;

namespace onrkn;

internal class daaba : IComparer<jnkze>
{
	private static daaba tgoni;

	public static daaba wtjus => tgoni;

	static daaba()
	{
		tgoni = new daaba();
	}

	public int Compare(jnkze x, jnkze y)
	{
		if (x.dclzl.Length < y.dclzl.Length)
		{
			return -1;
		}
		if (x.dclzl.Length > y.dclzl.Length)
		{
			return 1;
		}
		return StringComparer.OrdinalIgnoreCase.Compare(x.dclzl, y.dclzl);
	}
}
