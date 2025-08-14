using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceInclusion;

public interface ILogicalDeviceInclusionFactory
{
	IEnumerable<LogicalDevice> CreateLogicalDevices(BaseDevice newDevice);
}
