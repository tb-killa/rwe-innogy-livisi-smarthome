namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.ErrorHandling;

internal enum ErrorCode
{
	TransactionScopeAlreadyInUse,
	NoValidTransaction,
	UnexpectedErrorOccurred,
	CommandNotSupported,
	RecipientUnknown,
	SoftwareUpdateInProgress,
	InvalidRequest
}
