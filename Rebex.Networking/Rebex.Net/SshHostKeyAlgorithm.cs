using System;

namespace Rebex.Net;

[Flags]
public enum SshHostKeyAlgorithm
{
	None = 0,
	RSA = 1,
	DSS = 2,
	Certificate = 4,
	ED25519 = 8,
	ECDsaNistP256 = 0x10,
	ECDsaNistP384 = 0x20,
	ECDsaNistP521 = 0x40,
	Any = 0x7F
}
