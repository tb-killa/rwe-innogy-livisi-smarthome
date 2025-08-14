using System;

namespace Rebex.Security.Certificates;

[Flags]
public enum KeyUses
{
	DigitalSignature = 0x80,
	NonRepudiation = 0x40,
	KeyEncipherment = 0x20,
	DataEncipherment = 0x10,
	KeyAgreement = 8,
	KeyCertSign = 4,
	CrlSign = 2,
	EncipherOnly = 1,
	DecipherOnly = 0x8000
}
