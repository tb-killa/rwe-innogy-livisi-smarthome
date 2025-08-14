using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.ModelTransformationService.Extensions;

public static class BaseDeviceExtensions
{
	public static bool IsCoreDevice(this Device device)
	{
		return CoreConstants.CoreAppId.Contains(device.Product);
	}

	public static bool IsCoreDevice(this BaseDevice baseDevice)
	{
		return baseDevice.AppId.Contains(CoreConstants.CoreAppId);
	}
}
