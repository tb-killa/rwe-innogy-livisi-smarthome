using System.Collections.Generic;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.Entities.ErrorHandling.Faults;

public class ConfigurationExclusiveAccessFault : FaultBase
{
	public ConfigurationExclusiveAccessFault()
	{
	}

	public ConfigurationExclusiveAccessFault(string errorType, List<Property> errorData)
	{
		base.ErrorType = errorType;
		base.ErrorData = errorData;
	}
}
