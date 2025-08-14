using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.ModelTransformationService;

public interface IMemberConverterService
{
	SmartHome.Common.API.Entities.Entities.Member ToApiEntity(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Member shcMember);

	RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Member ToShcEntity(SmartHome.Common.API.Entities.Entities.Member apiMember);

	List<Property> ToApiState(MemberState state);
}
