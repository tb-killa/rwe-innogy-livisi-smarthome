using System.Security.Cryptography;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class ajezg : lnabj
{
	private AlgorithmIdentifier iawle;

	private rwolq lcojb;

	private rwolq ntqnc;

	private zjcch hxpfh;

	public HashingAlgorithmId yjrxh => iawle.vvmoi(p0: false);

	public byte[] znwxm => lcojb.rtrhq;

	public byte[] hulku => ntqnc.rtrhq;

	public byte[] vfdhm => hxpfh.rtrhq;

	public ajezg()
	{
	}

	public ajezg(byte[] serialNumber, byte[] issuerNameHash, byte[] issuerKeyHash, HashingAlgorithmId hashAlg)
	{
		iawle = AlgorithmIdentifier.heubo(hashAlg);
		lcojb = new rwolq(issuerNameHash);
		ntqnc = new rwolq(issuerKeyHash);
		hxpfh = new zjcch(serialNumber, allowNegative: false);
	}

	private void tneob(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in tneob
		this.tneob(p0, p1, p2);
	}

	private lnabj hcjbc(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			iawle = new AlgorithmIdentifier();
			return iawle;
		case 1:
			lcojb = new rwolq();
			return lcojb;
		case 2:
			ntqnc = new rwolq();
			return ntqnc;
		case 3:
			hxpfh = new zjcch();
			return hxpfh;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in hcjbc
		return this.hcjbc(p0, p1, p2);
	}

	private void krzwu()
	{
		if (iawle == null || 1 == 0)
		{
			throw new CryptographicException("Hash algorithm not found in Certificate ID.");
		}
		if (lcojb == null || 1 == 0)
		{
			throw new CryptographicException("Issuer name hash not found in Certificate ID.");
		}
		if (ntqnc == null || 1 == 0)
		{
			throw new CryptographicException("Issuer key hash not found in Certificate ID.");
		}
		if (hxpfh == null || 1 == 0)
		{
			throw new CryptographicException("Serial number not found in Certificate ID.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in krzwu
		this.krzwu();
	}

	private void fivqh(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in fivqh
		this.fivqh(p0, p1, p2);
	}

	private void teeal(fxakl p0)
	{
		p0.suudj(iawle, lcojb, ntqnc, hxpfh);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in teeal
		this.teeal(p0);
	}
}
