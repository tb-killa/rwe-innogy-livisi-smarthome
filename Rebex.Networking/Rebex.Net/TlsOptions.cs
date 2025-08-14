using System;

namespace Rebex.Net;

[Flags]
public enum TlsOptions
{
	None = 0,
	DoNotCacheSessions = 1,
	StayConnected = 2,
	DoNotInsertEmptyFragment = 0x100,
	SkipRollbackDetection = 0x200,
	SilentUnprotect = 0x400,
	Reserved = 0x800,
	AllowCloseWhileNegotiating = 0x1000,
	SilentClose = 0x2000,
	DisableRenegotiationExtension = 0x8000,
	DisableServerNameIndication = 0x10000,
	DisableExtendedMasterSecret = 0x20000
}
