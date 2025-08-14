using System;

namespace Rebex.Net;

[Flags]
public enum TlsVersion
{
	None = 0,
	SSL30 = 1,
	TLS10 = 2,
	TLS11 = 4,
	TLS12 = 8,
	Any = 0xE
}
