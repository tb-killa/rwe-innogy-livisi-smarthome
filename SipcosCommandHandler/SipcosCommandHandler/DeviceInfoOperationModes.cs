using System;

namespace SipcosCommandHandler;

[Flags]
public enum DeviceInfoOperationModes : byte
{
	MainsPowered = 1,
	EventListener = 2,
	BurstListener = 4,
	TripleBurstListener = 8,
	CyclicListener = 0x10,
	Router = 0x20,
	NetworkCoordinator = 0x40
}
