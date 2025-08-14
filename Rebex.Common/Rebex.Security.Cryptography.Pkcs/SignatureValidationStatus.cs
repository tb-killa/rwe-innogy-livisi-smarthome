using System;

namespace Rebex.Security.Cryptography.Pkcs;

[Flags]
public enum SignatureValidationStatus : long
{
	CertificateNotValid = 1L,
	CertificateNotAvailable = 2L,
	UnsupportedDigestAlgorithm = 4L,
	UnsupportedSignatureAlgorithm = 8L,
	InvalidSignature = 0x10L,
	InvalidKeyUsage = 0x20L,
	ContentTypeMismatch = 0x40L
}
