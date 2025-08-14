using Rebex.Security.Certificates;

namespace Rebex.Security.Cryptography.Pkcs;

public interface ICertificateFinder
{
	CertificateChain Find(SubjectIdentifier subjectIdentifier, CertificateStore additionalStore);
}
