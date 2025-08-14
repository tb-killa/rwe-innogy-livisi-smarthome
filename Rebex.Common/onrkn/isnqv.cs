using System.Security.Cryptography;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class isnqv : lnabj
{
	private readonly rwolq qabod = new rwolq();

	private readonly zjcch cghrr = new zjcch();

	private AlgorithmIdentifier ufcwu;

	private bool rtbvt;

	public int beqkw => cghrr.kybig();

	public HashingAlgorithmId swvyl
	{
		get
		{
			if (ufcwu == null || 1 == 0)
			{
				return HashingAlgorithmId.SHA1;
			}
			string value;
			if ((value = ufcwu.Oid.Value) != null && 0 == 0)
			{
				if (value == "1.2.840.113549.2.7")
				{
					return HashingAlgorithmId.SHA1;
				}
				if (value == "1.2.840.113549.2.8")
				{
					return HashingAlgorithmId.SHA224;
				}
				if (value == "1.2.840.113549.2.9")
				{
					return HashingAlgorithmId.SHA256;
				}
				if (value == "1.2.840.113549.2.10")
				{
					return HashingAlgorithmId.SHA384;
				}
				if (value == "1.2.840.113549.2.11")
				{
					return HashingAlgorithmId.SHA512;
				}
			}
			return (HashingAlgorithmId)0;
		}
	}

	public byte[] ufvba()
	{
		return (byte[])qabod.rtrhq.Clone();
	}

	public isnqv()
	{
	}

	public isnqv(byte[] salt, int iterationCount)
	{
		qabod = new rwolq(salt);
		cghrr = new zjcch(iterationCount);
	}

	private void qipvw(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in qipvw
		this.qipvw(p0, p1, p2);
	}

	private lnabj udkgk(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			if (p0 != rmkkr.zkxoz)
			{
				throw new CryptographicException("Salt-source not supported.");
			}
			return qabod;
		case 1:
			rtbvt = true;
			return cghrr;
		case 2:
			ufcwu = new AlgorithmIdentifier();
			return ufcwu;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in udkgk
		return this.udkgk(p0, p1, p2);
	}

	private void ytbqn(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ytbqn
		this.ytbqn(p0, p1, p2);
	}

	private void oikyz()
	{
		if (!rtbvt || 1 == 0)
		{
			throw new CryptographicException("Invalid PBEParameter.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in oikyz
		this.oikyz();
	}

	private void xalpo(fxakl p0)
	{
		if (ufcwu == null || 1 == 0)
		{
			p0.suudj(qabod, cghrr);
		}
		else
		{
			p0.suudj(qabod, cghrr, ufcwu);
		}
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xalpo
		this.xalpo(p0);
	}
}
