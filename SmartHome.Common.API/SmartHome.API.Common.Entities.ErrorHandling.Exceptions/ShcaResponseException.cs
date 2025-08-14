using System;
using System.Collections.Generic;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

namespace SmartHome.API.Common.Entities.ErrorHandling.Exceptions;

[Serializable]
public class ShcaResponseException : BaseException
{
	public ShcaResponseException()
	{
	}

	public ShcaResponseException(string errorType, List<Property> errorData)
		: base(errorType, errorData)
	{
	}

	public ShcaResponseException(string message, string errorType, List<Property> errorData)
		: base(message, errorType, errorData)
	{
	}
}
