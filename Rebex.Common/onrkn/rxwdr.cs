using System;
using System.Security.Cryptography;
using Rebex.Security.Certificates;

namespace onrkn;

internal class rxwdr : lnabj
{
	private ffncd tkzjf;

	private ovyea fzrsx;

	private xpnna xwsab;

	public ffncd qgdlw => tkzjf;

	internal cgxdn gkrhh
	{
		get
		{
			if (xwsab == null || 1 == 0)
			{
				return null;
			}
			return xwsab.qehke;
		}
	}

	internal hiegm wglak
	{
		get
		{
			if (gkrhh == null || 1 == 0)
			{
				return null;
			}
			return gkrhh.jgdyi;
		}
	}

	internal rxwdr()
	{
	}

	internal rxwdr(ffncd status, cgxdn response)
	{
		if ((status == ffncd.pqpgx || 1 == 0) && (response == null || 1 == 0))
		{
			throw new ArgumentException("OcspResponse cannot be null if OcspResponseStatus.Success was specified.", "response");
		}
		tkzjf = status;
		fzrsx = new ovyea((int)status);
		if (response != null && 0 == 0)
		{
			xwsab = new xpnna(response);
		}
	}

	public rxwdr(ffncd status, Certificate responder, DateTime producedAt, itxgi responses, params zyked[] extensions)
		: this(status, amwdd(responder, producedAt, responses, extensions))
	{
	}

	private static cgxdn amwdd(Certificate p0, DateTime p1, itxgi p2, zyked[] p3)
	{
		return new cgxdn(new jpwxz(p0, SignatureHashAlgorithm.SHA1), p0.GetSubject(), p1, p2, p3);
	}

	private void hnyyw(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in hnyyw
		this.hnyyw(p0, p1, p2);
	}

	private lnabj otanb(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			fzrsx = new ovyea();
			return fzrsx;
		case 65536:
			xwsab = new xpnna();
			return new rporh(xwsab, 0);
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in otanb
		return this.otanb(p0, p1, p2);
	}

	private void yjika()
	{
		if (fzrsx == null || 1 == 0)
		{
			throw new CryptographicException("OcspResponseStatus not found in OcspResponse.");
		}
		tkzjf = (ffncd)(fzrsx.akyte ?? (-1));
		if ((tkzjf == ffncd.pqpgx || 1 == 0) && (xwsab == null || 1 == 0))
		{
			throw new CryptographicException("ResponseBytes not found in OcspResponse.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in yjika
		this.yjika();
	}

	private void yndze(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in yndze
		this.yndze(p0, p1, p2);
	}

	private void pvuzq(fxakl p0)
	{
		if (xwsab == null || 1 == 0)
		{
			p0.suudj(fzrsx);
		}
		else
		{
			p0.suudj(fzrsx, new rporh(xwsab, 0));
		}
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in pvuzq
		this.pvuzq(p0);
	}

	public byte[] cykeg()
	{
		return fxakl.kncuz(this);
	}
}
