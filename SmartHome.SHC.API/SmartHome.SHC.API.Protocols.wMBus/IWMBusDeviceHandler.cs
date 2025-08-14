using System;
using System.Collections.Generic;
using SmartHome.SHC.API.Configuration;

namespace SmartHome.SHC.API.Protocols.wMBus;

public interface IWMBusDeviceHandler : IAddIn
{
	List<WMBusDeviceTypeIdentifier> HandledDeviceTypes { get; }

	Device GetDevice(WMBusDeviceState installationRequest, out bool isRequestValid);

	List<Capability> GenerateCapabilities(Device device);

	TimeSpan GetSendingInterval(Guid deviceId);
}
