using System;
using System.Collections.Generic;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class CryptographicAttributeValueCollection : CryptographicCollection, lnabj
{
	private readonly List<nnzwd> kmhek;

	public byte[] this[int index] => kmhek[index].lktyp;

	internal CryptographicAttributeValueCollection()
		: base(rmkkr.wguaf, new List<nnzwd>())
	{
		kmhek = (List<nnzwd>)base.zctor;
	}

	private lnabj rsvtr(rmkkr p0, bool p1, int p2)
	{
		nnzwd nnzwd = new nnzwd();
		kmhek.Add(nnzwd);
		return nnzwd;
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in rsvtr
		return this.rsvtr(p0, p1, p2);
	}

	internal void qejlr(nnzwd p0)
	{
		kmhek.Add(p0);
	}

	public void CopyTo(byte[][] array, int index)
	{
		if (array == null || 1 == 0)
		{
			throw new ArgumentNullException("array");
		}
		if (index < 0)
		{
			throw hifyx.nztrs("index", index, "Invalid index.");
		}
		if (index + kmhek.Count > array.Length)
		{
			throw new ArgumentException("Not enough space in the array.", "index");
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_005a;
		}
		goto IL_007f;
		IL_007f:
		if (num >= kmhek.Count)
		{
			return;
		}
		goto IL_005a;
		IL_005a:
		array[index] = kmhek[num].lktyp;
		index++;
		num++;
		goto IL_007f;
	}
}
