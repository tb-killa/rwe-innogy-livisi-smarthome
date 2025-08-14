using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace SmartHome.Common.API.ModelTransformationService;

public interface IConversionContext
{
	List<BaseDevice> Devices { get; }

	List<LogicalDevice> LogicalDevices { get; }
}
