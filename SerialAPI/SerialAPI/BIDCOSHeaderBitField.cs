using System;

namespace SerialAPI;

[Flags]
internal enum BIDCOSHeaderBitField : byte
{
	WakeUp = 1,
	WakeMeUp = 2,
	Broadcast = 4,
	Burst = 0x10,
	Bidi = 0x20,
	Repeated = 0x40,
	RepeatEnable = 0x80
}
