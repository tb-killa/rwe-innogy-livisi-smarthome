using System;

namespace Rebex.Security.Authentication;

[Flags]
public enum SspiRequirements
{
	Delegation = 1,
	MutualAuthentication = 2,
	Confidentiality = 0x10,
	Connection = 0x800,
	Integrity = 0x10000
}
