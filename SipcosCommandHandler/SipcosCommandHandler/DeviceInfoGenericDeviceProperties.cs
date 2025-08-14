using System;

namespace SipcosCommandHandler;

[Flags]
public enum DeviceInfoGenericDeviceProperties : short
{
	BinaryActuator = 1,
	DiscreteActuator = 2,
	BinarySensor = 4,
	DiscreteSensor = 8,
	LogicController = 0x10,
	Gateway = 0x20,
	Logger = 0x40,
	Configurator = 0x80,
	RemoteControlWithKeys = 0x100,
	RemoteControlWithDiscreteInput = 0x200,
	Indoor = 0x400,
	Outdoor = 0x800
}
