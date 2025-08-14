namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;

public enum ErrorResponseType
{
	VersionMismatchError,
	AuthorizationError,
	AuthenticationError,
	SoftwareUpdateInProgress,
	GenericError,
	ConfigurationUpdateError,
	ConfigurationAccessError,
	ActionExecutionError
}
