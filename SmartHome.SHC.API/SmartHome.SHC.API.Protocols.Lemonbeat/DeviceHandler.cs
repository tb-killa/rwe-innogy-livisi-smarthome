using System;
using System.Collections.Generic;
using SmartHome.SHC.API.Configuration;

namespace SmartHome.SHC.API.Protocols.Lemonbeat;

public abstract class DeviceHandler : IDeviceHandler
{
	public abstract List<LemonbeatDeviceTypeIdentifier> HandledDeviceTypes { get; }

	public abstract LemonbeatPhysicalDeviceDescription TranslateDeviceDescription(PhysicalDeviceDescription incommingDeviceDescription);

	public abstract TimeSpan? GetReachabilityPollingInterval(Guid deviceId);

	public void SetLemonbeatCoreServices(ILemonbeatCoreServices LemonbeatServices)
	{
	}

	public abstract List<Capability> GenerateCapabilities(Device device);
}
