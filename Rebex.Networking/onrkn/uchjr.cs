using System;
using Rebex.Net;
using Rebex.Security.Certificates;

namespace onrkn;

internal class uchjr : ICertificateVerifier
{
	private class bvdoo : SslCertificateValidationEventArgs
	{
		public TlsCertificateAcceptance? wiyph => base.Result;

		public bvdoo(TlsSocket socket, string commonName, CertificateChain certificateChain, ValidationOptions options)
			: base(socket, commonName, certificateChain)
		{
			base.Options = options;
		}
	}

	private readonly rlpyd cqavd;

	private readonly ICertificateVerifier trleu;

	private string fmgex;

	public uchjr(rlpyd owner, ICertificateVerifier originalVerifier)
	{
		cqavd = owner;
		trleu = originalVerifier;
	}

	public TlsCertificateAcceptance Verify(TlsSocket socket, string commonName, CertificateChain certificateChain)
	{
		string text = BitConverter.ToString(certificateChain.LeafCertificate.GetRawCertData());
		if (fmgex != null && 0 == 0 && fmgex == text && 0 == 0)
		{
			return TlsCertificateAcceptance.Accept;
		}
		TlsCertificateAcceptance? tlsCertificateAcceptance = null;
		SslSettings mlept = cqavd.mlept;
		ValidationOptions sslServerCertificateValidationOptions = mlept.SslServerCertificateValidationOptions;
		if ((commonName == null || 1 == 0) && socket.Entity == TlsConnectionEnd.Client)
		{
			commonName = "<unknown>";
		}
		if (mlept.SslAcceptAllCertificates && 0 == 0)
		{
			tlsCertificateAcceptance = TlsCertificateAcceptance.Accept;
		}
		else if (cqavd.clxxv && 0 == 0)
		{
			bvdoo bvdoo = new bvdoo(socket, commonName, certificateChain, sslServerCertificateValidationOptions);
			cqavd.pyugx(bvdoo);
			tlsCertificateAcceptance = bvdoo.wiyph;
		}
		TlsCertificateAcceptance tlsCertificateAcceptance2;
		if (tlsCertificateAcceptance.HasValue && 0 == 0)
		{
			tlsCertificateAcceptance2 = tlsCertificateAcceptance.Value;
		}
		else
		{
			ICertificateVerifier certificateVerifier = ((trleu != null) ? trleu : mlept.SslServerCertificateVerifier);
			if (certificateVerifier == CertificateVerifier.Default)
			{
				syxoe p = ((!mlept.SslStrictKeyUsageValidation || 1 == 0) ? syxoe.dovpm : syxoe.wruup);
				tlsCertificateAcceptance2 = CertificateVerifier.utdgi(socket, commonName, certificateChain, sslServerCertificateValidationOptions, p);
			}
			else
			{
				tlsCertificateAcceptance2 = certificateVerifier.Verify(socket, commonName, certificateChain);
			}
		}
		if (tlsCertificateAcceptance2 == TlsCertificateAcceptance.Accept || 1 == 0)
		{
			fmgex = text;
		}
		else if (tlsCertificateAcceptance2 == TlsCertificateAcceptance.CommonNameMismatch && commonName == "<unknown>" && 0 == 0)
		{
			tlsCertificateAcceptance2 = TlsCertificateAcceptance.UnknownServerName;
		}
		return tlsCertificateAcceptance2;
	}
}
