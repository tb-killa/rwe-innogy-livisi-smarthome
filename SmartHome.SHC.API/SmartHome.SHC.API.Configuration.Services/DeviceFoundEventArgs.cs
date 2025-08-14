using System;

namespace SmartHome.SHC.API.Configuration.Services;

public class DeviceFoundEventArgs : EventArgs
{
	public Device FoundDevice { get; set; }

	public DeviceFoundEventArgs(Device foundDevice)
	{
		FoundDevice = foundDevice;
	}
}
