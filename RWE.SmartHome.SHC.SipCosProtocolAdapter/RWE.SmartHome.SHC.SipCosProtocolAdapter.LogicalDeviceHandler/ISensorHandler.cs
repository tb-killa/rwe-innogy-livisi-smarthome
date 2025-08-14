using System.Collections.Generic;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

internal interface ISensorHandler : ILogicalDeviceHandler
{
	bool IsStatusRequestAllowed { get; }

	IEnumerable<byte> StatusInfoChannels { get; }
}
