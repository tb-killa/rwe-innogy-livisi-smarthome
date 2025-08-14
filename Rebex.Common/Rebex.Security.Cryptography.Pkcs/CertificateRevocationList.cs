using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class CertificateRevocationList : PkcsBase, lnabj
{
	private const string tnayj = "2.5.29.28";

	[NonSerialized]
	private nnzwd knass;

	[NonSerialized]
	private AlgorithmIdentifier rgzpc;

	[NonSerialized]
	private htykq yxgxq;

	[NonSerialized]
	private kmtni oevhn;

	[NonSerialized]
	private int? vhpug;

	[NonSerialized]
	private bool bcsmb;

	public DistinguishedName Issuer => oevhn.rrbpq;

	public DateTime ThisUpdate => oevhn.kuide;

	public DateTime NextUpdate => oevhn.pevmz;

	public int? CrlNumber
	{
		get
		{
			if (!bcsmb || 1 == 0)
			{
				CertificateExtension certificateExtension = oevhn.abicy["2.5.29.20"];
				if (certificateExtension == null || 1 == 0)
				{
					vhpug = null;
				}
				else
				{
					vhpug = zjcch.yowmh(certificateExtension.Value).kybig();
				}
				bcsmb = true;
			}
			return vhpug;
		}
	}

	public CertificateExtensionCollection Extensions => oevhn.abicy;

	public RevokedCertificateCollection RevokedCertificates => oevhn.imcen;

	public KeyAlgorithm KeyAlgorithm => rgzpc.qlesd();

	public SignatureHashAlgorithm SignatureHashAlgorithm => rgzpc.ldvqi();

	internal CertificateRevocationList(jpwxz signer, Certificate authority, DateTime thisUpdate, DateTime nextUpdate, IEnumerable revokedCertificates)
	{
		rgzpc = signer.kjail;
		oevhn = new kmtni(authority, rgzpc, thisUpdate, nextUpdate);
		byte[] subjectKeyIdentifier = authority.GetSubjectKeyIdentifier();
		if (subjectKeyIdentifier != null && 0 == 0 && subjectKeyIdentifier.Length > 0)
		{
			oevhn.abicy.Add(new CertificateExtension(new ObjectIdentifier("2.5.29.35"), critical: false, fxakl.kncuz(new ohkxj(subjectKeyIdentifier))));
		}
		DateTime now = DateTime.Now;
		now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
		IEnumerator enumerator = revokedCertificates.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				object current = enumerator.Current;
				if (current is RevokedCertificate item && 0 == 0)
				{
					oevhn.imcen.Add(item);
					continue;
				}
				if (current is Certificate certificate)
				{
					RevokedCertificate item2 = new RevokedCertificate(certificate.GetSerialNumber(), now, RevocationReason.Unspecified);
					oevhn.imcen.Add(item2);
					continue;
				}
				throw new CryptographicException("Invalid object in revoked certificates collection.");
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable && 0 == 0)
			{
				disposable.Dispose();
			}
		}
		oevhn.imcen.hksnh();
		MemoryStream memoryStream = new MemoryStream();
		try
		{
			fxakl fxakl = new fxakl(memoryStream);
			fxakl.afwyb();
			fxakl.kfyej(oevhn);
			fxakl.xljze();
			fxakl.imfsc();
			byte[] array = memoryStream.ToArray();
			byte[] data = signer.noegy(array);
			knass = new nnzwd(array);
			yxgxq = new htykq(data, 0);
		}
		finally
		{
			if (memoryStream != null && 0 == 0)
			{
				((IDisposable)memoryStream).Dispose();
			}
		}
	}

	internal CertificateRevocationList()
	{
	}

	public CertificateRevocationList(byte[] crl)
	{
		if (crl == null || 1 == 0)
		{
			throw new ArgumentNullException("crl");
		}
		hfnnn hfnnn = new hfnnn(this);
		hfnnn.vgmyo = true;
		hfnnn.Write(crl, 0, crl.Length);
		hfnnn.Close();
	}

	public static CertificateRevocationList Load(Stream input)
	{
		if (input == null || 1 == 0)
		{
			throw new ArgumentNullException("input");
		}
		int num = input.ReadByte();
		if (num != 48)
		{
			if (num < 0)
			{
				throw new CryptographicException("CRL is empty.");
			}
			MemoryStream memoryStream = new MemoryStream();
			try
			{
				memoryStream.WriteByte((byte)num);
				input.alskc(memoryStream);
				input.Close();
				byte[] buffer;
				string p;
				try
				{
					buffer = kjhmn.qzqgg(memoryStream.GetBuffer(), (int)memoryStream.Length, out p);
				}
				catch (FormatException ex)
				{
					throw new CryptographicException(ex.Message, ex);
				}
				if (p != "X509 CRL" && 0 == 0)
				{
					throw new CryptographicException("Unexpected file type '" + p + "'.");
				}
				input = new MemoryStream(buffer, writable: false);
				num = input.ReadByte();
			}
			finally
			{
				if (memoryStream != null && 0 == 0)
				{
					((IDisposable)memoryStream).Dispose();
				}
			}
			if (num != 48)
			{
				throw new CryptographicException("Unsupported CRL format.");
			}
		}
		CertificateRevocationList certificateRevocationList = new CertificateRevocationList();
		hfnnn hfnnn = new hfnnn(certificateRevocationList);
		try
		{
			hfnnn.vgmyo = true;
			hfnnn.WriteByte((byte)num);
			input.alskc(hfnnn);
			input.Close();
			return certificateRevocationList;
		}
		finally
		{
			if (hfnnn != null && 0 == 0)
			{
				((IDisposable)hfnnn).Dispose();
			}
		}
	}

	public byte[] ToArray()
	{
		return fxakl.kncuz(this);
	}

	public byte[] GetHash()
	{
		HashingAlgorithmId algorithm = rgzpc.vvmoi(p0: true);
		return HashingAlgorithm.ComputeHash(algorithm, knass.lktyp);
	}

	public byte[] GetSignature()
	{
		return (byte[])yxgxq.lssxa.Clone();
	}

	internal bool qbgic(Certificate p0)
	{
		SignatureParameters signatureParameters = new SignatureParameters();
		signatureParameters.HashAlgorithm = rgzpc.vvmoi(p0: true);
		signatureParameters.Format = SignatureFormat.Pkcs;
		return p0.VerifyMessage(knass.lktyp, yxgxq.lssxa, signatureParameters);
	}

	internal dyaoh kctag()
	{
		CertificateExtension certificateExtension = Extensions["2.5.29.28"];
		if (certificateExtension == null || 1 == 0)
		{
			return null;
		}
		dyaoh dyaoh = new dyaoh();
		hfnnn.qnzgo(dyaoh, certificateExtension.Value);
		return dyaoh;
	}

	public string GetDistributionPointUrl()
	{
		dyaoh dyaoh = kctag();
		if (dyaoh != null && 0 == 0 && dyaoh.bqemz != null && 0 == 0)
		{
			return dyaoh.bqemz.zlwug;
		}
		return null;
	}

	internal byte[] itfmd()
	{
		CertificateExtension certificateExtension = Extensions["2.5.29.35"];
		if (certificateExtension == null || 1 == 0)
		{
			return null;
		}
		ohkxj ohkxj = new ohkxj();
		hfnnn.qnzgo(ohkxj, certificateExtension.Value);
		return ohkxj.gvwnt;
	}

	internal CertificateRevocationListStatus vlpfp(Certificate p0, DateTime? p1, bool p2, sjhqe p3)
	{
		if (p1.HasValue && 0 == 0 && (p1.Value.Kind == DateTimeKind.Unspecified || 1 == 0))
		{
			throw new ArgumentException("A DateTimeKind has to be specified.", "validationTime");
		}
		CertificateRevocationListStatus certificateRevocationListStatus = CertificateRevocationListStatus.Valid;
		if (p1.HasValue && 0 == 0)
		{
			DateTime dateTime = dahxy.qsrcs(p1.Value);
			if (p2 && 0 == 0 && dateTime < dahxy.qsrcs(ThisUpdate) && 0 == 0)
			{
				certificateRevocationListStatus |= CertificateRevocationListStatus.TimeNotValid;
			}
			if (dahxy.qsrcs(NextUpdate) < dateTime && 0 == 0)
			{
				certificateRevocationListStatus |= CertificateRevocationListStatus.TimeNotValid;
			}
		}
		return certificateRevocationListStatus | kaain(p0, p3);
	}

	internal CertificateRevocationListStatus kaain(Certificate p0, sjhqe p1)
	{
		CertificateRevocationListStatus certificateRevocationListStatus = CertificateRevocationListStatus.Valid;
		byte[] array = itfmd();
		byte[] subjectKeyIdentifier = p0.GetSubjectKeyIdentifier();
		if (array != null && 0 == 0)
		{
			if (!zjcch.wduyr(array, subjectKeyIdentifier) || 1 == 0)
			{
				certificateRevocationListStatus |= CertificateRevocationListStatus.IssuerMismatch;
			}
		}
		else if (Issuer != null && 0 == 0 && (!Issuer.Equals(p0.GetSubject()) || 1 == 0))
		{
			certificateRevocationListStatus |= CertificateRevocationListStatus.IssuerMismatch;
		}
		IEnumerator<CertificateExtension> enumerator = Extensions.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				CertificateExtension current = enumerator.Current;
				string oid;
				if (current.Critical && 0 == 0 && ((oid = current.Oid) == null || false || !(oid == "2.5.29.28")))
				{
					certificateRevocationListStatus |= CertificateRevocationListStatus.UnknownCriticalExtension;
				}
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		if (((dahxy.qsrcs(ThisUpdate) < dahxy.qsrcs(p0.GetEffectiveDate())) ? true : false) || dahxy.qsrcs(ThisUpdate) > dahxy.qsrcs(p0.GetExpirationDate()))
		{
			certificateRevocationListStatus |= CertificateRevocationListStatus.WrongCrlUpdateTime;
		}
		if ((p0.GetIntendedUsage() & KeyUses.CrlSign) == 0 || 1 == 0)
		{
			certificateRevocationListStatus |= CertificateRevocationListStatus.WrongIssuerUsage;
		}
		if (p1 != null && 0 == 0)
		{
			p1.rfpvf(LogLevel.Debug, "CRL", "Verifying CRL signature.");
		}
		if (qbgic(p0) && 0 == 0)
		{
			if (p1 != null && 0 == 0)
			{
				p1.rfpvf(LogLevel.Debug, "CRL", "CRL signature is valid.");
			}
		}
		else
		{
			certificateRevocationListStatus |= CertificateRevocationListStatus.SignatureNotValid;
			if (p1 != null && 0 == 0)
			{
				p1.rfpvf(LogLevel.Error, "CRL", "CRL signature is not valid.");
			}
		}
		dyaoh dyaoh = kctag();
		if (dyaoh != null && 0 == 0)
		{
			int num = 0;
			if (dyaoh.zrcpk && 0 == 0)
			{
				num++;
			}
			if (dyaoh.gokvn && 0 == 0)
			{
				num++;
			}
			if (dyaoh.lnobp && 0 == 0)
			{
				num++;
			}
			if (num > 1)
			{
				certificateRevocationListStatus |= CertificateRevocationListStatus.Malformed;
			}
			if (dyaoh.lnobp && 0 == 0)
			{
				certificateRevocationListStatus |= CertificateRevocationListStatus.Malformed;
			}
		}
		return certificateRevocationListStatus;
	}

	internal bool miwza(Certificate p0)
	{
		dyaoh dyaoh = kctag();
		if (dyaoh != null && 0 == 0)
		{
			xxerq xxerq = p0.ortjx();
			if (dyaoh.zrcpk && 0 == 0 && xxerq.xhaoc && 0 == 0)
			{
				return false;
			}
			if (dyaoh.gokvn && 0 == 0 && (!xxerq.xhaoc || 1 == 0))
			{
				return false;
			}
		}
		return true;
	}

	internal RevokedCertificate twpgy(Certificate p0)
	{
		IEnumerator<RevokedCertificate> enumerator = RevokedCertificates.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				RevokedCertificate current = enumerator.Current;
				if (zjcch.wduyr(current.GetSerialNumber(), p0.GetSerialNumber()) && 0 == 0)
				{
					return current;
				}
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		return null;
	}

	public RevocationCheckResult CheckCertificate(Certificate subject)
	{
		if (subject == null || 1 == 0)
		{
			throw new ArgumentNullException("subject");
		}
		if (Issuer != null && 0 == 0 && (!Issuer.Equals(subject.GetIssuer()) || 1 == 0))
		{
			return new RevocationCheckResult(RevocationCheckStatus.IssuerMismatch);
		}
		kctag();
		if (!miwza(subject) || 1 == 0)
		{
			return new RevocationCheckResult(RevocationCheckStatus.NotSuitable);
		}
		RevokedCertificate revokedCertificate = twpgy(subject);
		if (revokedCertificate == null || 1 == 0)
		{
			return new RevocationCheckResult(RevocationCheckStatus.NotRevoked);
		}
		return new RevocationCheckResult(revokedCertificate);
	}

	private void nlvkn(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in nlvkn
		this.nlvkn(p0, p1, p2);
	}

	private lnabj aykkr(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			knass = new nnzwd();
			return knass;
		case 1:
			rgzpc = new AlgorithmIdentifier();
			return rgzpc;
		case 2:
			yxgxq = new htykq();
			return yxgxq;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in aykkr
		return this.aykkr(p0, p1, p2);
	}

	private void qodqr(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in qodqr
		this.qodqr(p0, p1, p2);
	}

	private void clrlo()
	{
		if (knass == null || 1 == 0)
		{
			throw new CryptographicException("TBSCertList not found in certificate.");
		}
		if (rgzpc == null || 1 == 0)
		{
			throw new CryptographicException("Signature algorithm not found in certificate.");
		}
		if (yxgxq == null || 1 == 0)
		{
			throw new CryptographicException("Signature not found in certificate.");
		}
		oevhn = new kmtni();
		hfnnn.qnzgo(oevhn, knass.lktyp);
		oevhn.abicy.hksnh();
		oevhn.imcen.hksnh();
		byte[] p = fxakl.kncuz(rgzpc);
		byte[] p2 = fxakl.kncuz(oevhn.zszue);
		if (!zjcch.wduyr(p, p2) || 1 == 0)
		{
			throw new CryptographicException("Signature algorithms do not match.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in clrlo
		this.clrlo();
	}

	private void owcjo(fxakl p0)
	{
		p0.suudj(knass, rgzpc, yxgxq);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in owcjo
		this.owcjo(p0);
	}

	internal void ukgmy()
	{
		oevhn.abicy.hksnh();
		oevhn.imcen.hksnh();
	}
}
