namespace Org.Mentalis.Security.Certificates;

public enum CertificateStatus
{
	ValidCertificate = 0,
	Expired = -2146762495,
	InvalidBasicConstraints = -2146869223,
	InvalidChain = -2146762486,
	InvalidNesting = -2146762494,
	InvalidPurpose = -2146762490,
	InvalidRole = -2146762493,
	InvalidSignature = -2146869244,
	NoCNMatch = -2146762481,
	ParentRevoked = -2146762484,
	RevocationFailure = -2146762482,
	RevocationServerOffline = -2146885613,
	Revoked = -2146885616,
	UntrustedRoot = -2146762487,
	UntrustedTestRoot = -2146762483,
	WrongUsage = -2146762480,
	OtherError = -1
}
