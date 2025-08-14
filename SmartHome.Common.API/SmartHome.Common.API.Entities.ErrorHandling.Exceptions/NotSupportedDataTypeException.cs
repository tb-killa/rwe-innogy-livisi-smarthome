using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

public class NotSupportedDataTypeException : Exception
{
	public NotSupportedDataTypeException()
	{
	}

	public NotSupportedDataTypeException(string message)
		: base(message)
	{
	}
}
