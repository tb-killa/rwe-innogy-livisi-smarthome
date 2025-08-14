namespace Rebex.Security.Cryptography.Pkcs;

public enum CertificateIncludeOption
{
	LeaveExisting = -1,
	None,
	ExcludeRoot,
	EndCertificateOnly,
	WholeChain
}
