using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

public interface IActuatorHandlerEntityTypes : IActuatorHandler, ILogicalDeviceHandler
{
	IEnumerable<Type> SupportedActuatorTypes { get; }
}
