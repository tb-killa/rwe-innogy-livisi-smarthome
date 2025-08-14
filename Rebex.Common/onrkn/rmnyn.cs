using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;

namespace onrkn;

internal static class rmnyn
{
	private static readonly IList<Func<string, object>> cozqb = new List<Func<string, object>>();

	public static void nkkuu(Func<string, object> p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("algFactory");
		}
		lock (cozqb)
		{
			IEnumerator<Func<string, object>> enumerator = cozqb.GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					Func<string, object> current = enumerator.Current;
					if (current == p0 && 0 == 0)
					{
						return;
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
			cozqb.Add(p0);
		}
	}

	private static object pzvvk(string p0)
	{
		lock (cozqb)
		{
			IEnumerator<Func<string, object>> enumerator = cozqb.GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					Func<string, object> current = enumerator.Current;
					string arg = ((p0.Equals("ecdh-sha2-curve25519", StringComparison.Ordinal) ? true : false) ? "curve25519-sha256" : p0);
					object obj = current(arg);
					if (obj != null && 0 == 0)
					{
						return obj;
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
		}
		return null;
	}

	public static zxjln zbhkd(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			return zxjln.mcbds;
		}
		if (p0.StartsWith("rsa", StringComparison.OrdinalIgnoreCase) && 0 == 0)
		{
			return zxjln.iuckt;
		}
		if (p0.StartsWith("dsa", StringComparison.OrdinalIgnoreCase) && 0 == 0)
		{
			return zxjln.iuckt;
		}
		if (!CryptoHelper.UseFipsAlgorithmsOnly || 1 == 0)
		{
			object obj = pzvvk(p0);
			if (obj != null && 0 == 0)
			{
				return zxjln.iuckt;
			}
			if (p0.Equals("ed25519-sha512", StringComparison.OrdinalIgnoreCase) && 0 == 0)
			{
				return zxjln.iuckt;
			}
		}
		return zxjln.mcbds;
	}

	private static Exception pcqje(string p0)
	{
		string text;
		if (CryptoHelper.UseFipsAlgorithmsOnly && 0 == 0)
		{
			if (p0.IndexOf("ed25519", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				return new CryptographicException("Ed25519 algorithm is not supported in FIPS-compliant mode.");
			}
			text = " Plugins are not allowed in FIPS-compliant mode.";
		}
		else
		{
			text = " See https://labs.rebex.net/curves/ for details.";
		}
		if (p0.IndexOf("x25519", StringComparison.OrdinalIgnoreCase) >= 0)
		{
			return new InvalidOperationException("X25519 algorithm needs a plugin." + text);
		}
		if (p0.IndexOf("brainpool", StringComparison.OrdinalIgnoreCase) >= 0)
		{
			return new InvalidOperationException("Brainpool algorithms need a plugin on this platform." + text);
		}
		if (p0.IndexOf("secp", StringComparison.OrdinalIgnoreCase) >= 0)
		{
			return new InvalidOperationException("Secp 256 k1 algorithms need a plugin on this platform." + text);
		}
		if (p0.IndexOf("-nistp", StringComparison.OrdinalIgnoreCase) >= 0)
		{
			return new InvalidOperationException("NIST P-256/384/521 Elliptic Curve algorithms need a plugin on this platform." + text);
		}
		return new NotSupportedException("Algorithm is not supported.");
	}

	public static imfrk bknre(string p0)
	{
		imfrk imfrk2 = vugtt(p0);
		if (imfrk2 == null || 1 == 0)
		{
			throw pcqje(p0);
		}
		return imfrk2;
	}

	public static imfrk vuzke(string p0, xtsej p1)
	{
		imfrk imfrk2 = null;
		switch (p1)
		{
		case xtsej.dipgs:
			return bknre(p0);
		case xtsej.ocahi:
		{
			imfrk obj = RSAManaged.icbbo(p0);
			if (obj == null || 1 == 0)
			{
				obj = DSAManaged.vrnse(p0);
			}
			imfrk2 = obj;
			break;
		}
		}
		if (imfrk2 == null || 1 == 0)
		{
			throw new NotSupportedException("Algorithm is not supported.");
		}
		return imfrk2;
	}

	public static imfrk aregb(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("name");
		}
		imfrk imfrk2 = null;
		if (imfrk2 == null || 1 == 0)
		{
			imfrk2 = RSAManaged.icbbo(p0);
		}
		return imfrk2;
	}

	public static imfrk fnqth(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("name");
		}
		imfrk imfrk2 = null;
		if (imfrk2 == null || 1 == 0)
		{
			imfrk2 = RSAManaged.icbbo(p0);
		}
		if (imfrk2 == null || 1 == 0)
		{
			imfrk2 = qpehr.jsbgt(p0);
		}
		return imfrk2;
	}

	public static imfrk vugtt(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("name");
		}
		imfrk imfrk2 = null;
		if (!CryptoHelper.UseFipsAlgorithmsOnly || 1 == 0)
		{
			object obj = pzvvk(p0);
			if (obj != null && 0 == 0)
			{
				imfrk2 = jhqmg.eepef(p0, obj);
			}
		}
		if (imfrk2 == null || 1 == 0)
		{
			imfrk2 = qpehr.jsbgt(p0);
		}
		if (imfrk2 == null || 1 == 0)
		{
			imfrk2 = jfvxw.pcftj(p0);
		}
		if (imfrk2 == null || 1 == 0)
		{
			imfrk2 = RSAManaged.icbbo(p0);
		}
		if (imfrk2 == null || 1 == 0)
		{
			imfrk2 = DSAManaged.vrnse(p0);
		}
		return imfrk2;
	}

	public static DiffieHellman hnofn()
	{
		return new DiffieHellmanManaged();
	}
}
