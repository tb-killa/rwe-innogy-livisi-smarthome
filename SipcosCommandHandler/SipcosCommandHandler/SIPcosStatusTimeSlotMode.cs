using System;

namespace SipcosCommandHandler;

[Flags]
public enum SIPcosStatusTimeSlotMode : byte
{
	VALUE_EXERCISE = 0x80,
	MODE_AUTO = 0x40,
	MODE_MANUAL = 0x20,
	MODE_WINDOW = 1
}
