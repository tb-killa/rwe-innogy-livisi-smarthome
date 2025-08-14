using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Rebex;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;

namespace onrkn;

internal static class kuoty
{
	private sealed class rlwcr
	{
		public byte[] frykf;

		public bool xlsig(eozod p0)
		{
			return jlfbq.oreja(p0.ewoaz.vfdhm, frykf);
		}
	}

	public const int dbimh = 16;

	private static Func<CertificateExtension, bool> iizbi;

	public static tmygz jnvtx(byte[] p0, Certificate p1, HashingAlgorithmId p2, byte[] p3)
	{
		byte[] issuerNameHash = HashingAlgorithm.ComputeHash(p2, p1.GetSubject().ToArray());
		byte[] issuerKeyHash = HashingAlgorithm.ComputeHash(p2, p1.GetPublicKey());
		ajezg certId = new ajezg(p0, issuerNameHash, issuerKeyHash, p2);
		if (p3 != null && 0 == 0)
		{
			return new tmygz(certId, zyked.qzkku(p3));
		}
		return new tmygz(certId);
	}

	public static eozod lywik(this rxwdr p0, byte[] p1)
	{
		rlwcr rlwcr = new rlwcr();
		rlwcr.frykf = p1;
		return p0.wglak.kndop.FirstOrDefault(rlwcr.xlsig);
	}

	public static bool twnrh(this rxwdr p0, byte[] p1)
	{
		zyked zyked2 = p0.gkrhh.jgdyi.atlhx["1.3.6.1.5.5.7.48.1.2"];
		if (zyked2 == null || 1 == 0)
		{
			return true;
		}
		try
		{
			byte[] rtrhq = rwolq.tvjgt(zyked2.Value).rtrhq;
			return jlfbq.oreja(p1, rtrhq);
		}
		catch (CryptographicException)
		{
			return false;
		}
	}

	public static rxwdr mddqi(zlbxa p0, string p1, tmygz p2, bool p3, awngk p4, out bool p5)
	{
		string p6;
		bool p7;
		rxwdr result = wvyup(p0, p1, p2, p3, p4, out p5, out p6, out p7);
		if (p7 && 0 == 0)
		{
			p0.raovz(p6, p4);
		}
		return result;
	}

	public static rxwdr wvyup(zlbxa p0, string p1, tmygz p2, bool p3, awngk p4, out bool p5, out string p6, out bool p7)
	{
		rxwdr rxwdr2 = null;
		p5 = false;
		p7 = false;
		if (p2.ypajc.Count != 1)
		{
			throw new ArgumentException("Unsupported OCSP request.", "ocspRequest");
		}
		p4.byfnx(LogLevel.Debug, "OCSP", "Trying to load OCSP response from cache '{0}'.", p1);
		ajezg ymogv = p2.ypajc[0].ymogv;
		p6 = brgjd.edcru("{0}#{1}#{2}", p1, brgjd.wlvqq(ymogv.vfdhm), Convert.ToBase64String(ymogv.znwxm));
		Stream stream = p0.xwedh(p6, p4);
		if (stream != null && 0 == 0)
		{
			try
			{
				rxwdr2 = duljq(stream, p4);
			}
			finally
			{
				stream.Dispose();
			}
			if (rxwdr2 != null && 0 == 0)
			{
				if (rxwdr2.wglak == null || false || rxwdr2.wglak.kndop == null || false || rxwdr2.wglak.kndop.Count == 0 || 1 == 0)
				{
					p4.byfnx(LogLevel.Error, "OCSP", "OCSP response found in cache has empty data '{0}'.", p1);
				}
				else
				{
					eozod eozod2 = rxwdr2.wglak.kndop[0];
					if (eozod2.egrdf.HasValue && 0 == 0 && dahxy.qsrcs(eozod2.egrdf.Value) > DateTime.UtcNow && 0 == 0)
					{
						p4.byfnx(LogLevel.Debug, "OCSP", "OCSP response loaded from cache '{0}'.", p1);
						p5 = true;
						return rxwdr2;
					}
					p4.byfnx(LogLevel.Debug, "OCSP", "OCSP response found in cache has expired '{0}'.", p1);
					p7 = true;
				}
			}
			else
			{
				p4.byfnx(LogLevel.Error, "OCSP", "OCSP response found in cache is unparsable '{0}'.", p1);
			}
		}
		if (p3 && 0 == 0)
		{
			p4.byfnx(LogLevel.Debug, "OCSP", "CacheOnly = true, returning null for '{0}'.", p1);
			return null;
		}
		p4.byfnx(LogLevel.Debug, "OCSP", "Trying to get OCSP response from '{0}'.", p1);
		byte[] array = fxakl.kncuz(p2);
		p4.iyauk(LogLevel.Verbose, "OCSP", "Sending OCSP request", array, 0, array.Length);
		rxwdr2 = null;
		stream = agbih.mmrkx(p1, array, p4, "OCSP", ujajv.slcvx("Content-Type", "application/ocsp-request"));
		if (stream != null && 0 == 0)
		{
			p4.byfnx(LogLevel.Debug, "OCSP", "Parsing OCSP response '{0}'.", p1);
			rxwdr2 = duljq(stream, p4);
			if (rxwdr2 != null && 0 == 0)
			{
				switch (rxwdr2.qgdlw)
				{
				case ffncd.pqpgx:
					break;
				default:
					p4.byfnx(LogLevel.Debug, "OCSP", "OCSP response status is '{1}' for '{0}'.", p1, lsltq(rxwdr2.qgdlw));
					return null;
				}
				if (rxwdr2.wglak == null || false || rxwdr2.wglak.kndop == null || false || rxwdr2.wglak.kndop.Count == 0 || 1 == 0)
				{
					p4.byfnx(LogLevel.Debug, "OCSP", "OCSP response has empty data '{0}'.", p1);
					return null;
				}
				p4.byfnx(LogLevel.Debug, "OCSP", "Saving OCSP response to cache '{0}'.", p1);
				stream.Position = 0L;
				p0.vrgug(p6, stream, p4);
				p7 = false;
			}
			else
			{
				p4.byfnx(LogLevel.Error, "OCSP", "Unable to parse OCSP response downloaded from '{0}'.", p1);
			}
		}
		else
		{
			p4.byfnx(LogLevel.Error, "OCSP", "Unable to download OCSP response from '{0}'.", p1);
		}
		return rxwdr2;
	}

	private static string lsltq(ffncd p0)
	{
		return p0 switch
		{
			ffncd.xcneo => "Unknown", 
			ffncd.pqpgx => "Success", 
			ffncd.qavqz => "MalformedRequest", 
			ffncd.xcyex => "InternalError", 
			ffncd.udrew => "TryLater", 
			ffncd.qbxnv => "SignatureRequired", 
			ffncd.mfxtk => "Unauthorized", 
			_ => "Undefined", 
		};
	}

	public static rxwdr duljq(Stream p0, awngk p1)
	{
		try
		{
			rxwdr rxwdr2 = new rxwdr();
			hfnnn hfnnn2 = new hfnnn(rxwdr2);
			try
			{
				p0.alskc(hfnnn2);
			}
			finally
			{
				if (hfnnn2 != null && 0 == 0)
				{
					((IDisposable)hfnnn2).Dispose();
				}
			}
			return rxwdr2;
		}
		catch (CryptographicException ex)
		{
			p1.byfnx(LogLevel.Error, "OCSP", "Unable to parse OCSP response: {0}", ex);
			return null;
		}
	}

	public static ValidationStatus vktjg(rxwdr p0, Certificate p1, awngk p2)
	{
		if (p0.gkrhh.pcvsi.vvmoi(p0: false) == (HashingAlgorithmId)0 || 1 == 0)
		{
			return ValidationStatus.UnsupportedSignatureAlgorithm;
		}
		CertificateCollection waxgs = p0.gkrhh.waxgs;
		Certificate certificate;
		if (waxgs != null && 0 == 0 && waxgs.Count > 0)
		{
			certificate = waxgs[0];
			string[] enhancedUsage = certificate.GetEnhancedUsage();
			if (enhancedUsage == null || false || Array.IndexOf(enhancedUsage, "1.3.6.1.5.5.7.3.9") < 0)
			{
				p2.rfpvf(LogLevel.Debug, "OCSP", "Ignoring delegated OCSP signer - no OcspSigning key usage.");
			}
			else if (!certificate.GetIssuer().Equals(p1.GetSubject()) || 1 == 0)
			{
				p2.rfpvf(LogLevel.Debug, "OCSP", "Ignoring delegated OCSP signer - not issued by same authority.");
			}
			else if (waxgs.Count == 1 || jlfbq.oreja(p1.GetPublicKey(), waxgs[1].GetPublicKey()))
			{
				if (certificate.mprva(p1) ? true : false)
				{
					goto IL_0167;
				}
				p2.rfpvf(LogLevel.Debug, "OCSP", "Ignoring delegated OCSP signer - certificate signature is not valid.");
			}
			else
			{
				CertificateChain certificateChain = new CertificateChain(waxgs.ToArray());
				ValidationResult validationResult = certificateChain.Validate(ValidationOptions.SkipRevocationCheck);
				if (validationResult.Valid ? true : false)
				{
					goto IL_0167;
				}
				p2.byfnx(LogLevel.Debug, "OCSP", "Ignoring delegated OCSP signer - chain is not valid (0x{0:X8} - {1}).", validationResult.NativeErrorCode, validationResult.Status);
			}
		}
		goto IL_027a;
		IL_0167:
		CertificateExtensionCollection extensions = certificate.Extensions;
		if (iizbi == null || 1 == 0)
		{
			iizbi = gzttz;
		}
		if (extensions.Any(iizbi) && 0 == 0)
		{
			p2.rfpvf(LogLevel.Debug, "OCSP", "Skipping revocation check of delegated OCSP signer (due to 'ocsp-nocheck' extension).");
		}
		else
		{
			vcnjn vcnjn2 = certificate.vgnxi();
			CrlDistributionPointCollection crlDistributionPoints = certificate.GetCrlDistributionPoints();
			p2.byfnx(LogLevel.Debug, "OCSP", "Skipping revocation check of delegated OCSP signer ('ocsp-nocheck' extension not found). Issued with AuthorityInfoAccess: {0}, CRL points: {1}.", (vcnjn2 == null) ? "<null>" : vcnjn2.Count.ToString(), (crlDistributionPoints == null) ? "<null>" : crlDistributionPoints.Count.ToString());
		}
		if (!p0.gkrhh.mjaxu(certificate) || 1 == 0)
		{
			p2.rfpvf(LogLevel.Debug, "OCSP", "Ignoring delegated OCSP signer - response signature is not valid.");
			goto IL_027a;
		}
		return (ValidationStatus)0L;
		IL_027a:
		if (p0.gkrhh.mjaxu(p1) && 0 == 0)
		{
			return (ValidationStatus)0L;
		}
		return ValidationStatus.SignatureNotValid;
	}

	private static bool gzttz(CertificateExtension p0)
	{
		return p0.Oid == "1.3.6.1.5.5.7.48.1.5";
	}
}
