using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceInclusion;

public interface IDiscoveryController
{
	BaseDevice GetDiscoveredBaseDevice(Guid id);

	void StartDiscovery(List<string> appIds);

	void StopDiscovery();
}
