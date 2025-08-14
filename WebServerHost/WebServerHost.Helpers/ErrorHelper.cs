using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.ErrorHandling;
using SmartHome.Common.API.Entities.Extensions;

namespace WebServerHost.Helpers;

internal static class ErrorHelper
{
	public static Error GetError(ErrorResponse errorResponse)
	{
		ErrorCode errorCode = GetErrorCode(errorResponse);
		Error error = new Error();
		error.ErrorCode = (int)errorCode;
		error.Description = errorCode.GetDescription();
		error.Messages = new List<string> { errorResponse.ErrorType.ToString() };
		Error error2 = error;
		if (errorResponse.Data != null)
		{
			error2.Data = errorResponse.Data.Select((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property p) => new SmartHome.Common.API.Entities.Entities.Property
			{
				Name = p.Name,
				Value = p.GetValueAsString()
			}).ToList();
		}
		return error2;
	}

	public static ErrorCode GetErrorCode(ErrorResponse errorResponse)
	{
		switch (errorResponse.ErrorType)
		{
		case ErrorResponseType.ActionExecutionError:
			return ErrorCode.ActionUnsupported;
		case ErrorResponseType.AuthorizationError:
		case ErrorResponseType.AuthenticationError:
			return ErrorCode.AuthenticationGeneric;
		case ErrorResponseType.ConfigurationAccessError:
			return ErrorCode.ConfigurationExclusiveAccess;
		case ErrorResponseType.ConfigurationUpdateError:
			return ErrorCode.ConfigurationUpdate;
		case ErrorResponseType.VersionMismatchError:
		case ErrorResponseType.SoftwareUpdateInProgress:
		case ErrorResponseType.GenericError:
			return ErrorCode.GeneralUnknown;
		default:
			return ErrorCode.InternalError;
		}
	}
}
