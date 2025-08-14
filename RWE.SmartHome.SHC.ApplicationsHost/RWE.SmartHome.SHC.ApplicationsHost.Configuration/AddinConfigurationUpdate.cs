using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.ApplicationsHost.Configuration;

internal class AddinConfigurationUpdate
{
	public List<BaseDevice> BaseDevices { get; set; }

	public List<LogicalDevice> LogicalDevices { get; set; }

	public List<Guid> DeleteBaseDevicesIds { get; set; }

	public List<Guid> DeleteLogicalDevicesIds { get; set; }

	public bool IsValid { get; set; }
}
