using System;

namespace SipcosCommandHandler;

[Flags]
public enum DeviceInfoInclusionModes : byte
{
	ProtectedSecurityUsingSIPcosRSA = 1,
	ProtectedSecurityUsingSIPcosSymmetricalKeyExchange = 2,
	SecurityUsingDefaulKey = 4,
	MACSecuritydisabled = 8,
	AdHoc = 0x10
}
