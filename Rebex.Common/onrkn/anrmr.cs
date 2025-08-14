using System.Security.Cryptography;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class anrmr : lnabj
{
	private wyjqw ibpor;

	private AlgorithmIdentifier cpkrs;

	private aipxl rshah;

	public wyjqw mhtzz => ibpor;

	public AlgorithmIdentifier wkske => cpkrs;

	public anrmr()
	{
	}

	public anrmr(ContentInfo contentInfo, AlgorithmIdentifier encryptionAlgorithm)
	{
		ibpor = new wyjqw(contentInfo.ContentType);
		rshah = contentInfo.jphgq();
		cpkrs = encryptionAlgorithm;
	}

	public anrmr toait()
	{
		anrmr anrmr2 = new anrmr();
		anrmr2.ibpor = ibpor;
		anrmr2.cpkrs = cpkrs;
		anrmr2.rshah = rshah.mcyhd();
		return anrmr2;
	}

	internal aipxl pgfiz()
	{
		return rshah;
	}

	private void xkciy(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xkciy
		this.xkciy(p0, p1, p2);
	}

	private lnabj moymr(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			ibpor = new wyjqw();
			return ibpor;
		case 1:
			cpkrs = new AlgorithmIdentifier();
			return cpkrs;
		case 65536:
			rshah = new aipxl();
			return new rwknq(rshah, 0, rmkkr.zkxoz);
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in moymr
		return this.moymr(p0, p1, p2);
	}

	private void lhvhu(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in lhvhu
		this.lhvhu(p0, p1, p2);
	}

	private void yiaew()
	{
		if (ibpor == null || 1 == 0)
		{
			throw new CryptographicException("Content info does not contain a type.");
		}
		if (cpkrs == null || 1 == 0)
		{
			throw new CryptographicException("Content info does not contain an encryption algorithm.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in yiaew
		this.yiaew();
	}

	private void sdmok(fxakl p0)
	{
		if (rshah == null || 1 == 0)
		{
			p0.suudj(ibpor, cpkrs);
		}
		else
		{
			p0.suudj(ibpor, cpkrs, new rwknq(new yhrrj(rshah), 0, rmkkr.osptv));
		}
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in sdmok
		this.sdmok(p0);
	}

	internal SymmetricKeyAlgorithm pxelf()
	{
		return cpkrs.jatvn();
	}
}
