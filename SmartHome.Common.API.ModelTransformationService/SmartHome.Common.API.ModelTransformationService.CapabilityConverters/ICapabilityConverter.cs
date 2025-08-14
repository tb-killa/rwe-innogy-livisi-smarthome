using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.ModelTransformationService.CapabilityConverters;

internal interface ICapabilityConverter
{
	Capability FromSmartHomeLogicalDevice(LogicalDevice logicalDevice, IConversionContext context);

	LogicalDevice ToSmartHomeLogicalDevice(Capability aCapability);

	List<Property> FromSmartHomeLogicalDeviceState(LogicalDeviceState state, IConversionContext context);
}
