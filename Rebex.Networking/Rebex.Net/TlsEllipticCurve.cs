using System;

namespace Rebex.Net;

[Flags]
public enum TlsEllipticCurve : long
{
	None = 0L,
	NistP256 = 1L,
	NistP384 = 2L,
	NistP521 = 4L,
	BrainpoolP256R1 = 8L,
	BrainpoolP384R1 = 0x10L,
	BrainpoolP512R1 = 0x20L,
	Curve25519 = 0x40L,
	All = 0x7FFFFFFFFFL
}
