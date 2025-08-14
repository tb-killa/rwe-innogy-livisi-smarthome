using System;
using System.Collections.Generic;
using onrkn;

namespace Rebex.Security.Certificates;

public abstract class CertificateEngine
{
	internal const ewirv ggapm = ewirv.liolk;

	internal const RevocationCheckModes alhyw = RevocationCheckModes.Ocsp | RevocationCheckModes.Crl;

	private readonly awngk usvyh;

	private static CertificateEngine bnjbu;

	private ewirv kykih;

	private RevocationCheckModes ogjqs;

	internal awngk awsqi => usvyh;

	public virtual ILogWriter LogWriter
	{
		get
		{
			return usvyh.xxboi;
		}
		set
		{
			usvyh.xxboi = value;
		}
	}

	internal ewirv cwesv
	{
		get
		{
			return kykih;
		}
		set
		{
			kykih = value;
		}
	}

	public RevocationCheckModes RevocationCheckModes
	{
		get
		{
			return ogjqs;
		}
		set
		{
			ogjqs = value;
		}
	}

	public static CertificateEngine Default => inkpv.utoah();

	public static CertificateEngine Internal => EnhancedCertificateEngine.yjnjz;

	public CertificateEngine()
	{
		Type type = GetType();
		if ((object)type == typeof(inkpv))
		{
			type = typeof(CertificateEngine);
		}
		usvyh = new awngk(type, null);
		cwesv = ewirv.liolk;
		RevocationCheckModes = RevocationCheckModes.Ocsp | RevocationCheckModes.Crl;
	}

	public abstract ValidationResult Validate(CertificateChain chain, CertificateValidationParameters parameters);

	public CertificateChain BuildChain(Certificate certificate)
	{
		return BuildChain(certificate, null);
	}

	public abstract CertificateChain BuildChain(Certificate certificate, IEnumerable<Certificate> extraStore);

	internal virtual CertificateChain wwils(Certificate p0, CertificateStore p1)
	{
		return BuildChain(p0, p1?.FindCertificates(CertificateFindOptions.None));
	}

	public static CertificateEngine GetCurrentEngine()
	{
		return bnjbu;
	}

	internal static CertificateEngine kemvq()
	{
		CertificateEngine certificateEngine = bnjbu;
		if (certificateEngine == null || 1 == 0)
		{
			certificateEngine = Default;
		}
		return certificateEngine;
	}

	public static void SetCurrentEngine(CertificateEngine engine)
	{
		bnjbu = engine;
	}

	internal void yxjbn(LogLevel p0, string p1, params object[] p2)
	{
		usvyh.byfnx(p0, "CERT", p1, p2);
	}

	internal void qhkmz(Certificate p0)
	{
		yxjbn(LogLevel.Debug, "Certificate chain building started ('{0}').", p0.GetSubjectName());
	}

	internal void muepb(Certificate p0, CertificateChain p1)
	{
		yxjbn(LogLevel.Debug, "Certificate chain building finished ('{0}', Length={1}, HasRoot={2}).", p0.GetSubjectName(), p1.Count, p1.RootCertificate != null);
	}

	internal void wbdsq(CertificateChain p0)
	{
		yxjbn(LogLevel.Debug, "Certificate chain validation started ('{0}').", (p0.LeafCertificate == null) ? "<empty chain>" : p0.LeafCertificate.GetSubjectName());
	}

	internal void qhkng(CertificateChain p0, ValidationResult p1)
	{
		yxjbn(LogLevel.Debug, "Certificate chain validation finished ('{0}', Valid={1}, Status={2}, NativeErrorCode=0x{3:X}).", (p0.LeafCertificate == null) ? "<empty chain>" : p0.LeafCertificate.GetSubjectName(), p1.Valid, p1.Status, p1.NativeErrorCode);
	}
}
