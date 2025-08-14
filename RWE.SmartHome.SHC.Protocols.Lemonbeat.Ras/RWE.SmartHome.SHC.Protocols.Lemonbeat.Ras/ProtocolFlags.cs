using System;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.Ras;

[Flags]
public enum ProtocolFlags : uint
{
	IP = 4u,
	IPv6 = 8u
}
