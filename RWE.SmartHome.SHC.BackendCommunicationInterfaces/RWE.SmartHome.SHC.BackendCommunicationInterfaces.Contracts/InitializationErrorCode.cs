namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

public enum InitializationErrorCode
{
	Success,
	Failure,
	InvalidRegistrationProcessStatus,
	InvalidPin,
	ShcNotSold,
	InvalidRegistrationToken,
	RegistrationProcessExpired,
	NotAuthorized
}
