namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate;

public enum DeviceUpdateResultCode
{
	AlreadyLatestVersion,
	NewerVersionAvailable,
	Failure,
	NotAuthorized
}
