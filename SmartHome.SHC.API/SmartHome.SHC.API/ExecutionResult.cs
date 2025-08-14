using System.Collections.Generic;
using SmartHome.SHC.API.PropertyDefinition;

namespace SmartHome.SHC.API;

public struct ExecutionResult
{
	public ExecutionStatus Status;

	public List<Property> Details;
}
