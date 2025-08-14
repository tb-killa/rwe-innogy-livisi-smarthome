using System;

namespace Rebex.Security.Cryptography.Pkcs;

[Flags]
public enum SignatureOptions : long
{
	DisableSignedAttributes = 1L,
	DisableMicrosoftExtensions = 2L,
	DisableSMimeCapabilities = 4L,
	SkipCertificateUsageCheck = 8L
}
