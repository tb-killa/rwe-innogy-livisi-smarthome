using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

public static class Extensions
{
	public static BuiltinPhysicalDeviceType GetBuiltinDeviceDeviceType(this BaseDevice baseDevice)
	{
		if (baseDevice.AppId == CoreConstants.CoreAppId)
		{
			return (BuiltinPhysicalDeviceType)Enum.Parse(typeof(BuiltinPhysicalDeviceType), baseDevice.DeviceType, ignoreCase: false);
		}
		return BuiltinPhysicalDeviceType.Unknown;
	}

	public static void SetBuiltinDeviceType(this BaseDevice baseDevice, BuiltinPhysicalDeviceType deviceType)
	{
		baseDevice.DeviceType = deviceType.ToString();
	}
}
