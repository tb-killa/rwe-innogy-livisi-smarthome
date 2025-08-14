using System;
using System.Collections.Generic;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

[Serializable]
public class ConfigurationUpdateException : BaseException
{
	public ConfigurationUpdateException(string errorType, List<Property> errorData)
	{
		base.ErrorType = errorType;
		base.ErrorData = errorData;
	}
}
