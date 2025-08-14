using System;
using System.Security.Cryptography;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class KeyTransRecipientInfo : RecipientInfo, lnabj
{
	private zjcch ammxh;

	private SubjectIdentifier ybovy;

	private AlgorithmIdentifier pdugt;

	private AlgorithmIdentifier loenh;

	private rwolq luwip;

	private byte[] pewwg;

	private CertificateChain hyuqk;

	internal override int bilco => ammxh.kybig();

	public override SubjectIdentifier RecipientIdentifier => ybovy;

	public override AlgorithmIdentifier KeyEncryptionAlgorithm
	{
		get
		{
			if (pdugt == null || 1 == 0)
			{
				return null;
			}
			if (loenh == null || 1 == 0)
			{
				loenh = pdugt.evxkk();
			}
			return loenh;
		}
	}

	public override byte[] EncryptedKey
	{
		get
		{
			if (luwip == null || false || luwip.rtrhq == null)
			{
				return null;
			}
			if (pewwg == null || 1 == 0)
			{
				pewwg = (byte[])luwip.rtrhq.Clone();
			}
			return pewwg;
		}
	}

	public override Certificate Certificate
	{
		get
		{
			if (hyuqk == null || false || hyuqk.Count == 0 || 1 == 0)
			{
				return null;
			}
			return hyuqk[0];
		}
	}

	public override CertificateChain CertificateChain
	{
		get
		{
			if (hyuqk == null || false || hyuqk.Count == 0 || 1 == 0)
			{
				return null;
			}
			return hyuqk;
		}
	}

	internal override bool vqzfk
	{
		get
		{
			if (hyuqk == null || 1 == 0)
			{
				return false;
			}
			return hyuqk[0].HasPrivateKey();
		}
	}

	internal KeyTransRecipientInfo()
	{
	}

	public KeyTransRecipientInfo(Certificate recipientCertificate)
		: this(recipientCertificate, null, SubjectIdentifierType.IssuerAndSerialNumber)
	{
	}

	public KeyTransRecipientInfo(Certificate recipientCertificate, SubjectIdentifierType recipientIdentifierType)
		: this(recipientCertificate, null, recipientIdentifierType)
	{
	}

	public KeyTransRecipientInfo(Certificate recipientCertificate, EncryptionParameters encryptionParameters)
		: this(recipientCertificate, encryptionParameters, SubjectIdentifierType.IssuerAndSerialNumber)
	{
	}

	public KeyTransRecipientInfo(Certificate recipientCertificate, EncryptionParameters encryptionParameters, SubjectIdentifierType recipientIdentifierType)
	{
		if (recipientCertificate == null || 1 == 0)
		{
			throw new ArgumentNullException("recipientCertificate");
		}
		int num;
		switch (recipientIdentifierType)
		{
		case SubjectIdentifierType.IssuerAndSerialNumber:
			num = 0;
			if (num == 0)
			{
				break;
			}
			goto case SubjectIdentifierType.SubjectKeyIdentifier;
		case SubjectIdentifierType.SubjectKeyIdentifier:
			num = 2;
			if (num != 0)
			{
				break;
			}
			goto default;
		default:
			throw new ArgumentException("Unsupported subject identifier type.", "recipientIdentifierType");
		}
		if (recipientCertificate.KeyAlgorithm != KeyAlgorithm.RSA)
		{
			throw new CryptographicException("The certificate uses an unsupported key algorithm.");
		}
		EncryptionParameters.lkmtd(encryptionParameters, AsymmetricKeyAlgorithmId.RSA, recipientCertificate.GetKeySize(), null, out var p);
		switch (p.vmeor)
		{
		case xdgzn.bntzq:
		{
			uchfa p2 = new uchfa(p);
			pdugt = new AlgorithmIdentifier("1.2.840.113549.1.1.7", fxakl.kncuz(p2));
			break;
		}
		case xdgzn.ctbmq:
			pdugt = new AlgorithmIdentifier(new ObjectIdentifier("1.2.840.113549.1.1.1"), new mdvaz().ionjf());
			break;
		default:
			throw new CryptographicException("Unsupported padding scheme.");
		}
		ammxh = new zjcch(num);
		ybovy = new SubjectIdentifier(recipientCertificate, recipientIdentifierType);
		hyuqk = CertificateEngine.kemvq().BuildChain(recipientCertificate);
	}

	internal override RecipientInfo krqfr()
	{
		KeyTransRecipientInfo keyTransRecipientInfo = new KeyTransRecipientInfo();
		keyTransRecipientInfo.ammxh = ammxh;
		keyTransRecipientInfo.ybovy = ybovy;
		keyTransRecipientInfo.pdugt = pdugt;
		keyTransRecipientInfo.luwip = luwip;
		keyTransRecipientInfo.hyuqk = hyuqk;
		return keyTransRecipientInfo;
	}

	public override EncryptionParameters GetEncryptionParameters()
	{
		if (pdugt == null || 1 == 0)
		{
			return null;
		}
		return pdugt.kwisk();
	}

	internal override void oyacr(CertificateStore p0, ICertificateFinder p1)
	{
		CertificateChain certificateChain = ybovy.mgsxu(p0, p1);
		if (certificateChain != null && 0 == 0)
		{
			hyuqk = certificateChain;
		}
	}

	internal override bool zqjdy(bool p0)
	{
		if (hyuqk == null || false || luwip == null || false || luwip.rtrhq == null)
		{
			return false;
		}
		if (hyuqk[0].KeyAlgorithm != KeyAlgorithm.RSA && 0 == 0)
		{
			return false;
		}
		if (hyuqk[0].kivnt(p0) && 0 == 0)
		{
			return true;
		}
		return false;
	}

	internal override byte[] ghozk(bool p0)
	{
		if (hyuqk == null || false || luwip == null || false || luwip.rtrhq == null)
		{
			return null;
		}
		if (hyuqk[0].KeyAlgorithm != KeyAlgorithm.RSA && 0 == 0)
		{
			return null;
		}
		EncryptionParameters encryptionParameters = pdugt.kwisk();
		if (encryptionParameters == null || 1 == 0)
		{
			return null;
		}
		if (!hyuqk[0].kivnt(p0) || 1 == 0)
		{
			return null;
		}
		encryptionParameters.Silent = p0;
		return hyuqk[0].nivtp(luwip.rtrhq, encryptionParameters);
	}

	internal override void bvglb(EnvelopedData p0)
	{
		base.bvglb(p0);
		if (p0 != null && 0 == 0 && (luwip == null || 1 == 0) && hyuqk[0].KeyAlgorithm == KeyAlgorithm.RSA)
		{
			byte[] symmetricKey = p0.GetSymmetricKey();
			if (symmetricKey == null || 1 == 0)
			{
				throw new CryptographicException("Cannot retrieve the symmetric key of the message.");
			}
			EncryptionParameters encryptionParameters = pdugt.kwisk();
			if (encryptionParameters == null || 1 == 0)
			{
				throw new CryptographicException("Unsupported key encryption algorithm.");
			}
			byte[] data = hyuqk[0].hoier(symmetricKey, encryptionParameters);
			luwip = new rwolq(data);
		}
	}

	internal override bool oaeit()
	{
		Certificate certificate = Certificate;
		if (certificate != null && 0 == 0)
		{
			return (certificate.GetIntendedUsage() & KeyUses.KeyEncipherment) != 0;
		}
		return true;
	}

	private void nfeac(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in nfeac
		this.nfeac(p0, p1, p2);
	}

	private lnabj fkuey(rmkkr p0, bool p1, int p2)
	{
		if (p2 == 0 || 1 == 0)
		{
			ammxh = new zjcch();
			return ammxh;
		}
		int num = 0;
		if (ammxh != null && 0 == 0)
		{
			num = ammxh.kybig();
		}
		if (num == 0 || 1 == 0)
		{
			switch (p2)
			{
			case 0:
				return ammxh;
			case 1:
				ybovy = new SubjectIdentifier(SubjectIdentifierType.IssuerAndSerialNumber);
				return ybovy.wdiep();
			case 2:
				pdugt = new AlgorithmIdentifier();
				return pdugt;
			case 3:
				luwip = new rwolq();
				return luwip;
			default:
				return null;
			}
		}
		if (num == 2)
		{
			switch (p2)
			{
			case 0:
				return ammxh;
			case 65536:
				ybovy = new SubjectIdentifier(SubjectIdentifierType.SubjectKeyIdentifier);
				return ybovy.wdiep();
			case 1:
				pdugt = new AlgorithmIdentifier();
				return pdugt;
			case 2:
				luwip = new rwolq();
				return luwip;
			default:
				return null;
			}
		}
		throw new CryptographicException("A RecipientInfo with unknown version encountered.");
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in fkuey
		return this.fkuey(p0, p1, p2);
	}

	private void jxbym(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in jxbym
		this.jxbym(p0, p1, p2);
	}

	private void bilvu()
	{
		if (ybovy == null || 1 == 0)
		{
			throw new CryptographicException("Recipient not found in RecipientInfo.");
		}
		if (pdugt == null || 1 == 0)
		{
			throw new CryptographicException("Key encryption algorithm not found in RecipientInfo.");
		}
		if (luwip == null || 1 == 0)
		{
			throw new CryptographicException("Encrypted key not found in RecipientInfo.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in bilvu
		this.bilvu();
	}

	private void qjsnb(fxakl p0)
	{
		p0.suudj(ammxh, ybovy.wdiep(), pdugt, luwip);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in qjsnb
		this.qjsnb(p0);
	}
}
