using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.DomainModel.Actions;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;

public interface IProtocolSpecificDeviceController
{
	List<Property> CreateTriggerEvent(LogicalDevice logicalDevice, int buttonId);

	ExecutionResult ExecuteAction(ActionContext context, ActionDescription action);
}
