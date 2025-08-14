using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using SmartHome.SHC.API.DeviceActivityLogging;

namespace RWE.SmartHome.SHC.DeviceActivity.DeviceActivity;

public interface IDalTypeResolver
{
	DeviceActivityLoggingType GetDeviceLoggingType(LogicalDevice logicalDeviceId);
}
