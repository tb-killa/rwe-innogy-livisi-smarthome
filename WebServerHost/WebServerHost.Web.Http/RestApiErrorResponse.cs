using System.Net;
using SmartHome.Common.API.Entities.ErrorHandling;
using WebServerHost.Web.Extensions;

namespace WebServerHost.Web.Http;

public class RestApiErrorResponse : ShcRestResponse
{
	public RestApiErrorResponse(ApiException apiError)
		: this(apiError.Error)
	{
	}

	public RestApiErrorResponse(Error error)
		: base(GetHttpCode((ErrorCode)error.ErrorCode), error.ToJson())
	{
	}

	private static HttpStatusCode GetHttpCode(ErrorCode errorCode)
	{
		switch (errorCode)
		{
		case ErrorCode.GeneralUnknown:
		case ErrorCode.InternalError:
		case ErrorCode.TemporaryUnavailable:
			return HttpStatusCode.InternalServerError;
		case ErrorCode.Timeout:
			return HttpStatusCode.RequestTimeout;
		case ErrorCode.ShcOperationError:
		case ErrorCode.InvalidArgument:
		case ErrorCode.UnsupportedRequest:
		case ErrorCode.AuthenticationGeneric:
		case ErrorCode.EntityMalformedContent:
		case ErrorCode.InvalidInteraction:
		case ErrorCode.InvalidParameter:
		case ErrorCode.ConfigurationUpdate:
			return HttpStatusCode.BadRequest;
		case ErrorCode.ServiceTooBusy:
			return HttpStatusCode.ServiceUnavailable;
		case ErrorCode.PreconditionFailed:
		case ErrorCode.LatestTaCVersionError:
			return HttpStatusCode.PreconditionFailed;
		case ErrorCode.AuthenticationAccessNotAllowed:
		case ErrorCode.InvalidTokenRequest:
		case ErrorCode.InvalidTenant:
		case ErrorCode.NoControllerAccess:
		case ErrorCode.NoPermissions:
		case ErrorCode.AccountTemporaryLocked:
		case ErrorCode.WebshopOnlyProduct:
		case ErrorCode.ProductRemovalDenied:
		case ErrorCode.ActionPermissionNotAllowed:
		case ErrorCode.RestoreConfigurationForMigrationsNotAllowed:
			return HttpStatusCode.Forbidden;
		case ErrorCode.InvalidClientCredentials:
		case ErrorCode.TokenSignatureInvalid:
		case ErrorCode.FailedToInitializeUserSession:
		case ErrorCode.TokenExpired:
		case ErrorCode.InvalidUserCredentials:
		case ErrorCode.InvalidPartner:
		case ErrorCode.UserHasNoShc:
			return HttpStatusCode.Unauthorized;
		case ErrorCode.ConnectionAlreadyInitialized:
		case ErrorCode.EntityAlreadyExists:
		case ErrorCode.ConfigurationExclusiveAccess:
		case ErrorCode.OneShcAlreadyRegistered:
		case ErrorCode.HardwareTypeMismatch:
			return HttpStatusCode.Conflict;
		case ErrorCode.SessionNotFound:
			return (HttpStatusCode)424;
		case ErrorCode.EntityDoesNotExist:
		case ErrorCode.InvalidAction:
		case ErrorCode.ActionUnsupported:
		case ErrorCode.DeviceCommunicationError:
		case ErrorCode.ControllerOffline:
		case ErrorCode.RegistrationFailure:
		case ErrorCode.VhscInstanceNotAvailable:
			return HttpStatusCode.NotFound;
		case ErrorCode.NoChangePerformed:
			return HttpStatusCode.NotModified;
		default:
			return HttpStatusCode.InternalServerError;
		}
	}
}
