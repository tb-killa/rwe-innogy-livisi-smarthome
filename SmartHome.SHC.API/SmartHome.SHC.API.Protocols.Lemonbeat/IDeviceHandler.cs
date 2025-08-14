using System;
using System.Collections.Generic;
using SmartHome.SHC.API.Configuration;

namespace SmartHome.SHC.API.Protocols.Lemonbeat;

public interface IDeviceHandler
{
	List<LemonbeatDeviceTypeIdentifier> HandledDeviceTypes { get; }

	LemonbeatPhysicalDeviceDescription TranslateDeviceDescription(PhysicalDeviceDescription incommingDeviceDescription);

	TimeSpan? GetReachabilityPollingInterval(Guid deviceId);

	void SetLemonbeatCoreServices(ILemonbeatCoreServices LemonbeatServices);

	List<Capability> GenerateCapabilities(Device device);
}
