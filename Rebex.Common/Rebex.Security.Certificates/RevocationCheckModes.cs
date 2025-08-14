using System;

namespace Rebex.Security.Certificates;

[Flags]
public enum RevocationCheckModes
{
	Ocsp = 1,
	Crl = 2
}
