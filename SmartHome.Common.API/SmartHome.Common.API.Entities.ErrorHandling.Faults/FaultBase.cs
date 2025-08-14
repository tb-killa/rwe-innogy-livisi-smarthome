using System.Collections.Generic;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.Entities.ErrorHandling.Faults;

public class FaultBase
{
	public string Message { get; set; }

	public string ErrorType { get; set; }

	public List<Property> ErrorData { get; set; }

	public override string ToString()
	{
		return Message ?? string.Empty;
	}
}
