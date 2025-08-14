using System.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class fyyhl : lnabj
{
	private AlgorithmIdentifier jgclk;

	private AlgorithmIdentifier bswbr;

	public AlgorithmIdentifier tqleq => jgclk;

	public AlgorithmIdentifier zmjnn => bswbr;

	public fyyhl()
	{
	}

	public fyyhl(AlgorithmIdentifier keyDerivationAlgorithm, AlgorithmIdentifier encryptionAlgorithm)
	{
		jgclk = keyDerivationAlgorithm;
		bswbr = encryptionAlgorithm;
	}

	private void mmwzl(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in mmwzl
		this.mmwzl(p0, p1, p2);
	}

	private lnabj dutxr(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			jgclk = new AlgorithmIdentifier();
			return jgclk;
		case 1:
			bswbr = new AlgorithmIdentifier();
			return bswbr;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in dutxr
		return this.dutxr(p0, p1, p2);
	}

	private void qyqlh(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in qyqlh
		this.qyqlh(p0, p1, p2);
	}

	private void ltmin()
	{
		if (jgclk == null || 1 == 0)
		{
			throw new CryptographicException("Key derivation algorithm not found in PBES2Parameters.");
		}
		if (bswbr == null || 1 == 0)
		{
			throw new CryptographicException("Key encryption algorithm not found in PBES2Parameters.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ltmin
		this.ltmin();
	}

	private void rupfi(fxakl p0)
	{
		p0.suudj(jgclk, bswbr);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in rupfi
		this.rupfi(p0);
	}
}
