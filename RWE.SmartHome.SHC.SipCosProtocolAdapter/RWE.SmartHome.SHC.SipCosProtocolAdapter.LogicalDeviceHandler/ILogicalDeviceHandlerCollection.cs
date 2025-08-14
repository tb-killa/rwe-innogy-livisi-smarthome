using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

internal interface ILogicalDeviceHandlerCollection
{
	ILogicalDeviceHandler GetLogicalDeviceHandler(LogicalDevice logicalDevice);

	ILogicalDeviceHandler GetAlarActuatorHandler();

	ILogicalDeviceHandler GetSirenAlarmActuator();
}
