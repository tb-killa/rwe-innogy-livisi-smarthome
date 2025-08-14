using System;
using System.Collections.Generic;
using Rebex.Security.Certificates;

namespace onrkn;

internal sealed class inkpv : CertificateEngine
{
	private static readonly CertificateEngine ocrkk = new inkpv(useLocalMachine: false);

	private static readonly CertificateEngine mpmvr = new inkpv(useLocalMachine: true);

	private readonly bool iavrz;

	public static CertificateEngine utoah()
	{
		return ocrkk;
	}

	public static CertificateEngine xzpdd()
	{
		return mpmvr;
	}

	private inkpv(bool useLocalMachine)
	{
		iavrz = useLocalMachine;
	}

	public override CertificateChain BuildChain(Certificate certificate, IEnumerable<Certificate> extraStore)
	{
		if (certificate == null || 1 == 0)
		{
			throw new ArgumentNullException("certificate");
		}
		CertificateStore certificateStore = ((extraStore != null) ? new CertificateStore(extraStore) : null);
		try
		{
			return wwils(certificate, certificateStore);
		}
		finally
		{
			if (certificateStore != null && 0 == 0)
			{
				certificateStore.Dispose();
			}
		}
	}

	internal override CertificateChain wwils(Certificate p0, CertificateStore p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("certificate");
		}
		qhkmz(p0);
		CertificateChain certificateChain = CertificateChain.BuildFrom(p0, p1, CertificateChain.fqjtz());
		if (dahxy.hdfhq && 0 == 0 && (certificateChain.RootCertificate == null || 1 == 0))
		{
			CertificateChain certificateChain2 = CertificateChain.BuildFrom(p0, p1, CertificateChainEngine.CurrentUser);
			if (certificateChain2.Count > certificateChain.Count)
			{
				certificateChain = certificateChain2;
			}
		}
		muepb(p0, certificateChain);
		return certificateChain;
	}

	public override ValidationResult Validate(CertificateChain chain, CertificateValidationParameters parameters)
	{
		if (chain == null || 1 == 0)
		{
			throw new ArgumentNullException("chain");
		}
		wbdsq(chain);
		ValidationOptions options = parameters?.Options ?? ValidationOptions.None;
		ValidationResult validationResult = chain.Validate(null, options, CertificateChain.fqjtz());
		qhkng(chain, validationResult);
		return validationResult;
	}
}
