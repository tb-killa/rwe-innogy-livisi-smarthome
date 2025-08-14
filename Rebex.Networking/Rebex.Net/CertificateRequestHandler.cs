using System;
using Rebex.Security.Certificates;

namespace Rebex.Net;

public abstract class CertificateRequestHandler
{
	private class uklpw : ICertificateRequestHandler
	{
		public CertificateChain Request(TlsSocket socket, DistinguishedName[] issuers)
		{
			return null;
		}
	}

	private class biwtx : ICertificateRequestHandler
	{
		public CertificateChain Request(TlsSocket socket, DistinguishedName[] issuers)
		{
			CertificateFindOptions options = CertificateFindOptions.HasPrivateKey | CertificateFindOptions.IsTimeValid | CertificateFindOptions.ClientAuthentication | CertificateFindOptions.IncludeSubordinateAuthorities;
			CertificateStore certificateStore = new CertificateStore(CertificateStoreName.My);
			Certificate[] array = ((issuers == null || false || issuers.Length <= 0) ? certificateStore.FindCertificates(options) : certificateStore.FindCertificates(issuers, options));
			if (array.Length == 0 || 1 == 0)
			{
				return null;
			}
			return CertificateChain.BuildFrom(array[0]);
		}
	}

	private class tomlp : ICertificateRequestHandler
	{
		private readonly CertificateChain cgwie;

		public tomlp(CertificateChain chain)
		{
			cgwie = chain;
		}

		public CertificateChain Request(TlsSocket socket, DistinguishedName[] issuers)
		{
			return cgwie;
		}
	}

	public static readonly ICertificateRequestHandler NoCertificate = new uklpw();

	public static readonly ICertificateRequestHandler StoreSearch = new biwtx();

	private CertificateRequestHandler()
	{
	}

	public static ICertificateRequestHandler CreateRequestHandler(CertificateChain certificateChain)
	{
		if (certificateChain == null || 1 == 0)
		{
			throw new ArgumentNullException("certificateChain");
		}
		if (certificateChain.Count == 0 || 1 == 0)
		{
			throw new ArgumentException("The chain is empty.", "certificateChain");
		}
		if (!certificateChain[0].HasPrivateKey() || 1 == 0)
		{
			throw new ArgumentException("The first certificate in the chain does not have an associated private key.", "certificateChain");
		}
		return new tomlp(certificateChain);
	}

	public static ICertificateRequestHandler CreateRequestHandler(Certificate certificate, CertificateChainEngine engine)
	{
		if (certificate == null || 1 == 0)
		{
			throw new ArgumentNullException("certificate");
		}
		CertificateChain certificateChain = CertificateChain.BuildFrom(certificate, engine);
		return CreateRequestHandler(certificateChain);
	}

	public static ICertificateRequestHandler CreateRequestHandler(Certificate certificate)
	{
		if (certificate == null || 1 == 0)
		{
			throw new ArgumentNullException("certificate");
		}
		CertificateChain certificateChain = CertificateChain.BuildFrom(certificate);
		return CreateRequestHandler(certificateChain);
	}
}
