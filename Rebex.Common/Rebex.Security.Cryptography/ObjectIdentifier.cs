using System;
using onrkn;

namespace Rebex.Security.Cryptography;

public sealed class ObjectIdentifier
{
	private string nfzdu;

	private string mitrc;

	public string Value => nfzdu;

	internal ObjectIdentifier()
	{
	}

	public ObjectIdentifier(ObjectIdentifier oid)
	{
		if (oid == null || 1 == 0)
		{
			throw new ArgumentNullException("oid");
		}
		nfzdu = oid.nfzdu;
		mitrc = oid.mitrc;
	}

	public ObjectIdentifier(string oid)
		: this(oid, null)
	{
	}

	public static implicit operator ObjectIdentifier(string oid)
	{
		if (oid == null || 1 == 0)
		{
			throw new ArgumentNullException("oid");
		}
		return new ObjectIdentifier(oid);
	}

	public static ObjectIdentifier Parse(byte[] buffer)
	{
		return wyjqw.ewqkw(buffer).scakm;
	}

	internal ObjectIdentifier(string oid, string friendlyName)
	{
		if (oid == null || false || oid.Length == 0 || 1 == 0)
		{
			nfzdu = "";
			mitrc = "";
		}
		else
		{
			gtgwo(oid);
			wfpvj(oid, friendlyName);
		}
	}

	internal void wfpvj(string p0, string p1)
	{
		nfzdu = p0;
		if (p1 == null || false || p1.Length == 0 || 1 == 0)
		{
			mitrc = nzcjz(p0);
			if (mitrc == null || 1 == 0)
			{
				mitrc = p0;
			}
		}
		else
		{
			mitrc = p1;
		}
	}

	public byte[] ToArray()
	{
		return ToArray(useDer: false);
	}

	public byte[] ToArray(bool useDer)
	{
		if (!useDer || 1 == 0)
		{
			return wyjqw.jizxm(nfzdu);
		}
		return fxakl.kncuz(new wyjqw(nfzdu));
	}

	private static void gtgwo(string p0)
	{
		wyjqw.jizxm(p0);
	}

	public override string ToString()
	{
		return nfzdu;
	}

	private static string nzcjz(string p0)
	{
		return p0;
	}
}
