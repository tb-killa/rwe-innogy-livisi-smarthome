namespace Rebex.Security.Cryptography.Pkcs;

public enum RevocationCheckStatus
{
	IssuerMismatch = 1,
	NotSuitable,
	NotRevoked,
	Revoked
}
