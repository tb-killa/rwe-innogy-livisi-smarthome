using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.ModelTransformationService;

public interface IHomeConverterService
{
	SmartHome.Common.API.Entities.Entities.Home ToApiEntity(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Home shcHome);

	RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Home ToShcEntity(SmartHome.Common.API.Entities.Entities.Home apiHome);

	List<Property> ToApiState(HomeState state);
}
