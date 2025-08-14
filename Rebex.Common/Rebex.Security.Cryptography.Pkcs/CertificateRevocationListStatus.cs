using System;

namespace Rebex.Security.Cryptography.Pkcs;

[Flags]
public enum CertificateRevocationListStatus : long
{
	Valid = 0L,
	TimeNotValid = 1L,
	IssuerMismatch = 2L,
	UnknownCriticalExtension = 4L,
	WrongCrlUpdateTime = 8L,
	WrongIssuerUsage = 0x10L,
	SignatureNotValid = 0x20L,
	Malformed = 0x40L
}
