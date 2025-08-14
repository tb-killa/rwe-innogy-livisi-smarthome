using System;
using System.ComponentModel;
using onrkn;

namespace Rebex.Security.Certificates;

[Flags]
public enum ValidationStatus : long
{
	TimeNotValid = 1L,
	TimeNotNested = 2L,
	Revoked = 4L,
	SignatureNotValid = 8L,
	WrongUsage = 0x10L,
	RootNotTrusted = 0x20L,
	UnknownRev = 0x40L,
	CyclicChain = 0x80L,
	InvalidExtension = 0x100L,
	InvalidPolicyConstraints = 0x200L,
	InvalidBasicConstraints = 0x400L,
	InvalidNameConstraints = 0x800L,
	UnsupportedNameConstraint = 0x1000L,
	NotDefinedNameConstraint = 0x2000L,
	NotPermittedNameConstraint = 0x4000L,
	ExcludedNameConstraint = 0x8000L,
	IncompleteChain = 0x10000L,
	CtlTimeNotValid = 0x20000L,
	CtlSignatureNotValid = 0x40000L,
	CtlWrongUsage = 0x80000L,
	OfflineRev = 0x1000000L,
	NoIssuanceChainPolicy = 0x2000000L,
	UnsupportedSignatureAlgorithm = 0x80000000L,
	UnknownError = 0x100000000L,
	PathTooLong = 0x200000000L,
	UnknownCriticalExtension = 0x400000000L,
	InvalidChain = 0x800000000L,
	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	[Obsolete("This value is no longer used.", false)]
	UnknownCa = 0x1000000000L,
	CnNotMatch = 0x2000000000L,
	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This value is no longer used.", false)]
	CaNotTrusted = 0x4000000000L,
	ExplicitDistrust = 0x8000000000L,
	Malformed = 0x10000000000L,
	MoreErrors = 0x20000000000L,
	WeakAlgorithm = 0x40000000000L,
	NotTrusted = 0x80000000000L
}
