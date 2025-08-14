using System.Collections;
using System.Security.Cryptography;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class cobxv : lnabj
{
	private zjcch xynwj;

	private zjcch fanko;

	private AlgorithmIdentifier yajrh;

	private ukjdk degxr;

	private knzkr mjary;

	private ukjdk ugxqb;

	private PublicKeyInfo araeq;

	private CertificateExtensionCollection smmna = new CertificateExtensionCollection();

	public byte[] wmddh => fanko.rtrhq;

	public AlgorithmIdentifier kqlmy => yajrh;

	public DistinguishedName mhoze => degxr.efqft;

	public DistinguishedName msxpx => ugxqb.efqft;

	public PublicKeyInfo wflxw => araeq;

	public knzkr hvjmd => mjary;

	public CertificateExtensionCollection bmefi => smmna;

	internal cobxv(DistinguishedName authority, byte[] serialNumber, DistinguishedName subject, PublicKeyInfo publicKey, AlgorithmIdentifier signatureAlgorithm, knzkr validity)
	{
		xynwj = new zjcch(2);
		fanko = new zjcch(serialNumber, allowNegative: false);
		yajrh = signatureAlgorithm;
		degxr = new ukjdk(authority);
		mjary = validity;
		ugxqb = new ukjdk(subject);
		araeq = publicKey;
	}

	internal cobxv()
	{
	}

	public int tiilm()
	{
		if (xynwj != null && 0 == 0)
		{
			return xynwj.kybig();
		}
		return 0;
	}

	private void uescj(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in uescj
		this.uescj(p0, p1, p2);
	}

	private lnabj okinj(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 65536:
			xynwj = new zjcch();
			return new rporh(xynwj, 0);
		case 0:
			fanko = new zjcch();
			return fanko;
		case 1:
			yajrh = new AlgorithmIdentifier();
			return yajrh;
		case 2:
			degxr = new ukjdk();
			return degxr;
		case 3:
			mjary = new knzkr();
			return mjary;
		case 4:
			ugxqb = new ukjdk();
			return ugxqb;
		case 5:
			araeq = new PublicKeyInfo();
			return araeq;
		case 65539:
			return new rporh(smmna, 3);
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in okinj
		return this.okinj(p0, p1, p2);
	}

	private void oiqzy(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in oiqzy
		this.oiqzy(p0, p1, p2);
	}

	private void yzkcf()
	{
		if (fanko == null || 1 == 0)
		{
			throw new CryptographicException("Serial number not found in certificate.");
		}
		if (yajrh == null || 1 == 0)
		{
			throw new CryptographicException("Signature not found in certificate.");
		}
		if (degxr == null || 1 == 0)
		{
			throw new CryptographicException("Issuer algorithm not found in certificate.");
		}
		if (mjary == null || 1 == 0)
		{
			throw new CryptographicException("Validity not found in certificate.");
		}
		if (ugxqb == null || 1 == 0)
		{
			throw new CryptographicException("Subject not found in certificate.");
		}
		if (araeq == null || 1 == 0)
		{
			throw new CryptographicException("Public key info not found in certificate.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in yzkcf
		this.yzkcf();
	}

	private void kbgid(fxakl p0)
	{
		ArrayList arrayList = new ArrayList();
		if (xynwj != null && 0 == 0)
		{
			arrayList.Add(new rporh(xynwj, 0));
		}
		arrayList.Add(fanko);
		arrayList.Add(yajrh);
		arrayList.Add(degxr);
		arrayList.Add(mjary);
		arrayList.Add(ugxqb);
		arrayList.Add(araeq);
		if (smmna.Count > 0)
		{
			arrayList.Add(new rporh(smmna, 3));
		}
		p0.aiflg(rmkkr.osptv, arrayList);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in kbgid
		this.kbgid(p0);
	}
}
