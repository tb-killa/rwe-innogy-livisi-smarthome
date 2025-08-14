namespace Rebex.Net;

public enum TlsCertificateAcceptance
{
	Accept = 0,
	CommonNameMismatch = -1,
	RevocationCheckFailed = -2,
	UnsupportedSignatureAlgorithm = -3,
	InvalidKeyUsage = -4,
	UnknownServerName = -5,
	Bad = 42,
	Unsupported = 43,
	Revoked = 44,
	Expired = 45,
	UnknownAuthority = 48,
	Other = 46
}
