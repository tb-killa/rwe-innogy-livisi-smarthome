using System;

namespace SmartHome.SHC.API.Protocols.Lemonbeat;

public interface IPhysicalDeviceProvider
{
	LemonbeatPhysicalDevice GetPhysicalDevice(Guid deviceId);
}
