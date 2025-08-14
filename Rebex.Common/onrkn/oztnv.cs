using System.Security.Cryptography;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class oztnv : lnabj
{
	private AlgorithmIdentifier ivppf;

	private rwolq lusej;

	public ObjectIdentifier gfhke => ivppf.Oid;

	public byte[] thcnt => lusej.rtrhq;

	public oztnv()
	{
	}

	public oztnv(ObjectIdentifier algorithm, byte[] digest)
	{
		ivppf = new AlgorithmIdentifier(algorithm, new mdvaz().ionjf());
		lusej = new rwolq(digest, clone: false);
	}

	private void muyyg(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in muyyg
		this.muyyg(p0, p1, p2);
	}

	private lnabj fupbn(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			ivppf = new AlgorithmIdentifier();
			return ivppf;
		case 1:
			lusej = new rwolq();
			return lusej;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in fupbn
		return this.fupbn(p0, p1, p2);
	}

	private void sfnrl(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in sfnrl
		this.sfnrl(p0, p1, p2);
	}

	private void oxmvz()
	{
		if (ivppf == null || 1 == 0)
		{
			throw new CryptographicException("Digest algorithm not found in DigestInfo.");
		}
		if (lusej == null || 1 == 0)
		{
			throw new CryptographicException("Digest not found in DigestInfo.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in oxmvz
		this.oxmvz();
	}

	public void vlfdh(fxakl p0)
	{
		p0.suudj(ivppf, lusej);
	}
}
