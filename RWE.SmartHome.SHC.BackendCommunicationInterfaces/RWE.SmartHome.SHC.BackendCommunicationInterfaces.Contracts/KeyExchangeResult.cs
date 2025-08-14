namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

public enum KeyExchangeResult
{
	Success,
	DeviceNotFound,
	UnexpectedException,
	InvalidTenant,
	DeviceBlacklisted,
	TooManyArguments
}
