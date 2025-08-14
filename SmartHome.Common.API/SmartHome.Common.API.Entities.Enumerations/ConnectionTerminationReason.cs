namespace SmartHome.Common.API.Entities.Enumerations;

public enum ConnectionTerminationReason
{
	UserLoggedOut,
	SessionExpired,
	ConnectionLost,
	SoftwareUpdate,
	DeviceReboot,
	TooManyConnections
}
