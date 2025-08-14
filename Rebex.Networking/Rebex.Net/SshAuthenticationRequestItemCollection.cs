using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rebex.Net;

public class SshAuthenticationRequestItemCollection : ReadOnlyCollection<SshAuthenticationRequestItem>
{
	internal SshAuthenticationRequestItemCollection()
		: base((IList<SshAuthenticationRequestItem>)new List<SshAuthenticationRequestItem>())
	{
	}

	internal SshAuthenticationRequestItemCollection(string[] prompt, bool[] echo)
		: this()
	{
		if (prompt == null || 1 == 0)
		{
			throw new ArgumentNullException("prompt", "Collection cannot be null.");
		}
		if (echo == null || 1 == 0)
		{
			throw new ArgumentNullException("echo", "Collection cannot be null.");
		}
		if (prompt.Length != echo.Length)
		{
			throw new ArgumentException("Prompt and echo arrays must be of same size.");
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0054;
		}
		goto IL_0075;
		IL_0054:
		khfwi(new SshAuthenticationRequestItem(prompt[num], !echo[num]));
		num++;
		goto IL_0075;
		IL_0075:
		if (num >= prompt.Length)
		{
			return;
		}
		goto IL_0054;
	}

	internal void khfwi(SshAuthenticationRequestItem p0)
	{
		base.Items.Add(p0);
	}

	internal string[] cqcma()
	{
		string[] array = new string[base.Count];
		int num = 0;
		if (num != 0)
		{
			goto IL_0012;
		}
		goto IL_0045;
		IL_0012:
		if (base[num].Response == null || 1 == 0)
		{
			array[num] = string.Empty;
		}
		else
		{
			array[num] = base[num].Response;
		}
		num++;
		goto IL_0045;
		IL_0045:
		if (num < base.Count)
		{
			goto IL_0012;
		}
		return array;
	}
}
