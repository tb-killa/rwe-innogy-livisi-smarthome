using System;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;

public class DeviceUpdateStateChangedEventArgs : EventArgs
{
	public Guid DeviceId { get; set; }

	public DeviceUpdateState NewDeviceUpdateState { get; set; }

	public DeviceUpdateState OldDeviceUpdateState { get; set; }

	public string FirmwareVersion { get; set; }
}
