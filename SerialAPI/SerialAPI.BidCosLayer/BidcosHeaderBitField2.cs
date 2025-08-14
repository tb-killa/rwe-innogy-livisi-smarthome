using System;

namespace SerialAPI.BidCosLayer;

[Flags]
internal enum BidcosHeaderBitField2 : byte
{
	WakeUp = 1,
	WakeMeUp = 2,
	Broadcast = 4,
	Burst = 0x10,
	Bidi = 0x20,
	Repeated = 0x40,
	RepeatEnable = 0x80
}
