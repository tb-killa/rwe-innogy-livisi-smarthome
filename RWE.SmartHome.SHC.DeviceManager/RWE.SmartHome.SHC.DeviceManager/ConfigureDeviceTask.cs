using System;

namespace RWE.SmartHome.SHC.DeviceManager;

internal class ConfigureDeviceTask
{
	public ConfigurationAction Action { get; set; }

	public Guid DeviceId { get; set; }

	public ConfigureDeviceTask(ConfigurationAction action, Guid deviceId)
	{
		Action = action;
		DeviceId = deviceId;
	}
}
