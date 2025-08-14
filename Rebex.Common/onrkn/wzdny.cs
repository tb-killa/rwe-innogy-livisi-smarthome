using System.Security.Cryptography;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class wzdny : lnabj
{
	private AlgorithmIdentifier udddl;

	private htykq avavy;

	private CertificateCollection odafr;

	internal wzdny()
	{
	}

	private void oizer(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in oizer
		this.oizer(p0, p1, p2);
	}

	private lnabj dvbct(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			udddl = new AlgorithmIdentifier();
			return udddl;
		case 1:
			avavy = new htykq();
			return avavy;
		case 65536:
			odafr = new CertificateCollection(isSet: false);
			return new rporh(odafr, 0);
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in dvbct
		return this.dvbct(p0, p1, p2);
	}

	private void jczkh()
	{
		if (udddl == null || 1 == 0)
		{
			throw new CryptographicException("SignatureAlgorithm not found in OcspRequest.");
		}
		if (avavy == null || 1 == 0)
		{
			throw new CryptographicException("Signature not found in OcspRequest.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in jczkh
		this.jczkh();
	}

	private void kurrg(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in kurrg
		this.kurrg(p0, p1, p2);
	}

	private void nqoxs(fxakl p0)
	{
		if (odafr == null || false || odafr.Count == 0 || 1 == 0)
		{
			p0.suudj(udddl, avavy);
		}
		else
		{
			p0.suudj(udddl, avavy, new rporh(odafr, 0));
		}
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in nqoxs
		this.nqoxs(p0);
	}
}
