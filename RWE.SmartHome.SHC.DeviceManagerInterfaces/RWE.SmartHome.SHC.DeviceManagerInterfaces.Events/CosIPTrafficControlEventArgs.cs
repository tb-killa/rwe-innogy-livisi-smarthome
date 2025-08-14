using System;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;

public class CosIPTrafficControlEventArgs : EventArgs
{
	public CosIPTrafficState TrafficState { get; set; }
}
