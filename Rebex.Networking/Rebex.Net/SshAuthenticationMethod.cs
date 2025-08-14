using System;

namespace Rebex.Net;

[Flags]
public enum SshAuthenticationMethod
{
	None = 0,
	Password = 1,
	KeyboardInteractive = 2,
	PublicKey = 4,
	GssapiWithMic = 8,
	Any = 0xF
}
