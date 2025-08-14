using System;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;

public static class ShcBaseDeviceIRepositoryExtensions
{
	public static Func<BaseDevice, bool> ShcBaseDevicesPredicate => (BaseDevice x) => x.Manufacturer == "RWE" && x.DeviceType == BuiltinPhysicalDeviceType.SHC.ToString();

	public static BaseDevice GetShcBaseDevice(this IRepository configurationRepository)
	{
		return configurationRepository.GetBaseDevices().FirstOrDefault(ShcBaseDevicesPredicate);
	}

	public static Guid GetShcBaseDeviceId(this IRepository configurationRepository)
	{
		Guid result = Guid.Empty;
		BaseDevice shcBaseDevice = configurationRepository.GetShcBaseDevice();
		if (shcBaseDevice != null)
		{
			result = shcBaseDevice.Id;
		}
		return result;
	}
}
