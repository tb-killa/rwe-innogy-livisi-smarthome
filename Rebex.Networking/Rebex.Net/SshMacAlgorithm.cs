using System;

namespace Rebex.Net;

[Flags]
public enum SshMacAlgorithm : long
{
	None = 0L,
	MD5 = 1L,
	SHA1 = 2L,
	SHA256 = 4L,
	SHA512 = 8L,
	Any = 0xFL
}
