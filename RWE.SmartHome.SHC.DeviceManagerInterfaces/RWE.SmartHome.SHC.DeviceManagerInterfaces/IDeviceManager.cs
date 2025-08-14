using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces;

public interface IDeviceManager : IService
{
	IDeviceList DeviceList { get; }

	IDeviceController this[IDeviceInformation deviceInformation] { get; }

	IDeviceController this[Guid deviceId] { get; }

	byte[] DefaultShcAddress { get; }

	IList<byte[]> ShcAddresses { get; }

	event EventHandler<SequenceFinishedEventArgs> SequenceFinished;

	ISipcosConfigurator CreateSipcosConfigurator();

	void IncludeDevice(Guid deviceId);

	Guid ExcludeDevice(Guid deviceId);

	bool RemoveAllPendingConfigurationUpdates();

	void ResetDeviceInclusionState(Guid deviceId);

	void DropDiscoveredDevices(BaseDevice[] devices);
}
