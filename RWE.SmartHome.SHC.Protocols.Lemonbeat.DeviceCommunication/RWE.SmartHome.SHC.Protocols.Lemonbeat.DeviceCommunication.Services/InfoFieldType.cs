namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;

public enum InfoFieldType : uint
{
	DeviceType = 1u,
	ManufacturerCode,
	Sgtin,
	Mac,
	HardwareVersion,
	BootloaderVersion,
	StackVersion,
	ApplicationVersion,
	Protocol,
	Product,
	IncludedFlag,
	Name,
	RadioMode,
	WakeupInterval,
	WakeupOffset,
	WakeupChannel,
	ChannelMap,
	ChannelScanTime,
	IPV6Address
}
