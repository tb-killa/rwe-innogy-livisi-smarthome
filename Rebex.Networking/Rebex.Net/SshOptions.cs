using System;

namespace Rebex.Net;

[Flags]
public enum SshOptions
{
	None = 0,
	DoNotSplitChannelPackets = 1,
	WaitForServerWelcomeMessage = 0x10000,
	TryPasswordFirst = 0x20000,
	PostponeChannelClose = 0x40000,
	EnableSignaturePadding = 0x80000,
	EnsureKeyAcceptable = 0x100000
}
