using System;
using System.Security.Cryptography;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class RevokedCertificate : lnabj
{
	private const string iucjh = "2.5.29.21";

	private zjcch bzyvu;

	private gfiwx peqhk;

	private CertificateExtensionCollection wtjjs;

	private RevocationReason? lrlwj;

	public DateTime RevocationDate => peqhk.fzcfd();

	public CertificateExtensionCollection Extensions
	{
		get
		{
			if (wtjjs == null || 1 == 0)
			{
				wtjjs = new CertificateExtensionCollection();
				if (lrlwj.HasValue && 0 == 0)
				{
					byte[] data = new byte[3]
					{
						10,
						1,
						(byte)lrlwj.Value
					};
					CertificateExtension item = new CertificateExtension("2.5.29.21", critical: false, data);
					wtjjs.Add(item);
					lrlwj = null;
				}
			}
			return wtjjs;
		}
	}

	internal RevokedCertificate()
	{
	}

	public RevokedCertificate(byte[] serialNumber, DateTime revocationDate, RevocationReason reason)
	{
		bzyvu = new zjcch(serialNumber);
		peqhk = new gfiwx(revocationDate);
		wtjjs = new CertificateExtensionCollection();
		wtjjs.Add(new CertificateExtension(new ObjectIdentifier("2.5.29.21"), critical: false, new ovyea(new byte[1] { (byte)reason }).ionjf()));
	}

	public RevokedCertificate(int serialNumber, DateTime revocationDate, RevocationReason reason)
		: this(new zjcch(serialNumber).rtrhq, revocationDate, reason)
	{
	}

	public byte[] GetSerialNumber()
	{
		return bzyvu.rtrhq;
	}

	public RevocationReason GetRevocationReason()
	{
		if (lrlwj.HasValue && 0 == 0)
		{
			return lrlwj.Value;
		}
		bool p;
		return unpxf(out p) ?? RevocationReason.Unspecified;
	}

	private RevocationReason? unpxf(out bool p0)
	{
		p0 = false;
		if (wtjjs == null || 1 == 0)
		{
			return null;
		}
		CertificateExtension certificateExtension = wtjjs["2.5.29.21"];
		if (certificateExtension == null || 1 == 0)
		{
			return null;
		}
		byte[] value = certificateExtension.Value;
		p0 = certificateExtension.Critical;
		if (value == null || false || value.Length != 3 || value[0] != 10 || value[1] != 1)
		{
			p0 = false;
			return null;
		}
		return (RevocationReason)value[2];
	}

	private void tqpnq(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
		bzyvu = null;
		peqhk = null;
		wtjjs = null;
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in tqpnq
		this.tqpnq(p0, p1, p2);
	}

	private lnabj xuhxv(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			bzyvu = new zjcch();
			return bzyvu;
		case 1:
			peqhk = new gfiwx();
			return peqhk;
		case 2:
			wtjjs = new CertificateExtensionCollection();
			return wtjjs;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xuhxv
		return this.xuhxv(p0, p1, p2);
	}

	private void rzsez(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in rzsez
		this.rzsez(p0, p1, p2);
	}

	private void psdxm()
	{
		if (bzyvu == null || 1 == 0)
		{
			throw new CryptographicException("Serial number not found in revoked certificate.");
		}
		if (peqhk == null || 1 == 0)
		{
			throw new CryptographicException("Revocation date not found in revoked certificate.");
		}
		if (wtjjs != null && 0 == 0 && wtjjs.Count == 1)
		{
			lrlwj = unpxf(out var p);
			if (lrlwj.HasValue && 0 == 0 && (!p || 1 == 0))
			{
				wtjjs = null;
			}
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in psdxm
		this.psdxm();
	}

	private void nkbdb(fxakl p0)
	{
		CertificateExtensionCollection extensions = Extensions;
		if (extensions.Count == 0 || 1 == 0)
		{
			p0.suudj(bzyvu, peqhk);
		}
		else
		{
			p0.suudj(bzyvu, peqhk, extensions);
		}
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in nkbdb
		this.nkbdb(p0);
	}
}
