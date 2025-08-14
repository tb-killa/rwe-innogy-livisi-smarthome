using System;
using System.Collections;
using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class CryptographicAttributeNode : lnabj
{
	private wyjqw geueb;

	private readonly CryptographicAttributeValueCollection pzmde = new CryptographicAttributeValueCollection();

	public ObjectIdentifier Oid
	{
		get
		{
			if (geueb == null || 1 == 0)
			{
				return null;
			}
			return geueb.scakm;
		}
	}

	public CryptographicAttributeValueCollection Values => pzmde;

	internal byte[] hwqek
	{
		get
		{
			if (pzmde.Count == 0 || 1 == 0)
			{
				return new byte[0];
			}
			return pzmde[0];
		}
	}

	internal CryptographicAttributeNode()
	{
	}

	public CryptographicAttributeNode(ObjectIdentifier oid, ICollection values)
		: this(oid, ktblg(values))
	{
	}

	private static byte[][] ktblg(ICollection p0)
	{
		byte[][] array = new byte[p0.Count][];
		IEnumerator enumerator = p0.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				object current = enumerator.Current;
				if (!(current is byte[][]) || 1 == 0)
				{
					throw new ArgumentException("The collection contains a value that is not an array of bytes.", "values");
				}
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable && 0 == 0)
			{
				disposable.Dispose();
			}
		}
		p0.CopyTo(array, 0);
		return array;
	}

	public CryptographicAttributeNode(ObjectIdentifier oid, params byte[][] values)
	{
		if (oid == null || 1 == 0)
		{
			throw new ArgumentNullException("oid");
		}
		if (values == null || 1 == 0)
		{
			throw new ArgumentNullException("values");
		}
		geueb = new wyjqw(oid);
		for (int i = 0; i < values.Length; i++)
		{
			nnzwd p = new nnzwd();
			hfnnn.qnzgo(p, values[i]);
			pzmde.qejlr(p);
		}
	}

	internal CryptographicAttributeNode rtdcm()
	{
		CryptographicAttributeNode cryptographicAttributeNode = new CryptographicAttributeNode();
		cryptographicAttributeNode.geueb = geueb;
		int num = 0;
		if (num != 0)
		{
			goto IL_0014;
		}
		goto IL_0036;
		IL_0014:
		byte[] data = pzmde[num];
		cryptographicAttributeNode.pzmde.qejlr(new nnzwd(data));
		num++;
		goto IL_0036;
		IL_0036:
		if (num < pzmde.Count)
		{
			goto IL_0014;
		}
		return cryptographicAttributeNode;
	}

	private void fbppo(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in fbppo
		this.fbppo(p0, p1, p2);
	}

	private lnabj zieqr(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			geueb = new wyjqw();
			return geueb;
		case 1:
			return pzmde;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in zieqr
		return this.zieqr(p0, p1, p2);
	}

	private void lpaxr(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in lpaxr
		this.lpaxr(p0, p1, p2);
	}

	private void tnppe()
	{
		if (geueb == null || 1 == 0)
		{
			throw new CryptographicException("Cryptographic attribute does not contain a type.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in tnppe
		this.tnppe();
	}

	private void fnnrk(fxakl p0)
	{
		p0.suudj(geueb, pzmde);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in fnnrk
		this.fnnrk(p0);
	}
}
