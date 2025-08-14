using System;
using System.Collections.Generic;
using SmartHome.SHC.API.Configuration;
using SmartHome.SHC.API.Configuration.Services;

namespace SmartHome.SHC.API.Protocols.CustomProtocol;

public interface ICustomProtocolDeviceHandler
{
	event EventHandler<DeviceFoundEventArgs> DeviceFoundEvent;

	List<Capability> GenerateCapabilities(Device device);

	void SetDiscoveryMode(DiscoveryMode discoveryMode);

	void DropDiscoveredDevice(Guid deviceId);
}
