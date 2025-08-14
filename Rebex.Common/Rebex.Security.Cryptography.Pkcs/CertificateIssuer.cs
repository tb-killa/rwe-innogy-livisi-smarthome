using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class CertificateIssuer
{
	private static void hujjx(Certificate p0, PublicKeyInfo p1, CertificateInfo p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("certificationAuthority");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("publicKey");
		}
		if (p2 == null || 1 == 0)
		{
			throw new ArgumentNullException("certificateInfo");
		}
		if (p1.KeyAlgorithm == null || 1 == 0)
		{
			throw new InvalidOperationException("Supplied public key has not been initialized.");
		}
	}

	public static Certificate Issue(Certificate certificationAuthority, CertificateInfo certificateInfo, PublicKeyInfo publicKey)
	{
		hujjx(certificationAuthority, publicKey, certificateInfo);
		SignatureHashAlgorithm? p = certificateInfo.nulsj();
		return oacbx(certificationAuthority, certificateInfo, publicKey, p);
	}

	private static Certificate oacbx(Certificate p0, CertificateInfo p1, PublicKeyInfo p2, SignatureHashAlgorithm? p3)
	{
		jpwxz p4 = new jpwxz(p0, p3);
		DistinguishedName subject = p0.GetSubject();
		byte[] array = p0.GetSubjectKeyIdentifier();
		if (array != null && 0 == 0 && (array.Length == 0 || 1 == 0))
		{
			array = null;
		}
		byte[] p5;
		if (p1 == null || 1 == 0)
		{
			p5 = null;
		}
		else if (p1.CrlDistributionPoints.Count == 0 || 1 == 0)
		{
			CertificateExtension certificateExtension = p0.prkbw("2.5.29.31");
			p5 = ((certificateExtension == null || false || certificateExtension.Value.Length <= 0) ? null : certificateExtension.Value);
		}
		else
		{
			p5 = fxakl.kncuz(p1.CrlDistributionPoints);
		}
		byte[] p6 = ((p1.hqdrd.Count > 0) ? fxakl.kncuz(p1.hqdrd) : null);
		return pzuyw(p4, p2, subject, p1, array, p5, p6);
	}

	public static Certificate Issue(CertificateInfo certificateInfo, PrivateKeyInfo privateKey)
	{
		if (certificateInfo == null || 1 == 0)
		{
			throw new ArgumentNullException("certificateInfo");
		}
		if (privateKey == null || 1 == 0)
		{
			throw new ArgumentNullException("privateKey");
		}
		SignatureHashAlgorithm? p = certificateInfo.nulsj();
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = new AsymmetricKeyAlgorithm();
		try
		{
			asymmetricKeyAlgorithm.ImportKey(privateKey);
			return rvgjp(asymmetricKeyAlgorithm, certificateInfo, p);
		}
		finally
		{
			if (asymmetricKeyAlgorithm != null && 0 == 0)
			{
				((IDisposable)asymmetricKeyAlgorithm).Dispose();
			}
		}
	}

	private static Certificate rvgjp(AsymmetricKeyAlgorithm p0, CertificateInfo p1, SignatureHashAlgorithm? p2)
	{
		jpwxz p3 = new jpwxz(p0, p2);
		PublicKeyInfo publicKey = p0.GetPublicKey();
		DistinguishedName subject = p1.Subject;
		byte[] p4 = ((p1.CrlDistributionPoints.Count <= 0) ? null : fxakl.kncuz(p1.CrlDistributionPoints));
		byte[] p5 = ((p1.hqdrd.Count > 0) ? fxakl.kncuz(p1.hqdrd) : null);
		return pzuyw(p3, publicKey, subject, p1, null, p4, p5);
	}

	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This overload of Issue method has been deprecated. Please use Issue(Certificate, CertificateInfo, PublicKeyInfo) overload instead.", false)]
	public static Certificate Issue(Certificate certificationAuthority, SignatureHashAlgorithm signatureHashAlgorithm, PublicKeyInfo publicKey, CertificateInfo certificateInfo)
	{
		hujjx(certificationAuthority, publicKey, certificateInfo);
		return oacbx(certificationAuthority, certificateInfo, publicKey, signatureHashAlgorithm);
	}

	[Obsolete("This overload of Issue method has been deprecated. Please use Issue(CertificateInfo, PrivateKeyInfo) overload instead.", false)]
	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static Certificate Issue(KeyAlgorithm keyAlgorithm, int keySize, SignatureHashAlgorithm signatureHashAlgorithm, CertificateInfo certificateInfo, out PrivateKeyInfo privateKey)
	{
		if (certificateInfo == null || 1 == 0)
		{
			throw new ArgumentNullException("certificateInfo");
		}
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = new AsymmetricKeyAlgorithm();
		try
		{
			switch (keyAlgorithm)
			{
			case KeyAlgorithm.RSA:
				asymmetricKeyAlgorithm.GenerateKey(AsymmetricKeyAlgorithmId.RSA, keySize);
				break;
			case KeyAlgorithm.DSA:
				asymmetricKeyAlgorithm.GenerateKey(AsymmetricKeyAlgorithmId.DSA, keySize);
				break;
			case KeyAlgorithm.ECDsa:
				asymmetricKeyAlgorithm.kvrol(AsymmetricKeyAlgorithmId.ECDsa, signatureHashAlgorithm switch
				{
					SignatureHashAlgorithm.SHA224 => "1.2.840.10045.3.1.7", 
					SignatureHashAlgorithm.SHA256 => "1.2.840.10045.3.1.7", 
					SignatureHashAlgorithm.SHA384 => "1.3.132.0.34", 
					SignatureHashAlgorithm.SHA512 => "1.3.132.0.35", 
					_ => throw new InvalidOperationException("Hash algorithm is not suitable for the specified key algorithm."), 
				}, keySize);
				break;
			case KeyAlgorithm.ED25519:
				if (signatureHashAlgorithm != SignatureHashAlgorithm.SHA512)
				{
					throw new InvalidOperationException("Hash algorithm is not suitable for the specified key algorithm.");
				}
				asymmetricKeyAlgorithm.kvrol(AsymmetricKeyAlgorithmId.EdDsa, "1.3.101.112", keySize);
				break;
			default:
				throw hifyx.nztrs("keyAlgorithm", keyAlgorithm, "Unsupported key algorithm.");
			}
			privateKey = asymmetricKeyAlgorithm.GetPrivateKey();
			return rvgjp(asymmetricKeyAlgorithm, certificateInfo, signatureHashAlgorithm);
		}
		finally
		{
			if (asymmetricKeyAlgorithm != null && 0 == 0)
			{
				((IDisposable)asymmetricKeyAlgorithm).Dispose();
			}
		}
	}

	private static Certificate pzuyw(jpwxz p0, PublicKeyInfo p1, DistinguishedName p2, CertificateInfo p3, byte[] p4, byte[] p5, byte[] p6)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("signer");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("publicKey");
		}
		if (p2 == null || 1 == 0)
		{
			throw new ArgumentNullException("issuer");
		}
		if (p3 == null || 1 == 0)
		{
			throw new ArgumentNullException("certificateInfo");
		}
		byte[] input = fxakl.kncuz(p1);
		byte[] data = HashingAlgorithm.ComputeHash(HashingAlgorithmId.SHA1, input);
		byte[] serialNumber = p3.GetSerialNumber();
		DistinguishedName subject = p3.Subject;
		KeyUses usage = p3.Usage;
		string[] extendedUsage = p3.GetExtendedUsage();
		DateTime effectiveDate = p3.EffectiveDate;
		DateTime expirationDate = p3.ExpirationDate;
		CertificateExtensionCollection certificateExtensionCollection = new CertificateExtensionCollection();
		IEnumerator<CertificateExtension> enumerator = p3.Extensions.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				CertificateExtension current = enumerator.Current;
				certificateExtensionCollection.Add(current);
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		string mailAddress = p3.MailAddress;
		string[] alternativeHostnames = p3.GetAlternativeHostnames();
		KeyValuePair<ObjectIdentifier, byte[]>[] otherNames = p3.GetOtherNames();
		if (serialNumber == null || 1 == 0)
		{
			throw new CryptographicException("Missing serial number in certificate info.");
		}
		if (subject == null || 1 == 0)
		{
			throw new CryptographicException("Missing subject in certificate info.");
		}
		if (expirationDate <= effectiveDate && 0 == 0)
		{
			throw new CertificateException("Certificate validity interval is empty.");
		}
		certificateExtensionCollection.Add(CertificateExtension.KeyUsage(usage));
		if ((usage & KeyUses.KeyCertSign) != 0 && 0 == 0)
		{
			qljry(certificateExtensionCollection, new CertificateExtension(new ObjectIdentifier("2.5.29.19"), critical: true, new byte[5] { 48, 3, 1, 1, 255 }));
		}
		qljry(certificateExtensionCollection, new CertificateExtension(new ObjectIdentifier("2.5.29.14"), critical: false, new rwolq(data).ionjf()));
		motgc motgc;
		string[] array;
		int num;
		if (extendedUsage != null && 0 == 0 && extendedUsage.Length > 0)
		{
			motgc = new motgc();
			array = extendedUsage;
			num = 0;
			if (num != 0)
			{
				goto IL_01d4;
			}
			goto IL_01ef;
		}
		goto IL_0215;
		IL_0215:
		azfwz azfwz = new azfwz();
		KeyValuePair<ObjectIdentifier, byte[]>[] array2;
		int num2;
		if (otherNames != null && 0 == 0)
		{
			array2 = otherNames;
			num2 = 0;
			if (num2 != 0)
			{
				goto IL_0233;
			}
			goto IL_0263;
		}
		goto IL_026b;
		IL_01ef:
		if (num < array.Length)
		{
			goto IL_01d4;
		}
		qljry(certificateExtensionCollection, new CertificateExtension(new ObjectIdentifier("2.5.29.37"), critical: false, fxakl.kncuz(motgc)));
		goto IL_0215;
		IL_026b:
		if (mailAddress != null && 0 == 0)
		{
			azfwz.gfsfs(mailAddress, ukmqt.yzwxj);
		}
		string[] array3;
		int num3;
		if (alternativeHostnames != null && 0 == 0)
		{
			array3 = alternativeHostnames;
			num3 = 0;
			if (num3 != 0)
			{
				goto IL_0298;
			}
			goto IL_02d7;
		}
		goto IL_02df;
		IL_0263:
		if (num2 < array2.Length)
		{
			goto IL_0233;
		}
		goto IL_026b;
		IL_0233:
		KeyValuePair<ObjectIdentifier, byte[]> keyValuePair = array2[num2];
		azfwz.gynds(keyValuePair.Key, new nnzwd(keyValuePair.Value));
		num2++;
		goto IL_0263;
		IL_02df:
		if (azfwz.Count > 0)
		{
			qljry(certificateExtensionCollection, new CertificateExtension(new ObjectIdentifier("2.5.29.17"), critical: false, fxakl.kncuz(azfwz)));
		}
		if (p4 != null && 0 == 0)
		{
			qljry(certificateExtensionCollection, new CertificateExtension(new ObjectIdentifier("2.5.29.35"), critical: false, fxakl.kncuz(new ohkxj(p4))));
		}
		if (p5 != null && 0 == 0)
		{
			qljry(certificateExtensionCollection, new CertificateExtension(new ObjectIdentifier("2.5.29.31"), critical: false, p5));
		}
		if (p6 != null && 0 == 0)
		{
			qljry(certificateExtensionCollection, new CertificateExtension(new ObjectIdentifier("1.3.6.1.5.5.7.1.1"), critical: false, p6));
		}
		nisgb p7 = new nisgb(p0, p2, serialNumber, subject, p1, certificateExtensionCollection, effectiveDate, expirationDate);
		MemoryStream memoryStream = new MemoryStream();
		try
		{
			fxakl fxakl = new fxakl(memoryStream);
			fxakl.afwyb();
			fxakl.kfyej(p7);
			fxakl.xljze();
			fxakl.imfsc();
			byte[] data2 = memoryStream.ToArray();
			return new Certificate(data2);
		}
		finally
		{
			if (memoryStream != null && 0 == 0)
			{
				((IDisposable)memoryStream).Dispose();
			}
		}
		IL_0298:
		string text = array3[num3];
		IPAddress iPAddress;
		try
		{
			iPAddress = IPAddress.Parse(text);
		}
		catch (FormatException)
		{
			iPAddress = null;
		}
		if (iPAddress != null && 0 == 0)
		{
			azfwz.usutz(iPAddress);
		}
		else
		{
			azfwz.gfsfs(text, ukmqt.zhlgm);
		}
		num3++;
		goto IL_02d7;
		IL_02d7:
		if (num3 < array3.Length)
		{
			goto IL_0298;
		}
		goto IL_02df;
		IL_01d4:
		string oid = array[num];
		motgc.Add(new wyjqw(oid));
		num++;
		goto IL_01ef;
	}

	public static CertificateRevocationList IssueRevocationList(Certificate certificationAuthority, SignatureHashAlgorithm signatureHashAlgorithm, RevocationListInfo revocationListInfo, IEnumerable revokedCertificates)
	{
		if (certificationAuthority == null || 1 == 0)
		{
			throw new ArgumentNullException("certificationAuthority");
		}
		if (revocationListInfo == null || 1 == 0)
		{
			throw new ArgumentNullException("revocationListInfo");
		}
		if (revokedCertificates == null || 1 == 0)
		{
			throw new ArgumentNullException("revokedCertificates");
		}
		jpwxz signer = new jpwxz(certificationAuthority, signatureHashAlgorithm);
		DateTime thisUpdate = revocationListInfo.ThisUpdate;
		DateTime nextUpdate = revocationListInfo.NextUpdate;
		if (nextUpdate <= thisUpdate && 0 == 0)
		{
			throw new CertificateException("Invalid next update time.");
		}
		return new CertificateRevocationList(signer, certificationAuthority, thisUpdate, nextUpdate, revokedCertificates);
	}

	private static void qljry(CertificateExtensionCollection p0, CertificateExtension p1)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_0034;
		IL_0006:
		if (p0[num].Oid == p1.Oid && 0 == 0)
		{
			p0[num] = p1;
			return;
		}
		num++;
		goto IL_0034;
		IL_0034:
		if (num < p0.Count)
		{
			goto IL_0006;
		}
		p0.Add(p1);
	}
}
