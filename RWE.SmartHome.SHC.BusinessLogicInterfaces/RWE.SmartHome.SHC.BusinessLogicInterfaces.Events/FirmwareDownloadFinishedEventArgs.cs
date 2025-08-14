using System;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class FirmwareDownloadFinishedEventArgs : EventArgs
{
	public DeviceDescriptor DeviceInfo { get; set; }

	public DeviceFirmwareDescriptor Firmware { get; set; }
}
