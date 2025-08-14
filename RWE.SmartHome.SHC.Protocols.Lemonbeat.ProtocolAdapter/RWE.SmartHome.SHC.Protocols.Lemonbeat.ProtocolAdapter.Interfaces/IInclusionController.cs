using System;
using System.Net;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;

internal interface IInclusionController
{
	event Action DongleInitialized;

	void IncludeAsync(DeviceInformation device);

	bool InitializeDongle(IPAddress address);

	void ExcludeAsync(DeviceInformation device);

	void ResetDeviceInclusionState(Guid deviceId);

	void DropDiscoveredDevices(BaseDevice[] devices);
}
