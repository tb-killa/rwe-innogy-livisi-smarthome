using RWE.SmartHome.SHC.Core;
using Rebex.Net;
using Rebex.Security.Certificates;

namespace RWE.SmartHome.SHC.SHCRelayDriver;

public class CertificateResolver : ICertificateRequestHandler
{
	private readonly ICertificateManager certificateManager;

	private CertificateChain certificateChain;

	internal CertificateResolver(ICertificateManager certMgr)
	{
		certificateManager = certMgr;
	}

	public CertificateChain Request(TlsSocket socket, DistinguishedName[] issuers)
	{
		if (certificateChain == null)
		{
			Certificate cert = new Certificate(certificateManager.GetPersonalCertificate());
			CertificateStore store = new CertificateStore(CertificateStoreName.My, CertificateStoreLocation.CurrentUser);
			certificateChain = CertificateChain.BuildFrom(cert, store);
		}
		return certificateChain;
	}
}
