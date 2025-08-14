namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging;

public enum ActivityType
{
	LogicalDeviceStateChanged = 0,
	ShcStarted = 1,
	ShcConnectivityStateChanged = 2,
	ShcSoftwareUpdate = 3,
	ShcFactoryReset = 4,
	ShcMemoryLoadExceeded = 5,
	ShcMemoryLoadOK = 6,
	ShcRightButtonPressed = 7,
	ShcLemonbeatDongleFailure = 8,
	ShcWMBusDongleFailure = 9,
	ShcUSBDeviceRemoved = 10,
	ShcUSBDevicePlugged = 11,
	ShcUserLogin = 1000,
	ShcUserLogout = 1001,
	ShcLoginFailed = 1002,
	ShcAppTokenFailure = 2000,
	ShcDeviceUnReachable = 2001,
	ShcDeviceReachable = 2002,
	DeviceIncluded = 2003,
	DeviceExcluded = 2004,
	DeviceActive = 2005,
	LocalCommunicationStatus = 2006
}
