using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.API.Interfaces;

public interface IEntityCache
{
	Location GetLocation(Guid id);

	LogicalDevice GetLogicalDevice(Guid id);

	BaseDevice GetBaseDevice(Guid id);

	Interaction GetInteraction(Guid id);

	Home GetHome(Guid id);

	HomeSetup GetHomeSetup(Guid id);

	Member GetMember(Guid id);
}
