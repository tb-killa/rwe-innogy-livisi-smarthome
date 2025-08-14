using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

internal interface ISensorHandlerEntityTypes : ISensorHandler, ILogicalDeviceHandler
{
	IEnumerable<Type> SupportedSensorTypes { get; }
}
