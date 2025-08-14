using System;

namespace Org.Mentalis.Security.Ssl;

[Flags]
public enum SecureProtocol
{
	None = 0,
	Ssl3 = 2,
	Tls1 = 4
}
