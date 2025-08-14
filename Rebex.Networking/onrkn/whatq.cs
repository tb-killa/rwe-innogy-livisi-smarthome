using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rebex.Net;
using Rebex.Security.Certificates;

namespace onrkn;

internal class whatq : ICertificateRequestHandler
{
	private sealed class skqnp
	{
		private sealed class wiqjz
		{
			public skqnp rvvft;

			public Certificate fmsti;

			public bool yqbld(Certificate p0)
			{
				return p0.Thumbprint == fmsti.Thumbprint;
			}
		}

		public Certificate[] kbuac;

		public bool sgggs(Certificate p0)
		{
			wiqjz wiqjz = new wiqjz();
			wiqjz.rvvft = this;
			wiqjz.fmsti = p0;
			return kbuac.Any(wiqjz.yqbld);
		}
	}

	private readonly List<Certificate> zfida;

	private readonly ICertificateRequestHandler gmxix;

	public whatq(CertificateCollection certificates, ICertificateRequestHandler secondaryHandler)
	{
		zfida = new List<Certificate>(certificates);
		ICertificateRequestHandler certificateRequestHandler = secondaryHandler;
		if (certificateRequestHandler == null || 1 == 0)
		{
			certificateRequestHandler = CertificateRequestHandler.NoCertificate;
		}
		gmxix = certificateRequestHandler;
	}

	public CertificateChain Request(TlsSocket socket, DistinguishedName[] issuers)
	{
		CertificateStore certificateStore = new CertificateStore((ICollection)zfida);
		try
		{
			CertificateChain certificateChain = uobmb(certificateStore, issuers, CertificateFindOptions.HasPrivateKey | CertificateFindOptions.IsTimeValid | CertificateFindOptions.ClientAuthentication | CertificateFindOptions.IncludeSubordinateAuthorities);
			if (certificateChain == null || 1 == 0)
			{
				certificateChain = uobmb(certificateStore, issuers, CertificateFindOptions.HasPrivateKey | CertificateFindOptions.IncludeSubordinateAuthorities);
			}
			if (certificateChain != null && 0 == 0)
			{
				return certificateChain;
			}
		}
		finally
		{
			if (certificateStore != null && 0 == 0)
			{
				((IDisposable)certificateStore).Dispose();
			}
		}
		return gmxix.Request(socket, issuers);
	}

	private CertificateChain uobmb(CertificateStore p0, DistinguishedName[] p1, CertificateFindOptions p2)
	{
		Func<Certificate, bool> func = null;
		skqnp skqnp = new skqnp();
		if (p1 != null && 0 == 0 && p1.Length > 0)
		{
			skqnp.kbuac = p0.FindCertificates(p1, p2);
		}
		else
		{
			skqnp.kbuac = p0.FindCertificates(p2);
		}
		if (skqnp.kbuac != null && 0 == 0 && skqnp.kbuac.Length > 0)
		{
			List<Certificate> source = zfida;
			if (func == null || 1 == 0)
			{
				func = skqnp.sgggs;
			}
			Certificate cert = source.First(func);
			return CertificateChain.BuildFrom(cert);
		}
		return null;
	}
}
