using System;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;

namespace RWE.SmartHome.SHC.DeviceManager.Events;

public class DeviceReachableChangedEventArgs : EventArgs
{
	public IBasicDeviceInformation DeviceInformation { get; private set; }

	public bool Reachable { get; set; }

	public DeviceReachableChangedEventArgs(IBasicDeviceInformation deviceInformation, bool reachable)
	{
		Reachable = reachable;
		DeviceInformation = deviceInformation;
	}
}
