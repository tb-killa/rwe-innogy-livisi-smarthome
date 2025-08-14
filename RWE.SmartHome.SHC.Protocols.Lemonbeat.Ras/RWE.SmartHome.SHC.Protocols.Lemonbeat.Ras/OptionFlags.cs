using System;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.Ras;

[Flags]
internal enum OptionFlags : uint
{
	UseCountryAndAreaCodes = 1u,
	SpecificIpAddr = 2u,
	SpecificNameServers = 4u,
	IpHeaderCompression = 8u,
	RemoteDefaultGateway = 0x10u,
	DisableLcpExtensions = 0x20u,
	TerminalBeforeDial = 0x40u,
	TerminalAfterDial = 0x80u,
	ModemLights = 0x100u,
	SwCompression = 0x200u,
	RequireEncryptedPw = 0x400u,
	RequireMsEncryptedPw = 0x800u,
	RequireDataEncryption = 0x1000u,
	NetworkLogon = 0x2000u,
	UseLogonCredentials = 0x4000u,
	PromoteAlternates = 0x8000u,
	SecureLocalFiles = 0x10000u,
	DialAsLocalCall = 0x20000u,
	ProhibitPAP = 0x40000u,
	ProhibitCHAP = 0x80000u,
	ProhibitMsCHAP = 0x100000u,
	ProhibitMsCHAP2 = 0x200000u,
	ProhibitEAP = 0x400000u,
	PreviewUserPw = 0x1000000u,
	NoUserPwRetryDialog = 0x2000000u,
	CustomScript = 0x80000000u
}
