using System;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

internal class CustomSwitchSettings : SwitchSettings
{
	private Action action;

	public CustomSwitchSettings(Action action)
	{
		this.action = action;
		base.CommandType = CommandType.CustomAction;
	}

	public void Execute()
	{
		action();
	}
}
