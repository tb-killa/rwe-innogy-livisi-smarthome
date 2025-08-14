using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Control;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.DomainModel.Actions;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;

public interface IDeviceController
{
	void RegisterProtocolSpecificDeviceController(ProtocolIdentifier protocolIdentifier, IProtocolSpecificDeviceController deviceController);

	ControlResult TriggerSensorRequest(ActionContext context, Guid sensorId, int buttonId);

	ExecutionResult ExecuteAction(ActionContext context, ActionDescription action);
}
