using System;

namespace Rebex.Security.Certificates;

[Flags]
public enum ValidationOptions
{
	None = 0,
	IgnoreTimeNotValid = 1,
	IgnoreCtlTimeNotValid = 2,
	IgnoreTimeNotNested = 4,
	IgnoreAllTimeNotValid = 7,
	IgnoreInvalidBasicConstraints = 8,
	AllowUnknownCa = 0x10,
	IgnoreWrongUsage = 0x20,
	IgnoreInvalidPolicy = 0x80,
	IgnoreAllRevUnknown = 0xF00,
	UseCacheOnly = 0x10000000,
	IgnoreCnNotMatch = 0x20000000,
	SkipRevocationCheck = 0x40000000,
	IgnoreInvalidChain = 0x8000000
}
