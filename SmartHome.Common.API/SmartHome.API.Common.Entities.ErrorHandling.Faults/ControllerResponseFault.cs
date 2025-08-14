using System.Collections.Generic;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.ErrorHandling.Faults;

namespace SmartHome.API.Common.Entities.ErrorHandling.Faults;

public class ControllerResponseFault : FaultBase
{
	public ControllerResponseFault()
	{
	}

	public ControllerResponseFault(string errorType, List<Property> errorData)
	{
		base.ErrorType = errorType;
		base.ErrorData = errorData;
	}
}
