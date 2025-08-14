using System;
using System.Collections;

namespace Rebex;

public class MultiComparer : IComparer
{
	private IComparer[] jawsw;

	public MultiComparer(params IComparer[] comparerChain)
	{
		if (comparerChain == null || 1 == 0)
		{
			throw new ArgumentException("Argument comparerChain can't be null.");
		}
		if (comparerChain.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("At least one IComparer must be set in comparerChain.");
		}
		bool flag = false;
		int num = 0;
		if (num != 0)
		{
			goto IL_003b;
		}
		goto IL_0051;
		IL_003b:
		if (comparerChain[num] != null && 0 == 0)
		{
			flag = true;
			if (flag)
			{
				goto IL_0057;
			}
		}
		num++;
		goto IL_0051;
		IL_0051:
		if (num < comparerChain.Length)
		{
			goto IL_003b;
		}
		goto IL_0057;
		IL_0057:
		if (!flag || 1 == 0)
		{
			throw new ArgumentException("Argument comparerChain must contain at least one instance of IComparer.");
		}
		jawsw = comparerChain;
	}

	public int Compare(object x, object y)
	{
		int num = 0;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0008;
		}
		goto IL_003b;
		IL_0008:
		if (jawsw[num2] != null && 0 == 0)
		{
			num = jawsw[num2].Compare(x, y);
			if (num != 0 && 0 == 0)
			{
				return num;
			}
		}
		num2++;
		goto IL_003b;
		IL_003b:
		if (num2 < jawsw.Length)
		{
			goto IL_0008;
		}
		return num;
	}
}
