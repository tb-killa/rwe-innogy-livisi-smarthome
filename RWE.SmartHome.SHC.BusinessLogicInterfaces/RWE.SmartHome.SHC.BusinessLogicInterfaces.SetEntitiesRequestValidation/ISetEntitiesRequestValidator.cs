using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;

public interface ISetEntitiesRequestValidator
{
	ValidationResult ValidateRequest(IEnumerable<Interaction> interactions, IEnumerable<Location> locations, IEnumerable<BaseDevice> baseDevices, IEnumerable<LogicalDevice> logicalDevices, IEnumerable<HomeSetup> homeSetups);
}
