using System;
using System.Collections.Generic;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

[Serializable]
public class BaseException : Exception
{
	public string ErrorType { get; set; }

	public List<Property> ErrorData { get; set; }

	public BaseException()
	{
	}

	public BaseException(string message)
		: base(message)
	{
	}

	public BaseException(string errorType, List<Property> errorData)
	{
		ErrorType = errorType;
		ErrorData = errorData;
	}

	public BaseException(string message, string errorType, List<Property> errorData)
		: base(message)
	{
		ErrorType = errorType;
		ErrorData = errorData;
	}
}
