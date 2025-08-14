namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.Ras;

internal enum ConnectionState
{
	OpenPort = 0,
	PortOpened = 1,
	ConnectDevice = 2,
	DeviceConnected = 3,
	AllDevicesConnected = 4,
	Authenticate = 5,
	AuthNotify = 6,
	AuthRetry = 7,
	AuthCallback = 8,
	AuthChangePassword = 9,
	AuthProject = 10,
	AuthLinkSpeed = 11,
	AuthAck = 12,
	ReAuthenticate = 13,
	Authenticated = 14,
	PrepareForCallback = 15,
	WaitForModemReset = 16,
	WaitForCallback = 17,
	Projected = 18,
	Interactive = 4096,
	RetryAuthentication = 4097,
	CallbackSetByCaller = 4098,
	PasswordExpired = 4099,
	Connected = 8192,
	Disconnected = 8193
}
