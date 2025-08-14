using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;

public interface IDeviceFirmwareRepository
{
	event EventHandler<FirmwareDownloadFinishedEventArgs> FirmwareDownloadFinished;

	void DownloadFirmware(DeviceDescriptor deviceDescriptor, string url, string md5Hash, string targetVersion);

	DeviceFirmwareDescriptor GetFirmware(DeviceDescriptor deviceDescriptor);

	List<DeviceDescriptor> GetDownloadedFirmwareDescriptors();

	void DeleteFirmware(DeviceDescriptor deviceDescriptor);
}
