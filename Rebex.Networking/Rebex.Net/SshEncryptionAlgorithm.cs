using System;

namespace Rebex.Net;

[Flags]
public enum SshEncryptionAlgorithm : long
{
	None = 0L,
	RC4 = 1L,
	TripleDES = 2L,
	AES = 4L,
	Blowfish = 8L,
	Twofish = 0x10L,
	Chacha20Poly1305 = 0x20L,
	Any = 0x3FL
}
