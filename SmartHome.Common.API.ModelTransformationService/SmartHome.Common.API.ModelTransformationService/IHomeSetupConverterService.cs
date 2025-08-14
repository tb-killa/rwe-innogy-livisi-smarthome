using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.ModelTransformationService;

public interface IHomeSetupConverterService
{
	SmartHome.Common.API.Entities.Entities.HomeSetup ToApiEntity(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.HomeSetup shcHomeSetup);

	RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.HomeSetup ToShcEntity(SmartHome.Common.API.Entities.Entities.HomeSetup apiHomeSetup);
}
