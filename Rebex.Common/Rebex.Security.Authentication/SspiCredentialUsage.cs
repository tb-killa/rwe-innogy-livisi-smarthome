using System;

namespace Rebex.Security.Authentication;

[Flags]
public enum SspiCredentialUsage
{
	Inbound = 1,
	Outbound = 2
}
