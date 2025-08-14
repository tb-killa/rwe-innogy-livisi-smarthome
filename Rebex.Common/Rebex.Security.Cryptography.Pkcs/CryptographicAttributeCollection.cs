using System;
using System.Collections.Generic;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class CryptographicAttributeCollection : CryptographicCollection<CryptographicAttributeNode>, lnabj
{
	public new CryptographicAttributeNode this[int index]
	{
		get
		{
			return base[index];
		}
		set
		{
			base[index] = value;
		}
	}

	public CryptographicAttributeNode this[string oid]
	{
		get
		{
			if (oid == null || 1 == 0)
			{
				throw new ArgumentNullException("oid");
			}
			IEnumerator<CryptographicAttributeNode> enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					CryptographicAttributeNode current = enumerator.Current;
					if (current.Oid.Value == oid && 0 == 0)
					{
						return current;
					}
				}
			}
			finally
			{
				if (enumerator != null && 0 == 0)
				{
					enumerator.Dispose();
				}
			}
			return null;
		}
	}

	internal CryptographicAttributeCollection()
		: base(rmkkr.wguaf)
	{
	}

	private lnabj kdosu(rmkkr p0, bool p1, int p2)
	{
		CryptographicAttributeNode cryptographicAttributeNode = new CryptographicAttributeNode();
		base.lquvo.Add(cryptographicAttributeNode);
		return cryptographicAttributeNode;
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in kdosu
		return this.kdosu(p0, p1, p2);
	}

	internal void lgfev(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("oid");
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_001b;
		}
		goto IL_0051;
		IL_0051:
		if (num >= base.Count)
		{
			return;
		}
		goto IL_001b;
		IL_001b:
		if (this[num].Oid.Value == p0 && 0 == 0)
		{
			RemoveAt(num);
			num--;
		}
		num++;
		goto IL_0051;
	}
}
