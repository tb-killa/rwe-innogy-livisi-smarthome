using System.Collections.Generic;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.Entities.ErrorHandling.Faults;

public class ConfigurationUpdateFault : FaultBase
{
	public ConfigurationUpdateFault()
	{
	}

	public ConfigurationUpdateFault(string errorType, List<Property> errorData)
	{
		base.ErrorType = errorType;
		base.ErrorData = errorData;
	}
}
