using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class SignerInfo : lnabj
{
	private zjcch sjpbz;

	private SubjectIdentifier axyxz;

	private readonly bool ebuej;

	private AlgorithmIdentifier vvyqz;

	private ObjectIdentifier dskqf;

	private nnzwd kduxv;

	private CryptographicAttributeCollection gselo;

	private DateTime jvhpr;

	private SubjectIdentifier ptfke;

	private SecureMimeCapabilityCollection imtoq;

	private AlgorithmIdentifier fertl;

	private AlgorithmIdentifier dhsnr;

	private rwolq gbwfq;

	private CryptographicAttributeCollection ngdll;

	private CertificateChain hgxia;

	private SignedData rplfm;

	private byte[] ochth;

	public SubjectIdentifier SignerIdentifier => axyxz;

	public ObjectIdentifier DigestAlgorithm
	{
		get
		{
			if (vvyqz == null || 1 == 0)
			{
				return null;
			}
			if (dskqf == null || 1 == 0)
			{
				dskqf = new ObjectIdentifier(vvyqz.Oid);
			}
			return dskqf;
		}
	}

	internal bool ogvst => gbwfq != null;

	public AlgorithmIdentifier SignatureAlgorithm
	{
		get
		{
			if (fertl == null || 1 == 0)
			{
				return null;
			}
			if (dhsnr == null || 1 == 0)
			{
				dhsnr = fertl.evxkk();
			}
			return dhsnr;
		}
	}

	public byte[] Signature
	{
		get
		{
			if (gbwfq == null || 1 == 0)
			{
				return null;
			}
			if (ochth == null || 1 == 0)
			{
				ochth = (byte[])gbwfq.rtrhq.Clone();
			}
			return ochth;
		}
	}

	public CryptographicAttributeCollection SignedAttributes => gselo;

	public CryptographicAttributeCollection UnsignedAttributes => ngdll;

	public Certificate Certificate
	{
		get
		{
			if (hgxia == null || false || hgxia.Count == 0 || 1 == 0)
			{
				return null;
			}
			return hgxia[0];
		}
	}

	public CertificateChain CertificateChain
	{
		get
		{
			if (hgxia == null || false || hgxia.Count == 0 || 1 == 0)
			{
				return null;
			}
			return hgxia;
		}
	}

	public DateTime SigningTime
	{
		get
		{
			return jvhpr;
		}
		set
		{
			if (gbwfq != null && 0 == 0)
			{
				throw new CryptographicException("Unable to set signing time after a signature has been created.");
			}
			jvhpr = value;
		}
	}

	public SubjectIdentifier EncryptionKeyPreference
	{
		get
		{
			return ptfke;
		}
		set
		{
			if (gbwfq != null && 0 == 0)
			{
				throw new CryptographicException("Unable to set encryption key preference after a signature has been created.");
			}
			ptfke = value;
		}
	}

	public SecureMimeCapabilityCollection Capabilities => imtoq;

	internal int byjeu => sjpbz.kybig();

	internal SignedData ncyct => rplfm;

	public SignatureHashAlgorithm ToDigestAlgorithm()
	{
		return vvyqz.ldvqi();
	}

	internal bool iblrs(AlgorithmIdentifier p0)
	{
		if (vvyqz == null || 1 == 0)
		{
			return false;
		}
		return vvyqz.Oid.Value == p0.Oid.Value;
	}

	internal void lndip(SignedData p0)
	{
		if (p0 != null && 0 == 0)
		{
			if (rplfm != null && 0 == 0)
			{
				throw new CryptographicException("The signer was already associated with another message.");
			}
			rplfm = p0;
		}
	}

	internal void daovh(CertificateStore p0, ICertificateFinder p1)
	{
		CertificateChain certificateChain = axyxz.mgsxu(p0, p1);
		if (certificateChain != null && 0 == 0)
		{
			hgxia = certificateChain;
		}
	}

	internal void zljkd(CertificateCollection p0, CertificateIncludeOption p1)
	{
		if (hgxia == null || false || hgxia.Count < 1)
		{
			return;
		}
		if (!p0.Contains(hgxia[0]) || 1 == 0)
		{
			p0.Add(hgxia[0]);
		}
		if (p1 == CertificateIncludeOption.EndCertificateOnly)
		{
			return;
		}
		int num = 1;
		if (num == 0)
		{
			goto IL_005d;
		}
		goto IL_00c2;
		IL_00c2:
		if (num >= hgxia.Count)
		{
			return;
		}
		goto IL_005d;
		IL_005d:
		Certificate certificate = hgxia[num];
		if (p1 == CertificateIncludeOption.ExcludeRoot && num == hgxia.Count - 1 && certificate.GetIssuerName() == certificate.GetSubjectName() && 0 == 0)
		{
			return;
		}
		if (!p0.Contains(certificate) || 1 == 0)
		{
			p0.Add(certificate);
		}
		num++;
		goto IL_00c2;
	}

	public SignatureParameters GetSignatureParameters()
	{
		if (vvyqz == null || false || fertl == null)
		{
			return null;
		}
		HashingAlgorithmId hashingAlgorithmId = vvyqz.vvmoi(p0: false);
		if (hashingAlgorithmId == (HashingAlgorithmId)0 || 1 == 0)
		{
			return null;
		}
		SignatureParameters signatureParameters = fertl.jwptd(hashingAlgorithmId);
		if (signatureParameters == null || 1 == 0)
		{
			return null;
		}
		return signatureParameters;
	}

	public SignatureValidationResult Validate()
	{
		return lmlip(p0: false, ValidationOptions.None, CertificateChainEngine.Auto, azsca.uiamg);
	}

	public SignatureValidationResult Validate(bool verifySignatureOnly, ValidationOptions options)
	{
		return lmlip(verifySignatureOnly, options, CertificateChainEngine.Auto, azsca.uiamg);
	}

	public SignatureValidationResult Validate(bool verifySignatureOnly, ValidationOptions options, CertificateChainEngine engine)
	{
		return lmlip(verifySignatureOnly, options, engine, azsca.uiamg);
	}

	internal SignatureValidationResult lmlip(bool p0, ValidationOptions p1, CertificateChainEngine p2, azsca p3)
	{
		SignatureValidationResult signatureValidationResult = new SignatureValidationResult();
		byte[] array = rplfm.nddnj(vvyqz);
		if (array == null || 1 == 0)
		{
			signatureValidationResult.cwomp(SignatureValidationStatus.UnsupportedDigestAlgorithm);
			return signatureValidationResult;
		}
		if (kduxv != null && 0 == 0)
		{
			ObjectIdentifier objectIdentifier = kbseu();
			if (rplfm.sldbz.Value != objectIdentifier.Value && 0 == 0)
			{
				signatureValidationResult.cwomp(SignatureValidationStatus.ContentTypeMismatch);
			}
			byte[] p4 = nkiep();
			if (!zjcch.wduyr(array, p4) || 1 == 0)
			{
				signatureValidationResult.cwomp(SignatureValidationStatus.InvalidSignature);
				return signatureValidationResult;
			}
		}
		if (hgxia == null || false || hgxia.Count == 0 || 1 == 0)
		{
			signatureValidationResult.cwomp(SignatureValidationStatus.CertificateNotAvailable);
			return signatureValidationResult;
		}
		HashingAlgorithmId hashingAlgorithmId = vvyqz.vvmoi(p0: false);
		if (hashingAlgorithmId == (HashingAlgorithmId)0 || false || !HashingAlgorithm.IsSupported(hashingAlgorithmId) || 1 == 0)
		{
			signatureValidationResult.cwomp(SignatureValidationStatus.UnsupportedDigestAlgorithm);
			return signatureValidationResult;
		}
		SignatureParameters signatureParameters = fertl.jwptd(hashingAlgorithmId);
		if (signatureParameters == null || 1 == 0)
		{
			signatureValidationResult.cwomp(SignatureValidationStatus.UnsupportedSignatureAlgorithm);
			return signatureValidationResult;
		}
		byte[] p5 = ((kduxv == null) ? array : HashingAlgorithm.ComputeHash(hashingAlgorithmId, kduxv.lktyp));
		if (!hgxia.tlhda(p5, gbwfq.rtrhq, signatureParameters) || 1 == 0)
		{
			signatureValidationResult.cwomp(SignatureValidationStatus.InvalidSignature);
			return signatureValidationResult;
		}
		if (p0 && 0 == 0)
		{
			return signatureValidationResult;
		}
		if (((p1 & ValidationOptions.IgnoreWrongUsage) == 0 || 1 == 0) && (!zkqzn(hgxia[0], p3) || 1 == 0))
		{
			signatureValidationResult.cwomp(SignatureValidationStatus.InvalidKeyUsage);
		}
		ValidationResult p6 = hgxia.Validate(null, p1, p2);
		signatureValidationResult.mpunj(p6);
		return signatureValidationResult;
	}

	private bool zkqzn(Certificate p0, azsca p1)
	{
		switch (p1)
		{
		case azsca.xtlsb:
			return true;
		case azsca.uiamg:
			return true;
		case azsca.iuqjz:
		{
			KeyUses intendedUsage = p0.GetIntendedUsage();
			if ((intendedUsage & (KeyUses.DigitalSignature | KeyUses.NonRepudiation)) == 0 || 1 == 0)
			{
				return false;
			}
			string[] enhancedUsage = p0.GetEnhancedUsage();
			if (enhancedUsage != null && 0 == 0)
			{
				return enhancedUsage.babpw("1.3.6.1.5.5.7.3.4", "2.5.29.37.0") >= 0;
			}
			return true;
		}
		default:
			throw new InvalidOperationException("Unsupported key usage check type.");
		}
	}

	public void Sign()
	{
		xzonp((SignatureOptions)0L, azsca.uiamg);
	}

	public void Sign(SignatureOptions options)
	{
		xzonp(options, azsca.uiamg);
	}

	internal void xzonp(SignatureOptions p0, azsca p1)
	{
		if (gbwfq != null && 0 == 0)
		{
			throw new CryptographicException("The signer already has a signature.");
		}
		if (rplfm == null || 1 == 0)
		{
			throw new CryptographicException("The signer has not been associated with a message yet.");
		}
		if ((p0 & SignatureOptions.SkipCertificateUsageCheck) == 0 && (!zkqzn(hgxia[0], p1) || 1 == 0))
		{
			throw new CryptographicException("The signer's certificate is not intended for signing data.");
		}
		HashingAlgorithmId hashingAlgorithmId = vvyqz.vvmoi(p0: false);
		if (hashingAlgorithmId == (HashingAlgorithmId)0 || 1 == 0)
		{
			throw new CryptographicException("The signer's digest algorithm is not supported.");
		}
		if (!HashingAlgorithm.IsSupported(hashingAlgorithmId) || 1 == 0)
		{
			throw new CryptographicException("The signer's digest algorithm is not supported.");
		}
		byte[] array = rplfm.nddnj(vvyqz);
		if (array == null || 1 == 0)
		{
			throw new CryptographicException("The signer's digest algorithm is not supported.");
		}
		if (jvhpr <= new DateTime(1970, 1, 1) && 0 == 0)
		{
			jvhpr = DateTime.Now;
		}
		if (fertl.Oid.Value != "1.2.840.113549.1.1.10" && 0 == 0)
		{
			switch (fertl.qlesd())
			{
			default:
				throw new CryptographicException(brgjd.edcru("The signer's signature algorithm '{0}' is not supported.", fertl.Oid.Value));
			case KeyAlgorithm.RSA:
			case KeyAlgorithm.DSA:
				break;
			}
		}
		SignatureParameters signatureParameters = fertl.jwptd(hashingAlgorithmId);
		if (signatureParameters == null || 1 == 0)
		{
			throw new CryptographicException("Unsupported signature algorithm.");
		}
		imtoq.hksnh();
		byte[] p2;
		if ((p0 & SignatureOptions.DisableSignedAttributes) == 0)
		{
			gselo.lgfev("1.2.840.113549.1.9.3");
			gselo.lgfev("1.2.840.113549.1.9.4");
			gselo.Add(new CryptographicAttributeNode(new ObjectIdentifier("1.2.840.113549.1.9.3"), new wyjqw(rplfm.ContentInfo.ContentType).ionjf()));
			gselo.Add(new CryptographicAttributeNode(new ObjectIdentifier("1.2.840.113549.1.9.4"), new rwolq(array).ionjf()));
			lzlaq(p0);
			gselo.hksnh();
			byte[] array2 = fxakl.kncuz(gselo);
			kduxv = new nnzwd(array2);
			p2 = HashingAlgorithm.ComputeHash(hashingAlgorithmId, array2);
		}
		else
		{
			p2 = array;
		}
		signatureParameters.Silent = rplfm.Silent & ebuej;
		byte[] data = hgxia[0].vvtcj(p2, signatureParameters);
		gbwfq = new rwolq(data, clone: false);
	}

	private void lzlaq(SignatureOptions p0)
	{
		gselo.lgfev("1.2.840.113549.1.9.5");
		gselo.lgfev("1.2.840.113549.1.9.15");
		gselo.lgfev("1.2.840.113549.1.9.16.2.11");
		gselo.lgfev("1.3.6.1.4.1.311.16.4");
		if (jvhpr > new DateTime(1970, 1, 1) && 0 == 0)
		{
			gselo.Add(new CryptographicAttributeNode(new ObjectIdentifier("1.2.840.113549.1.9.5"), new gfiwx(jvhpr).ionjf()));
		}
		if (imtoq.Count > 0 && (p0 & SignatureOptions.DisableSMimeCapabilities) == 0)
		{
			gselo.Add(new CryptographicAttributeNode(new ObjectIdentifier("1.2.840.113549.1.9.15"), fxakl.kncuz(imtoq)));
		}
		if (ptfke != null && 0 == 0)
		{
			if ((p0 & SignatureOptions.DisableMicrosoftExtensions) == 0 && ptfke.Type == SubjectIdentifierType.IssuerAndSerialNumber)
			{
				gselo.Add(new CryptographicAttributeNode(new ObjectIdentifier("1.3.6.1.4.1.311.16.4"), fxakl.kncuz(ptfke.wdiep())));
			}
			pddyr pddyr = ptfke.ymxec();
			if (pddyr != null && 0 == 0)
			{
				gselo.Add(new CryptographicAttributeNode(new ObjectIdentifier("1.2.840.113549.1.9.16.2.11"), fxakl.kncuz(pddyr.scqbh())));
			}
		}
	}

	private CryptographicAttributeCollection hymgn(bool p0)
	{
		CryptographicAttributeCollection cryptographicAttributeCollection = new CryptographicAttributeCollection();
		if (kduxv != null && 0 == 0)
		{
			byte[] lktyp = kduxv.lktyp;
			lktyp[0] = 49;
			hfnnn.qnzgo(cryptographicAttributeCollection, lktyp);
		}
		if (p0 && 0 == 0)
		{
			cryptographicAttributeCollection.hksnh();
		}
		return cryptographicAttributeCollection;
	}

	private DateTime koooo()
	{
		CryptographicAttributeNode cryptographicAttributeNode = gselo["1.2.840.113549.1.9.5"];
		if (cryptographicAttributeNode == null || false || cryptographicAttributeNode.Values.Count == 0 || 1 == 0)
		{
			return new DateTime(1970, 1, 1);
		}
		return gfiwx.btezo(cryptographicAttributeNode.hwqek).fzcfd();
	}

	private SubjectIdentifier jtglp()
	{
		CryptographicAttributeNode cryptographicAttributeNode = gselo["1.2.840.113549.1.9.16.2.11"];
		if (cryptographicAttributeNode == null || false || cryptographicAttributeNode.Values.Count == 0 || 1 == 0)
		{
			cryptographicAttributeNode = gselo["1.3.6.1.4.1.311.16.4"];
			if (cryptographicAttributeNode == null || false || cryptographicAttributeNode.Values.Count == 0 || 1 == 0)
			{
				return null;
			}
			sxlwf sxlwf = new sxlwf();
			hfnnn.qnzgo(sxlwf, cryptographicAttributeNode.hwqek);
			return new SubjectIdentifier(sxlwf);
		}
		byte[] hwqek = cryptographicAttributeNode.hwqek;
		if (hwqek.Length == 0 || false || (hwqek[0] & 0x80) == 0 || 1 == 0)
		{
			return null;
		}
		pddyr pddyr = new pddyr(65536 + (hwqek[0] & 0x1F));
		hfnnn.qnzgo(pddyr.scqbh(), cryptographicAttributeNode.hwqek);
		return pddyr.pjlww();
	}

	private SecureMimeCapabilityCollection pmmsh(bool p0)
	{
		CryptographicAttributeNode cryptographicAttributeNode = gselo["1.2.840.113549.1.9.15"];
		SecureMimeCapabilityCollection secureMimeCapabilityCollection;
		if (cryptographicAttributeNode == null || false || cryptographicAttributeNode.Values.Count == 0 || 1 == 0)
		{
			secureMimeCapabilityCollection = new SecureMimeCapabilityCollection();
		}
		else
		{
			secureMimeCapabilityCollection = new SecureMimeCapabilityCollection();
			hfnnn.qnzgo(secureMimeCapabilityCollection, cryptographicAttributeNode.hwqek);
		}
		if (p0 && 0 == 0)
		{
			secureMimeCapabilityCollection.hksnh();
		}
		return secureMimeCapabilityCollection;
	}

	private ObjectIdentifier kbseu()
	{
		CryptographicAttributeNode cryptographicAttributeNode = gselo["1.2.840.113549.1.9.3"];
		if (cryptographicAttributeNode == null || false || cryptographicAttributeNode.Values.Count == 0 || 1 == 0)
		{
			return null;
		}
		return wyjqw.ewqkw(cryptographicAttributeNode.hwqek).scakm;
	}

	private byte[] nkiep()
	{
		CryptographicAttributeNode cryptographicAttributeNode = gselo["1.2.840.113549.1.9.4"];
		if (cryptographicAttributeNode == null || false || cryptographicAttributeNode.Values.Count == 0 || 1 == 0)
		{
			return null;
		}
		return rwolq.tvjgt(cryptographicAttributeNode.hwqek).rtrhq;
	}

	private void uyoup(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in uyoup
		this.uyoup(p0, p1, p2);
	}

	private lnabj dmpys(rmkkr p0, bool p1, int p2)
	{
		if (p2 == 0 || 1 == 0)
		{
			sjpbz = new zjcch();
			return sjpbz;
		}
		int num = 0;
		if (sjpbz != null && 0 == 0)
		{
			num = sjpbz.kybig();
		}
		switch (num)
		{
		case 1:
			switch (p2)
			{
			case 1:
				axyxz = new SubjectIdentifier(SubjectIdentifierType.IssuerAndSerialNumber);
				return axyxz.wdiep();
			case 2:
				vvyqz = new AlgorithmIdentifier();
				return vvyqz;
			case 65536:
				kduxv = new nnzwd();
				return kduxv;
			case 3:
				fertl = new AlgorithmIdentifier();
				return fertl;
			case 4:
				gbwfq = new rwolq();
				return gbwfq;
			case 65537:
				return new rwknq(ngdll, 1, rmkkr.wguaf);
			default:
				return null;
			}
		case 3:
			switch (p2)
			{
			case 1:
				vvyqz = new AlgorithmIdentifier();
				return vvyqz;
			case 65536:
				if (axyxz == null || 1 == 0)
				{
					axyxz = new SubjectIdentifier(SubjectIdentifierType.SubjectKeyIdentifier);
					return axyxz.wdiep();
				}
				kduxv = new nnzwd();
				return kduxv;
			case 2:
				fertl = new AlgorithmIdentifier();
				return fertl;
			case 3:
				gbwfq = new rwolq();
				return gbwfq;
			case 65537:
				return new rwknq(ngdll, 1, rmkkr.wguaf);
			default:
				return null;
			}
		default:
			throw new CryptographicException("A SignerInfo with unknown version encountered.");
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in dmpys
		return this.dmpys(p0, p1, p2);
	}

	private void bxjny(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in bxjny
		this.bxjny(p0, p1, p2);
	}

	private void eyhfg()
	{
		if (axyxz == null || 1 == 0)
		{
			throw new CryptographicException("Signer not found in SignerInfo.");
		}
		if (vvyqz == null || 1 == 0)
		{
			throw new CryptographicException("Digest algorithm not found in SignerInfo.");
		}
		if (fertl == null || 1 == 0)
		{
			throw new CryptographicException("Signature algorithm not found in SignerInfo.");
		}
		if (gbwfq == null || 1 == 0)
		{
			throw new CryptographicException("Signature not found in SignerInfo.");
		}
		bool flag;
		if (gbwfq.rtrhq.Length == 0 || 1 == 0)
		{
			gbwfq = null;
			flag = false;
			if (!flag)
			{
				goto IL_0094;
			}
		}
		flag = true;
		goto IL_0094;
		IL_0094:
		gselo = hymgn(flag);
		if (!flag || 1 == 0)
		{
			kduxv = null;
		}
		jvhpr = koooo();
		ptfke = jtglp();
		imtoq = pmmsh(flag);
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in eyhfg
		this.eyhfg();
	}

	private void ewgaw(fxakl p0)
	{
		p0.afwyb();
		ArrayList arrayList = new ArrayList();
		arrayList.Add(sjpbz);
		arrayList.Add(axyxz.wdiep());
		arrayList.Add(vvyqz);
		if (gbwfq != null && 0 == 0)
		{
			if (kduxv != null && 0 == 0)
			{
				arrayList.Add(new rwknq(kduxv, 0, rmkkr.wguaf));
			}
		}
		else
		{
			lzlaq((SignatureOptions)0L);
			arrayList.Add(new rwknq(gselo, 0, rmkkr.wguaf));
		}
		arrayList.Add(fertl);
		if (gbwfq != null && 0 == 0)
		{
			arrayList.Add(gbwfq);
		}
		else
		{
			arrayList.Add(new rwolq(new byte[0]));
		}
		if (ngdll.Count > 0)
		{
			arrayList.Add(new rwknq(ngdll, 1, rmkkr.wguaf));
		}
		p0.suudj((lnabj[])arrayList.ToArray(typeof(lnabj)));
		p0.xljze();
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ewgaw
		this.ewgaw(p0);
	}

	internal SignerInfo()
	{
		ngdll = new CryptographicAttributeCollection();
	}

	public SignerInfo(Certificate signerCertificate)
		: this(signerCertificate, SubjectIdentifierType.IssuerAndSerialNumber, SignatureHashAlgorithm.SHA1)
	{
	}

	public SignerInfo(Certificate signerCertificate, SubjectIdentifierType signerIdentifierType)
		: this(signerCertificate, signerIdentifierType, SignatureHashAlgorithm.SHA1)
	{
	}

	public SignerInfo(Certificate signerCertificate, SubjectIdentifierType signerIdentifierType, SignatureHashAlgorithm digestAlgorithm)
		: this(signerCertificate, SignatureParameters.preyi(digestAlgorithm, "digestAlgorithm"), signerIdentifierType)
	{
	}

	public SignerInfo(Certificate signerCertificate, SignatureParameters signatureParameters)
		: this(signerCertificate, signatureParameters, SubjectIdentifierType.SubjectKeyIdentifier)
	{
	}

	public SignerInfo(Certificate signerCertificate, SignatureParameters signatureParameters, SubjectIdentifierType signerIdentifierType)
	{
		if (signerCertificate == null || 1 == 0)
		{
			throw new ArgumentNullException("signerCertificate");
		}
		SignatureParameters.ffnfz(signatureParameters, bpkgq.zjdcx(signerCertificate.KeyAlgorithm), signerCertificate.GetKeySize(), out var p, out var p2, out var p3);
		goies hqtwc = p3.hqtwc;
		if (p != 0 && 0 == 0 && p != SignatureFormat.Pkcs)
		{
			throw new CryptographicException("Only PKCS #7 format is supported for CMS.");
		}
		vvyqz = AlgorithmIdentifier.heubo(p2);
		if (vvyqz == null || 1 == 0)
		{
			throw new CryptographicException("Unsupported digest algorithm.");
		}
		switch (signerCertificate.KeyAlgorithm)
		{
		case KeyAlgorithm.RSA:
			switch (hqtwc)
			{
			case goies.lfkki:
				switch (p2)
				{
				case HashingAlgorithmId.SHA1:
				case HashingAlgorithmId.MD4:
				case HashingAlgorithmId.MD5:
					fertl = new AlgorithmIdentifier(new ObjectIdentifier("1.2.840.113549.1.1.1"), new mdvaz().ionjf());
					break;
				case HashingAlgorithmId.SHA224:
					fertl = new AlgorithmIdentifier(new ObjectIdentifier("1.2.840.113549.1.1.14"), new mdvaz().ionjf());
					break;
				case HashingAlgorithmId.SHA256:
					fertl = new AlgorithmIdentifier(new ObjectIdentifier("1.2.840.113549.1.1.11"), new mdvaz().ionjf());
					break;
				case HashingAlgorithmId.SHA384:
					fertl = new AlgorithmIdentifier(new ObjectIdentifier("1.2.840.113549.1.1.12"), new mdvaz().ionjf());
					break;
				case HashingAlgorithmId.SHA512:
					fertl = new AlgorithmIdentifier(new ObjectIdentifier("1.2.840.113549.1.1.13"), new mdvaz().ionjf());
					break;
				default:
					throw new CryptographicException("Unsupported digest algorithm for RSA.");
				}
				break;
			case goies.mrskp:
				fertl = new AlgorithmIdentifier(parameters: fxakl.kncuz(new bbiry(p3)), oid: "1.2.840.113549.1.1.10");
				break;
			default:
				throw new CryptographicException("Unsupported padding scheme for RSA.");
			}
			break;
		case KeyAlgorithm.DSA:
			if (p2 != HashingAlgorithmId.SHA1)
			{
				throw new CryptographicException("Unsupported digest algorithm for DSA.");
			}
			if (hqtwc != goies.gbwxv)
			{
				throw new CryptographicException("Unsupported padding scheme for DSA.");
			}
			fertl = new AlgorithmIdentifier(new ObjectIdentifier("1.2.840.10040.4.3"));
			break;
		default:
			throw new CryptographicException("The certificate uses an unsupported key algorithm.");
		}
		if (!signerCertificate.HasPrivateKey() || 1 == 0)
		{
			throw new CryptographicException("The certificate does not have an associated private key.");
		}
		int num;
		switch (signerIdentifierType)
		{
		case SubjectIdentifierType.IssuerAndSerialNumber:
			num = 1;
			if (num != 0)
			{
				break;
			}
			goto case SubjectIdentifierType.SubjectKeyIdentifier;
		case SubjectIdentifierType.SubjectKeyIdentifier:
			num = 3;
			if (num != 0)
			{
				break;
			}
			goto default;
		default:
			throw new ArgumentException("Unsupported subject identifier type.", "signerIdentifierType");
		}
		sjpbz = new zjcch(num);
		axyxz = new SubjectIdentifier(signerCertificate, signerIdentifierType);
		hgxia = CertificateEngine.kemvq().BuildChain(signerCertificate);
		ebuej = signatureParameters == null || 1 == 0 || signatureParameters.Silent;
		gselo = new CryptographicAttributeCollection();
		ngdll = new CryptographicAttributeCollection();
		jvhpr = new DateTime(1970, 1, 1);
		ptfke = null;
		imtoq = new SecureMimeCapabilityCollection();
		imtoq.Add(new SecureMimeCapability(new ObjectIdentifier("1.2.840.113549.3.7")));
		imtoq.Add(new SecureMimeCapability(new ObjectIdentifier("2.16.840.1.101.3.4.1.2")));
		imtoq.Add(new SecureMimeCapability(new ObjectIdentifier("2.16.840.1.101.3.4.1.22")));
		imtoq.Add(new SecureMimeCapability(new ObjectIdentifier("2.16.840.1.101.3.4.1.42")));
		imtoq.Add(new SecureMimeCapability(new ObjectIdentifier("1.3.14.3.2.26")));
		imtoq.Add(new SecureMimeCapability(new ObjectIdentifier("2.16.840.1.101.3.4.2.1")));
		imtoq.Add(new SecureMimeCapability(new ObjectIdentifier("2.16.840.1.101.3.4.2.2")));
		imtoq.Add(new SecureMimeCapability(new ObjectIdentifier("2.16.840.1.101.3.4.2.3")));
	}

	internal SignerInfo haaka(SignedData p0)
	{
		SignerInfo signerInfo = new SignerInfo();
		signerInfo.sjpbz = sjpbz;
		signerInfo.axyxz = axyxz;
		signerInfo.vvyqz = vvyqz;
		signerInfo.fertl = fertl;
		signerInfo.gbwfq = gbwfq;
		if (kduxv != null && 0 == 0)
		{
			signerInfo.kduxv = kduxv;
			signerInfo.gselo = gselo;
			signerInfo.jvhpr = jvhpr;
			signerInfo.ptfke = ptfke;
			signerInfo.imtoq = imtoq;
		}
		else
		{
			signerInfo.gselo = new CryptographicAttributeCollection();
			IEnumerator<CryptographicAttributeNode> enumerator = gselo.GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					CryptographicAttributeNode current = enumerator.Current;
					signerInfo.gselo.Add(current.rtdcm());
				}
			}
			finally
			{
				if (enumerator != null && 0 == 0)
				{
					enumerator.Dispose();
				}
			}
			signerInfo.jvhpr = jvhpr;
			signerInfo.ptfke = ptfke;
			signerInfo.imtoq = new SecureMimeCapabilityCollection();
			IEnumerator<SecureMimeCapability> enumerator2 = imtoq.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext() ? true : false)
				{
					SecureMimeCapability current2 = enumerator2.Current;
					signerInfo.imtoq.Add(current2.xxvjn());
				}
			}
			finally
			{
				if (enumerator2 != null && 0 == 0)
				{
					enumerator2.Dispose();
				}
			}
		}
		IEnumerator<CryptographicAttributeNode> enumerator3 = ngdll.GetEnumerator();
		try
		{
			while (enumerator3.MoveNext() ? true : false)
			{
				CryptographicAttributeNode current3 = enumerator3.Current;
				signerInfo.ngdll.Add(current3.rtdcm());
			}
		}
		finally
		{
			if (enumerator3 != null && 0 == 0)
			{
				enumerator3.Dispose();
			}
		}
		signerInfo.hgxia = hgxia;
		signerInfo.rplfm = p0;
		return signerInfo;
	}
}
