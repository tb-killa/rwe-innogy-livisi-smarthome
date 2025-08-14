using System;

namespace Org.Mentalis.Security.Certificates;

[Flags]
public enum KeyUsage
{
	DataEncipherment = 0x10,
	DigitalSignature = 0x80,
	KeyAgreement = 8,
	KeyCertSign = 4,
	KeyEncipherment = 0x20,
	NonRepudiation = 0x40,
	CrlSign = 2
}
