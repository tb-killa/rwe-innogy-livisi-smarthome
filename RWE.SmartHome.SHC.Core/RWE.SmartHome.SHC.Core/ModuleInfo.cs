using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging;

namespace RWE.SmartHome.SHC.Core;

internal class ModuleInfo
{
	public Module Identifier { get; set; }

	public IModule Instance { get; set; }

	public Severity LogLevel { get; set; }
}
