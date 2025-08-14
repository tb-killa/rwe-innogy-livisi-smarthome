using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.ModelTransformationService;

public interface ICapabilityConverterService
{
	Capability FromSmartHomeLogicalDevice(LogicalDevice logicalDevice);

	LogicalDevice ToSmartHomeLogicalDevice(Capability aCapability);

	List<Property> FromSmartHomeLogicalDeviceState(LogicalDeviceState state);
}
