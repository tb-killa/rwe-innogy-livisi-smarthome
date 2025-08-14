using System.Collections.Generic;
using System.Net;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class azfwz : CryptographicCollection<ygomx>, lnabj
{
	internal azfwz()
		: base(rmkkr.osptv)
	{
	}

	private lnabj xzdyf(rmkkr p0, bool p1, int p2)
	{
		ygomx ygomx2 = new ygomx();
		if (ygomx2 == null || 1 == 0)
		{
			return null;
		}
		base.lquvo.Add(ygomx2);
		return ygomx2;
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xzdyf
		return this.xzdyf(p0, p1, p2);
	}

	public void gynds(ObjectIdentifier p0, lnabj p1)
	{
		Add(new ygomx(p0, p1));
	}

	public void gfsfs(string p0, ukmqt p1)
	{
		Add(new ygomx(p0, p1));
	}

	public void usutz(IPAddress p0)
	{
		Add(new ygomx(p0));
	}

	public string[] hhoay(ukmqt p0)
	{
		List<string> list = new List<string>();
		IEnumerator<ygomx> enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				ygomx current = enumerator.Current;
				if (current.jcvng == p0)
				{
					list.Add(((vesyi)current.wntxx).dcokg);
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
		return list.ToArray();
	}

	public DistinguishedName[] xngqi()
	{
		List<DistinguishedName> list = new List<DistinguishedName>();
		IEnumerator<ygomx> enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				ygomx current = enumerator.Current;
				if (current.jcvng == ukmqt.isxiv)
				{
					list.Add(((ukjdk)current.wntxx).efqft);
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
		return list.ToArray();
	}

	public IPAddress[] daile()
	{
		List<IPAddress> list = new List<IPAddress>();
		IEnumerator<ygomx> enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				ygomx current = enumerator.Current;
				if (current.jcvng == ukmqt.kwrqu)
				{
					byte[] rtrhq = ((rwolq)current.wntxx).rtrhq;
					if (rtrhq.Length == 4 || rtrhq.Length == 16)
					{
						list.Add(new IPAddress(rtrhq));
					}
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
		return list.ToArray();
	}
}
