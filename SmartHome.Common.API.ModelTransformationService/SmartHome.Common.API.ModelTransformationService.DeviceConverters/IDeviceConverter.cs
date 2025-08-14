using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.ModelTransformationService.DeviceConverters;

internal interface IDeviceConverter
{
	Device FromSmartHomeBaseDevice(BaseDevice baseDevice, bool includeCapabilities);

	BaseDevice ToSmartHomeBaseDevice(Device aDevice);

	List<Property> FromSmartHomeDeviceState(PhysicalDeviceState state);
}
