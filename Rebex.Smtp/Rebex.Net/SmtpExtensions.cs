using System;

namespace Rebex.Net;

[Flags]
public enum SmtpExtensions : long
{
	Pipelining = 1L,
	Chunking = 2L,
	BinaryMime = 4L,
	EightBitMime = 8L,
	EnhancedTurn = 0x10000L,
	EnhancedStatusCodes = 0x20000L,
	DeliveryStatusNotifications = 0x40000L,
	MessageSizeDeclaration = 0x80000L,
	ExplicitSecurity = 0x100000L,
	All = 0x1F000FL
}
