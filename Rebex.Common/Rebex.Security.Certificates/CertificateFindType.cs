using System;

namespace Rebex.Security.Certificates;

[Flags]
public enum CertificateFindType : long
{
	SubjectKeyIdentifier = 0L
}
