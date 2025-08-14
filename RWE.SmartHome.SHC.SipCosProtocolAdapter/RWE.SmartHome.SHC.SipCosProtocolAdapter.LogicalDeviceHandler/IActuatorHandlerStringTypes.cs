using System.Collections.Generic;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

public interface IActuatorHandlerStringTypes : IActuatorHandler, ILogicalDeviceHandler
{
	IEnumerable<string> SupportedActuatorTypes { get; }
}
