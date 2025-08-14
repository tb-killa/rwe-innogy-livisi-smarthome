using System.Collections.Generic;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

internal interface ISensorHandlerStringTypes : ISensorHandler, ILogicalDeviceHandler
{
	IEnumerable<string> SupportedSensorTypes { get; }
}
