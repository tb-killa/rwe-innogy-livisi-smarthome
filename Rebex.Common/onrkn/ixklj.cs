using System;
using System.Collections.Generic;
using Rebex;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class ixklj
{
	private class yxesv
	{
		private DateTime nqtgx;

		private DateTime cfeps;

		public yxesv(DateTime start, DateTime end)
		{
			if (start > end && 0 == 0)
			{
				throw new ArgumentException("Interval dates are reversed. Interval starts after end.");
			}
			nqtgx = start;
			cfeps = end;
		}

		public bool sktba(yxesv p0)
		{
			if (((nqtgx < p0.nqtgx) ? true : false) || ((cfeps > p0.cfeps) ? true : false) || ((nqtgx > p0.cfeps) ? true : false) || cfeps < p0.nqtgx)
			{
				return false;
			}
			return true;
		}

		public bool lixvv(DateTime p0)
		{
			if (nqtgx <= p0 && 0 == 0 && p0 <= cfeps && 0 == 0)
			{
				return true;
			}
			return false;
		}
	}

	private enum lbxan
	{
		pqcxw,
		tothp,
		spaqn
	}

	private sealed class ualfs
	{
		public string aumxd;

		public Certificate cymoz;

		public string bxqas()
		{
			return brgjd.edcru("{0}\r\n  SUBJECT: {1}\r\n  SUBJ-KEY-ID: {2}\r\n  ISSUER: {3}\r\n  AUTH-KEY-ID: {4}\r\n  SN: {5}\r\n  THUMBPRINT: {6}\r\n  EFFECTIVE: {7}\r\n  EXPIRATION: {8}\r\n  KEY-ALG: {9} {10}\r\n  SIGNATURE-HASH: {11}", aumxd, cymoz.GetSubject(), ryqxx(cymoz.GetSubjectKeyIdentifier()), cymoz.GetIssuer(), ryqxx(cymoz.GetAuthorityKeyIdentifier()), ryqxx(cymoz.GetSerialNumber()), cymoz.Thumbprint, cymoz.GetEffectiveDate(), cymoz.GetExpirationDate(), cymoz.KeyAlgorithm, cymoz.GetKeySize(), cymoz.GetSignatureHashAlgorithm());
		}
	}

	private sealed class lupvg
	{
		public string iazzy;

		public CertificateRevocationList froxg;

		public string pktcx;

		public string grqdq()
		{
			return brgjd.edcru("{0}\r\n  URL: {1}\r\n  ISSUER: {2}\r\n  AUTH-KEY-ID: {3}\r\n  THIS-UPDATE: {4}\r\n  NEXT-UPDATE: {5}", iazzy, pktcx, froxg.Issuer, ryqxx(froxg.itfmd()), froxg.ThisUpdate, froxg.NextUpdate);
		}
	}

	private const int sfkqf = 1024;

	private const string swjei = "2.5.29.15";

	private const string qwxuu = "2.5.29.35";

	private const string mywet = "2.5.29.14";

	private const string zckwc = "2.5.29.19";

	private const string ucuzp = "2.5.29.17";

	private const string fdpal = "2.5.29.18";

	private const string xnkxh = "2.5.29.30";

	private const string xdixr = "2.5.29.36";

	private const string eyqwy = "2.5.29.32";

	private const string gdqdj = "2.5.29.33";

	private const string htxbb = "2.5.29.28";

	private const string hktnd = "2.5.29.21";

	private readonly mnzit bafuf;

	private readonly zlbxa mqmyf;

	private readonly CertificateChain bzhcm;

	private readonly Certificate oboxd;

	private readonly CertificateValidationParameters avduz;

	private readonly ValidationOptions cdvat;

	private readonly ewirv pbggn;

	private readonly bool mzjjb;

	private readonly bool uspcs;

	private readonly CertificateStore glfhz;

	private readonly awngk zhejd;

	private readonly DateTime vvvmu;

	private int wwipt;

	private bool conml;

	private bool snqzm;

	private ixklj(CertificateEngine engine, zlbxa ocspCache, mnzit crlCache, CertificateChain chain, CertificateValidationParameters parameters, CertificateStore additionalTrustedRootAuthorities)
	{
		if (chain == null || 1 == 0)
		{
			throw new ArgumentNullException("chain");
		}
		if (chain.Count == 0 || 1 == 0)
		{
			throw new ArgumentException("Provided chain contains no certificate.", "chain");
		}
		EnhancedCertificateEngine enhancedCertificateEngine = engine as EnhancedCertificateEngine;
		zlbxa obj = ocspCache;
		if (obj == null || 1 == 0)
		{
			obj = zlbxa.nxjdn;
		}
		mqmyf = obj;
		mnzit obj2 = crlCache;
		if (obj2 == null || 1 == 0)
		{
			obj2 = mnzit.zgnxs;
		}
		bafuf = obj2;
		bzhcm = chain;
		oboxd = chain.RootCertificate;
		CertificateValidationParameters certificateValidationParameters = parameters;
		if (certificateValidationParameters == null || 1 == 0)
		{
			certificateValidationParameters = new CertificateValidationParameters();
		}
		avduz = certificateValidationParameters;
		if (enhancedCertificateEngine != null && 0 == 0)
		{
			pbggn = enhancedCertificateEngine.cwesv;
			mzjjb = (enhancedCertificateEngine.RevocationCheckModes & RevocationCheckModes.Ocsp) != 0;
			uspcs = (enhancedCertificateEngine.RevocationCheckModes & RevocationCheckModes.Crl) != 0;
		}
		else
		{
			pbggn = ewirv.liolk;
			mzjjb = true;
			uspcs = true;
		}
		cdvat = avduz.Options;
		glfhz = additionalTrustedRootAuthorities;
		cdvat &= ~ValidationOptions.AllowUnknownCa;
		cdvat &= ~ValidationOptions.IgnoreInvalidBasicConstraints;
		zhejd = engine.awsqi;
		vvvmu = DateTime.UtcNow;
	}

	public static ValidationResult wcbka(CertificateEngine p0, zlbxa p1, mnzit p2, CertificateChain p3, CertificateValidationParameters p4, CertificateStore p5)
	{
		return new ixklj(p0, p1, p2, p3, p4, p5).gjkap();
	}

	private ValidationStatus ozdde(ValidationStatus p0)
	{
		conml = false;
		return p0;
	}

	private void jazzx(LogLevel p0, string p1, params object[] p2)
	{
		zhejd.byfnx(p0, "CERT", p1, p2);
	}

	private void rqbgf(LogLevel p0, string p1, Certificate p2)
	{
		ualfs ualfs = new ualfs();
		ualfs.aumxd = p1;
		ualfs.cymoz = p2;
		zhejd.gstqy(p0, "CERT", ualfs.bxqas);
	}

	private void kpcsr(LogLevel p0, string p1, CertificateRevocationList p2, string p3)
	{
		lupvg lupvg = new lupvg();
		lupvg.iazzy = p1;
		lupvg.froxg = p2;
		lupvg.pktcx = p3;
		zhejd.gstqy(p0, "CERT", lupvg.grqdq);
	}

	private static string ryqxx(byte[] p0)
	{
		if (p0 != null && 0 == 0 && p0.Length != 0 && 0 == 0)
		{
			return BitConverter.ToString(p0);
		}
		return "<empty>";
	}

	private ValidationResult gjkap()
	{
		conml = true;
		ValidationStatus validationStatus = (ValidationStatus)0L;
		yxesv p = null;
		bool flag = oboxd != null;
		wwipt = ((flag ? true : false) ? bzhcm.Count : (bzhcm.Count + 1));
		if (oboxd != null && 0 == 0)
		{
			jazzx(LogLevel.Debug, "Checking Root trust '{0}'.", oboxd.GetSubject());
			validationStatus |= kdtwt();
			jazzx(LogLevel.Debug, "Verifying Root '{0}'.", oboxd.GetSubject());
			rqbgf(LogLevel.Verbose, "Verifying Root:", oboxd);
			validationStatus |= gyhdj(oboxd, out p);
			validationStatus |= kopyt(oboxd, oboxd, p, p, lbxan.spaqn, validationStatus);
		}
		int num = ((flag ? true : false) ? (bzhcm.Count - 2) : (bzhcm.Count - 1));
		Certificate p2 = oboxd;
		yxesv p3 = p;
		int num2 = num;
		while (num2 >= 0 && (conml ? true : false))
		{
			Certificate certificate = bzhcm[num2];
			jazzx(LogLevel.Debug, "Verifying certificate '{0}'.", certificate.GetSubject());
			rqbgf(LogLevel.Verbose, "Verifying certificate:", certificate);
			validationStatus |= gyhdj(certificate, out var p4);
			validationStatus |= kopyt(certificate, p2, p4, p3, (num2 != 0 && 0 == 0) ? lbxan.tothp : lbxan.pqcxw, validationStatus);
			jazzx(LogLevel.Debug, "Verification result is '{1}' (CRL not validated yet), certificate: '{0}'.", certificate.GetSubject(), validationStatus);
			p2 = certificate;
			p3 = p4;
			num2--;
		}
		if ((cdvat & ValidationOptions.SkipRevocationCheck) == 0 || 1 == 0)
		{
			ValidationStatus validationStatus2 = ~(ValidationStatus.TimeNotValid | ValidationStatus.IncompleteChain | ValidationStatus.WeakAlgorithm);
			if ((validationStatus & validationStatus2) != 0)
			{
				jazzx(LogLevel.Debug, "Skipping revocation check because the certificate chain is not valid.");
				validationStatus |= ValidationStatus.UnknownRev | ValidationStatus.OfflineRev;
			}
			else if (oboxd == null || 1 == 0)
			{
				jazzx(LogLevel.Debug, "Unable to check revocation status due to missing root certificate.");
				validationStatus |= ValidationStatus.UnknownRev | ValidationStatus.OfflineRev;
			}
			else
			{
				validationStatus |= cqtxk();
			}
		}
		return new ValidationResult(validationStatus == (ValidationStatus)0L, (long)validationStatus, 0);
	}

	private ValidationStatus cqtxk()
	{
		jazzx(LogLevel.Debug, "Verifying revocation status.");
		ValidationStatus validationStatus = (ValidationStatus)0L;
		int num = bzhcm.Count - 1;
		Certificate p = oboxd;
		switch (pbggn)
		{
		case ewirv.nnctr:
			validationStatus |= fiiew(oboxd, oboxd, lbxan.spaqn);
			if (bzhcm[num] == oboxd)
			{
				num--;
			}
			break;
		case ewirv.liolk:
			if (bzhcm[num] == oboxd)
			{
				num--;
			}
			break;
		case ewirv.auxqh:
			{
				if (bzhcm.Count == 0 || false || bzhcm[0] == oboxd)
				{
					num = -1;
					if (num != 0)
					{
						goto IL_00cb;
					}
				}
				num = 0;
				goto IL_00cb;
			}
			IL_00cb:
			if (bzhcm.Count > 1)
			{
				p = bzhcm[1];
			}
			break;
		}
		int num2 = num;
		while (num2 >= 0 && (conml ? true : false))
		{
			Certificate certificate = bzhcm[num2];
			validationStatus |= fiiew(certificate, p, (num2 != 0 && 0 == 0) ? lbxan.tothp : lbxan.pqcxw);
			if ((!conml || 1 == 0) && num2 > 0)
			{
				validationStatus |= ValidationStatus.UnknownRev | ValidationStatus.OfflineRev;
			}
			p = certificate;
			num2--;
		}
		return validationStatus;
	}

	private ValidationStatus ejtob(ValidationStatus p0, lbxan p1)
	{
		if (p0 == (ValidationStatus)0L)
		{
			return (ValidationStatus)0L;
		}
		if ((p0 & ValidationStatus.Revoked) != 0)
		{
			return ozdde(p0);
		}
		int num;
		switch (p1)
		{
		case lbxan.pqcxw:
			num = 256;
			if (num != 0)
			{
				break;
			}
			goto case lbxan.tothp;
		case lbxan.tothp:
			num = 1024;
			if (num != 0)
			{
				break;
			}
			goto case lbxan.spaqn;
		case lbxan.spaqn:
			num = 2048;
			if (num != 0)
			{
				break;
			}
			goto default;
		default:
			throw new ArgumentOutOfRangeException("placement", "Unknown placement.");
		}
		if (((uint)cdvat & (uint)num) == 0 || 1 == 0)
		{
			return ozdde(p0);
		}
		return (ValidationStatus)0L;
	}

	private ValidationStatus gyhdj(Certificate p0, out yxesv p1)
	{
		p1 = null;
		if (p0 == null || 1 == 0)
		{
			return (ValidationStatus)0L;
		}
		DateTime dateTime = dahxy.qsrcs(p0.GetEffectiveDate());
		DateTime dateTime2 = dahxy.qsrcs(p0.GetExpirationDate());
		if (dateTime > dateTime2 && 0 == 0)
		{
			if ((cdvat & ValidationOptions.IgnoreTimeNotValid) == 0 || 1 == 0)
			{
				return ValidationStatus.TimeNotValid;
			}
		}
		else
		{
			p1 = new yxesv(dateTime, dateTime2);
		}
		return (ValidationStatus)0L;
	}

	private ValidationStatus kdtwt()
	{
		byte[] rawCertData = oboxd.GetRawCertData();
		CertificateStore certificateStore = new CertificateStore(CertificateStoreName.Root);
		Certificate[] array = certificateStore.FindCertificates(oboxd.GetIssuer(), CertificateFindOptions.None);
		int num = 0;
		if (num != 0)
		{
			goto IL_0030;
		}
		goto IL_0069;
		IL_0030:
		if (zjcch.wduyr(array[num].GetRawCertData(), rawCertData) && 0 == 0)
		{
			jazzx(LogLevel.Debug, "Root certificate found in system certificate store.");
			snqzm = true;
			return (ValidationStatus)0L;
		}
		num++;
		goto IL_0069;
		IL_00db:
		int num2;
		if (num2 < array.Length)
		{
			goto IL_009f;
		}
		goto IL_00e2;
		IL_00e2:
		jazzx(LogLevel.Debug, "Root certificate not found in certificate store.");
		if ((cdvat & ValidationOptions.AllowUnknownCa) == 0 || 1 == 0)
		{
			return ozdde(ValidationStatus.RootNotTrusted);
		}
		return (ValidationStatus)0L;
		IL_0069:
		if (num < array.Length)
		{
			goto IL_0030;
		}
		if (glfhz != null && 0 == 0)
		{
			array = glfhz.FindCertificates(oboxd.GetIssuer(), CertificateFindOptions.None);
			num2 = 0;
			if (num2 != 0)
			{
				goto IL_009f;
			}
			goto IL_00db;
		}
		goto IL_00e2;
		IL_009f:
		if (zjcch.wduyr(array[num2].GetRawCertData(), rawCertData) && 0 == 0)
		{
			jazzx(LogLevel.Debug, "Root certificate found in extra certificate store.");
			snqzm = true;
			return (ValidationStatus)0L;
		}
		num2++;
		goto IL_00db;
	}

	private ValidationStatus yrhni(Certificate p0, lbxan p1)
	{
		byte[] rawCertData = p0.GetRawCertData();
		CertificateStore certificateStore = new CertificateStore(CertificateStoreName.Disallowed);
		Certificate[] array = certificateStore.FindCertificates(p0.GetIssuer(), CertificateFindOptions.None);
		int num = 0;
		if (num != 0)
		{
			goto IL_0026;
		}
		goto IL_0066;
		IL_0026:
		if (zjcch.wduyr(array[num].GetRawCertData(), rawCertData) && 0 == 0)
		{
			if (p1 == lbxan.spaqn)
			{
				return ozdde(ValidationStatus.RootNotTrusted | ValidationStatus.ExplicitDistrust | ValidationStatus.NotTrusted);
			}
			return ozdde(ValidationStatus.ExplicitDistrust | ValidationStatus.NotTrusted);
		}
		num++;
		goto IL_0066;
		IL_0066:
		if (num < array.Length)
		{
			goto IL_0026;
		}
		return (ValidationStatus)0L;
	}

	private ValidationStatus kopyt(Certificate p0, Certificate p1, yxesv p2, yxesv p3, lbxan p4, ValidationStatus p5)
	{
		nisgb p6 = new nisgb(p0.GetRawCertData());
		p5 |= fntlu(p0, p1);
		p5 |= yrhni(p0, p4);
		p5 |= tjpfg(p0);
		p5 |= xoqye(p2, p3);
		p5 |= mzopt(p0, p6, p4);
		p5 |= pqrch(p0, p6, p4);
		p5 |= oghkq(p0, p6, p4);
		p5 |= ujwlj(p0, p6, p1, p4, p5);
		return p5;
	}

	private ValidationStatus tjpfg(Certificate p0)
	{
		ValidationStatus result = (ValidationStatus)0L;
		IEnumerator<CertificateExtension> enumerator = p0.Extensions.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				CertificateExtension current = enumerator.Current;
				if (!current.Critical)
				{
					continue;
				}
				string oid;
				if ((oid = current.Oid) != null && 0 == 0)
				{
					if (fnfqw.ausgb == null || 1 == 0)
					{
						fnfqw.ausgb = new Dictionary<string, int>(10)
						{
							{ "2.5.29.35", 0 },
							{ "2.5.29.14", 1 },
							{ "2.5.29.15", 2 },
							{ "2.5.29.19", 3 },
							{ "2.5.29.17", 4 },
							{ "2.5.29.18", 5 },
							{ "2.5.29.30", 6 },
							{ "2.5.29.36", 7 },
							{ "2.5.29.32", 8 },
							{ "2.5.29.33", 9 }
						};
					}
					if (fnfqw.ausgb.TryGetValue(oid, out var value) && 0 == 0)
					{
						switch (value)
						{
						case 0:
						case 1:
						case 2:
						case 3:
						case 4:
						case 5:
							continue;
						}
					}
				}
				return ozdde(ValidationStatus.UnknownCriticalExtension);
			}
			return result;
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
	}

	private ValidationStatus fiiew(Certificate p0, Certificate p1, lbxan p2)
	{
		if ((cdvat & ValidationOptions.SkipRevocationCheck) != ValidationOptions.None && 0 == 0)
		{
			return (ValidationStatus)0L;
		}
		jazzx(LogLevel.Debug, "Verifying revocation status of '{0}'.", p0.GetSubject());
		vcnjn vcnjn2 = ((mzjjb ? true : false) ? p0.vgnxi() : null);
		if (vcnjn2 == null || 1 == 0)
		{
			if (mzjjb && 0 == 0)
			{
				jazzx(LogLevel.Debug, "No OCSP specified for '{0}'.", p0.GetSubject());
			}
			else
			{
				jazzx(LogLevel.Debug, "OCSP disabled for '{0}'.", p0.GetSubject());
			}
		}
		else
		{
			ValidationStatus? validationStatus = tayya(vcnjn2, p0, p1, p2);
			if (validationStatus.HasValue && 0 == 0)
			{
				return validationStatus.Value;
			}
		}
		CrlDistributionPointCollection crlDistributionPointCollection = ((uspcs ? true : false) ? p0.GetCrlDistributionPoints() : null);
		if (crlDistributionPointCollection == null || 1 == 0)
		{
			if (uspcs && 0 == 0)
			{
				jazzx(LogLevel.Debug, "No CRL specified for '{0}'.", p0.GetSubject());
			}
			else
			{
				jazzx(LogLevel.Debug, "CRL disabled for '{0}'.", p0.GetSubject());
			}
			if (p2 == lbxan.spaqn)
			{
				return (ValidationStatus)0L;
			}
			if (vcnjn2 != null && 0 == 0)
			{
				return ejtob(ValidationStatus.UnknownRev | ValidationStatus.OfflineRev, p2);
			}
			return ejtob(ValidationStatus.UnknownRev, p2);
		}
		return nduju(crlDistributionPointCollection, p0, p1, p2);
	}

	private ValidationStatus? tayya(vcnjn p0, Certificate p1, Certificate p2, lbxan p3)
	{
		bool p4 = (cdvat & ValidationOptions.UseCacheOnly) != 0;
		IEnumerator<xsxeo> enumerator = p0.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				xsxeo current = enumerator.Current;
				if (current.uxfmu != rvoup.nyjao || current.mpfci == null || false || !current.mpfci.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}
				byte[] serialNumber = p1.GetSerialNumber();
				byte[] randomBytes = CryptoHelper.GetRandomBytes(16);
				tmygz p5 = kuoty.jnvtx(serialNumber, p2, HashingAlgorithmId.SHA1, randomBytes);
				bool p6;
				rxwdr rxwdr2 = kuoty.mddqi(mqmyf, current.mpfci, p5, p4, zhejd, out p6);
				if (rxwdr2 == null || false || ((rxwdr2.qgdlw != ffncd.pqpgx) ? true : false) || rxwdr2.wglak == null || false || rxwdr2.wglak.kndop == null)
				{
					continue;
				}
				if ((!p6 || 1 == 0) && (!rxwdr2.twnrh(randomBytes) || 1 == 0))
				{
					jazzx(LogLevel.Debug, "OCSP response has invalid Nonce extension (OCSP '{1}' of '{0}').", p1.GetSubject(), current.mpfci);
					continue;
				}
				eozod eozod2 = rxwdr2.lywik(serialNumber);
				if (eozod2 == null)
				{
					jazzx(LogLevel.Debug, "OCSP response does not contain data for requested certificate (OCSP '{1}' of '{0}').", p1.GetSubject(), current.mpfci);
					continue;
				}
				jazzx(LogLevel.Debug, "Verifying OCSP response '{1}' of '{0}'.", p1.GetSubject(), current.mpfci);
				ValidationStatus validationStatus = kuoty.vktjg(rxwdr2, p2, zhejd);
				jazzx(LogLevel.Debug, "OCSP response verification result is '{2}' (OCSP '{1}' of '{0}').", p1.GetSubject(), current.mpfci, validationStatus);
				switch (pjsfv(eozod2))
				{
				case ezpxd.zyamk:
					jazzx(LogLevel.Debug, "Certificate is not revoked by OCSP '{1}' of '{0}'.", p1.GetSubject(), current.mpfci);
					break;
				case ezpxd.dugxd:
					validationStatus |= ozdde(ValidationStatus.Revoked);
					jazzx(LogLevel.Debug, "Certificate is revoked by OCSP '{1}' of '{0}'.", p1.GetSubject(), current.mpfci);
					break;
				case ezpxd.pjjlw:
					jazzx(LogLevel.Debug, "Certificate status is unknown (OCSP '{1}' of '{0}').", p1.GetSubject(), current.mpfci);
					continue;
				default:
					jazzx(LogLevel.Debug, "Certificate status is undefined (OCSP '{1}' of '{0}').", p1.GetSubject(), current.mpfci);
					continue;
				}
				return ejtob(validationStatus, p3);
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

	private ValidationStatus nduju(CrlDistributionPointCollection p0, Certificate p1, Certificate p2, lbxan p3)
	{
		bool flag = false;
		bool p4 = (cdvat & ValidationOptions.UseCacheOnly) != 0;
		IEnumerator<CrlDistributionPoint> enumerator = p0.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				CrlDistributionPoint current = enumerator.Current;
				string text = rvryq(current);
				if (text == null || 1 == 0)
				{
					if (p3 != lbxan.spaqn || current.wbnox == null)
					{
						continue;
					}
					flag = true;
					if (flag)
					{
						continue;
					}
				}
				flag = true;
				CertificateRevocationList certificateRevocationList = pltec.qujyk(bafuf, text, p4, zhejd);
				ValidationStatus validationStatus;
				if (certificateRevocationList == null || 1 == 0)
				{
					validationStatus = ejtob(ValidationStatus.UnknownRev | ValidationStatus.OfflineRev, p3);
					if (validationStatus != (ValidationStatus)0L)
					{
						return validationStatus;
					}
					continue;
				}
				dyaoh dyaoh2 = certificateRevocationList.kctag();
				if (dyaoh2 != null && 0 == 0 && dyaoh2.bqemz != null && 0 == 0 && (!rtsxt(dyaoh2.bqemz, text) || 1 == 0))
				{
					validationStatus = ejtob(ValidationStatus.UnknownRev | ValidationStatus.Malformed, p3);
					if (validationStatus != (ValidationStatus)0L)
					{
						return validationStatus;
					}
				}
				jazzx(LogLevel.Debug, "Verifying CRL '{1}' of '{0}'.", p1.GetSubject(), text);
				kpcsr(LogLevel.Verbose, "Verifying CRL:", certificateRevocationList, text);
				validationStatus = ljjan(p1, p2, certificateRevocationList);
				jazzx(LogLevel.Debug, "CRL verification result is '{2}' (CRL '{1}' of '{0}').", p1.GetSubject(), text, validationStatus);
				jazzx(LogLevel.Debug, "Searching revoked certificates in CRL '{1}' of '{0}'.", p1.GetSubject(), text);
				if (abxyk(p1, certificateRevocationList) && 0 == 0)
				{
					validationStatus |= ozdde(ValidationStatus.Revoked);
					jazzx(LogLevel.Debug, "Certificate is revoked by CRL '{1}' of '{0}'.", p1.GetSubject(), text);
				}
				else
				{
					jazzx(LogLevel.Debug, "Certificate is not revoked by CRL '{1}' of '{0}'.", p1.GetSubject(), text);
				}
				validationStatus = ejtob(validationStatus, p3);
				if (validationStatus != (ValidationStatus)0L)
				{
					return validationStatus;
				}
				if (dyaoh2 == null || false || dyaoh2.cfkdr == (hqjml)0 || 1 == 0)
				{
					return validationStatus;
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
		if (flag && 0 == 0)
		{
			return (ValidationStatus)0L;
		}
		return ValidationStatus.UnknownRev;
	}

	private string rvryq(CrlDistributionPoint p0)
	{
		string[] array = p0.ujxot();
		if (array == null || 1 == 0)
		{
			return null;
		}
		string[] array2 = array;
		int num = 0;
		if (num != 0)
		{
			goto IL_001e;
		}
		goto IL_0043;
		IL_001e:
		string text = array2[num];
		if (text.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && 0 == 0)
		{
			return text;
		}
		num++;
		goto IL_0043;
		IL_0043:
		if (num < array2.Length)
		{
			goto IL_001e;
		}
		return null;
	}

	private bool rtsxt(zaraq p0, string p1)
	{
		string[] array = p0.yxays();
		if (array == null || 1 == 0)
		{
			return false;
		}
		string[] array2 = array;
		int num = 0;
		if (num != 0)
		{
			goto IL_001e;
		}
		goto IL_003e;
		IL_001e:
		string value = array2[num];
		if (p1.Equals(value) && 0 == 0)
		{
			return true;
		}
		num++;
		goto IL_003e;
		IL_003e:
		if (num < array2.Length)
		{
			goto IL_001e;
		}
		return false;
	}

	private ValidationStatus ljjan(Certificate p0, Certificate p1, CertificateRevocationList p2)
	{
		ValidationStatus validationStatus = (ValidationStatus)0L;
		CertificateRevocationListStatus certificateRevocationListStatus = p2.vlpfp(p1, vvvmu, p2: false, zhejd);
		if (certificateRevocationListStatus != CertificateRevocationListStatus.Valid)
		{
			if ((certificateRevocationListStatus & CertificateRevocationListStatus.WrongIssuerUsage) != 0 && ((cdvat & ValidationOptions.IgnoreWrongUsage) == 0 || 1 == 0))
			{
				validationStatus |= ValidationStatus.WrongUsage;
			}
			if ((certificateRevocationListStatus & CertificateRevocationListStatus.TimeNotValid) != 0 && ((cdvat & ValidationOptions.IgnoreTimeNotValid) == 0 || 1 == 0))
			{
				validationStatus |= ValidationStatus.OfflineRev;
			}
			if ((certificateRevocationListStatus & CertificateRevocationListStatus.UnknownCriticalExtension) != 0)
			{
				validationStatus |= ValidationStatus.UnknownCriticalExtension;
			}
			if ((certificateRevocationListStatus & CertificateRevocationListStatus.IssuerMismatch) != 0)
			{
				validationStatus |= ValidationStatus.SignatureNotValid;
			}
			if ((certificateRevocationListStatus & CertificateRevocationListStatus.SignatureNotValid) != 0)
			{
				validationStatus |= ValidationStatus.SignatureNotValid;
			}
			if ((certificateRevocationListStatus & CertificateRevocationListStatus.Malformed) != 0)
			{
				validationStatus |= ValidationStatus.Malformed;
			}
			if (validationStatus != (ValidationStatus)0L)
			{
				return ozdde(validationStatus | ValidationStatus.UnknownRev);
			}
		}
		if (((cdvat & ValidationOptions.IgnoreInvalidBasicConstraints) == 0 || 1 == 0) && (!p2.miwza(p0) || 1 == 0))
		{
			return ozdde(validationStatus | ValidationStatus.UnknownRev | ValidationStatus.InvalidBasicConstraints);
		}
		return validationStatus;
	}

	private bool abxyk(Certificate p0, CertificateRevocationList p1)
	{
		RevokedCertificate revokedCertificate = p1.twpgy(p0);
		if (revokedCertificate != null && 0 == 0 && dahxy.qsrcs(revokedCertificate.RevocationDate) <= dahxy.qsrcs(vvvmu) && 0 == 0 && revokedCertificate.GetRevocationReason() != RevocationReason.RemoveFromCrl)
		{
			return true;
		}
		return false;
	}

	private ezpxd pjsfv(eozod p0)
	{
		if (p0.ikelp.ynadv == ezpxd.dugxd && p0.ikelp.lawha != null && 0 == 0)
		{
			DateTime dateTime = dahxy.qsrcs(vvvmu);
			DateTime dateTime2 = dahxy.qsrcs(p0.cgyfx);
			DateTime dateTime3 = dahxy.qsrcs(p0.ikelp.lawha.dmgnf);
			if (dateTime < dateTime3 && 0 == 0 && dateTime3 < dateTime2.AddMinutes(-1.0) && 0 == 0)
			{
				return ezpxd.zyamk;
			}
		}
		return p0.ikelp.ynadv;
	}

	private ValidationStatus oghkq(Certificate p0, nisgb p1, lbxan p2)
	{
		return (ValidationStatus)0L;
	}

	private ValidationStatus mzopt(Certificate p0, nisgb p1, lbxan p2)
	{
		if (p2 == lbxan.pqcxw || 1 == 0)
		{
			return (ValidationStatus)0L;
		}
		ValidationStatus validationStatus = (ValidationStatus)0L;
		CertificateExtension certificateExtension = p1.fdmty["2.5.29.15"];
		KeyUses intendedUsage = p0.GetIntendedUsage();
		bool flag = (intendedUsage & KeyUses.KeyCertSign) != 0;
		if (certificateExtension != null && 0 == 0 && certificateExtension.Value != null && 0 == 0 && (intendedUsage == (KeyUses)0 || 1 == 0))
		{
			validationStatus |= ValidationStatus.InvalidExtension;
		}
		if ((!flag || 1 == 0) && ((cdvat & ValidationOptions.IgnoreWrongUsage) == 0 || 1 == 0))
		{
			validationStatus |= ozdde(ValidationStatus.WrongUsage);
		}
		return validationStatus;
	}

	private ValidationStatus pqrch(Certificate p0, nisgb p1, lbxan p2)
	{
		if (p2 == lbxan.pqcxw || 1 == 0)
		{
			return (ValidationStatus)0L;
		}
		ValidationStatus validationStatus = (ValidationStatus)0L;
		if (p2 != lbxan.spaqn)
		{
			if (wwipt <= 0 && ((cdvat & ValidationOptions.IgnoreInvalidBasicConstraints) == 0 || 1 == 0))
			{
				validationStatus |= ozdde(ValidationStatus.PathTooLong);
			}
			wwipt--;
		}
		if (p1.cdybo() < 2)
		{
			return validationStatus;
		}
		CertificateExtension certificateExtension = p1.fdmty["2.5.29.19"];
		if (certificateExtension == null || 1 == 0)
		{
			return validationStatus;
		}
		xxerq xxerq2 = p0.ortjx();
		if (certificateExtension != null && 0 == 0)
		{
			wwipt = Math.Min(xxerq2.fuext, wwipt);
		}
		if ((certificateExtension == null || false || !certificateExtension.Critical || 1 == 0) && ((cdvat & ValidationOptions.IgnoreInvalidBasicConstraints) == 0 || 1 == 0) && p2 == lbxan.tothp)
		{
			validationStatus |= ozdde(ValidationStatus.InvalidBasicConstraints);
		}
		if (p1.cdybo() < 2)
		{
			return ozdde(validationStatus | ValidationStatus.NotTrusted);
		}
		if ((certificateExtension == null || false || !xxerq2.xhaoc || 1 == 0) && ((cdvat & ValidationOptions.IgnoreInvalidBasicConstraints) == 0 || 1 == 0))
		{
			validationStatus |= ozdde(ValidationStatus.InvalidBasicConstraints);
		}
		return validationStatus;
	}

	private ValidationStatus ujwlj(Certificate p0, nisgb p1, Certificate p2, lbxan p3, ValidationStatus p4)
	{
		if ((p0.KeyAlgorithm == KeyAlgorithm.RSA || 1 == 0) && p0.GetKeySize() < 1024)
		{
			p4 |= ozdde(ValidationStatus.SignatureNotValid | ValidationStatus.WeakAlgorithm);
		}
		bool flag = (p4 & ValidationStatus.SignatureNotValid) == 0;
		switch (p0.GetSignatureHashAlgorithm())
		{
		case SignatureHashAlgorithm.MD5:
			if (p3 != lbxan.spaqn)
			{
				p4 |= ValidationStatus.WeakAlgorithm;
			}
			break;
		default:
			jazzx(LogLevel.Info, "Unable to verify certificate signature. Unsupported signature algorithm '{0}'.", p0.dyagy().Oid.Value);
			flag = false;
			p4 |= ozdde(ValidationStatus.UnsupportedSignatureAlgorithm);
			break;
		case SignatureHashAlgorithm.SHA1:
		case SignatureHashAlgorithm.SHA256:
		case SignatureHashAlgorithm.SHA384:
		case SignatureHashAlgorithm.SHA512:
		case SignatureHashAlgorithm.SHA224:
			break;
		}
		if (p2 == null || 1 == 0)
		{
			return p4 | ValidationStatus.IncompleteChain;
		}
		switch (p2.KeyAlgorithm)
		{
		case KeyAlgorithm.ECDsa:
		{
			string curve = p2.awyrh();
			if (AsymmetricKeyAlgorithm.IsSupported(AsymmetricKeyAlgorithmId.ECDsa, curve, 0) ? true : false)
			{
				break;
			}
			goto default;
		}
		default:
			jazzx(LogLevel.Info, "Unable to verify certificate signature. Unsupported key algorithm '{0}'.", p2.lukcl().Oid.Value);
			flag = false;
			p4 |= ozdde(ValidationStatus.UnsupportedSignatureAlgorithm);
			break;
		case KeyAlgorithm.RSA:
		case KeyAlgorithm.DSA:
			break;
		}
		if (dahxy.yaxnv && 0 == 0 && p3 == lbxan.spaqn && snqzm && 0 == 0)
		{
			jazzx(LogLevel.Debug, "Root certificate is implicitly trusted.");
			flag = false;
		}
		if (!flag || 1 == 0)
		{
			return p4;
		}
		jazzx(LogLevel.Debug, "Verifying certificate signature.");
		if (!p1.qpdba(p2) || 1 == 0)
		{
			jazzx(LogLevel.Error, "Certificate signature is not valid.");
			p4 |= ozdde(ValidationStatus.SignatureNotValid);
		}
		else
		{
			jazzx(LogLevel.Debug, "Certificate signature is valid.");
		}
		return p4;
	}

	private ValidationStatus xoqye(yxesv p0, yxesv p1)
	{
		ValidationStatus validationStatus = (ValidationStatus)0L;
		if (p0 == null || false || p1 == null)
		{
			return (ValidationStatus)0L;
		}
		if (((cdvat & ValidationOptions.IgnoreTimeNotValid) == 0 || 1 == 0) && (!p0.lixvv(vvvmu) || 1 == 0))
		{
			validationStatus |= ValidationStatus.TimeNotValid;
		}
		return validationStatus;
	}

	internal ValidationStatus fntlu(Certificate p0, Certificate p1)
	{
		if (p1 == null || 1 == 0)
		{
			return ValidationStatus.IncompleteChain;
		}
		if ((cdvat & ValidationOptions.IgnoreInvalidChain) != ValidationOptions.None && 0 == 0)
		{
			return (ValidationStatus)0L;
		}
		byte[] authorityKeyIdentifier = p0.GetAuthorityKeyIdentifier();
		byte[] subjectKeyIdentifier = p1.GetSubjectKeyIdentifier();
		if (authorityKeyIdentifier != null && 0 == 0 && subjectKeyIdentifier != null && 0 == 0)
		{
			if (!zjcch.wduyr(authorityKeyIdentifier, subjectKeyIdentifier) || 1 == 0)
			{
				return ozdde(ValidationStatus.InvalidChain);
			}
			return (ValidationStatus)0L;
		}
		if (!p0.GetIssuer().Equals(p1.GetSubject()) || 1 == 0)
		{
			return ozdde(ValidationStatus.InvalidChain);
		}
		return (ValidationStatus)0L;
	}
}
