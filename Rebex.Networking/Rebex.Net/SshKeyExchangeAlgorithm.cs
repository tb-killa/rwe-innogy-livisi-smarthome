using System;

namespace Rebex.Net;

[Flags]
public enum SshKeyExchangeAlgorithm
{
	None = 0,
	DiffieHellmanGroup1SHA1 = 1,
	DiffieHellmanGroup14SHA1 = 2,
	DiffieHellmanGroupExchangeSHA1 = 4,
	DiffieHellmanGroupExchangeSHA256 = 8,
	ECDiffieHellmanNistP256 = 0x10,
	ECDiffieHellmanNistP384 = 0x20,
	ECDiffieHellmanNistP521 = 0x40,
	Curve25519 = 0x80,
	DiffieHellmanOakleyGroupSHA256 = 0x100,
	DiffieHellmanOakleyGroupSHA512 = 0x200,
	Any = 0x3FF
}
