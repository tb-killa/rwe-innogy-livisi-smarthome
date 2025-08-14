using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Net;

public abstract class CertificateVerifier
{
	private class wwddm : ICertificateVerifier
	{
		public TlsCertificateAcceptance Verify(TlsSocket socket, string commonName, CertificateChain certificateChain)
		{
			return TlsCertificateAcceptance.Accept;
		}
	}

	private class fywrg : ICertificateVerifier
	{
		public TlsCertificateAcceptance Verify(TlsSocket socket, string commonName, CertificateChain certificateChain)
		{
			return utdgi(socket, commonName, certificateChain, ValidationOptions.None, syxoe.iuljy);
		}
	}

	internal const string pdzyb = "<unknown>";

	public static readonly ICertificateVerifier AcceptAll = new wwddm();

	public static readonly ICertificateVerifier Default = new fywrg();

	private CertificateVerifier()
	{
	}

	internal static TlsCertificateAcceptance utdgi(TlsSocket p0, string p1, CertificateChain p2, ValidationOptions p3, syxoe p4)
	{
		bool flag = p0.Entity == TlsConnectionEnd.Server;
		if (flag && 0 == 0)
		{
			p1 = null;
		}
		else if ((p1 == null || false || p1 == "<unknown>") && ((p3 & ValidationOptions.IgnoreCnNotMatch) == 0 || 1 == 0))
		{
			return TlsCertificateAcceptance.UnknownServerName;
		}
		ValidationResult validationResult = p2.Validate(p1, p3);
		TlsCertificateAcceptance tlsCertificateAcceptance = ((!validationResult.Valid || 1 == 0) ? syoyk(validationResult.Status) : TlsCertificateAcceptance.Accept);
		if (p0.Entity == TlsConnectionEnd.Client && ((p3 & ValidationOptions.IgnoreWrongUsage) == 0 || 1 == 0))
		{
			Certificate leafCertificate = p2.LeafCertificate;
			if (leafCertificate != null && 0 == 0 && (!pmesp(leafCertificate, flag, p4) || 1 == 0))
			{
				if (validationResult.Valid && 0 == 0)
				{
					tlsCertificateAcceptance = TlsCertificateAcceptance.InvalidKeyUsage;
				}
				validationResult.mpqyw(p0: false, 16L);
			}
		}
		if (tlsCertificateAcceptance != TlsCertificateAcceptance.Accept && 0 == 0)
		{
			ILogWriter logWriter = p0.LogWriter;
			if (logWriter != null && 0 == 0 && logWriter.Level <= LogLevel.Info)
			{
				logWriter.Write(LogLevel.Info, typeof(TlsSocket), p0.pxzpp, "TLS", brgjd.edcru("Certificate verification status: {0} (0x{1:X8})", validationResult.Status, validationResult.NativeErrorCode));
			}
		}
		return tlsCertificateAcceptance;
	}

	internal static bool pmesp(Certificate p0, bool p1, syxoe p2)
	{
		switch (p2)
		{
		case syxoe.iuljy:
			return true;
		case syxoe.dovpm:
			return cmacj(p0, p1).GetValueOrDefault(rhenm(p0));
		case syxoe.wruup:
			if (rhenm(p0) && 0 == 0)
			{
				bool? flag = cmacj(p0, p1);
				if (flag != true || 1 == 0)
				{
					return !flag.HasValue;
				}
				return true;
			}
			return false;
		default:
			throw hifyx.nztrs("keyUsageCheckMode", (int)p2, "Unsupported option.");
		}
	}

	private static bool? cmacj(Certificate p0, bool p1)
	{
		string[] enhancedUsage = p0.GetEnhancedUsage();
		if (enhancedUsage == null || 1 == 0)
		{
			return null;
		}
		return enhancedUsage.babpw((p1 ? true : false) ? "1.3.6.1.5.5.7.3.2" : "1.3.6.1.5.5.7.3.1", "2.5.29.37.0") >= 0;
	}

	private static bool rhenm(Certificate p0)
	{
		KeyUses intendedUsage = p0.GetIntendedUsage();
		return (intendedUsage & (KeyUses.DigitalSignature | KeyUses.KeyEncipherment | KeyUses.KeyAgreement)) != 0;
	}

	internal static TlsCertificateAcceptance syoyk(ValidationStatus p0)
	{
		if (p0 == (ValidationStatus)0L)
		{
			return TlsCertificateAcceptance.Accept;
		}
		if ((p0 & (ValidationStatus.RootNotTrusted | ValidationStatus.IncompleteChain)) != 0)
		{
			return TlsCertificateAcceptance.UnknownAuthority;
		}
		if ((p0 & ValidationStatus.Revoked) != 0)
		{
			return TlsCertificateAcceptance.Revoked;
		}
		if ((p0 & ValidationStatus.UnsupportedSignatureAlgorithm) != 0)
		{
			return TlsCertificateAcceptance.UnsupportedSignatureAlgorithm;
		}
		if ((p0 & ValidationStatus.SignatureNotValid) != 0)
		{
			return TlsCertificateAcceptance.Bad;
		}
		if ((p0 & ValidationStatus.CnNotMatch) != 0)
		{
			return TlsCertificateAcceptance.CommonNameMismatch;
		}
		if ((p0 & ValidationStatus.TimeNotValid) != 0)
		{
			return TlsCertificateAcceptance.Expired;
		}
		if ((p0 & (ValidationStatus.UnknownError | ValidationStatus.Malformed)) != 0)
		{
			return TlsCertificateAcceptance.Other;
		}
		if ((p0 & ValidationStatus.WrongUsage) != 0)
		{
			return TlsCertificateAcceptance.Bad;
		}
		if ((p0 & (ValidationStatus.UnknownRev | ValidationStatus.OfflineRev)) != 0)
		{
			return TlsCertificateAcceptance.RevocationCheckFailed;
		}
		return TlsCertificateAcceptance.Other;
	}

	internal static string nmzmo(TlsCertificateAcceptance p0, string p1, CertificateChain p2, string p3)
	{
		string text = brgjd.rbxxu(p1[0]) + p1.Substring(1);
		switch (p0)
		{
		case TlsCertificateAcceptance.Bad:
			return brgjd.edcru("{0} certificate was rejected by the verifier because it is bad (certificate is corrupt, contains signatures that do not verify correctly, etc.).", text);
		case TlsCertificateAcceptance.Unsupported:
			return brgjd.edcru("{0} certificate was rejected by the verifier because it is unsupported.", text);
		case TlsCertificateAcceptance.Revoked:
			return brgjd.edcru("{0} certificate was rejected by the verifier because it is revoked.", text);
		case TlsCertificateAcceptance.Expired:
			return brgjd.edcru("{0} certificate was rejected by the verifier because it has expired.", text);
		case TlsCertificateAcceptance.UnknownAuthority:
			return brgjd.edcru("{0} certificate was rejected by the verifier because of an unknown certificate authority.", text);
		case TlsCertificateAcceptance.CommonNameMismatch:
		{
			string text2 = null;
			if (p2.Count > 0)
			{
				text2 = p2[0].GetCommonName();
			}
			if (text2 == null || 1 == 0)
			{
				text2 = "(n/a)";
			}
			return brgjd.edcru("{0} certificate was rejected by the verifier because the certificate's common name '{1}' does not match the hostname '{2}'.", text, text2, p3);
		}
		case TlsCertificateAcceptance.RevocationCheckFailed:
			return brgjd.edcru("Unable to perform revocation check of the {0} certificate.", p1);
		case TlsCertificateAcceptance.UnsupportedSignatureAlgorithm:
			return brgjd.edcru("{0} certificate signature algorithm is not supported on this platform.", text);
		case TlsCertificateAcceptance.InvalidKeyUsage:
			return brgjd.edcru("{0} certificate was rejected by the verifier because of invalid key usage.", text);
		case TlsCertificateAcceptance.UnknownServerName:
			return "Certificate's common name could not be verified because the target host name is unknown.";
		default:
			return brgjd.edcru("{0} certificate was rejected by the verifier because of other problem.", text);
		}
	}

	internal static mjddr rjdmd(TlsCertificateAcceptance p0, TlsProtocol p1)
	{
		switch (p0)
		{
		case TlsCertificateAcceptance.Bad:
		case TlsCertificateAcceptance.Unsupported:
		case TlsCertificateAcceptance.Revoked:
		case TlsCertificateAcceptance.Expired:
			return (mjddr)p0;
		case TlsCertificateAcceptance.UnknownAuthority:
			if (p1 == TlsProtocol.SSL30)
			{
				return mjddr.vyvjd;
			}
			return mjddr.kxgat;
		case TlsCertificateAcceptance.InvalidKeyUsage:
			return mjddr.fvtwt;
		default:
			return mjddr.vyvjd;
		}
	}
}
