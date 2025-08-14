using System;

namespace Org.Mentalis.Security.Certificates;

[Flags]
public enum VerificationFlags
{
	None = 0,
	IgnoreTimeNotValid = 1,
	IgnoreCtlTimeNotValid = 2,
	IgnoreTimeNotNested = 4,
	IgnoreInvalidBasicContraints = 8,
	IgnoreAllTimeChecks = 7,
	AllowUnknownCA = 0x10,
	IgnoreWrongUsage = 0x20,
	IgnoreInvalidName = 0x40,
	IgnoreInvalidPolicy = 0x80,
	IgnoreEndRevUnknown = 0x100,
	IgnoreSignerRevUnknown = 0x200,
	IgnoreCARevUnknown = 0x400,
	IgnoreRootRevUnknown = 0x800,
	IgnoreAllRevUnknown = 0xF00,
	AllowTestroot = 0x8000,
	TrustTestroot = 0x4000
}
