namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public enum EncryptedKeyResponseStatus
{
	Success,
	DeviceNotFound,
	UnexpectedException,
	BackendServiceNotReachable,
	InvalidTenant,
	Blacklisted
}
