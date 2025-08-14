namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender;

public enum NotificationSendResult
{
	Success,
	Unauthorized,
	UnexpectedFailure,
	NoContingentProvisioned,
	InvalidDestination
}
