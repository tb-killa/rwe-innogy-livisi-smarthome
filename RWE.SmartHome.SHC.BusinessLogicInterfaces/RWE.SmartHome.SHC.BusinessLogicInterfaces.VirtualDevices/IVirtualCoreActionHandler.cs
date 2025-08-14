using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.DomainModel.Actions;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;

public interface IVirtualCoreActionHandler
{
	void RequestState(Guid deviceId);

	ExecutionResult ExecuteAction(ActionContext context, ActionDescription action);
}
