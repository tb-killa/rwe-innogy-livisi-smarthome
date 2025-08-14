using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;

public class Router : LogicalDevice
{
	protected override Entity CreateClone()
	{
		return new Router();
	}
}
