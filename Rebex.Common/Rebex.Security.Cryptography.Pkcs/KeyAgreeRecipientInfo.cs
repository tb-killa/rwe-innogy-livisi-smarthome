using System.Collections;
using System.Security.Cryptography;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class KeyAgreeRecipientInfo : RecipientInfo, lnabj
{
	private zjcch ksudg;

	private yyzlj crknj;

	private SubjectIdentifier otxci;

	private rwolq pjvbr;

	private AlgorithmIdentifier ohhmm;

	private AlgorithmIdentifier qucff;

	private yxkxe fqgap;

	private iwfml luocl;

	private CertificateChain xhodn;

	private byte[] ecabq;

	internal override int bilco => ksudg.kybig();

	public SubjectIdentifier OriginatorIdentifier => otxci;

	public override SubjectIdentifier RecipientIdentifier => luocl.simzu;

	public override AlgorithmIdentifier KeyEncryptionAlgorithm
	{
		get
		{
			if (ohhmm == null || 1 == 0)
			{
				return null;
			}
			if (qucff == null || 1 == 0)
			{
				qucff = ohhmm.evxkk();
			}
			return qucff;
		}
	}

	public override byte[] EncryptedKey
	{
		get
		{
			if (ecabq == null || 1 == 0)
			{
				ecabq = (byte[])luocl.lkfuo.Clone();
			}
			return ecabq;
		}
	}

	internal override bool vqzfk => false;

	public override Certificate Certificate
	{
		get
		{
			if (xhodn == null || false || xhodn.Count == 0 || 1 == 0)
			{
				return null;
			}
			return xhodn[0];
		}
	}

	public override CertificateChain CertificateChain
	{
		get
		{
			if (xhodn == null || false || xhodn.Count == 0 || 1 == 0)
			{
				return null;
			}
			return xhodn;
		}
	}

	internal KeyAgreeRecipientInfo()
	{
	}

	internal override RecipientInfo krqfr()
	{
		KeyAgreeRecipientInfo keyAgreeRecipientInfo = new KeyAgreeRecipientInfo();
		keyAgreeRecipientInfo.ksudg = ksudg;
		keyAgreeRecipientInfo.crknj = crknj;
		keyAgreeRecipientInfo.otxci = otxci;
		keyAgreeRecipientInfo.pjvbr = pjvbr;
		keyAgreeRecipientInfo.fqgap = fqgap;
		keyAgreeRecipientInfo.luocl = luocl;
		keyAgreeRecipientInfo.ohhmm = ohhmm;
		keyAgreeRecipientInfo.xhodn = xhodn;
		return keyAgreeRecipientInfo;
	}

	internal override void oyacr(CertificateStore p0, ICertificateFinder p1)
	{
		iwfml iwfml = null;
		CertificateChain certificateChain = null;
		int num = 0;
		if (num != 0)
		{
			goto IL_0010;
		}
		goto IL_0087;
		IL_0010:
		iwfml iwfml2 = fqgap[num];
		CertificateChain certificateChain2 = iwfml2.simzu.mgsxu(p0, p1);
		if (certificateChain2 != null && 0 == 0 && certificateChain2.Count > 0)
		{
			Certificate certificate = certificateChain2[0];
			if (certificate.HasPrivateKey())
			{
				iwfml = iwfml2;
				certificateChain = certificateChain2;
				goto IL_0098;
			}
			if (certificateChain == null || 1 == 0)
			{
				iwfml = iwfml2;
				certificateChain = certificateChain2;
			}
		}
		else if (iwfml == null || 1 == 0)
		{
			iwfml = iwfml2;
		}
		num++;
		goto IL_0087;
		IL_0098:
		if (iwfml == null || 1 == 0)
		{
			throw new CryptographicException("No recipients found in RecipientInfo.");
		}
		luocl = iwfml;
		xhodn = certificateChain;
		return;
		IL_0087:
		if (num < fqgap.Count)
		{
			goto IL_0010;
		}
		goto IL_0098;
	}

	internal override bool zqjdy(bool p0)
	{
		return false;
	}

	internal override byte[] ghozk(bool p0)
	{
		return null;
	}

	internal override bool oaeit()
	{
		Certificate certificate = Certificate;
		if (certificate != null && 0 == 0)
		{
			return (certificate.GetIntendedUsage() & KeyUses.KeyAgreement) != 0;
		}
		return true;
	}

	private void cizcr(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.cxxlq, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in cizcr
		this.cizcr(p0, p1, p2);
	}

	private lnabj shzvn(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			ksudg = new zjcch();
			return ksudg;
		case 65536:
			crknj = new yyzlj();
			return new rporh(crknj, 0);
		case 65537:
			pjvbr = new rwolq();
			return new rporh(pjvbr, 1);
		case 1:
			ohhmm = new AlgorithmIdentifier();
			return ohhmm;
		case 2:
			fqgap = new yxkxe();
			return fqgap;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in shzvn
		return this.shzvn(p0, p1, p2);
	}

	private void gkfzm(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in gkfzm
		this.gkfzm(p0, p1, p2);
	}

	private void xmzyw()
	{
		if (crknj != null && 0 == 0)
		{
			throw new CryptographicException("Originator not found in RecipientInfo.");
		}
		if (ohhmm != null && 0 == 0)
		{
			throw new CryptographicException("Key encryption algorithm not found in RecipientInfo.");
		}
		if (fqgap != null && 0 == 0)
		{
			throw new CryptographicException("Encrypted keys not found in RecipientInfo.");
		}
		otxci = crknj.haacf();
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in xmzyw
		this.xmzyw();
	}

	private void qgoyl(fxakl p0)
	{
		ArrayList arrayList = new ArrayList();
		arrayList.Add(ksudg);
		arrayList.Add(new rporh(crknj, 0));
		if (pjvbr != null && 0 == 0)
		{
			arrayList.Add(new rporh(pjvbr, 1));
		}
		arrayList.Add(ohhmm);
		arrayList.Add(fqgap);
		p0.aiflg(rmkkr.osptv, arrayList);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in qgoyl
		this.qgoyl(p0);
	}
}
