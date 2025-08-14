using System;

namespace Org.Mentalis.Security.Ssl;

[Flags]
public enum SecurityFlags
{
	Default = 0,
	MutualAuthentication = 1,
	DontSendEmptyRecord = 2,
	IgnoreMaxProtocol = 4
}
