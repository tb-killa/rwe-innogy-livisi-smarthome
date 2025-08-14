using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;

public class DeviceConfigurationFinishedEventArgs : EventArgs
{
	public Guid PhysicalDeviceId { get; private set; }

	public bool Successful { get; private set; }

	public DeviceConfigurationFinishedEventArgs(Guid physicalDeviceId, bool successful)
	{
		PhysicalDeviceId = physicalDeviceId;
		Successful = successful;
	}
}
