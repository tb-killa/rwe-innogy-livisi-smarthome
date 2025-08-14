using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class DeviceWasFactoryResetEventArgs
{
	public Guid DeviceId { get; set; }
}
