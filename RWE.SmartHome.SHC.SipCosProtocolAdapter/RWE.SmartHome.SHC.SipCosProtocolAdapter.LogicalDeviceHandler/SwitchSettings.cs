using System;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

public abstract class SwitchSettings
{
	public const int NoOffTimer = 0;

	public Guid DeviceId { get; set; }

	public CommandType CommandType { get; protected set; }
}
