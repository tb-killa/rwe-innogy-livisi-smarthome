using System;

namespace Rebex.Security.Certificates;

[Flags]
public enum CertificateFindOptions : long
{
	None = 0L,
	HasPrivateKey = 1L,
	IsTimeValid = 2L,
	ClientAuthentication = 4L,
	ServerAuthentication = 8L,
	IncludeSubordinateAuthorities = 0x10L
}
