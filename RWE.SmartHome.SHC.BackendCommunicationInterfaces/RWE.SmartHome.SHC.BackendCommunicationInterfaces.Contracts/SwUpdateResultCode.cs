namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

public enum SwUpdateResultCode
{
	AlreadyLatestVersion,
	NewerVersionAvailable,
	Failure,
	NotAuthorized
}
