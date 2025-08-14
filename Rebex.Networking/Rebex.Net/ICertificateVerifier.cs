using Rebex.Security.Certificates;

namespace Rebex.Net;

public interface ICertificateVerifier
{
	TlsCertificateAcceptance Verify(TlsSocket socket, string commonName, CertificateChain certificateChain);
}
