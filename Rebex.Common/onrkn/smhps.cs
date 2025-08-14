using System;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class smhps : imfrk
{
	private readonly qpehr nxcvw;

	private smhps(qpehr provider)
	{
		nxcvw = provider;
	}

	public static imfrk rzowl(string p0)
	{
		if (p0 == null || false || !p0.StartsWith("rsa", StringComparison.Ordinal) || 1 == 0)
		{
			return null;
		}
		qpehr qpehr2 = qpehr.jsbgt(p0);
		if (qpehr2 == null || 1 == 0)
		{
			return RSAManaged.icbbo(p0);
		}
		return new smhps(qpehr2);
	}

	private eatps uaxjn(PrivateKeyInfo p0)
	{
		mdxtm mdxtm2 = nxcvw.kdlln(p0);
		return new fvjkt(mdxtm2, mdxtm2);
	}

	eatps imfrk.xaunu(PrivateKeyInfo p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in uaxjn
		return this.uaxjn(p0);
	}

	private eatps wytwl(PublicKeyInfo p0)
	{
		mdxtm publicKeyAlgorithm = nxcvw.orngu(p0);
		return new fvjkt(publicKeyAlgorithm, null);
	}

	eatps imfrk.neqkn(PublicKeyInfo p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in wytwl
		return this.wytwl(p0);
	}

	private eatps fxcfw()
	{
		mdxtm mdxtm2 = nxcvw.jkeyc();
		return new fvjkt(mdxtm2, mdxtm2);
	}

	eatps imfrk.poerm()
	{
		//ILSpy generated this explicit interface implementation from .override directive in fxcfw
		return this.fxcfw();
	}
}
