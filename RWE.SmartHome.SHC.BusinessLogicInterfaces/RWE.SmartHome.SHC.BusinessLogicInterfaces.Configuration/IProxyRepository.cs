using RWE.SmartHome.Common.ControlNodeSHCContracts.API.Interfaces;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;

public interface IProxyRepository : IRepository, IEntityCache
{
	void UpdateBaseDevice(BaseDevice baseDevice);

	void UpdateLogicalDevice(LogicalDevice logicalDevice);

	void UpdateLocation(Location location);
}
