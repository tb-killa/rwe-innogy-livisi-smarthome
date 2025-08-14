using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.ModelTransformationService;

public interface ILocationConverterService
{
	SmartHome.Common.API.Entities.Entities.Location FromSmartHomeLocation(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Location location);

	RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Location ToSmartHomeLocation(SmartHome.Common.API.Entities.Entities.Location location);
}
