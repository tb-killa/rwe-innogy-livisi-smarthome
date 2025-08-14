using SmartHome.Common.API.Entities.Extensions;

namespace SmartHome.Common.API.Entities.ErrorHandling;

public enum ErrorCode
{
	[EnumDescription("An unknown error has occurred")]
	GeneralUnknown = 1000,
	[EnumDescription("One of the services is unavailable or an unhandled error occurred in the service")]
	ServiceUnavailable = 1001,
	[EnumDescription("One of the services timed out")]
	Timeout = 1002,
	[EnumDescription("Internal API error")]
	InternalError = 1003,
	[EnumDescription("SHC side exception or error")]
	ShcOperationError = 1004,
	[EnumDescription("One of the request parameters value is invalid or has the wrong value")]
	InvalidArgument = 1005,
	[EnumDescription("The service is too busy and can't handle requests right now")]
	ServiceTooBusy = 1006,
	[EnumDescription("The request is not valid")]
	UnsupportedRequest = 1007,
	[EnumDescription("A precondition for the given operation was not met. The client may retry the operation after 3-5 minutes")]
	PreconditionFailed = 1008,
	[EnumDescription("The checksum for add-in failed")]
	AddinChecksumFailed = 1009,
	[EnumDescription("Unknown error occurred during Authentication or Authorization")]
	AuthenticationGeneric = 2000,
	[EnumDescription("No Ownership for SHC with given serial number ")]
	AuthenticationAccessNotAllowed = 2001,
	[EnumDescription("The token request is invalid - malformed or wrong parameter values")]
	InvalidTokenRequest = 2002,
	[EnumDescription("The client ID or password are invalid")]
	InvalidClientCredentials = 2003,
	[EnumDescription("The token signature is invalid - it was not issued by the proper authority")]
	TokenSignatureInvalid = 2004,
	[EnumDescription("User session initialization failed")]
	FailedToInitializeUserSession = 2005,
	[EnumDescription("Another connection with the same token was initialized for the SHC")]
	ConnectionAlreadyInitialized = 2006,
	[EnumDescription("The lifetime of the token has expired. A refresh token request should be sent")]
	TokenExpired = 2007,
	[EnumDescription("The combination client - username doesn’t match; the username tried to login using a client from another tenant (provider)")]
	InvalidTenant = 2008,
	[EnumDescription("The username or password provided for token generation are invalid")]
	InvalidUserCredentials = 2009,
	[EnumDescription("Access is is not allowed in the system")]
	NoControllerAccess = 2010,
	[EnumDescription("Client or user does not have the appropriate permissions to execute the requested operation")]
	NoPermissions = 2011,
	[EnumDescription("Session not initialized or disconnected")]
	SessionNotFound = 2012,
	[EnumDescription("This account is currently locked. Please try again in X minutes")]
	AccountTemporaryLocked = 2013,
	InvalidPartner = 2014,
	[EnumDescription("The requested entity does not exist")]
	EntityDoesNotExist = 3000,
	[EnumDescription("The provided content is mall formatted and can’t be parsed (JSON)")]
	EntityMalformedContent = 3001,
	[EnumDescription("The performed change for entities contains no value")]
	NoChangePerformed = 3002,
	[EnumDescription("The provided entity already exists")]
	EntityAlreadyExists = 3003,
	[EnumDescription("The provided interaction is not valid")]
	InvalidInteraction = 3004,
	[EnumDescription("Premium Services can be enabled only through the Webshop")]
	WebshopOnlyProduct = 3500,
	[EnumDescription("Paid products (by webshop/smart codes) cannot be removed")]
	ProductRemovalDenied = 3501,
	[EnumDescription("The triggered action is invalid")]
	InvalidAction = 4000,
	[EnumDescription("One of the action parameters is invalid")]
	InvalidParameter = 4001,
	[EnumDescription("The user does not have the appropriate permissions to the trigger the action")]
	ActionPermissionNotAllowed = 4002,
	[EnumDescription("The triggered action type is unsupported")]
	ActionUnsupported = 4003,
	[EnumDescription("There was an error updating the configuration")]
	ConfigurationUpdate = 5000,
	[EnumDescription("The configuration is locked by another process")]
	ConfigurationExclusiveAccess = 5001,
	[EnumDescription("Communication with the SHC failed")]
	DeviceCommunicationError = 5002,
	[EnumDescription("Latest version of terms and conditions and data privacy policy was not accepted by the user")]
	LatestTaCVersionError = 5003,
	[EnumDescription("Only one SHC can be registered per user")]
	OneShcAlreadyRegistered = 5004,
	[EnumDescription("The user has no registered SHC device")]
	UserHasNoShc = 5005,
	[EnumDescription("The SHC is offline")]
	ControllerOffline = 5006,
	RestoreConfigurationForMigrationsNotAllowed = 5007,
	HardwareTypeMismatch = 5008,
	[EnumDescription("The registration process failed")]
	RegistrationFailure = 5009,
	VhscInstanceNotAvailable = 5010,
	[EnumDescription("Request not allowed or too many tries. Try again in 60 minutes")]
	SmartCodeRequestNotAllowed = 6000,
	[EnumDescription("Request not allowed. The requested smart code cannot be redeemed because it was already redeemed")]
	SmartCodeCannotBeRedeemed = 6001,
	[EnumDescription("Access restricted because too many invalid smart code pins were provided")]
	SmartCodeRestrictedAccess = 6002,
	[EnumDescription("The service is temporarely unavailable")]
	TemporaryUnavailable = 7000
}
