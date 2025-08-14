using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SmartHome.Common.API.Entities.Extensions;

namespace SmartHome.Common.API.Entities.ErrorHandling;

public class ApiException : Exception
{
	public Error Error { get; private set; }

	public ApiException(ErrorCode errorCode, params string[] messages)
		: this(CreateErrorModel(errorCode, messages))
	{
	}

	public ApiException(HttpStatusCode errorCode, params string[] messages)
		: this(CreateErrorModel(errorCode, messages))
	{
	}

	public static Error CreateErrorModel(ErrorCode errorCode, params string[] messages)
	{
		Error error = new Error();
		error.ErrorCode = (int)errorCode;
		error.Description = errorCode.GetDescription();
		error.Messages = new List<string>(messages);
		return error;
	}

	public static Error CreateErrorModel(HttpStatusCode errorCode, params string[] messages)
	{
		Error error = new Error();
		error.ErrorCode = (int)errorCode;
		error.Description = errorCode.GetDescription();
		error.Messages = new List<string>(messages);
		return error;
	}

	public ApiException(Error error)
		: base(error.Messages.Any() ? error.Messages[0] : error.Description)
	{
		Error = error;
	}
}
