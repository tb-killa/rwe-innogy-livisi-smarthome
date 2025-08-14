using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.DomainModel.Actions;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

public interface IActuatorHandler : ILogicalDeviceHandler
{
	IEnumerable<byte> StatusInfoChannels { get; }

	bool IsStatusRequestAllowed { get; }

	int MinStatusRequestPollingIterval { get; }

	IEnumerable<SwitchSettings> CreateCosIpCommand(ActionContext ac, ActionDescription action);

	bool GetIsPeriodicStatusPollingActive(LogicalDevice logicalDevice);

	bool CanExecuteAction(ActionDescription action);
}
