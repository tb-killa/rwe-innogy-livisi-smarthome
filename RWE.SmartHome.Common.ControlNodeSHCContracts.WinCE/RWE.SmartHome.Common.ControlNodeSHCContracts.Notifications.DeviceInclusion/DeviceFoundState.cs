namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.DeviceInclusion;

public enum DeviceFoundState
{
	Unknown,
	ReadyForInclusion,
	NoDeviceKeyBackendUnreachable,
	NoDeviceKeyAvailable,
	AddressCollision,
	NoDeviceKeyDeviceBlacklisted,
	NoDeviceKeyInvalidTenant,
	MaximumNumberOfDevicesReached,
	AddInNotFound
}
