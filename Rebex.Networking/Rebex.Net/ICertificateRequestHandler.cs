using Rebex.Security.Certificates;

namespace Rebex.Net;

public interface ICertificateRequestHandler
{
	CertificateChain Request(TlsSocket socket, DistinguishedName[] issuers);
}
