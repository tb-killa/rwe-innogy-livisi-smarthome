using System.Security.Cryptography;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class SecureMimeCapability : lnabj
{
	private wyjqw jjybc;

	private nnzwd ruxvp;

	public ObjectIdentifier Oid => jjybc.scakm;

	public byte[] Parameters
	{
		get
		{
			if (ruxvp == null || 1 == 0)
			{
				return null;
			}
			return ruxvp.lktyp;
		}
	}

	internal SecureMimeCapability()
	{
	}

	public SecureMimeCapability(ObjectIdentifier oid)
		: this(oid, null)
	{
	}

	public SecureMimeCapability(ObjectIdentifier oid, byte[] parameters)
	{
		jjybc = new wyjqw(oid);
		if (parameters != null && 0 == 0)
		{
			nnzwd p = new nnzwd();
			hfnnn.qnzgo(p, parameters);
			ruxvp = p;
		}
	}

	internal SecureMimeCapability xxvjn()
	{
		SecureMimeCapability secureMimeCapability = new SecureMimeCapability();
		secureMimeCapability.jjybc = jjybc;
		if (ruxvp != null && 0 == 0)
		{
			secureMimeCapability.ruxvp = ruxvp.ltfsl();
		}
		return secureMimeCapability;
	}

	private void qcmji(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in qcmji
		this.qcmji(p0, p1, p2);
	}

	private lnabj lxghj(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			jjybc = new wyjqw();
			return jjybc;
		case 1:
			ruxvp = new nnzwd();
			return ruxvp;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in lxghj
		return this.lxghj(p0, p1, p2);
	}

	private void whzuu(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in whzuu
		this.whzuu(p0, p1, p2);
	}

	private void mjvup()
	{
		if (jjybc == null || 1 == 0)
		{
			throw new CryptographicException("S/MIME capability does not contain an identifier.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in mjvup
		this.mjvup();
	}

	private void geleu(fxakl p0)
	{
		if (ruxvp == null || 1 == 0)
		{
			p0.suudj(jjybc);
		}
		else
		{
			p0.suudj(jjybc, ruxvp);
		}
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in geleu
		this.geleu(p0);
	}
}
