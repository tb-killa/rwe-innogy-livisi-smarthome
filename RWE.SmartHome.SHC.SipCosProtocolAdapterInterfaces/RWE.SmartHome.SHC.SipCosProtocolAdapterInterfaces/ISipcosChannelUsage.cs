using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

public interface ISipcosChannelUsage
{
	IEnumerable<int> GetUsedDeviceFunctions(Trigger trigger);

	IEnumerable<int> GetUsedDeviceFunctions(ActionDescription action);

	int? GetMaxTimePointsPerWeekday(Guid logicalDeviceId);
}
