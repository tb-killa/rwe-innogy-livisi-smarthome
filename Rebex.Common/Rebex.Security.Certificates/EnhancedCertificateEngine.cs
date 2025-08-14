using System;
using System.Collections.Generic;
using System.IO;
using onrkn;

namespace Rebex.Security.Certificates;

public class EnhancedCertificateEngine : CertificateEngine
{
	private static readonly CertificateEngine lmvvn = new EnhancedCertificateEngine();

	private CertificateStore baogz;

	private string vswew;

	private zlbxa qtlth;

	private mnzit qqooh;

	internal static CertificateEngine yjnjz => lmvvn;

	public string CacheLocation
	{
		get
		{
			return vswew;
		}
		set
		{
			if (string.IsNullOrEmpty(value) && 0 == 0)
			{
				vswew = null;
				qtlth = null;
				qqooh = null;
				return;
			}
			string fullPath = Path.GetFullPath(value);
			if (!Directory.Exists(fullPath) || 1 == 0)
			{
				Directory.CreateDirectory(fullPath);
			}
			vswew = fullPath;
			qtlth = new zlbxa(fullPath);
			qqooh = new mnzit(fullPath);
		}
	}

	public EnhancedCertificateEngine()
	{
	}

	public EnhancedCertificateEngine(IEnumerable<Certificate> trustedRootAuthorities)
	{
		if (trustedRootAuthorities == null || 1 == 0)
		{
			throw new ArgumentNullException("trustedRootAuthorities", "Value cannot be null.");
		}
		baogz = new CertificateStore(trustedRootAuthorities);
	}

	public override CertificateChain BuildChain(Certificate certificate, IEnumerable<Certificate> extraStore)
	{
		if (certificate == null || 1 == 0)
		{
			throw new ArgumentNullException("certificate");
		}
		qhkmz(certificate);
		CertificateStore certificateStore = ((extraStore != null) ? new CertificateStore(extraStore) : null);
		if (baogz != null && 0 == 0)
		{
			if (certificateStore == null || 1 == 0)
			{
				certificateStore = baogz;
			}
			else
			{
				certificateStore.nbjwv(baogz.FindCertificates(CertificateFindOptions.None));
			}
		}
		CertificateChain result = CertificateChain.BuildFrom(certificate, certificateStore, CertificateChain.fqjtz());
		qhkmz(certificate);
		return result;
	}

	public override ValidationResult Validate(CertificateChain chain, CertificateValidationParameters parameters)
	{
		if (chain == null || 1 == 0)
		{
			throw new ArgumentNullException("chain");
		}
		wbdsq(chain);
		ValidationResult validationResult = ixklj.wcbka(this, qtlth, qqooh, chain, parameters, baogz);
		qhkng(chain, validationResult);
		return validationResult;
	}
}
