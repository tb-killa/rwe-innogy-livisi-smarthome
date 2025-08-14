using SmartHome.SHC.API.PropertyDefinition;

namespace SmartHome.SHC.API.Configuration;

public class ExecutionContext
{
	public Property[] Details;

	public ExecutionSource Source { get; set; }
}
