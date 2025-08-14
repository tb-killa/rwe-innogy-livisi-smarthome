using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration.DevicesDefinitions;

public interface IDeviceDefinitionsProvider
{
	BaseDeviceDefinition GetDeviceDefinition(string appId, string deviceType, FirmwareVersion firmwareVersion);

	IEnumerable<LogicalDeviceDefinition> GetLogicalDeviceDefinition(BaseDevice physicalDevice);
}
