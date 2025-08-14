using System;
using System.Collections.Generic;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

[Serializable]
public class ShcErrorResponseException : BaseException
{
	public ShcErrorResponseException()
	{
	}

	public ShcErrorResponseException(string errorType, List<Property> errorData)
		: base(errorType, errorData)
	{
	}

	public ShcErrorResponseException(string message, string errorType, List<Property> errorData)
		: base(message, errorType, errorData)
	{
	}
}
