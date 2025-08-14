using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.SHC.BusinessLogic.ConfigurationTransformation;

internal interface IEntityRelationsGraphConfigurationProxy
{
	List<Interaction> GetInteractions();

	BaseDevice GetBaseDevice(Guid id);

	LogicalDevice GetLogicalDevice(Guid id);
}
