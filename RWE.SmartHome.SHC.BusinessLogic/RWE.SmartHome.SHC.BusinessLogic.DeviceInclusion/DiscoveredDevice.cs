using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogic.DeviceInclusion;

internal class DiscoveredDevice
{
	public DateTime TimeOfDiscovery { get; private set; }

	public BaseDevice Device { get; private set; }

	public DiscoveredDevice(DateTime timeOfDiscovery, BaseDevice device)
	{
		TimeOfDiscovery = timeOfDiscovery;
		Device = device;
	}
}
